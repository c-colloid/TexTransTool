#if NDMF_1_5_x
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using nadena.dev.ndmf;
using nadena.dev.ndmf.rq;
using nadena.dev.ndmf.rq.unity.editor;
using net.rs64.TexTransCore.BlendTexture;
using net.rs64.TexTransCore.Utils;
using net.rs64.TexTransTool.TextureStack;
using UnityEditor;
using UnityEngine;

namespace net.rs64.TexTransTool.NDMF
{
    internal class NodeExecuteDomain : IEditorCallDomain, IDisposable
    {
        HashSet<UnityEngine.Object> _transferredObject = new();
        protected readonly StackManager<ImmediateTextureStack> _textureStacks;
        protected readonly ITextureManager _textureManager;

        ComputeContext _ctx;

        List<Renderer> _proxyDomainRenderers;
        Dictionary<Renderer, Renderer> _proxy2OriginRendererDict;

        Dictionary<Renderer, Action<Renderer>> _rendererApplyRecaller = new();//origin 2 apply call


        public bool UsedTextureStack { get; private set; } = false;
        public bool UsedMaterialReplace { get; private set; } = false;
        public bool UsedSetMesh { get; private set; } = false;
        public bool UsedLookAt { get; private set; } = false;


        public NodeExecuteDomain(IEnumerable<(Renderer origin, Renderer proxy)> renderers, ComputeContext computeContext)
        {
            _proxyDomainRenderers = renderers.Select(i => i.proxy).ToList();
            _proxy2OriginRendererDict = renderers.ToDictionary(i => i.proxy, i => i.origin);
            _textureManager = new TextureManager(true);
            _textureStacks = new(_textureManager);
            _ctx = computeContext;
        }

        public void LookAt(UnityEngine.Object obj) { _ctx?.Observe(obj); UsedLookAt = true; }

        public void AddTextureStack<BlendTex>(Texture dist, BlendTex setTex) where BlendTex : TextureBlend.IBlendTexturePair
        {
            _textureStacks.AddTextureStack(dist as Texture2D, setTex);
            UsedTextureStack = true;
        }
        public IEnumerable<Renderer> EnumerateRenderer() { return _proxyDomainRenderers; }

        public ITextureManager GetTextureManager() => _textureManager;

        public bool IsPreview() => false;

        private void RegisterRecall(Renderer proxyRenderer, Action<Renderer> recall)
        {
            if (!_proxy2OriginRendererDict.ContainsKey(proxyRenderer)) { throw new InvalidOperationException($" {proxyRenderer.name} はプロキシーリストにないよ...?"); }

            if (_rendererApplyRecaller.ContainsKey(_proxy2OriginRendererDict[proxyRenderer])) { _rendererApplyRecaller[_proxy2OriginRendererDict[proxyRenderer]] += recall; }
            else { _rendererApplyRecaller[_proxy2OriginRendererDict[proxyRenderer]] = recall; }
        }

        public void ReplaceMaterials(Dictionary<Material, Material> mapping, bool one2one = true)
        {
            foreach (var dr in _proxyDomainRenderers)
            {
                RegisterRecall(dr, i => RendererUtility.SwapMaterials(i, mapping));
                RendererUtility.SwapMaterials(dr, mapping);
            }
            if(one2one) foreach (var matKV in mapping) { RegisterReplace(matKV.Key, matKV.Value); }
            this.transferAssets(mapping.Values);
            UsedMaterialReplace = true;
        }

        public void SetMesh(Renderer renderer, Mesh mesh)
        {
            RegisterRecall(renderer, i => i.SetMesh(mesh));
            renderer.SetMesh(mesh);
            UsedSetMesh = true;
        }

        public void RegisterReplace(UnityEngine.Object oldObject, UnityEngine.Object nowObject)
        {
            ObjectRegistry.RegisterReplacedObject(oldObject, nowObject);
        }
        public bool OriginEqual(UnityEngine.Object l, UnityEngine.Object r)
        {
            if (l == r) { return true; }
            if (l is Renderer lRen && r is Renderer rRen)
            {
                if (RenderersDomain.GetOrigin(_proxy2OriginRendererDict, lRen) == RenderersDomain.GetOrigin(_proxy2OriginRendererDict, rRen)) { return true; }
            }
            return ObjectRegistry.GetReference(l) == ObjectRegistry.GetReference(r);
        }

        public void SetSerializedProperty(UnityEditor.SerializedProperty property, UnityEngine.Object value)
        {
            property.objectReferenceValue = value;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public void TransferAsset(UnityEngine.Object asset)
        {
            _transferredObject.Add(asset);
            TempAssetContainer.TempPost(asset);

            // これらじゃ Unityによる謎の破棄を回避できなかった
            // asset.hideFlags = HideFlags.DontUnloadUnusedAsset;
            // UnityEditor.EditorUtility.SetDirty(asset);
        }


        public void DomainFinish()
        {
            var MergedStacks = _textureStacks.MergeStacks();

            foreach (var mergeResult in MergedStacks)
            {
                if (mergeResult.FirstTexture == null || mergeResult.MergeTexture == null) continue;
                SetTexture(mergeResult.FirstTexture, mergeResult.MergeTexture);
                TransferAsset(mergeResult.MergeTexture);
            }

            _textureManager.DestroyDeferred();
            _textureManager.CompressDeferred();


            void SetTexture(Texture2D firstTexture, Texture2D mergeTexture)
            {
                var mats = RendererUtility.GetFilteredMaterials(_proxyDomainRenderers);
                ReplaceMaterials(MaterialUtility.ReplaceTextureAll(mats, firstTexture, mergeTexture));
                RegisterReplace(firstTexture, mergeTexture);
            }
        }

        public void Dispose()
        {
            foreach (var obj in _transferredObject) { UnityEngine.Object.DestroyImmediate(obj, true); }
            _transferredObject.Clear();

            _ctx = null;
        }

        internal void DomainRecaller(Renderer original, Renderer proxy)
        {
            if (!_rendererApplyRecaller.ContainsKey(original))
            {
                Debug.Log($"{original.name} is can not Recall");
                return;
            }

            _rendererApplyRecaller[original].Invoke(proxy);
        }
    }


    internal static class TempAssetContainer
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Page;
        }

        static AvatarDomainAsset s_container;
        static string s_containerPath = Path.Combine(AssetSaver.SaveDirectory, "TempAssets.asset");

        public static void TempPost(UnityEngine.Object asset)
        {
            if (s_container == null)
            {
                AssetSaver.CheckSaveDirectory();
                s_container = ScriptableObject.CreateInstance<AvatarDomainAsset>();
                AssetDatabase.CreateAsset(s_container, s_containerPath);
            }
            s_container.AddSubObject(asset);
        }

        static void Page()
        {
            AssetDatabase.DeleteAsset(s_containerPath);
        }
    }





}
#endif
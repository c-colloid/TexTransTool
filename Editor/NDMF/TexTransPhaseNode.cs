using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nadena.dev.ndmf;
using nadena.dev.ndmf.preview;
using UnityEngine;
using UnityEngine.Profiling;

namespace net.rs64.TexTransTool.NDMF
{
    internal class TexTransPhaseNode : IRenderFilterNode
    {
        //TODO : 決め打ちじゃなくて、もっと調べて正しい状態にしてもいい気がする。
        public RenderAspects Reads => _nodeDomain.UsedLookAt ? RenderAspects.Everything : 0;
        public RenderAspects WhatChanged
        {
            get
            {
                RenderAspects flag = 0;

                if (_nodeDomain.UsedMaterialReplace) flag |= RenderAspects.Material;
                if (_nodeDomain.UsedSetMesh) flag |= RenderAspects.Mesh | RenderAspects.Texture;
                if (_nodeDomain.UsedTextureStack) flag |= RenderAspects.Material | RenderAspects.Texture;

                return flag;
            }
        }

        IEnumerable<TexTransBehavior> _targetBehaviors;
        IEnumerable<TexTransPhase> _targetPhase;

        NodeExecuteDomain _nodeDomain;
        public TexTransPhaseNode(IEnumerable<TexTransBehavior> targetBehaviors, IEnumerable<TexTransPhase> targetPhase)
        {
            _targetBehaviors = targetBehaviors;
            _targetPhase = targetPhase;
        }
        public TexTransPhaseNode(TexTransPhaseNode source) : this(source._targetBehaviors, source._targetPhase) { }
        public void NodeExecuteAndInit(IEnumerable<(Renderer origin, Renderer proxy)> proxyPairs, ComputeContext ctx)
        {
            Profiler.BeginSample("NodeExecuteDomain.ctr");
            _nodeDomain?.Dispose();
            _nodeDomain = new NodeExecuteDomain(proxyPairs, ctx, ObjectRegistry.ActiveRegistry);
            Profiler.EndSample();
            Profiler.BeginSample("apply ttb s");
            foreach (var ttb in _targetBehaviors)
            {
                if (ttb == null) { continue; }
                ctx.Observe(ttb);

                Profiler.BeginSample("apply-" + ttb.name);
                ttb.Apply(_nodeDomain);
                Profiler.EndSample();
            }
            Profiler.EndSample();
            Profiler.BeginSample("DomainFinish");
            _nodeDomain.DomainFinish();
            Profiler.EndSample();
        }
        public void OnFrame(Renderer original, Renderer proxy)
        {
            _nodeDomain?.DomainRecaller(original, proxy);
        }

        void IDisposable.Dispose()
        {
            _nodeDomain?.Dispose();
            _nodeDomain = null;
        }
        public async Task<IRenderFilterNode> Refresh(
            IEnumerable<(Renderer, Renderer)> proxyPairs,
            ComputeContext context,
            RenderAspects updatedAspects
        )
        {
            await Task.Delay(0);

            var node = new TexTransPhaseNode(this);
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Profiler.BeginSample("node.NodeExecuteAndInit");
            node.NodeExecuteAndInit(proxyPairs, context);
            Profiler.EndSample();

            timer.Stop();
#if TTT_DISPLAY_RUNTIME_LOG
            Debug.Log($" time:{timer.ElapsedMilliseconds}ms - Refresh: {string.Join("-", node._targetPhase.Select(i => i.ToString()))}  \n  {string.Join("-", proxyPairs.Select(r => r.Item1.gameObject.name))} ");
#endif
            return node;
        }
        public override string ToString()
        {
            return base.ToString() + string.Join("-", _targetPhase.Select(i => i.ToString()));
        }
    }
}

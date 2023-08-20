﻿#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Rs64.TexTransTool.ShaderSupport
{
    public class liltoonSupport : IShaderSupport
    {
        public string SupprotShaderName => "lilToon";

        public void MaterialCustomSetting(Material material)
        {
            var MainTex = material.GetTexture("_MainTex") as Texture2D;
            material.SetTexture("_BaseMap", MainTex);
            material.SetTexture("_BaseColorMap", MainTex);
        }

        public List<PropAndTexture> GetPropertyAndTextures(Material material, bool IsGNTFMP = false)
        {
            var PropEnvsDict = new Dictionary<string, Texture>();

            PropEnvsDict.Add("_MainTex", material.GetTexture("_MainTex") as Texture2D);
            PropEnvsDict.Add("_MainColorAdjustMask", material.GetTexture("_MainColorAdjustMask") as Texture2D);
            PropEnvsDict.Add("_Main2ndTex", material.GetTexture("_Main2ndTex") as Texture2D);
            PropEnvsDict.Add("_Main2ndBlendMask", material.GetTexture("_Main2ndBlendMask") as Texture2D);
            // PropertyAndTextures.Add(new PropAndTexture("_Main2ndDissolveMask", material.GetTexture("_Main2ndDissolveMask") as Texture2D));
            // PropertyAndTextures.Add(new PropAndTexture("_Main2ndDissolveNoiseMask", material.GetTexture("_Main2ndDissolveNoiseMask") as Texture2D));
            PropEnvsDict.Add("_Main3rdTex", material.GetTexture("_Main3rdTex") as Texture2D);
            PropEnvsDict.Add("_Main3rdBlendMask", material.GetTexture("_Main3rdBlendMask") as Texture2D);
            // PropertyAndTextures.Add(new PropAndTexture("_Main3rdDissolveMask", material.GetTexture("_Main3rdDissolveMask") as Texture2D));
            // PropertyAndTextures.Add(new PropAndTexture("_Main3rdDissolveNoiseMask", material.GetTexture("_Main3rdDissolveNoiseMask") as Texture2D));
            PropEnvsDict.Add("_AlphaMask", material.GetTexture("_AlphaMask") as Texture2D);
            PropEnvsDict.Add("_BumpMap", material.GetTexture("_BumpMap") as Texture2D);
            PropEnvsDict.Add("_Bump2ndMap", material.GetTexture("_Bump2ndMap") as Texture2D);
            PropEnvsDict.Add("_Bump2ndScaleMask", material.GetTexture("_Bump2ndScaleMask") as Texture2D);
            PropEnvsDict.Add("_AnisotropyTangentMap", material.GetTexture("_AnisotropyTangentMap") as Texture2D);
            PropEnvsDict.Add("_AnisotropyScaleMask", material.GetTexture("_AnisotropyScaleMask") as Texture2D);
            PropEnvsDict.Add("_BacklightColorTex", material.GetTexture("_BacklightColorTex") as Texture2D);
            PropEnvsDict.Add("_ShadowStrengthMask", material.GetTexture("_ShadowStrengthMask") as Texture2D);
            PropEnvsDict.Add("_ShadowBorderMask", material.GetTexture("_ShadowBorderMask") as Texture2D);
            PropEnvsDict.Add("_ShadowBlurMask", material.GetTexture("_ShadowBlurMask") as Texture2D);
            PropEnvsDict.Add("_ShadowColorTex", material.GetTexture("_ShadowColorTex") as Texture2D);
            PropEnvsDict.Add("_Shadow2ndColorTex", material.GetTexture("_Shadow2ndColorTex") as Texture2D);
            PropEnvsDict.Add("_Shadow3rdColorTex", material.GetTexture("_Shadow3rdColorTex") as Texture2D);
            PropEnvsDict.Add("_SmoothnessTex", material.GetTexture("_SmoothnessTex") as Texture2D);
            PropEnvsDict.Add("_MetallicGlossMap", material.GetTexture("_MetallicGlossMap") as Texture2D);
            PropEnvsDict.Add("_ReflectionColorTex", material.GetTexture("_ReflectionColorTex") as Texture2D);
            // PropertyAndTextures.Add(new PropAndTexture("_MatCapBlendMask", material.GetTexture("_MatCapBlendMask") as Texture2D));
            // PropertyAndTextures.Add(new PropAndTexture("_MatCap2ndBlendMask", material.GetTexture("_MatCap2ndBlendMask") as Texture2D));
            PropEnvsDict.Add("_RimColorTex", material.GetTexture("_RimColorTex") as Texture2D);
            PropEnvsDict.Add("_GlitterColorTex", material.GetTexture("_GlitterColorTex") as Texture2D);
            // PropertyAndTextures.Add(new PropAndTexture("_GlitterShapeTex", material.GetTexture("_GlitterShapeTex") as Texture2D));
            PropEnvsDict.Add("_EmissionMap", material.GetTexture("_EmissionMap") as Texture2D);
            PropEnvsDict.Add("_EmissionBlendMask", material.GetTexture("_EmissionBlendMask") as Texture2D);
            // PropertyAndTextures.Add(new PropAndTexture("_EmissionGradTex", material.GetTexture("_EmissionGradTex") as Texture2D));
            PropEnvsDict.Add("_Emission2ndMap", material.GetTexture("_Emission2ndMap") as Texture2D);
            PropEnvsDict.Add("_Emission2ndBlendMask", material.GetTexture("_Emission2ndBlendMask") as Texture2D);
            // PropertyAndTextures.Add(new PropAndTexture("_Emission2ndGradTex", material.GetTexture("_Emission2ndGradTex") as Texture2D));
            PropEnvsDict.Add("_ParallaxMap", material.GetTexture("_ParallaxMap") as Texture2D);
            PropEnvsDict.Add("_AudioLinkMask", material.GetTexture("_AudioLinkMask") as Texture2D);
            // PropertyAndTextures.Add(new PropAndTexture("_DissolveMask", material.GetTexture("_DissolveMask") as Texture2D));
            // PropertyAndTextures.Add(new PropAndTexture("_DissolveNoiseMask", material.GetTexture("_DissolveNoiseMask") as Texture2D));
            PropEnvsDict.Add("_OutlineTex", material.GetTexture("_OutlineTex") as Texture2D);
            PropEnvsDict.Add("_OutlineWidthMask", material.GetTexture("_OutlineWidthMask") as Texture2D);
            PropEnvsDict.Add("_OutlineVectorTex", material.GetTexture("_OutlineVectorTex") as Texture2D);

            void ColorMul(string TexPropName, string ColorPorpName)
            {
                var Color = material.GetColor(ColorPorpName);

                var MainTex = PropEnvsDict[TexPropName];
                if (MainTex == null)
                {
                    if (IsGNTFMP)
                    {
                        PropEnvsDict[TexPropName] = CreateColorTex(Color);
                    }
                }
                else
                {
                    PropEnvsDict[TexPropName] = CreatMuldRenderTexture(Color, MainTex);
                }
            }
            void FloatMul(string TexPropName, string FloatProp)
            {
                var ShadowStrength = material.GetFloat(FloatProp);

                var ShadowStrengthMask = PropEnvsDict[TexPropName];
                if (ShadowStrengthMask == null)
                {
                    if (IsGNTFMP)
                    {
                        PropEnvsDict[TexPropName] = CreateColorTex(new Color(ShadowStrength, ShadowStrength, ShadowStrength, ShadowStrength));
                    }
                }
                else
                {
                    PropEnvsDict[TexPropName] = CreatMuldRenderTexture(new Color(ShadowStrength, ShadowStrength, ShadowStrength, ShadowStrength), ShadowStrengthMask);
                }
            }

            if (lilDifferenceRecordI.IsDifference_MainColor)
            {
                ColorMul("_MainTex", "_Color");
            }
            if (lilDifferenceRecordI.IsDifference_MainTexHSVG)
            {
                var MainTex = PropEnvsDict["_MainTex"];
                var ColorAdjustMask = PropEnvsDict["_MainColorAdjustMask"];

                if (MainTex is Texture2D MainTex2d && MainTex2d != null)
                {
                    var MainTexRt = new RenderTexture(MainTex2d.width, MainTex2d.height, 0, RenderTextureFormat.ARGB32);

                    var MainTexHSVG = material.GetColor("_MainTexHSVG");
                    var Mat = new Material(Shader.Find("Hidden/ColorAdjustShader"));

                    Mat.SetFloat("_UseMask", ColorAdjustMask == null ? 0 : 1);
                    if (ColorAdjustMask != null) { Mat.SetTexture("_Mask", ColorAdjustMask); }
                    Mat.SetColor("_HSVG", MainTexHSVG);

                    Graphics.Blit(MainTex2d, MainTexRt, Mat);
                    PropEnvsDict["_MainTex"] = MainTexRt;
                }
                else if (MainTex is RenderTexture MainTexRt && MainTexRt != null)
                {
                    var SwapRt = new RenderTexture(MainTexRt.descriptor);

                    Graphics.CopyTexture(MainTex, SwapRt);

                    var MainTexHSVG = material.GetColor("_MainTexHSVG");

                    var Mat = new Material(Shader.Find("Hidden/ColorAdjustShader"));
                    Mat.SetFloat("_UseMask", ColorAdjustMask == null ? 0 : 1);
                    if (ColorAdjustMask != null) { Mat.SetTexture("_Mask", ColorAdjustMask); }
                    Mat.SetColor("_HSVG", MainTexHSVG);

                    Graphics.Blit(SwapRt, MainTexRt, Mat);
                }
            }
            if (lilDifferenceRecordI.IsDifference_MainColor2nd)
            {
                ColorMul("_Main2ndTex", "_Color2nd");
            }
            if (lilDifferenceRecordI.IsDifference_MainColor3rd)
            {
                ColorMul("_Main3rdTex", "_Color3rd");
            }
            if (lilDifferenceRecordI.IsDifference_ShadowStrength)
            {
                FloatMul("_ShadowStrengthMask", "_ShadowStrength");
            }
            if (lilDifferenceRecordI.IsDifference_ShadowColor)
            {
                ColorMul("_ShadowColorTex", "_ShadowColor");
            }
            if (lilDifferenceRecordI.IsDifference_Shadow2ndColor)
            {
                ColorMul("_Shadow2ndColorTex", "_Shadow2ndColor");
            }
            if (lilDifferenceRecordI.IsDifference_Shadow3rdColor)
            {
                ColorMul("_Shadow3rdColorTex", "_Shadow3rdColor");
            }
            if (lilDifferenceRecordI.IsDifference_EmissionColor)
            {
                ColorMul("_EmissionMap", "_EmissionColor");
            }
            if (lilDifferenceRecordI.IsDifference_EmissionBlend)
            {
                FloatMul("_EmissionBlendMask", "_EmissionBlend");
            }
            if (lilDifferenceRecordI.IsDifference_Emission2ndColor)
            {
                ColorMul("_Emission2ndMap", "_Emission2ndColor");
            }
            if (lilDifferenceRecordI.IsDifference_Emission2ndBlend)
            {
                FloatMul("_Emission2ndBlendMask", "_Emission2ndBlend");
            }
            if (lilDifferenceRecordI.IsDifference_AnisotropyScale)
            {
                FloatMul("_AnisotropyScaleMask", "_AnisotropyScale");
            }
            if (lilDifferenceRecordI.IsDifference_BacklightColor)
            {
                ColorMul("_BacklightColorTex", "_BacklightColor");
            }
            if (lilDifferenceRecordI.IsDifference_Smoothness)
            {
                FloatMul("_SmoothnessTex", "_Smoothness");
            }
            if (lilDifferenceRecordI.IsDifference_Metallic)
            {
                FloatMul("_MetallicGlossMap", "_Metallic");
            }
            if (lilDifferenceRecordI.IsDifference_ReflectionColor)
            {
                ColorMul("_ReflectionColorTex", "_ReflectionColor");
            }
            if (lilDifferenceRecordI.IsDifference_RimColor)
            {
                ColorMul("_RimColorTex", "_RimColor");
            }
            if (lilDifferenceRecordI.IsDifference_GlitterColor)
            {
                ColorMul("_GlitterColorTex", "_GlitterColor");
            }
            if (lilDifferenceRecordI.IsDifference_OutlineColor)
            {
                ColorMul("_OutlineTex", "_OutlineColor");
            }
            if (lilDifferenceRecordI.IsDifference_OutlineWidth)
            {
                var FloatProp = "_OutlineWidth";
                var TexPropName = "_OutlineWidthMask";

                var ShadowStrength = material.GetFloat(FloatProp) / lilDifferenceRecordI._OutlineWidth;

                var ShadowStrengthMask = PropEnvsDict[TexPropName];
                if (ShadowStrengthMask == null)
                {
                    if (IsGNTFMP)
                    {
                        PropEnvsDict[TexPropName] = CreateColorTex(new Color(ShadowStrength, ShadowStrength, ShadowStrength, ShadowStrength));
                    }
                }
                else
                {
                    PropEnvsDict[TexPropName] = CreatMuldRenderTexture(new Color(ShadowStrength, ShadowStrength, ShadowStrength, ShadowStrength), ShadowStrengthMask);
                }
            }


            var PropAndTexture = new List<PropAndTexture>();
            foreach (var PropEnv in PropEnvsDict)
            {
                PropAndTexture.Add(new PropAndTexture(PropEnv.Key, PropEnv.Value));
            }
            return PropAndTexture;
        }

        private static RenderTexture CreatMuldRenderTexture(Color Color, Texture MainTex)
        {
            var MainTexRt = new RenderTexture(MainTex.width, MainTex.height, 0, RenderTextureFormat.ARGB32);
            var Mat = new Material(Shader.Find("Hidden/ColorMulShader"));
            Mat.SetColor("_Color", Color);
            Graphics.Blit(MainTex, MainTexRt, Mat);
            return MainTexRt;
        }

        private static Texture2D CreateColorTex(Color Color)
        {
            var MainTex2d = new Texture2D(1, 1);
            MainTex2d.SetPixel(0, 0, Color);
            MainTex2d.Apply();
            return MainTex2d;
        }

        /*
対応状況まとめ

基本的にタイリングやオフセットには対応しない。
色変更の系統は、白(1,1,1,1)の時、まとめる前と同じ見た目になるようにする。
マスクなどの系統は、プロパティの値が1の時、まとめてないときと同じようになるようにする。  アウトラインを除く。


色設定 ---

< 色Tex * 色Color * (色調補正 * 色補正マスクTex)
ただし、グラデーションは無理。

2nd 3nd & BlendMask 2nd 3nd --
< (2,3)Tex * (2,3)Color
< (2,3)BlendMaskTex
カラーなどはまとめるが...UV0でかつ、デカール化されてない場合のみ。

Dissolve 2nd 3nd
マットキャップと同じような感じで..適当にまとめるべきではない。

影設定 ---

< マスクと強度Tex * マスクと強度Float
< 影色(1,2,3)Tex * 影色(1,2,3)Color
マスクと強度　や影色等は色と加味してまとめるが、
範囲やぼかしなどはどうしようもない。そもそもまとめるべきかというと微妙


ぼかし量マスクやAOMapは私が全く使ったことがないため、勝手がわからないため保留。

発光設定 ---

発光 1nd 2nd --
< 色/マスクTex * 色Color
< マスクTex * マスクFloat
UVModeがUV0の時はまとめる。

グラデーションはまず無理。
合成モードはどうにもできない。ユーザーが一番いいのを設定すべき。

マスクも一応まとめはするが、どのような風になるかは保証できない。

ノーマルマップ・光沢設定 ---

ノーマルマップ 1nd 2nd & 異方性反射 --
< ノーマルマップ(1,2)Tex
< ノーマルマップ2のマスクと強度
< 異方性反射ノーマルマップTex
< 異方性反射強度Tex * 異方性反射強度Float
まとめはするがどうなるかは保証できない。

逆光ライト --
< 色/マスクtex * 色Color

反射 --
< 滑らかさTex * 滑らかさFloat
< 金属度Tex * 金属度Float
< 色/マスクTex * 色Color
はまとめる。

MatCap系統は基本的に無理...そもそもこの方針だとできない。

リムライト --
< 色/マスクTex * 色Color

ラメ --
UV0の場合のみ。
< 色/マスクTex * 色Color

Shape周りは勝手がわからないため保留。

拡張設定 ---

輪郭線設定 --
< 色Tex * 色Color * 色調補正
< マスクと太さTex * マスクと太さFloat**

** 全体を見て全体の最大値を加味した値で補正する。ここだけ特殊な設定をする。

UV0の時だけまとめる。
< ノーマルマップ

視差マップ --
勝手がわからないため保留

AudioLink --
まとめれるものはない。

Dissolve --
そもそもまとめるべきではない。

IDMask --
ステンシル設定 --
レンダリング設定 --
ライトベイク設定 --
テッセレーション --
最適化 --

これらはそもそもまとめれるものではない。


ファーや屈折、宝石、などの高負荷系統は、独自の設定が強いし、そもそもまとめるべきかというと微妙。分けといたほうが軽いとかがありそうだから。
*/

        class lilDifferenceRecord
        {
            public bool IsInitilized = false;

            public Color _Color;
            public bool IsDifference_MainColor;

            public Color _MainTexHSVG;
            public bool IsDifference_MainTexHSVG;

            public Color _Color2nd;
            public bool IsDifference_MainColor2nd;

            public Color _Color3rd;
            public bool IsDifference_MainColor3rd;

            public float _ShadowStrength;
            public bool IsDifference_ShadowStrength;

            public Color _ShadowColor;
            public bool IsDifference_ShadowColor;

            public Color _Shadow2ndColor;
            public bool IsDifference_Shadow2ndColor;

            public Color _Shadow3rdColor;
            public bool IsDifference_Shadow3rdColor;

            public Color _EmissionColor;
            public bool IsDifference_EmissionColor;

            public float _EmissionBlend;
            public bool IsDifference_EmissionBlend;

            public Color _Emission2ndColor;
            public bool IsDifference_Emission2ndColor;

            public float _Emission2ndBlend;
            public bool IsDifference_Emission2ndBlend;

            public float _AnisotropyScale;
            public bool IsDifference_AnisotropyScale;

            public Color _BacklightColor;
            public bool IsDifference_BacklightColor;

            public float _Smoothness;
            public bool IsDifference_Smoothness;

            public float _Metallic;
            public bool IsDifference_Metallic;

            public Color _ReflectionColor;
            public bool IsDifference_ReflectionColor;

            public Color _RimColor;
            public bool IsDifference_RimColor;

            public Color _GlitterColor;
            public bool IsDifference_GlitterColor;

            public Color _OutlineColor;
            public bool IsDifference_OutlineColor;

            public float _OutlineWidth;
            public bool IsDifference_OutlineWidth;

        }
        lilDifferenceRecord lilDifferenceRecordI = new lilDifferenceRecord();

        public void AddRecord(Material material)
        {
            if (material == null) return;

            if (!lilDifferenceRecordI.IsInitilized)
            {
                lilDifferenceRecordI._Color = material.GetColor("_Color");
                lilDifferenceRecordI.IsDifference_MainColor = false;

                lilDifferenceRecordI._MainTexHSVG = material.GetColor("_MainTexHSVG");
                lilDifferenceRecordI.IsDifference_MainTexHSVG = false;

                lilDifferenceRecordI._Color2nd = material.GetColor("_Color2nd");
                lilDifferenceRecordI.IsDifference_MainColor2nd = false;

                lilDifferenceRecordI._Color3rd = material.GetColor("_Color3rd");
                lilDifferenceRecordI.IsDifference_MainColor3rd = false;

                lilDifferenceRecordI._ShadowStrength = material.GetFloat("_ShadowStrength");
                lilDifferenceRecordI.IsDifference_ShadowStrength = false;

                lilDifferenceRecordI._ShadowColor = material.GetColor("_ShadowColor");
                lilDifferenceRecordI.IsDifference_ShadowColor = false;

                lilDifferenceRecordI._Shadow2ndColor = material.GetColor("_Shadow2ndColor");
                lilDifferenceRecordI.IsDifference_Shadow2ndColor = false;

                lilDifferenceRecordI._Shadow3rdColor = material.GetColor("_Shadow3rdColor");
                lilDifferenceRecordI.IsDifference_Shadow3rdColor = false;

                lilDifferenceRecordI._EmissionColor = material.GetColor("_EmissionColor");
                lilDifferenceRecordI.IsDifference_EmissionColor = false;

                lilDifferenceRecordI._EmissionBlend = material.GetFloat("_EmissionBlend");
                lilDifferenceRecordI.IsDifference_EmissionBlend = false;

                lilDifferenceRecordI._Emission2ndColor = material.GetColor("_Emission2ndColor");
                lilDifferenceRecordI.IsDifference_Emission2ndColor = false;

                lilDifferenceRecordI._Emission2ndBlend = material.GetFloat("_Emission2ndBlend");
                lilDifferenceRecordI.IsDifference_Emission2ndBlend = false;

                lilDifferenceRecordI._AnisotropyScale = material.GetFloat("_AnisotropyScale");
                lilDifferenceRecordI.IsDifference_AnisotropyScale = false;

                lilDifferenceRecordI._BacklightColor = material.GetColor("_BacklightColor");
                lilDifferenceRecordI.IsDifference_BacklightColor = false;

                lilDifferenceRecordI._Smoothness = material.GetFloat("_Smoothness");
                lilDifferenceRecordI.IsDifference_Smoothness = false;

                lilDifferenceRecordI._Metallic = material.GetFloat("_Metallic");
                lilDifferenceRecordI.IsDifference_Metallic = false;

                lilDifferenceRecordI._ReflectionColor = material.GetColor("_ReflectionColor");
                lilDifferenceRecordI.IsDifference_ReflectionColor = false;

                lilDifferenceRecordI._RimColor = material.GetColor("_RimColor");
                lilDifferenceRecordI.IsDifference_RimColor = false;

                lilDifferenceRecordI._GlitterColor = material.GetColor("_GlitterColor");
                lilDifferenceRecordI.IsDifference_GlitterColor = false;

                lilDifferenceRecordI._OutlineColor = material.GetColor("_OutlineColor");
                lilDifferenceRecordI.IsDifference_OutlineColor = false;

                lilDifferenceRecordI._OutlineWidth = material.GetFloat("_OutlineWidth");
                lilDifferenceRecordI.IsDifference_OutlineWidth = false;

                lilDifferenceRecordI.IsInitilized = true;
            }
            else
            {
                if (lilDifferenceRecordI._Color != material.GetColor("_Color")) lilDifferenceRecordI.IsDifference_MainColor = true;
                if (lilDifferenceRecordI._MainTexHSVG != material.GetColor("_MainTexHSVG")) lilDifferenceRecordI.IsDifference_MainTexHSVG = true;
                if (lilDifferenceRecordI._Color2nd != material.GetColor("_Color2nd")) lilDifferenceRecordI.IsDifference_MainColor2nd = true;
                if (lilDifferenceRecordI._Color3rd != material.GetColor("_Color3rd")) lilDifferenceRecordI.IsDifference_MainColor3rd = true;
                if (!Mathf.Approximately(lilDifferenceRecordI._ShadowStrength, material.GetFloat("_ShadowStrength"))) lilDifferenceRecordI.IsDifference_ShadowStrength = true;
                if (lilDifferenceRecordI._ShadowColor != material.GetColor("_ShadowColor")) lilDifferenceRecordI.IsDifference_ShadowColor = true;
                if (lilDifferenceRecordI._Shadow2ndColor != material.GetColor("_Shadow2ndColor")) lilDifferenceRecordI.IsDifference_Shadow2ndColor = true;
                if (lilDifferenceRecordI._Shadow3rdColor != material.GetColor("_Shadow3rdColor")) lilDifferenceRecordI.IsDifference_Shadow3rdColor = true;
                if (lilDifferenceRecordI._EmissionColor != material.GetColor("_EmissionColor")) lilDifferenceRecordI.IsDifference_EmissionColor = true;
                if (!Mathf.Approximately(lilDifferenceRecordI._EmissionBlend, material.GetFloat("_EmissionBlend"))) lilDifferenceRecordI.IsDifference_EmissionBlend = true;
                if (lilDifferenceRecordI._Emission2ndColor != material.GetColor("_Emission2ndColor")) lilDifferenceRecordI.IsDifference_Emission2ndColor = true;
                if (!Mathf.Approximately(lilDifferenceRecordI._Emission2ndBlend, material.GetFloat("_Emission2ndBlend"))) lilDifferenceRecordI.IsDifference_Emission2ndBlend = true;
                if (!Mathf.Approximately(lilDifferenceRecordI._AnisotropyScale, material.GetFloat("_AnisotropyScale"))) lilDifferenceRecordI.IsDifference_AnisotropyScale = true;
                if (lilDifferenceRecordI._BacklightColor != material.GetColor("_BacklightColor")) lilDifferenceRecordI.IsDifference_BacklightColor = true;
                if (!Mathf.Approximately(lilDifferenceRecordI._Smoothness, material.GetFloat("_Smoothness"))) lilDifferenceRecordI.IsDifference_Smoothness = true;
                if (!Mathf.Approximately(lilDifferenceRecordI._Metallic, material.GetFloat("_Metallic"))) lilDifferenceRecordI.IsDifference_Metallic = true;
                if (lilDifferenceRecordI._ReflectionColor != material.GetColor("_ReflectionColor")) lilDifferenceRecordI.IsDifference_ReflectionColor = true;
                if (lilDifferenceRecordI._RimColor != material.GetColor("_RimColor")) lilDifferenceRecordI.IsDifference_RimColor = true;
                if (lilDifferenceRecordI._GlitterColor != material.GetColor("_GlitterColor")) lilDifferenceRecordI.IsDifference_GlitterColor = true;
                if (lilDifferenceRecordI._OutlineColor != material.GetColor("_OutlineColor")) lilDifferenceRecordI.IsDifference_OutlineColor = true;
                if (!Mathf.Approximately(lilDifferenceRecordI._OutlineWidth, material.GetFloat("_OutlineWidth"))) { lilDifferenceRecordI.IsDifference_OutlineWidth = true; lilDifferenceRecordI._OutlineWidth = Mathf.Max(lilDifferenceRecordI._OutlineWidth, material.GetFloat("_OutlineWidth")); }
            }

        }

        public void ClearRecord()
        {
            lilDifferenceRecordI = new lilDifferenceRecord();
        }
    }
}
#endif
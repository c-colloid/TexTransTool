#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using net.rs64.TexTransCore.BlendTexture;
using net.rs64.TexTransTool.Utils;
using UnityEngine;
using static net.rs64.TexTransCore.BlendTexture.TextureBlendUtils;
using static net.rs64.TexTransTool.MultiLayerImage.MultiLayerImageCanvas;
namespace net.rs64.TexTransTool.MultiLayerImage
{
    [AddComponentMenu("TexTransTool/MultiLayer/TTT LayerFolder")]
    public class LayerFolder : AbstractLayer
    {
        public bool PassThrough;

        public override void EvaluateTexture(LayerStack layerStack)
        {
            if (!Visible) { layerStack.Stack.Add(new BlendLayer(this, null, BlendMode)); return; }
            var subStack = layerStack.CreateSubStack;
            var Layers = transform.GetChildren()
            .Select(I => I.GetComponent<AbstractLayer>())
            .Reverse();
            foreach (var layer in Layers) { layer.EvaluateTexture(subStack); }

            if (subStack.Stack.Count() == 0) { return; }
            if (!Clipping && PassThrough)
            {
                foreach (var layer in subStack.GetLayers)
                {
                    MultipleRenderTexture((RenderTexture)layer.Texture, new Color(1, 1, 1, Opacity));

                    if (!LayerMask.LayerMaskDisabled && LayerMask.MaskTexture != null) { MaskDrawRenderTexture((RenderTexture)layer.Texture, LayerMask.MaskTexture); }
                    layerStack.AddRenderTexture(this, layer.Texture as RenderTexture, layer.BlendType);
                }
            }
            else
            {
                var rt = new RenderTexture(layerStack.CanvasSize.x, layerStack.CanvasSize.y, 0);
                TextureBlendUtils.BlendBlit(rt, subStack.GetLayers);
                TextureBlendUtils.MultipleRenderTexture(rt, new Color(1, 1, 1, Opacity));
                if (!LayerMask.LayerMaskDisabled && LayerMask.MaskTexture != null) { MaskDrawRenderTexture(rt, LayerMask.MaskTexture); }

                if (Clipping) { layerStack.AddRtForClipping(this, rt, BlendMode); }
                else { layerStack.AddRenderTexture(this, rt, PassThrough ? BlendType.Normal : BlendMode); }
            }

        }
    }
}
#endif
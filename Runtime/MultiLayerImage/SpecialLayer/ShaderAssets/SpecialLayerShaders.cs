using net.rs64.TexTransCore;
using UnityEngine;
namespace net.rs64.TexTransTool.MultiLayerImage
{
    internal static class SpecialLayerShaders
    {
        [TexTransInitialize]
        public static void Init()
        {
            HSLAdjustmentShader = Shader.Find(HSL_ADJUSTMENT_SHADER);
            LevelAdjustmentShader = Shader.Find(LEVEL_ADJUSTMENT_SHADER);
            SelectiveColorAdjustment = Shader.Find(SELECTIVE_COLOR_ADJUSTMENT_SHADER);
            LuminanceMappingShader = Shader.Find(LUMINANCE_MAPPING_SHADER);
            YAxisFixedGradientShader = Shader.Find(Y_AXIS_FIXED_GRADIENT);
            ColorizeShader = Shader.Find(COLORIZE_SHADER);
            GaussianBluerComputeShader = TexTransCoreRuntime.LoadAsset(GAUSSIAN_BLUER_COMPUTE_SHADER, typeof(ComputeShader)) as ComputeShader;
        }

        public const string HSL_ADJUSTMENT_SHADER = "Hidden/HSLAdjustment";
        public static Shader HSLAdjustmentShader;

        public const string LEVEL_ADJUSTMENT_SHADER = "Hidden/LevelAdjustment";
        public static Shader LevelAdjustmentShader;

        public const string SELECTIVE_COLOR_ADJUSTMENT_SHADER = "Hidden/SelectiveColorAdjustment";
        public static Shader SelectiveColorAdjustment;
        public const string LUMINANCE_MAPPING_SHADER = "Hidden/LuminanceMappingShader";
        public static Shader LuminanceMappingShader;

        public const string Y_AXIS_FIXED_GRADIENT = "Hidden/YAxisFixedGradient";
        public static Shader YAxisFixedGradientShader;

        public const string COLORIZE_SHADER= "Hidden/ColorizeShader";
        public static Shader ColorizeShader;
        public const string GAUSSIAN_BLUER_COMPUTE_SHADER = "d8ade7140a0c2234f83e9374588e8497";
        public static ComputeShader GaussianBluerComputeShader;

    }
}

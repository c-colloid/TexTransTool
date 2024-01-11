#if NDMF_1_0_x
using nadena.dev.ndmf;
using net.rs64.TexTransTool.Build.NDMF;
using net.rs64.TexTransTool.Build;

[assembly: ExportsPlugin(typeof(NDMFPlugin))]

namespace net.rs64.TexTransTool.Build.NDMF
{

    internal class NDMFPlugin : Plugin<NDMFPlugin>
    {
        public override string QualifiedName => "net.rs64.tex-trans-tool";
        public override string DisplayName => "TexTransTool";
        protected override void Configure()
        {
            var seq = InPhase(BuildPhase.Resolving);

            seq.Run(PreviewCancelerPass.Instance);
            seq.Run(ResolvingPass.Instance);


            seq = InPhase(BuildPhase.Transforming);

            seq.BeforePlugin("io.github.azukimochi.light-limit-changer");
            seq.WithRequiredExtension(typeof(TexTransToolContext), s =>
            {
                seq.Run(FindAtPhasePass.Instance);
                seq.Run(BeforeUVModificationPass.Instance);
                seq.Run(MidwayMergeStackPass.Instance);
                seq.Run(UVModificationPass.Instance);
                seq.Run(AfterUVModificationPass.Instance);
                seq.Run(UnDefinedPass.Instance);
            });

        }
    }

}
#endif
#if NDMF
using nadena.dev.ndmf;
using net.rs64.TexTransTool.ReferenceResolver;
using static net.rs64.TexTransTool.Build.AvatarBuildUtils;

namespace net.rs64.TexTransTool.Build.NDMF
{
    abstract class TTTPass<T> : Pass<T> where T : Pass<T>, new()
    {
        protected TexTransBuildSession TTTContext(BuildContext context)
        {
            return context.Extension<TexTransToolContext>().TTTBuildContext;
        }
    }
    internal class ResolvingPass : Pass<ResolvingPass>
    {
        protected override void Execute(BuildContext context)
        {
            var resolverContext = new ResolverContext(context.AvatarRootObject);
            resolverContext.ResolvingFor(context.AvatarRootObject.GetComponentsInChildren<AbstractResolver>());
        }
    }
    internal class FindAtPhasePass : TTTPass<FindAtPhasePass>
    {
        protected override void Execute(BuildContext context)
        {
            TTTContext(context).FindAtPhaseTTT();
        }
    }
    internal class BeforeUVModificationPass : TTTPass<BeforeUVModificationPass>
    {
        protected override void Execute(BuildContext context)
        {
            TTTContext(context).ApplyFor(TexTransPhase.BeforeUVModification);
        }
    }
    internal class MidwayMargeStackPass : TTTPass<MidwayMargeStackPass>
    {
        protected override void Execute(BuildContext context)
        {
            TTTContext(context).MidwayMargeStack();
        }
    }
    internal class UVModificationPass : TTTPass<UVModificationPass>
    {
        protected override void Execute(BuildContext context)
        {
            TTTContext(context).ApplyFor(TexTransPhase.UVModification);
        }
    }
    internal class AfterUVModificationPass : TTTPass<AfterUVModificationPass>
    {
        protected override void Execute(BuildContext context)
        {
            TTTContext(context).ApplyFor(TexTransPhase.AfterUVModification);
        }
    }
    internal class UnDefinedPass : TTTPass<UnDefinedPass>
    {
        protected override void Execute(BuildContext context)
        {
            TTTContext(context).ApplyFor(TexTransPhase.UnDefined);
        }
    }
}
#endif
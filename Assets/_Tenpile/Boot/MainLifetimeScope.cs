using Tenpile.View;
using VContainer;
using VContainer.Unity;

namespace Tenpile.Boot
{
    /// <summary>Scope của scene Main, con của RootLifetimeScope.</summary>
    public sealed class MainLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<BoardView>();
        }
    }
}

using MessagePipe;
using Tenpile.App;
using VContainer;
using VContainer.Unity;

namespace Tenpile.Boot
{
    /// <summary>Scope gốc trong scene Boot, sống suốt app. Nối port với adapter ở các mốc sau.</summary>
    public sealed class RootLifetimeScope : LifetimeScope
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            builder.Register<GameSession>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BootFlow>().WithParameter<LifetimeScope>(this);
        }
    }
}

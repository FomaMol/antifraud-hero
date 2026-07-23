using Services;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayService>().AsSelf();
        }
    }
}
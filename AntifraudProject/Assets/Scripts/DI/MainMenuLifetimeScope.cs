using Services.MainMenu;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuService>().AsSelf();
        }
    }
}
using Localization;
using Services;
using Services.MainMenu;
using Services.Scenes;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        public static IObjectResolver GlobalContainer { get; private set; }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PersistingService>().AsSelf();
            builder.RegisterEntryPoint<LocalizationService>().AsSelf();
            builder.RegisterEntryPoint<ScenesService>().AsSelf();
        }

        protected override void Awake()
        {
            base.Awake();
            GlobalContainer = Container;
        }
    }
}

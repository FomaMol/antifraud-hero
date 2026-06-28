using Localization;
using Services;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        public static IObjectResolver GlobalContainer { get; private set; }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PersistingService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<LocalizationService>().AsSelf();
        }

        protected override void Awake()
        {
            base.Awake();
            GlobalContainer = Container;
        }
    }
}

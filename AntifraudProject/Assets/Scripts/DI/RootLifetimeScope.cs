using GameLogic.UI.Popups;
using Localization;
using Services;
using Services.Scenes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private PopupService _popupService;
        
        public static IObjectResolver GlobalContainer { get; private set; }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PersistingService>().AsSelf();
            builder.RegisterEntryPoint<LocalizationService>().AsSelf();
            builder.RegisterEntryPoint<ScenesService>().AsSelf();
            builder.RegisterComponent(_popupService).AsImplementedInterfaces().AsSelf();
        }

        protected override void Awake()
        {
            base.Awake();
            GlobalContainer = Container;
        }
    }
}

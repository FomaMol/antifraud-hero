using Cysharp.Threading.Tasks;
using GameLogic.UI.Popups;
using Localization;
using Popups;
using Services.Scenes;
using VContainer;

namespace Services.MainMenu
{
    public class MainMenuService
    {
        [Inject] private readonly PersistingService _persistingService;
        [Inject] private readonly ScenesService _scenesService;
        [Inject] private readonly LocalizationService _localizationService;
        [Inject] private readonly PopupService _popupService;

        public bool WasGameplayPlayed => _persistingService.WasGameplayPlayed;
        public bool SoundsEnabled => _persistingService.SoundsEnabled;

        public void StartNewGame()
        {
            _persistingService.ResetGameplayProgress();
            _scenesService.LaunchGameplay();
        }

        public void ContinueGame()
        {
            _scenesService.LaunchGameplay();
        }

        public void ToggleLocalization()
        {
            _localizationService.ChangeLanguage(_localizationService.Language == "en" ? "ru" : "en");
        }

        public void ToggleAudio()
        {
            _persistingService.SetSoundsEnabled(!_persistingService.SoundsEnabled);
        }

        public void OpenHowToPlayPopup()
        {
            _popupService.OpenPopup<IHowToPlayPopupPresenter>(new HowToPlayPopupPresenter(), withBackGround: false).Forget();
        }

        public void ContinueGameWithHowToPlay()
        {
            _scenesService.ShowHowToPlayOnGameplayLaunch = true;
            _scenesService.LaunchGameplay();
        }
    }
}
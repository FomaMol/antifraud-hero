using Localization;
using Services.Scenes;
using VContainer;

namespace Services.MainMenu
{
    public class MainMenuService
    {
        [Inject] private readonly PersistingService _persistingService;
        [Inject] private readonly ScenesService _scenesService;
        [Inject] private readonly LocalizationService _localizationService;

        public bool WasGameplayPlayed => _persistingService.WasGameplayPlayed;

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
    }
}
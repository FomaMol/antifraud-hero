using Services.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets.MainMenu
{
    public class MainMenuWidget : Widget
    {
        [Inject] private readonly MainMenuService _mainMenuService;

        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _howToPlayButton;
        [SerializeField] private Button _changeLanguageButton;
        [SerializeField] private Button _toggleSoundsButton;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _continueButton.onClick.AddListener(OnContinueButtonClick);
            _newGameButton.onClick.AddListener(OnNewGameButtonClick);
            _howToPlayButton.onClick.AddListener(OnHowToPlayButtonClick);
            _changeLanguageButton.onClick.AddListener(OnChangeLanguageButtonClick);
            _toggleSoundsButton.onClick.AddListener(OnToggleSoundsButtonClick);

            UpdateView();
        }

        private void UpdateView()
        {
            _continueButton.gameObject.SetActive(_mainMenuService.WasGameplayPlayed);
            var label = _toggleSoundsButton.GetComponentInChildren<TMP_Text>();
            if (_mainMenuService.SoundsEnabled)
                label.fontStyle &= ~FontStyles.Strikethrough;
            else
                label.fontStyle |= FontStyles.Strikethrough;
        }

        private void OnContinueButtonClick()
        {
            _mainMenuService.ContinueGame();
        }

        private void OnNewGameButtonClick()
        {
            _mainMenuService.StartNewGame();
        }

        private void OnHowToPlayButtonClick()
        {
            _mainMenuService.OpenHowToPlayPopup();
        }

        private void OnChangeLanguageButtonClick()
        {
            _mainMenuService.ToggleLocalization();
        }

        private void OnToggleSoundsButtonClick()
        {
            _mainMenuService.ToggleAudio();
            UpdateView();
        }
    }
}
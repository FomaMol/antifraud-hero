using System;
using Localization;
using Services.Scenes;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets.MainMenu
{
    public class MainMenuWidget : Widget
    {
        [Inject] private readonly ScenesService _scenesService;
        [Inject] private readonly LocalizationService _localizationService;
        
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
        }

        private void OnContinueButtonClick()
        {
            _scenesService.LaunchGameplay();
        }

        private void OnNewGameButtonClick()
        {
            _scenesService.LaunchGameplay();
        }

        private void OnHowToPlayButtonClick()
        {
        }

        private void OnChangeLanguageButtonClick()
        {
            _localizationService.ChangeLanguage(_localizationService.Language == "en" ? "ru" : "en");
        }

        private void OnToggleSoundsButtonClick()
        {
        }
    }
}
using System;
using Localization;
using Services;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets.ControlBar
{
    public class ControlBarWidget : Widget
    {
        [Inject] private GameplayService _gameplayService;
        
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _declineButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private TextMeshProLocalization _helpCountText;

        public void Initialize()
        {
            _acceptButton.onClick.AddListener(OnAcceptButtonClick);
            _declineButton.onClick.AddListener(OnDeclineButtonClick);
            _helpButton.onClick.AddListener(OnHelpButtonClick);
            _gameplayService.OnChangeCase.Subscribe(OnChangeCase).AddTo(this);
            _gameplayService.OnUseHelp.Subscribe(OnUseHelp).AddTo(this);
            UpdateView();
        }

        private void OnAcceptButtonClick()
        {
            _gameplayService.MakeChoice(isValid: true);
        }

        private void OnDeclineButtonClick()
        {
            _gameplayService.MakeChoice(isValid: false);
        }

        private void OnHelpButtonClick()
        {
            _gameplayService.TryUseHelp();
        }

        private void OnChangeCase(Unit _)
        {
            UpdateView();
        }

        private void OnUseHelp(Unit _)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _helpButton.interactable = !_gameplayService.HelpWasUsedInCurrentCase && _gameplayService.AvailableHelpsNumber > 0;
            _helpCountText.SetArgs(_gameplayService.AvailableHelpsNumber);
            if (_gameplayService.HelpWasUsedInCurrentCase)
            {
                _acceptButton.interactable = _gameplayService.GetExpectedDecisionForCase(_gameplayService.CurrentCaseIndex);
                _declineButton.interactable = !_gameplayService.GetExpectedDecisionForCase(_gameplayService.CurrentCaseIndex);
            }
            else
            {
                _acceptButton.interactable = true;
                _declineButton.interactable = true;
            }
        }

        private void Start()
        {
            Initialize();
        }
    }
}
using System;
using Services;
using TMPro;
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
        [SerializeField] private TMP_Text _helpCountText;

        public void Initialize()
        {
            _acceptButton.onClick.AddListener(() => _gameplayService.MakeChoice(isValid: true));
            _declineButton.onClick.AddListener(() => _gameplayService.MakeChoice(isValid: false));
        }

        private void Start()
        {
            Initialize();
        }
    }
}
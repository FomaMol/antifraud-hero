using System;
using Localization;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets
{
    public class ToolWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
        [Inject] private readonly LocalizationService _localizationService;

        [SerializeField] private ToolType _toolType;
        
        [Space]
        [SerializeField] private Transform _toolState;
        [SerializeField] private Transform _emptyState;

        [Space]
        [SerializeField] private TMP_Text _dataText;
        [SerializeField] private Button _validateButton;
        [SerializeField] private Transform _validationResultVerified;
        [SerializeField] private Transform _validationResultFailed;

        public void Initialize()
        {
            _validateButton.onClick.AddListener(OnValidateButtonClick);
            CleanUp();
        }

        private void OnValidateButtonClick()
        {
            var validationResult = GetValidationResult(); 
            _validationResultVerified.gameObject.SetActive(validationResult);
            _validationResultFailed.gameObject.SetActive(!validationResult);
        }

        private bool GetValidationResult()
        {
            var currentCaseConfig = _gameplayService.CurrentCaseConfig;

            return _toolType switch
            {
                ToolType.Link => currentCaseConfig.LinkValidationResult == 1,
                ToolType.Contact => currentCaseConfig.SenderValidationResult == 1,
                ToolType.Db => currentCaseConfig.DbValidationResult == 1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string GetValidationText()
        {
            var currentCaseConfig = _gameplayService.CurrentCaseConfig;

            var key = _toolType switch
            {
                ToolType.Link => currentCaseConfig.LinkValidationText,
                ToolType.Contact => currentCaseConfig.SenderValidationText,
                ToolType.Db => currentCaseConfig.DbValidationText,
                _ => throw new ArgumentOutOfRangeException()
            };
            return _localizationService.Get(key);
        }

        public void InsertData(string data)
        {
            CleanUp();
            _dataText.SetText(data);
            _emptyState.gameObject.SetActive(false);
            _toolState.gameObject.SetActive(true);
        }
        
        public void CleanUp()
        {
            _toolState.gameObject.SetActive(false);
            _emptyState.gameObject.SetActive(true);
            _validationResultVerified.gameObject.SetActive(false);
            _validationResultFailed.gameObject.SetActive(false);
        }
    }
}
using Localization;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets.CaseListBar
{
    public class CaseInfoWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
        
        [SerializeField] private TextMeshProLocalization _caseTitle;
        [SerializeField] private Image _smsTypeIconImage;
        [SerializeField] private Image _webTypeIconImage;
        [SerializeField] private TextMeshProLocalization _smsTypeText;
        [SerializeField] private TextMeshProLocalization _webTypeText;
        [SerializeField] private Transform _selectedBackground;
        [SerializeField] private Image _successResultIconImage;
        [SerializeField] private Image _failResultIconImage;

        public void Initialize(int number, string caseType, bool selected)
        {
            _caseTitle.SetArgs(number);
            _smsTypeIconImage.gameObject.SetActive(caseType.ToLower() == "sms");
            _webTypeIconImage.gameObject.SetActive(caseType.ToLower() == "email");
            _smsTypeText.gameObject.SetActive(caseType.ToLower() == "sms");
            _webTypeText.gameObject.SetActive(caseType.ToLower() == "email");
            _selectedBackground.gameObject.SetActive(selected);
            if (_gameplayService.TryGetCaseDecisionFromHistory(number - 1, out var decision))
            {
                var wasSuccess = decision == _gameplayService.GetExpectedDecisionForCase(number - 1);
                _successResultIconImage.gameObject.SetActive(wasSuccess);
                _failResultIconImage.gameObject.SetActive(!wasSuccess);
            }
            else
            {
                _successResultIconImage.gameObject.SetActive(false);
                _failResultIconImage.gameObject.SetActive(false);
            }
        }
    }
}
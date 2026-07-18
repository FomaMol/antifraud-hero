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
        
        [SerializeField] private TMP_Text _caseTypeText;
        [SerializeField] private TextMeshProLocalization _caseNumberText;
        [SerializeField] private Image _typeIconImage;
        [SerializeField] private Transform _selectedMarker;
        [SerializeField] private Transform _selectedBackground;
        [SerializeField] private Image _resultIconImage;

        public void Initialize(int number, string caseType, bool selected)
        {
            _caseTypeText.SetText(caseType);
            _caseNumberText.SetArgs(number);
            _typeIconImage.color = caseType.ToLower() == "sms" ? Color.blue : Color.yellow;
            _selectedMarker.gameObject.SetActive(selected);
            _selectedBackground.gameObject.SetActive(selected);
            if (_gameplayService.TryGetCaseDecisionFromHistory(number - 1, out var decision))
            {
                _resultIconImage.color = decision == _gameplayService.GetExpectedDecisionForCase(number - 1) ? Color.green : Color.red;
                _resultIconImage.gameObject.SetActive(true);
            }
            else
            {
                _resultIconImage.gameObject.SetActive(false);
            }
        }
    }
}
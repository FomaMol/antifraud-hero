using GameLogic.UI.Popups;
using Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Popups
{
    public interface ICaseResultPopupPresenter : IPopupPresenter
    {
        public bool Success { get; }
        public string ButtonTextLocalizationKey { get; }
        public string ResultTextLocalizationKey { get; }
    }

    public class Help_CaseResultPopupPresenter : ICaseResultPopupPresenter
    {
        public bool Success { get; }
        public string ButtonTextLocalizationKey => "popups.case_result.close_button";
        public string ResultTextLocalizationKey { get; }

        public Help_CaseResultPopupPresenter(bool success, string resultTextLocalizationKey)
        {
            Success = success;
            ResultTextLocalizationKey = resultTextLocalizationKey;
        }
    }
    
    public class CaseResultPopupPresenter : ICaseResultPopupPresenter
    {
        public bool Success { get; }
        public string ButtonTextLocalizationKey => "popups.case_result.success";
        public string ResultTextLocalizationKey { get; }

        public CaseResultPopupPresenter(bool success, string resultTextLocalizationKey)
        {
            Success = success;
            ResultTextLocalizationKey = resultTextLocalizationKey;
        }
    }
    
    public class CaseResultPopup : PopupBase<ICaseResultPopupPresenter>
    {
        [SerializeField] private GameObject _successHeaderContainer;
        [SerializeField] private GameObject _failHeaderContainer;
        [SerializeField] private Button _nextCaseRedButton;
        [SerializeField] private Button _nextCaseGreenButton;
        [SerializeField] private TextMeshProLocalization _buttonText;
        [SerializeField] private TextMeshProLocalization _resultText;

        protected override void Init()
        {
            _nextCaseRedButton.onClick.AddListener(OnNextCaseButtonClick);
            _nextCaseGreenButton.onClick.AddListener(OnNextCaseButtonClick);
        }

        protected override void OnOpen(ICaseResultPopupPresenter popupPresenter)
        {
            _successHeaderContainer.SetActive(popupPresenter.Success);
            _failHeaderContainer.SetActive(!popupPresenter.Success);
            _nextCaseGreenButton.gameObject.SetActive(_successHeaderContainer.activeSelf);
            _nextCaseRedButton.gameObject.SetActive(_failHeaderContainer.activeSelf);
            _buttonText.SetText(popupPresenter.ButtonTextLocalizationKey);
            _resultText.SetText(popupPresenter.ResultTextLocalizationKey);
            base.OnOpen(popupPresenter);
        }

        private void OnNextCaseButtonClick()
        {
            Close();
        }
    }
}
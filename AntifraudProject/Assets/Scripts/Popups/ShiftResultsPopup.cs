using System;
using GameLogic.UI.Popups;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popups
{
    public interface IShiftResultsPopupPresenter : IPopupPresenter
    {
        public enum ClickType
        {
            Retry,
            Continue,
            Menu
        }
        
        public bool IsSuccess { get; }
        public string NarratorTextLocalizationKey { get; }
        public int CheckedAmount { get; }
        public int ErrorsAmount { get; }
        public int Winrate { get; }
        public Action<ClickType> OnClick { get; }
    }

    public class Success_ShiftResultsPopupPresenter : IShiftResultsPopupPresenter
    {
        public bool IsSuccess => true;
        public string NarratorTextLocalizationKey => "popups.shift_results.narrator_speech_success";
        public int CheckedAmount { get; }
        public int ErrorsAmount { get; }
        public int Winrate { get; }
        public Action<IShiftResultsPopupPresenter.ClickType> OnClick { get; }

        public Success_ShiftResultsPopupPresenter(int checkedAmount, int errorsAmount, int winrate, Action<IShiftResultsPopupPresenter.ClickType> onClick)
        {
            CheckedAmount = checkedAmount;
            ErrorsAmount = errorsAmount;
            Winrate = winrate;
            OnClick = onClick;
        }
    }
    
    public class Fail_ShiftResultsPopupPresenter : IShiftResultsPopupPresenter
    {
        public bool IsSuccess => false;
        public string NarratorTextLocalizationKey => "popups.shift_results.narrator_speech_fail";
        public int CheckedAmount { get; }
        public int ErrorsAmount { get; }
        public int Winrate { get; }
        public Action<IShiftResultsPopupPresenter.ClickType> OnClick { get; }

        public Fail_ShiftResultsPopupPresenter(int checkedAmount, int errorsAmount, int winrate, Action<IShiftResultsPopupPresenter.ClickType> onClick)
        {
            CheckedAmount = checkedAmount;
            ErrorsAmount = errorsAmount;
            Winrate = winrate;
            OnClick = onClick;
        }
    }
    
    public class ShiftResultsPopup : PopupBase<IShiftResultsPopupPresenter>
    {
        [SerializeField] private TextMeshProLocalization _narratorTextLocalizationKey;
        [SerializeField] private TMP_Text _checkedAmountText;
        [SerializeField] private TMP_Text _errorsAmountText;
        [SerializeField] private TMP_Text _winrateText;
        [SerializeField] private GameObject _successTextContainer;
        [SerializeField] private GameObject _failTextContainer;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _menuButton;

        protected override void Init()
        {
            _retryButton.onClick.AddListener(() => OnButtonClick(IShiftResultsPopupPresenter.ClickType.Retry));
            _continueButton.onClick.AddListener(() => OnButtonClick(IShiftResultsPopupPresenter.ClickType.Continue));
            _menuButton.onClick.AddListener(() => OnButtonClick(IShiftResultsPopupPresenter.ClickType.Menu));
        }

        protected override void OnOpen(IShiftResultsPopupPresenter popupPresenter)
        {
            _retryButton.gameObject.SetActive(!popupPresenter.IsSuccess);
            _continueButton.gameObject.SetActive(popupPresenter.IsSuccess);
            _menuButton.gameObject.SetActive(true);
            _successTextContainer.gameObject.SetActive(popupPresenter.IsSuccess);
            _failTextContainer.gameObject.SetActive(!popupPresenter.IsSuccess);
            _narratorTextLocalizationKey.SetText(popupPresenter.NarratorTextLocalizationKey);
            _checkedAmountText.SetText("{0}", popupPresenter.CheckedAmount);
            _errorsAmountText.SetText("{0}", popupPresenter.ErrorsAmount);
            _winrateText.SetText("{0}%", popupPresenter.CheckedAmount);
            base.OnOpen(popupPresenter);
        }

        private void OnButtonClick(IShiftResultsPopupPresenter.ClickType type)
        {
            PopupPresenter.OnClick(type);
            Close();
        }
    }
}
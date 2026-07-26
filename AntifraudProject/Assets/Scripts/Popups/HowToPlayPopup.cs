using GameLogic.UI.Popups;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Popups
{
    public interface IHowToPlayPopupPresenter : IPopupPresenter
    {
    }

    public class HowToPlayPopupPresenter : IHowToPlayPopupPresenter
    {
        
    }
    
    public class HowToPlayPopup : PopupBase<IHowToPlayPopupPresenter>
    {
        [Inject] private readonly LocalizationService _localizationService;
        
        [SerializeField] private GameObject _firstTutorialStepUnavailable;
        [SerializeField] private GameObject _secondTutorialStepUnavailable;
        [SerializeField] private GameObject _thirdTutorialStepUnavailable;
        [SerializeField] private Button _nextStepButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private TMP_Text _norratorMessageText;
        
        private int _currentStepIndex = 0;

        protected override void Init()
        {
            _nextStepButton.onClick.AddListener(OnNextStepButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        protected override void OnOpen(IHowToPlayPopupPresenter popupPresenter)
        {
            SetStep(_currentStepIndex = 0);
            
            base.OnOpen(popupPresenter);
        }

        private void OnNextStepButtonClick()
        {
            SetStep(++_currentStepIndex);
        }

        private void OnPlayButtonClick()
        {
            Close();
        }

        private void SetStep(int index)
        {
            if (index == 0)
            {
                _firstTutorialStepUnavailable.SetActive(false);
                _secondTutorialStepUnavailable.SetActive(true);
                _thirdTutorialStepUnavailable.SetActive(true);
                _nextStepButton.gameObject.SetActive(true);
                _playButton.gameObject.SetActive(false);
                _norratorMessageText.SetText(_localizationService.Get("popups.howtoplay.norrator_message1"));
            }
            else if (index == 1)
            {
                _firstTutorialStepUnavailable.SetActive(false);
                _secondTutorialStepUnavailable.SetActive(false);
                _thirdTutorialStepUnavailable.SetActive(true);
                _nextStepButton.gameObject.SetActive(true);
                _playButton.gameObject.SetActive(false);
                _norratorMessageText.SetText(_localizationService.Get("popups.howtoplay.norrator_message2"));
            }
            else if (index == 2)
            {
                _firstTutorialStepUnavailable.SetActive(false);
                _secondTutorialStepUnavailable.SetActive(false);
                _thirdTutorialStepUnavailable.SetActive(false);
                _nextStepButton.gameObject.SetActive(false);
                _playButton.gameObject.SetActive(true);
                _norratorMessageText.SetText(_localizationService.Get("popups.howtoplay.norrator_message3"));
            }
        }
    }
}
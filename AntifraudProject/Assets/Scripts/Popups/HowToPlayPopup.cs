using GameLogic.UI.Popups;
using Localization;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Button _nextStepButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProLocalization _narratorSpeechText;
        [SerializeField] private TextMeshProLocalization _tutorialText;
        [SerializeField] private GameObject[] _stepBackgrounds;

        private readonly string[] NarratorSpeechTextKeys = {
            "popups.howtoplay.norrator_message1",
            "popups.howtoplay.norrator_message2",
            "popups.howtoplay.norrator_message3"
        };
        
        private readonly string[] TutorialTextKeys =
        {
            "popups.howtoplay.card1",
            "popups.howtoplay.card2",
            "popups.howtoplay.card3"
        };

        private int _currentStepIndex = 0;

        protected override void Init()
        {
            _nextStepButton.onClick.AddListener(OnNextStepButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _skipButton.onClick.AddListener(OnSkipButtonClick);
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

        private void OnSkipButtonClick()
        {
            Close();
        }

        private void SetStep(int index)
        {
            foreach (var stepBackground in _stepBackgrounds)
                stepBackground.SetActive(false);
            _stepBackgrounds[index].SetActive(true);
            _narratorSpeechText.SetText(NarratorSpeechTextKeys[index]);
            _tutorialText.SetText(TutorialTextKeys[index]);
            _nextStepButton.gameObject.SetActive(index != 2);
            _playButton.gameObject.SetActive(index == 2);
        }
    }
}
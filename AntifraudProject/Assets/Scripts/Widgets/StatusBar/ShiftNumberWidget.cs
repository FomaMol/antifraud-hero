using Localization;
using Services;
using UniRx;
using UnityEngine;
using VContainer;

namespace Widgets.StatusBar
{
    public class ShiftNumberWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
        
        [SerializeField] private TextMeshProLocalization _shiftNumberText;
        
        public void Initialize()
        {
            _gameplayService.OnChangeShift.Subscribe(OnChangeShift);
            UpdateView();
        }

        private void UpdateView()
        {
            _shiftNumberText.SetArgs(_gameplayService.CurrentShiftIndex + 1);
        }

        private void OnChangeShift(Unit unit)
        {
            UpdateView();   
        }
    }
}
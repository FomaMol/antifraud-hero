using Localization;
using Services;
using UniRx;
using UnityEngine;
using VContainer;

namespace Widgets.StatusBar
{
    public class WinrateWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
        
        [SerializeField] private TextMeshProLocalization _winrateText;
        
        public void Initialize()
        {
            UpdateView();
            _gameplayService.OnChangeCase.Subscribe(OnChangeCase).AddTo(this);
        }

        private void OnChangeCase(Unit unit)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _winrateText.SetArgs((int)(_gameplayService.GetWinRateForCurrentShift() * 100));
        }
    }
}
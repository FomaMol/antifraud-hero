using System.Collections.Generic;
using System.Linq;
using Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets.StatusBar
{
    public class ShiftProgressWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
        
        [SerializeField] private ReusableItemsWidget _cases;
        
        public void Initialize()
        {
            _gameplayService.OnChangeCase.Subscribe(OnChangeCase);
            UpdateView();
        }

        private void OnChangeCase(Unit unit)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            void ItemInitializeLogic(int i, Image view, CaseConfig caseConfig)
            {
                if (i == _gameplayService.CurrentCaseIndex)
                {
                    view.color = Color.white;
                }
                else if (_gameplayService.TryGetCaseDecisionFromHistory(i, out var decision))
                {
                    view.color = decision == _gameplayService.GetExpectedDecisionForCase(i) ? Color.green : Color.red;
                }
                else
                {
                    view.color = Color.gray;
                }
            }
            _cases.SetItems<Image, CaseConfig>(_gameplayService.CaseConfigsInCurrentShift, ItemInitializeLogic);
        }
    }
}
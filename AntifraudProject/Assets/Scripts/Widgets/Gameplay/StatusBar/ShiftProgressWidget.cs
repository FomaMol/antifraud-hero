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
        [SerializeField] private Sprite _successCaseSprite;
        [SerializeField] private Sprite _failCaseSprite;
        [SerializeField] private Sprite _currentCaseSprite;
        [SerializeField] private Sprite _voidCaseSprite;
        
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
                    view.sprite = _currentCaseSprite;
                }
                else if (_gameplayService.TryGetCaseDecisionFromHistory(i, out var decision))
                {
                    view.sprite = decision == _gameplayService.GetExpectedDecisionForCase(i) ? _successCaseSprite : _failCaseSprite;
                }
                else
                {
                    view.sprite = _voidCaseSprite;
                }
            }
            _cases.SetItems<Image, CaseConfig>(_gameplayService.CaseConfigsInCurrentShift, ItemInitializeLogic);
        }
    }
}
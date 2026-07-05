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
            void ItemInitializeLogic(int i, Image view, bool isActive)
            {
                view.color = isActive ? Color.cyan : Color.white;
            }
            _cases.SetItems<Image, bool>(GetCaseStatuses().ToList(), ItemInitializeLogic);
        }

        private IEnumerable<bool> GetCaseStatuses()
        {
            for (int i = 0; i < _gameplayService.TotalCaseCountInCurrentShift; ++i)
                yield return i <= _gameplayService.CurrentCaseIndex;
        }
    }
}
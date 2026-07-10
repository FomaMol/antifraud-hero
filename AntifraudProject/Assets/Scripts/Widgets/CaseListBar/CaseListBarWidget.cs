using Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets.CaseListBar
{
    public class CaseListBarWidget : MonoBehaviour
    {
        [Inject] private readonly GameplayService _gameplayService;
        
        [SerializeField] private ReusableItemsWidget _cases;
        [SerializeField] private ScrollRect _scrollRect;

        public void Initialize()
        {
            _gameplayService.OnChangeCase.Subscribe(OnCaseChange).AddTo(this); // TODO: проверить что везде так
            UpdateView();
        }

        private void OnCaseChange(Unit unit)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            void InitializeLogic(int index, CaseInfoWidget caseInfoWidget, CaseConfig caseConfig)
            {
                caseInfoWidget.Initialize(index + 1, caseConfig.TypeText, _gameplayService.CurrentCaseIndex == index);
            }
            _cases.SetItems<CaseInfoWidget, CaseConfig>(_gameplayService.CaseConfigsInCurrentShift, InitializeLogic);
            SnapTo(_gameplayService.CurrentCaseIndex);
        }
        
        private void SnapTo(int index)
        {
            Canvas.ForceUpdateCanvases();

            var contentPos = (Vector2)_scrollRect.transform.InverseTransformPoint(_scrollRect.content.position);
            var targetPos = (Vector2)_scrollRect.transform.InverseTransformPoint(_cases.GetItem(index).position);

            var diff = contentPos - targetPos;

            // центрируем элемент во вьюпорте
            if (_scrollRect.horizontal)
                diff.x += _scrollRect.viewport.rect.width * 0.5f;
            if (_scrollRect.vertical)
                diff.y -= _scrollRect.viewport.rect.height * 0.5f;

            diff.x = _scrollRect.horizontal ? diff.x : 0f;
            diff.y = _scrollRect.vertical ? diff.y : 0f;

            _scrollRect.content.anchoredPosition = diff;
            _scrollRect.velocity = Vector2.zero;
        }

        private void Start()
        {
            Initialize();
        }
    }
}
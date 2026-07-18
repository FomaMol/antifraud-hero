using System;
using Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets
{
    public class ToolTabsWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;

        [SerializeField] private Button _linkButton;
        [SerializeField] private Transform _linkSelectedBackground;
        [SerializeField] private Transform _linkUnavaliableForeground;
        [SerializeField] private Transform _linkAlertPoint;
        [Space] 
        [SerializeField] private Button _contactButton;
        [SerializeField] private Transform _contactSelectedBackground;
        [SerializeField] private Transform _contactUnavaliableForeground;
        [SerializeField] private Transform _contactAlertPoint;
        [Space]
        [SerializeField] private Button _dbButton;
        [SerializeField] private Transform _dbSelectedBackground;
        [SerializeField] private Transform _dbUnavaliableForeground;
        [SerializeField] private Transform _dbAlertPoint;

        public ToolType SelectedTool { get; private set; }

        public void Init(Action<ToolType> onClick)
        {
            _gameplayService.OnChangeShift.Subscribe(OnChangeShift).AddTo(this);
            _gameplayService.OnChangeCase.Subscribe(OnChangeCase).AddTo(this);
            
            void HandleClick(ToolType toolType)
            {
                SelectedTool = toolType;
                UpdateView();
                onClick(toolType);
            }

            _linkButton.onClick.AddListener(() => HandleClick(ToolType.Link));
            _contactButton.onClick.AddListener(() => HandleClick(ToolType.Contact));
            _dbButton.onClick.AddListener(() => HandleClick(ToolType.Db));

            HideAllAlertPoints();
            UpdateView();

            HandleClick(ToolType.Link);
        }

        private void OnChangeShift(Unit _)
        {
            UpdateView();
        }

        private void OnChangeCase(Unit _)
        {
            HideAllAlertPoints();
        }

        private void UpdateView()
        {
            _linkSelectedBackground.gameObject.SetActive(false);
            _contactSelectedBackground.gameObject.SetActive(false);
            _dbSelectedBackground.gameObject.SetActive(false);
            switch (SelectedTool)
            {
                case ToolType.Link:
                    _linkSelectedBackground.gameObject.SetActive(true);
                    break;
                case ToolType.Contact:
                    _contactSelectedBackground.gameObject.SetActive(true);
                    break;
                case ToolType.Db:
                    _dbSelectedBackground.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(SelectedTool), SelectedTool, null);
            }

            _linkButton.enabled = _gameplayService.IsToolAvailable(ToolType.Link);
            _linkUnavaliableForeground.gameObject.SetActive(!_gameplayService.IsToolAvailable(ToolType.Link));
            _contactButton.enabled = _gameplayService.IsToolAvailable(ToolType.Contact);
            _contactUnavaliableForeground.gameObject.SetActive(!_gameplayService.IsToolAvailable(ToolType.Contact));
            _dbButton.enabled = _gameplayService.IsToolAvailable(ToolType.Db);
            _dbUnavaliableForeground.gameObject.SetActive(!_gameplayService.IsToolAvailable(ToolType.Db));
            
            SetAlertPointVisible(SelectedTool, false);
        }

        public void TryShowAlertPoint(ToolType toolType)
        {
            if (toolType != SelectedTool)
                SetAlertPointVisible(toolType, true);
        }

        private void HideAllAlertPoints()
        {
            SetAlertPointVisible(ToolType.Link, false);
            SetAlertPointVisible(ToolType.Contact, false);
            SetAlertPointVisible(ToolType.Db, false);
        }
        
        private void SetAlertPointVisible(ToolType toolType, bool visible)
        {
            switch (toolType)
            {
                case ToolType.Link:
                    _linkAlertPoint.gameObject.SetActive(visible);
                    break;
                case ToolType.Contact:
                    _contactAlertPoint.gameObject.SetActive(visible);
                    break;
                case ToolType.Db:
                    _dbAlertPoint.gameObject.SetActive(visible);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
            }
        }
    }
}
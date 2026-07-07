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
        [Space] 
        [SerializeField] private Button _contactButton;
        [SerializeField] private Transform _contactSelectedBackground;
        [SerializeField] private Transform _contactUnavaliableForeground;
        [Space]
        [SerializeField] private Button _dbButton;
        [SerializeField] private Transform _dbSelectedBackground;
        [SerializeField] private Transform _dbUnavaliableForeground;

        public ToolType SelectedTool { get; private set; }

        public void Init(Action<ToolType> onClick)
        {
            _gameplayService.OnChangeShift.Subscribe(OnChangeShift);
            
            void HandleClick(ToolType toolType)
            {
                SelectedTool = toolType;
                UpdateView();
                onClick(toolType);
            }

            _linkButton.onClick.AddListener(() => HandleClick(ToolType.Link));
            _contactButton.onClick.AddListener(() => HandleClick(ToolType.Contact));
            _dbButton.onClick.AddListener(() => HandleClick(ToolType.Db));

            UpdateView();

            HandleClick(ToolType.Link);
        }

        private void OnChangeShift(Unit _)
        {
            UpdateView();
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
        }
    }
}
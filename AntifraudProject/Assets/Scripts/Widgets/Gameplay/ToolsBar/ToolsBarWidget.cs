using System;
using Localization;
using Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Widgets
{
    public enum ToolType
    {
        Link,
        Contact,
        Db
    }

    public class ToolsBarWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
         
        [SerializeField] private ToolTabsWidget _toolTabsWidget;
        [SerializeField] private ToolWidget _linkToolSection;
        [SerializeField] private ToolWidget _contactToolSection;
        [SerializeField] private ToolWidget _dbToolSection;

        public void OpenTab(ToolType toolType)
        {
            _linkToolSection.gameObject.SetActive(false);
            _contactToolSection.gameObject.SetActive(false);
            _dbToolSection.gameObject.SetActive(false);
            switch (toolType)
            {
                case ToolType.Link:
                    _linkToolSection.gameObject.SetActive(true);
                    break;
                case ToolType.Contact:
                    _contactToolSection.gameObject.SetActive(true);
                    break;
                case ToolType.Db:
                    _dbToolSection.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
            }
        }

        public void InsertData(ToolType toolType, string data)
        {
            switch (toolType)
            {
                case ToolType.Link:
                    _linkToolSection.InsertData(data);
                    break;
                case ToolType.Contact:
                    _contactToolSection.InsertData(data);
                    break;
                case ToolType.Db:
                    _dbToolSection.InsertData(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
            }
            _toolTabsWidget.TryShowAlertPoint(toolType);
        }

        public void CleanUp()
        {
            _linkToolSection.CleanUp();
            _contactToolSection.CleanUp();
            _dbToolSection.CleanUp();
        }

        public void Initialize()
        {
            _gameplayService.OnChangeCase.Subscribe(OnChangeCase).AddTo(this);
            
            _linkToolSection.Initialize();
            _contactToolSection.Initialize();
            _dbToolSection.Initialize();

            _toolTabsWidget.Init(OpenTab);
        }

        private void OnChangeCase(Unit unit)
        {
            CleanUp();
        }

        private void Start()
        {
            Initialize();
        }
    }
}
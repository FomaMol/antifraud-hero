using System;
using Localization;
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
        [SerializeField] private ToolTabsWidget _toolTabsWidget;
        [SerializeField] private LinkToolWidget _linkToolSection;
        [SerializeField] private ContactToolWidget _contactToolSection;
        [SerializeField] private DbToolWidget _dbToolSection;

        [Inject] private LocalizationService _localizationService;

        public LocalizationService LocalizationService => _localizationService;

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

        public void Initialize()
        {
            _linkToolSection.Init();
            _contactToolSection.Init();
            _dbToolSection.Init();

            _toolTabsWidget.Init(OpenTab);

            Debug.Log(_localizationService.Get("hello.world"));
            _localizationService.ChangeLanguage("en");
            Debug.Log(_localizationService.Get("hello.world"));
        }

        private void Start()
        {
            Initialize();
        }
    }
}
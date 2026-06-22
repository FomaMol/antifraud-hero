using System;
using UnityEngine;
using UnityEngine.UI;

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

        public void Init()
        {
            _linkToolSection.Init();
            _contactToolSection.Init();
            _dbToolSection.Init();

            _toolTabsWidget.Init(OpenTab);
        }

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

        private void Awake()
        {
            Init();
        }
    }
}
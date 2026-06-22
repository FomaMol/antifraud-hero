using System;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{
    public class ToolTabsWidget : Widget
    {
        [SerializeField] private Button _linkButton;
        [SerializeField] private Transform _linkSelectedBackground;
        [Space]
        [SerializeField] private Button _contactButton;
        [SerializeField] private Transform _contactSelectedBackground;
        [Space]
        [SerializeField] private Button _dbButton;
        [SerializeField] private Transform _dbSelectedBackground;
        
        public void Init(Action<ToolType> onClick)
        {
            void HandleClick(ToolType toolType)
            {
                _linkSelectedBackground.gameObject.SetActive(false);
                _contactSelectedBackground.gameObject.SetActive(false);
                _dbSelectedBackground.gameObject.SetActive(false);
                switch (toolType)
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
                        throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
                }
                onClick(toolType);
            }
            
            _linkButton.onClick.AddListener(() => HandleClick(ToolType.Link));   
            _contactButton.onClick.AddListener(() => HandleClick(ToolType.Contact));   
            _dbButton.onClick.AddListener(() => HandleClick(ToolType.Db));
            
            HandleClick(ToolType.Link);
        }
    }
}
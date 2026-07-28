using GameLogic.UI.Popups;
using Localization;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Popups
{
    public interface INewToolPopupPresenter : IPopupPresenter
    {
        public string ToolNameLocalizationKey { get; }
        public string ToolDescriptionLocalizationKey { get; }
    }

    public class LinkTool_NewToolPopupPresenter : INewToolPopupPresenter
    {
        public string ToolNameLocalizationKey => "popups.new_tool.link_scanner_name";
        public string ToolDescriptionLocalizationKey => "popups.new_tool.link_scanner_description";
    }

    public class ContactTool_NewToolPopupPresenter : INewToolPopupPresenter
    {
        public string ToolNameLocalizationKey => "popups.new_tool.contact_scanner_name";
        public string ToolDescriptionLocalizationKey => "popups.new_tool.contact_scanner_description";
    }

    public class DbTool_NewToolPopupPresenter : INewToolPopupPresenter
    {
        public string ToolNameLocalizationKey => "popups.new_tool.db_scanner_name";
        public string ToolDescriptionLocalizationKey => "popups.new_tool.db_scanner_description";
    }
    
    public class NewToolPopup : PopupBase<INewToolPopupPresenter>
    {
        [Inject] private readonly LocalizationService _localizationService;

        [SerializeField] private TextMeshProLocalization _toolName;
        [SerializeField] private TextMeshProLocalization _toolDescription;
        [SerializeField] private Button _closeButton;

        protected override void Init()
        {
            base.Init();
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        protected override void OnOpen(INewToolPopupPresenter popupPresenter)
        {
            _toolName.SetText(popupPresenter.ToolNameLocalizationKey);
            _toolDescription.SetText(popupPresenter.ToolDescriptionLocalizationKey);
            base.OnOpen(popupPresenter);
        }

        private void OnCloseButtonClick()
        {
            Close();
        }
    }
}
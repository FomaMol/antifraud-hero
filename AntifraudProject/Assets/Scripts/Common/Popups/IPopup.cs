using System;

namespace GameLogic.UI.Popups
{
    public interface IPopup
    {
        public Type PopupInterfaceType { get; }
        public void Init(PopupService popupService);
        public void OnOpen(IPopupPresenter popupPresenter);
        public void OnUpdate(float dt);
        public void OnClose();
        public void OnBackgroundButtonClick();
    }
}
using System;
using UnityEngine;

namespace GameLogic.UI.Popups
{
    public abstract class PopupBase<TPopupPresenterInterface> : MonoBehaviour, IPopup where TPopupPresenterInterface : IPopupPresenter
    {
        [SerializeField] private PopupWindowAnimation _popupWindowAnimation;
        
        protected PopupService PopupService { get; private set; }
        protected TPopupPresenterInterface PopupPresenter { get; private set; }
     
        public Type PopupInterfaceType => typeof(TPopupPresenterInterface);
        
        void IPopup.Init(PopupService popupService)
        {
            PopupService = popupService;
            Init();
        }

        void IPopup.OnOpen(IPopupPresenter popupPresenter)
        {
            PopupPresenter = (TPopupPresenterInterface)popupPresenter;
            OnOpen(PopupPresenter);
        }

        void IPopup.OnUpdate(float dt)
        {
            OnUpdate(dt);
        }

        void IPopup.OnClose()
        {
            OnClose();
            PopupPresenter = default;
        }

        void IPopup.OnBackgroundButtonClick()
        {
            OnBackgroundButtonClick();
        }

        protected virtual void Init()
        {
            
        }

        protected virtual void OnOpen(TPopupPresenterInterface popupPresenter)
        {
            gameObject.SetActive(true);
            _popupWindowAnimation?.OnOpen();
        }

        public virtual void OnUpdate(float dt)
        {
            
        }
        
        protected virtual void OnClose()
        {
            _popupWindowAnimation?.OnClose();
            gameObject.SetActive(false);
        }

        protected virtual void OnBackgroundButtonClick()
        {
            Close();
        }

        protected virtual bool NeedToPlayCloseSound() => true;

        public void Close()
        {
            PopupService.ClosePopup<TPopupPresenterInterface>();
        }
    }
}
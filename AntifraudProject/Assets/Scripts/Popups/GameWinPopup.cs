using System;
using GameLogic.UI.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace Popups
{
    public interface IGameWinPopupPresenter : IPopupPresenter
    {
    }

    public class GameWinPopupPresenter : IGameWinPopupPresenter
    {
    }
    
    public class GameWinPopup : PopupBase<IGameWinPopupPresenter>
    {
        [SerializeField] private Button _closeButton;

        protected override void Init()
        {
            _closeButton.onClick.AddListener(Close);
        }
    }
}
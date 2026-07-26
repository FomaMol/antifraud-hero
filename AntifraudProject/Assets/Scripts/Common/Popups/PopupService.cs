using System;
using System.Collections.Generic;
using System.Linq;
using Common.EventBus;
using Common.Extensions;
using common.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace GameLogic.UI.Popups
{
    public struct OpenedPopupData
    {
        public readonly IPopup Popup;
        public readonly bool WithBackGround;

        public OpenedPopupData(IPopup popup, bool withBackGround)
        {
            Popup = popup;
            WithBackGround = withBackGround;
        }
    }
    
    public class PopupService : MonoBehaviour, IInitializable, ITickable
    {
        [Inject] private readonly IObjectResolver _container;

        [SerializeField] private MonoBehaviour[] _popups;
        [SerializeField] private Image _background;
        [SerializeField] private Button _backgroundButton;
        [SerializeField] private float _backgroundFade = 0.75f;
        
        private readonly CompositeDisposable _disposable = new();
        private readonly Dictionary<Type, IPopup> _registeredPopupsLookup = new();
        private readonly OrderedDictionary<Type, OpenedPopupData> _openedPopups = new();
        private readonly Queue<Func<UniTask>> _popupOpenQueue = new();
        
        private Tween _backgroundTween;
        
        public bool HasActivePopups => _openedPopups.Count > 0;

        public void Initialize()
        {
            _backgroundButton.onClick.AddListener(OnBackgroundButtonClick);
            
            foreach (var registeredPopup in _popups)
            {
                _container.InjectGameObject(registeredPopup.gameObject);

                if (!registeredPopup.TryGetComponent<IPopup>(out var popupInterface))
                    throw new Exception($"Could not find popup {registeredPopup.GetType().Name}"); 
                
                _registeredPopupsLookup.Add(popupInterface.PopupInterfaceType, popupInterface);
                popupInterface.Init(this);
                registeredPopup.gameObject.SetActive(false);
            }
    
            _background.gameObject.SetActive(false);
        }

        public async UniTask OpenPopup<TPopupDataInterface>(TPopupDataInterface popupData, bool queued = false, bool withBackGround = true) where TPopupDataInterface : IPopupPresenter
        {
            if (_openedPopups.ContainsKey(typeof(TPopupDataInterface)))
            {
                Debug.LogWarning($"Popup associated with '{typeof(TPopupDataInterface).Name}' already opened");
                return;
            }

            if (!_registeredPopupsLookup.TryGetValue(typeof(TPopupDataInterface), out var popup))
                throw new Exception($"Could not find popup {typeof(TPopupDataInterface).Name}");

            if (queued && HasActivePopups)
            {
                _popupOpenQueue.Enqueue(() => OpenPopup(popupData, false));
                return;
            }
            
            popup.OnOpen(popupData);
            _openedPopups.Add(typeof(TPopupDataInterface), new OpenedPopupData(popup, withBackGround));
            if (withBackGround)
            {
                await StartBackgroundFadeIn();
            }
        }

        public void ClosePopup<TPopupDataInterface>() where TPopupDataInterface : IPopupPresenter
        {
            if (!_openedPopups.TryGetValue(typeof(TPopupDataInterface), out var popup))
                throw new Exception($"Could not find popup associated with '{typeof(TPopupDataInterface).Name}'");
            
            _openedPopups.Remove(typeof(TPopupDataInterface));
            popup.Popup.OnClose();
            //
            if (!_openedPopups.Values.Any(it => it.WithBackGround))
            {
                if (_popupOpenQueue.Count > 0)
                {
                    OpenQueuedPopup();
                }
                else
                {
                    StartBackgroundFadeOut().Forget();
                }
            }
        }

        void ITickable.Tick()
        {
            foreach (var keyValuePair in _openedPopups.Values)
            {
                keyValuePair.Popup.OnUpdate(Time.deltaTime);
            }
        }

        public void CloseAllPopups()
        {
            _openedPopups.ForEach(it => it.Value.Popup.OnClose());
            _openedPopups.Clear();
            StartBackgroundFadeOut().Forget();
            _popupOpenQueue.Clear();
        }
        
        public bool IsPopupOpened<TPopupDataInterface>() where TPopupDataInterface : IPopupPresenter
        {
            return _openedPopups.ContainsKey(typeof(TPopupDataInterface));
        }

        private void OpenQueuedPopup()
        {
            _popupOpenQueue.Dequeue()().Forget();
        }

        private async UniTask StartBackgroundFadeIn()
        {
            _background.transform.SetAsFirstSibling();
            _background.gameObject.SetActive(true);
            _backgroundTween?.Kill();
            _backgroundTween = _background.DOFade(_backgroundFade, 0.2f);
            await _backgroundTween.AsyncWaitForCompletion();
        }

        private async UniTask StartBackgroundFadeOut()
        {
            _backgroundTween?.Kill();
            _backgroundTween = _background.DOFade(0.0f, 0.2f).OnComplete(() => _background.gameObject.SetActive(false));
            await _backgroundTween.AsyncWaitForCompletion();
        }

        private void OnBackgroundButtonClick()
        {
            var openedPopupData = _openedPopups.Last().Value;
            if (openedPopupData.WithBackGround)
                openedPopupData.Popup.OnBackgroundButtonClick();
        }

        public void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}

using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.UI.Popups
{
    public class PopupWindowAnimation : MonoBehaviour
    {
        [CanBeNull]
        [SerializeField] private Image _window;

        private Sequence _animation;

        public void OnOpen()
        {
            _animation?.Kill();
            _animation = DOTween.Sequence();
            _animation.SetUpdate(true);

            transform.localScale = Vector3.zero;

            gameObject.SetActive(true);
            _animation.Join(transform.DOScale(Vector3.one, 0.3f));
            if (_window != null) 
                _animation.Join(_window.DOFade(1f, 0.1f));
            _animation.SetEase(Ease.OutCubic);
        }

        public void OnClose()
        {
            _animation?.Kill();
            _animation = DOTween.Sequence();
            _animation.SetUpdate(true);

            if (_window != null)
                _animation.Join(_window.DOFade(0f, 0.1f));
            
            _animation.Join(transform.DOScale(Vector3.zero, 0.2f));
            _animation.onComplete += delegate
            {
                gameObject.SetActive(false);
            };
            _animation.SetEase(Ease.InOutCirc);
        }
    }
}
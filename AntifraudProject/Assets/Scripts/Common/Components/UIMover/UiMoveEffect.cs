using DG.Tweening;
using UnityEngine;

namespace Common.Components.UIMover
{
    [RequireComponent(typeof(RectTransform))]
    public class UiMoveEffect : MonoBehaviour
    {
        [SerializeField] private bool _isPlayOnAwake = true;
        [SerializeField] private Vector2 _offset = new Vector2(0f, -100f);
        [SerializeField] [Range(0.1f, 10f)] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        private RectTransform _rectTransform;
        private Vector2 _sourcePosition;
        private Tween _tween;
        private bool _isPlaying;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            _sourcePosition = _rectTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            if (_isPlayOnAwake)
                StartMoving();
        }

        private void OnDisable()
        {
            StopMoving();
        }

        public void StartMoving()
        {
            if (_isPlaying)
                StopMoving();

            _rectTransform.anchoredPosition = _sourcePosition;
            _tween = _rectTransform
                .DOAnchorPos(_sourcePosition + _offset, _duration * 0.5f)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);

            _isPlaying = true;
        }

        public void StopMoving()
        {
            if (!_isPlaying)
                return;

            _tween?.Kill();
            _tween = null;
            _rectTransform.anchoredPosition = _sourcePosition;
            _isPlaying = false;
        }

        private void OnDestroy()
        {
            StopMoving();
        }
    }
}

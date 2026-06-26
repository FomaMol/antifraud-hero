using System;
using UnityEngine;
using DG.Tweening;

namespace Common.Components.UIPulser
{
    public class UiPulseEffect : MonoBehaviour
    {
        [SerializeField] private bool _isPlayOnAwake;
        [SerializeField] [Range(1, 100)] private float _maxScale = 1.2f;
        [SerializeField] [Range(0.2f, 10)] private float _duration = 0.5f;
        [SerializeField] [Range(1, 100)] private int _pulseCount = 3;

        public event Action OnComplete;

        private Sequence _animationSequence;
        private Transform _transform;
        private Vector3 _sourceScale;

        private bool _isPlaying;

        private void Awake()
        {
            _transform = transform;
            _sourceScale = _transform.localScale;
        }

        private void Start()
        {
            if (_isPlayOnAwake)
                StartPulsing();
        }

        public void StartPulsing()
        {
            if (_isPlaying)
                StopPulsing();

            var onePulseDuration = _duration / _pulseCount;
            _animationSequence = DOTween.Sequence()
                .Append(_transform.DOScale(_maxScale, onePulseDuration * 0.5f))
                .Append(_transform.DOScale(_sourceScale, onePulseDuration * 0.5f))
                .SetLoops(_pulseCount)
                .SetLink(gameObject)
                .OnKill(() =>
                {
                    _transform.localScale = _sourceScale;
                    _isPlaying = false;

                    OnComplete?.Invoke();
                });

            _isPlaying = true;
        }

        public void StopPulsing()
        {
            if (!_isPlaying)
                return;

            _animationSequence.Complete();
            DOTween.Kill(_animationSequence);

            _transform.localScale = _sourceScale;
            _isPlaying = false;
        }

        private void OnDestroy()
        {
            StopPulsing();
        }
    }
}
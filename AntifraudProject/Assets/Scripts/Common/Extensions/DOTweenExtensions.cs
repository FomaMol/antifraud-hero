using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.Extensions
{
    public static class DOTweenExtensions
    {
        public static async UniTask CancelWithCancellationToken(
            this Sequence sequence,
            CancellationToken token,
            bool complete = true)
        {
            while (!token.IsCancellationRequested && (sequence.active || sequence.intId != -999))
            {
                await UniTask.Yield();
            }

            if (token.IsCancellationRequested && !sequence.IsComplete())
            {
                sequence.Kill(complete);
            }

            return;
        }

        /// <summary>
        /// Двигает объект к цели по кривой Безье
        /// </summary>
        public static Tween DoArcMove(this Transform target,
            Vector3 targetPoint,
            Vector3 arcOffsetStart,
            Vector3 arcOffsetEnd,
            float duration,
            Ease ease = Ease.Linear)
        {
            var startPoint = target.position;
            var inPoint = startPoint + arcOffsetStart;
            var outPoint = startPoint + arcOffsetEnd;
            var path = new[] { targetPoint, inPoint, outPoint };
            var previousPos = startPoint;
            return target.DOPath(path, duration, PathType.CubicBezier)
                .SetEase(ease)
                .OnUpdate(() =>
                {
                    var currentPos = target.position;
                    var direction = currentPos - previousPos;
                    previousPos = currentPos;

                    if (direction.sqrMagnitude > 0.0001f)
                    {
                        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        target.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
                    }
                });
        }
        
        public static void AddScaleAnimation(this Button button, float downScale = 0.9f, float duration = 0.1f)
        {
            var eventTrigger = button.gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = button.gameObject.AddComponent<EventTrigger>();
            }

            var originalScale = button.transform.localScale;
            Tweener currentTween = null;

            void KillTween()
            {
                currentTween?.Kill();
                currentTween = null;
            }

            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener((data) =>
            {
                if (button != null && button.interactable)
                {
                    KillTween();
                    currentTween = button.transform.DOScale(originalScale * downScale, duration)
                        .SetEase(Ease.OutQuad)
                        .SetLink(button.gameObject);
                }
            });

            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener((data) =>
            {
                if (button != null)
                {
                    KillTween();
                    currentTween = button.transform.DOScale(originalScale, duration)
                        .SetEase(Ease.OutQuad)
                        .SetLink(button.gameObject);
                }
            });

            eventTrigger.triggers.Add(pointerDown);
            eventTrigger.triggers.Add(pointerUp);
        }
    }
}
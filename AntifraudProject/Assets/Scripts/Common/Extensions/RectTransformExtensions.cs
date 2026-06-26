using UnityEngine;

namespace Common.Extensions
{
    public static class RectTransformExtensions
    {
        public static bool OverlapsInWorld(this RectTransform a, RectTransform b) {
            return a.GetWorldRect().Overlaps(b.GetWorldRect());
        }

        public static bool OverlapsInWorld(this RectTransform a, RectTransform b, bool allowInverse) {
            return a.GetWorldRect().Overlaps(b.GetWorldRect(), allowInverse);
        }

        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            var localRect = rectTransform.rect;
 
            return new Rect
            {
                min = rectTransform.TransformPoint(localRect.min),
                max = rectTransform.TransformPoint(localRect.max)
            };
        }
        
        public static Vector2 ToCanvasLocalPosition(this RectTransform uiElement, Transform target)
        {
            var myCanvas = uiElement.GetComponentInParent<Canvas>();
            var canvasRect = myCanvas.GetComponent<RectTransform>();
            
            Vector2 screenPos;
    
            // Определяем, что за объект - UI или мировой
            if (target is RectTransform targetRect)
            {
                // Это UI элемент - получаем его экранную позицию
                var targetCanvas = target.GetComponentInParent<Canvas>();
                screenPos = RectTransformUtility.WorldToScreenPoint(targetCanvas.worldCamera, targetRect.position);
            }
            else
            {
                // Это мировой объект (SpriteRenderer и т.д.)
                screenPos = Camera.main.WorldToScreenPoint(target.position);
            }
    
            // Конвертируем экранную позицию в локальные координаты нашего Canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                myCanvas.worldCamera,
                out var localPos
            );
    
            return localPos;
        }
        
        public static Vector2 GetClampedAnchoredPos(this RectTransform canvasRect, Vector3 targetWorldPosition)
        {
            var screenPos = Camera.main!.WorldToScreenPoint(targetWorldPosition);
    
            if (screenPos.z < 0) 
                screenPos *= -1;

            var canvasSize = canvasRect.rect.size;
            var center = canvasSize * 0.5f;

            var viewportPos = Camera.main!.ScreenToViewportPoint(screenPos); 
            var posInCanvas = new Vector2(
                (viewportPos.x * canvasSize.x) - center.x,
                (viewportPos.y * canvasSize.y) - center.y
            );

            var kX = Mathf.Abs(center.x / (Mathf.Abs(posInCanvas.x) < 0.001f ? 0.001f : posInCanvas.x));
            var kY = Mathf.Abs(center.y / (Mathf.Abs(posInCanvas.y) < 0.001f ? 0.001f : posInCanvas.y));

            var k = Mathf.Min(kX, kY);

            return posInCanvas * k;
        }
    }
}
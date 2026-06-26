using UnityEngine;
using UnityEngine.UI;

namespace Common.Extensions
{
    public static class ScrollRectExtensions
    {
        public static Vector2 CalculateNormalizedPositionToPoint(this ScrollRect scrollView, Vector2 point)
        {
            Vector2 contentSize = scrollView.content.rect.size;
            Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
            Vector2 contentScale = scrollView.content.localScale;
        
            contentSize.Scale(contentScale);
            point.Scale(contentScale);
        
            Vector2 scrollPosition = scrollView.normalizedPosition;
            if (scrollView.horizontal && contentSize.x > viewportSize.x)
                scrollPosition.x = Mathf.Clamp01((point.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
            if (scrollView.vertical && contentSize.y > viewportSize.y)
                scrollPosition.y = Mathf.Clamp01((point.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));
        
            return scrollPosition;
        }
        
        public static Vector2 CalculateNormalizedPositionToItem(this ScrollRect scrollView, RectTransform item)
        {
            Vector2 itemCenterPoint = scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));
        
            Vector2 contentSizeOffset = scrollView.content.rect.size;
            contentSizeOffset.Scale(scrollView.content.pivot);
        
            return scrollView.CalculateNormalizedPositionToPoint(itemCenterPoint + contentSizeOffset);
        }
    }
}
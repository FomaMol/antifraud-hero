using UnityEngine;

namespace Common.Components
{
    public class CanvasSafeAreaHandler : MonoBehaviour
    {
        [SerializeField] private Vector2 offsetMin = Vector2.zero;
        [SerializeField] private Vector2 offsetMax = Vector2.zero;

        private void Awake()
        {
            var bottomSafeArea = Screen.safeArea.y;
            var topSafeArea = Screen.height - (bottomSafeArea + Screen.safeArea.height);

            if (topSafeArea <= 0 || !TryGetComponent(out RectTransform rectTransform))
                return;

            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }
    }
}
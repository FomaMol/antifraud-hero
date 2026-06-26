using UnityEngine;

namespace Common.SafeArea
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private bool _enableTopSafeArea = true;
        [SerializeField] private bool _enableBottomSafeArea = true;
        [SerializeField] private bool _alwaysUpdate = false;

        private RectTransform _rectTransform = null;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            SetAnchors();
        }

        private void Update()
        {
            if (_alwaysUpdate)
            {
                SetAnchors();
            }
        }

        private void SetAnchors()
        {
            var safeArea = Screen.safeArea;
            if (_enableBottomSafeArea)
            {
                _rectTransform.anchorMin = new Vector2(safeArea.xMin / Screen.width, safeArea.yMin / Screen.height);
            }
            if (_enableTopSafeArea)
            {
                _rectTransform.anchorMax = new Vector2(safeArea.xMax / Screen.width, safeArea.yMax / Screen.height);
            }
        }
    }
}
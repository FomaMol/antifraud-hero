using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Widgets
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPLinkHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Camera uiCamera; // null для Screen Space - Overlay
        public TMP_Text Text { get; private set; }

        public event System.Action<string, string> OnLinkClicked;

        private void Awake() => Text = GetComponent<TMP_Text>();

        public void OnPointerDown(PointerEventData eventData)
        {
            var linkIndex = TMP_TextUtilities.FindIntersectingLink(Text, eventData.position, uiCamera);
            if (linkIndex == -1) return;

            var linkInfo = Text.textInfo.linkInfo[linkIndex];
            var id = linkInfo.GetLinkID();
            var linkText = linkInfo.GetLinkText();
            OnLinkClicked?.Invoke(id, linkText);
        }
    }
}
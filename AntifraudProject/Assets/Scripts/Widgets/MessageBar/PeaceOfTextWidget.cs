using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{
    public class PeaceOfTextWidget : Widget
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _informationTypeText;
        [SerializeField] private TMP_Text _text;

        public void Initialize(string type, string text, Action<(string dataType, string text)> onClick)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onClick?.Invoke((type, text)));
            _informationTypeText.SetText(type);
            _text.SetText(text);
        }
    }
}
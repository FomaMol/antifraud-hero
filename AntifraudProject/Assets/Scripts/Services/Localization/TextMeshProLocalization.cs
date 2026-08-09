using System;
using Common.Extensions;
using DI;
using TMPro;
using UnityEngine;
using VContainer;

namespace Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public partial class TextMeshProLocalization : MonoBehaviour
    {
        [Inject] private LocalizationService _localization;

        [SerializeField] private string _localizationId;
        [SerializeField] private Color _accentColor;

        private TMP_Text _text;
        private object[] _args;

        public string LocalizationId
        {
            get => _localizationId;
            set
            {
                _localizationId = value;
                _args = null;
                UpdateText();
            }
        }
        public TMP_Text TextComponent => _text ??= GetComponent<TextMeshProUGUI>();

        private void Start()
        {
            _localization.OnLanguageChanged += UpdateText;

            if (_localizationId == string.Empty) return;
            UpdateText();
        }

        public void SetText(string localizationId)
        {
            _localizationId = localizationId;
            _args = null;
            UpdateText();
        }

        public void SetTextFormatted(string localizationId, params object[] args)
        {
            _localizationId = localizationId;
            _args = args;
            UpdateText();
        }

        public void SetArgs(params object[] args)
        {
            _args = args;
            UpdateText();
        }

        public void SetTextFormatted(string localizationId, object arg) => SetTextFormatted(localizationId, new[] { arg });
        public void SetTextFormatted(LocalizableText localizableText) => SetTextFormatted(localizableText.Id, localizableText.Args);
    
        private void UpdateText()
        {
            if (string.IsNullOrEmpty(_localizationId))
            {
                TextComponent.SetText("");
                return;
            }

            var colorHex = $"#{ColorUtility.ToHtmlStringRGBA(_accentColor)}";

            if (_args == null || _args.Length == 0)
            {
                TextComponent.SetText(_localization.Get(_localizationId).FormatColored(colorHex));
                return;
            }

            TextComponent.SetText(string.Format(_localization.Get(_localizationId), _args).FormatColored(colorHex));
        }

        private void OnDestroy()
        {
            _localization.OnLanguageChanged -= UpdateText;
        }
    }
}

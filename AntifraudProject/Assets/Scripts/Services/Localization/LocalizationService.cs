using System;
using System.Globalization;
using Services;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;
using VContainer.Unity;

namespace Localization
{
    public class LocalizationService : IInitializable
    {
        private const string DEFAULT_LANGUAGE = "en";

        private LocalizationConfig _config;
        [Inject] private PersistingService _persistingService;

        public string Language => _persistingService.CurrentLanguage == string.Empty ? DEFAULT_LANGUAGE : _persistingService.CurrentLanguage;
        public event Action OnLanguageChanged;

        public void Initialize()
        {
            _config = new LocalizationConfig();
        }

        public void ChangeLanguage(string language)
        {
            Assert.IsTrue(language == "en" || language == "ru");
            _persistingService.SetLanguage(language);
            OnLanguageChanged?.Invoke();
        }

        public string Get(string localizationId)
        {
            if (!_config.Table.ContainsKey(localizationId))
            {
                Debug.LogWarning($"No localization for key: {localizationId}");
                return localizationId;
            }

            var localizations = _config.Table[localizationId];
            var language = Language;

            if (localizations.ContainsKey(language)) 
                return _config.Table[localizationId][language];

            Debug.LogWarning($"Unsupported language: {language}");
            return localizations[DEFAULT_LANGUAGE];
        }

        public bool HasLocalization(string localizationId)
        {
            return _config.Table.ContainsKey(localizationId);
        }

        public string FormatTime(DateTime time)
        {
            var format = Language == "ru" ? "HH:mm" : "h:mm tt";
            return time.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}

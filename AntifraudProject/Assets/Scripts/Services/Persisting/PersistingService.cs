using System;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using VContainer.Unity;

namespace Services
{
    public class PersistingService : IInitializable
    {
        private const string GameplayConfigKey = "GameplayConfig";
        
        private SaveData _saveDataCache;

        public string CurrentLanguage => _saveDataCache.CurrentLanguage;
        public int CurrentShiftIndex => _saveDataCache.Gameplay.CurrentShift;
        public int CurrentCaseIndex => _saveDataCache.Gameplay.CurrentCase;
        public int AvailableHelpsNumber => _saveDataCache.Gameplay.AvailableHelpsNumber;
        public bool HelpWasUsedInCurrentCase => _saveDataCache.Gameplay.HelpWasUsedInCurrentCase;
        public IReadOnlyList<int> CasesOrder => _saveDataCache.Gameplay.CasesOrder;
        public IReadOnlyList<bool> CaseDecisionHistory => _saveDataCache.Gameplay.CaseResultHistory;
        public IReadOnlyList<DateTime> CaseMessageTimes => _saveDataCache.Gameplay.CaseMessageTimes.Select(DateTime.FromBinary).ToList();
        public bool WasGameplayPlayed => _saveDataCache.WasGameplayPlayed;
        public bool SoundsEnabled => _saveDataCache.SoundsEnabled;

        public event Action<bool> OnSoundsEnabledUpdate;

        public void SetLanguage(string language)
        {
            _saveDataCache.CurrentLanguage = language;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCurrentShift(int shift)
        {
            _saveDataCache.Gameplay.CurrentShift = shift;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCurrentCase(int caseNumber)
        {
            _saveDataCache.Gameplay.CurrentCase = caseNumber;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetAvailableHelpsNumber(int number)
        {
            _saveDataCache.Gameplay.AvailableHelpsNumber = number;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetHelpWasUsedInCurrentCase(bool isUsed)
        {
            _saveDataCache.Gameplay.HelpWasUsedInCurrentCase = isUsed;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCasesOrder(IReadOnlyList<int> casesOrder)
        {
            _saveDataCache.Gameplay.CasesOrder = new List<int>(casesOrder);
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCaseResultHistory(IReadOnlyList<bool> caseResultHistory)
        {
            _saveDataCache.Gameplay.CaseResultHistory = new List<bool>(caseResultHistory);
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCaseMessageTimes(IReadOnlyList<DateTime> caseMessageTimes)
        {
            _saveDataCache.Gameplay.CaseMessageTimes = caseMessageTimes.Select(t => t.ToBinary()).ToList();
            SaveGameplayConfig(_saveDataCache);
        }

        public void Initialize()
        {
            _saveDataCache = LoadGameplayConfig();
            if (_saveDataCache == null)
            {
                _saveDataCache = new SaveData();
                SaveGameplayConfig(_saveDataCache);
            }
        }

        private static void SaveGameplayConfig(SaveData saveData)
        {
            var json = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString(GameplayConfigKey, json);
            PlayerPrefs.Save();
        }

        private static SaveData LoadGameplayConfig()
        {
            var json = PlayerPrefs.GetString(GameplayConfigKey, null);
            return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<SaveData>(json);
        }

        public void ResetGameplayProgress()
        {
            _saveDataCache.Gameplay = new GameplaySaveData();
            _saveDataCache.WasGameplayPlayed = false;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetWasGameplayPlayed(bool wasGameplayPlayed)
        {
            _saveDataCache.WasGameplayPlayed = wasGameplayPlayed;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetSoundsEnabled(bool enabled)
        {
            _saveDataCache.SoundsEnabled = enabled;
            SaveGameplayConfig(_saveDataCache);
            OnSoundsEnabledUpdate?.Invoke(enabled);
        }
    }
}

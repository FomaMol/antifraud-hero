using System.Collections.Generic;
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
        public int CurrentShiftIndex => _saveDataCache.CurrentShift;
        public int CurrentCaseIndex => _saveDataCache.CurrentCase;
        public int AvailableHelpsNumber => _saveDataCache.AvailableHelpsNumber;
        public bool HelpWasUsedInCurrentCase => _saveDataCache.HelpWasUsedInCurrentCase;
        public IReadOnlyList<int> CasesOrder => _saveDataCache.CasesOrder;
        public IReadOnlyList<bool> CaseDecisionHistory => _saveDataCache.CaseResultHistory;

        public void SetLanguage(string language)
        {
            _saveDataCache.CurrentLanguage = language;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCurrentShift(int shift)
        {
            _saveDataCache.CurrentShift = shift;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCurrentCase(int caseNumber)
        {
            _saveDataCache.CurrentCase = caseNumber;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetAvailableHelpsNumber(int number)
        {
            _saveDataCache.AvailableHelpsNumber = number;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetHelpWasUsedInCurrentCase(bool isUsed)
        {
            _saveDataCache.HelpWasUsedInCurrentCase = isUsed;
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCasesOrder(IReadOnlyList<int> casesOrder)
        {
            _saveDataCache.CasesOrder = new List<int>(casesOrder);
            SaveGameplayConfig(_saveDataCache);
        }

        public void SetCaseResultHistory(IReadOnlyList<bool> caseResultHistory)
        {
            _saveDataCache.CaseResultHistory = new List<bool>(caseResultHistory);
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
    }
}

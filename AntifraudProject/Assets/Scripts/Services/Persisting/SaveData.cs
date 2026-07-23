using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameplaySaveData
    {
        [JsonProperty] public int CurrentShift = 0;
        [JsonProperty] public int CurrentCase = 0;
        [JsonProperty] public int AvailableHelpsNumber = 3;
        [JsonProperty] public bool HelpWasUsedInCurrentCase = false;
        [JsonProperty] public List<int> CasesOrder = new();
        [JsonProperty] public List<bool> CaseResultHistory = new();
    }
    
    [JsonObject(MemberSerialization.OptIn)]
    public class SaveData
    {
        [JsonProperty] public string CurrentLanguage = "en";
        [JsonProperty] public bool WasGameplayPlayed = false;
        [JsonProperty] public GameplaySaveData Gameplay = new();
    }
}
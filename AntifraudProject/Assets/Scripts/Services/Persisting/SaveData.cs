using Newtonsoft.Json;

namespace Services
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SaveData
    {
        [JsonProperty] public string CurrentLanguage = "en";
        [JsonProperty] public int CurrentShift = 0;
        [JsonProperty] public int CurrentCase = 0;
    }
}
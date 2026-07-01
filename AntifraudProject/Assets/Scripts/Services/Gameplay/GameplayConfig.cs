using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameplayConfig
    {
        [JsonProperty("sender_tool_open_shift")]
        public int SenderToolOpenShift { get; private set; }

        [JsonProperty("db_tool_open_shift")]
        public int DbToolOpenShift { get; private set; }

        [JsonProperty("shifts")]
        public IReadOnlyList<ShiftConfig> Shifts { get; private set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ShiftConfig
    {
        [JsonProperty("min_winrate")]
        public int MinWinrate { get; private set; }

        [JsonProperty("cases")]
        public IReadOnlyList<CaseConfig> Cases { get; private set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CaseConfig
    {
        [JsonProperty("type_text")]
        public string TypeText { get; private set; }

        [JsonProperty("message_text")]
        public string MessageText { get; private set; }

        [JsonProperty("link_validation_text")]
        public string LinkValidationText { get; private set; }

        [JsonProperty("link_validation_result")]
        public int LinkValidationResult { get; private set; }

        [JsonProperty("sender_validation_text")]
        public string SenderValidationText { get; private set; }

        [JsonProperty("sender_validation_result")]
        public int SenderValidationResult { get; private set; }

        [JsonProperty("db_validation_text")]
        public string DbValidationText { get; private set; }

        [JsonProperty("db_validation_result")]
        public int DbValidationResult { get; private set; }

        [JsonProperty("result_text")]
        public string ResultText { get; private set; }
    }
}

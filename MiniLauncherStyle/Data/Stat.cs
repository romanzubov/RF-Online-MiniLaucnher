using Newtonsoft.Json;

namespace MiniLauncherStyle.Data
{
    /// <summary>
    /// Модель данных статистики Chip War для десериализации из JSON.
    /// </summary>
    public class ChipWarStatistics
    {
        [JsonProperty("bcc")]
        public int BccPercent { get; set; }

        [JsonProperty("ccc")]
        public int CccPercent { get; set; }

        [JsonProperty("acc")]
        public int AccPercent { get; set; }

        [JsonProperty("destroed_race")]
        public string DestroyedRace { get; set; }

        [JsonProperty("ore_percent")]
        public int OrePercent { get; set; }
    }
}

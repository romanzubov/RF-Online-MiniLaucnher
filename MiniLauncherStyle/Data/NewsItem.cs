using Newtonsoft.Json;

namespace MiniLauncherStyle.Data
{
    /// <summary>
    /// Модель данных новости для десериализации из JSON.
    /// </summary>
    public class NewsItem
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("datetime")]
        public string DateTime { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

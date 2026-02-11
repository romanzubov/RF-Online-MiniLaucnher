using System.Collections.Generic;

namespace MiniLauncherStyle.Helper
{
    /// <summary>
    /// Утилита для преобразования локальных кодов языка в настроек игры.
    /// </summary>
    public static class NationCodeHelper
    {
        private static readonly Dictionary<string, string> CodeToLanguageMap = new Dictionary<string, string>
        {
            { "ko_kr", "Korea" },
            { "pt_br", "Brazil" },
            { "zn_cn", "China" },
            { "en_gb", "Europe" },
            { "en_id", "Indonesia" },
            { "ja_jp", "Japan" },
            { "en_ph", "Philippines" },
            { "ru_ru", "Russia" },
            { "zh_tw", "Taiwan" },
            { "es_es", "Spain" },
            { "th_th", "Thailand" }
        };

        private const string DefaultLanguage = "Russia";

        /// <summary>
        /// Преобразует код региона (напр. "ru_ru") в название языка для R3Engine.ini (напр. "Russia").
        /// </summary>
        /// <param name="nationCode">Код региона (напр. "ko_kr", "ru_ru")</param>
        /// <returns>Название языка для конфигурации игры</returns>
        public static string EncodeNationCode(string nationCode)
        {
            if (string.IsNullOrEmpty(nationCode))
            {
                return DefaultLanguage;
            }

            string normalizedCode = nationCode.ToLowerInvariant().Replace('-', '_');

            if (CodeToLanguageMap.TryGetValue(normalizedCode, out string language))
            {
                return language;
            }

            return DefaultLanguage;
        }
    }
}

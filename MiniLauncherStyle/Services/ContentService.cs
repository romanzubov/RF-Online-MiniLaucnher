using MiniLauncher.Data;
using MiniLauncher.Utils;
using MiniLauncherStyle.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MiniLauncherStyle.Services
{
    /// <summary>
    /// Результат загрузки данных с сервера.
    /// </summary>
    /// <typeparam name="T">Тип загруженных данных</typeparam>
    public class DataLoadResult<T>
    {
        public bool Success { get; private set; }
        public T Data { get; private set; }
        public string Error { get; private set; }

        public static DataLoadResult<T> Ok(T data)
        {
            return new DataLoadResult<T> { Success = true, Data = data };
        }

        public static DataLoadResult<T> Fail(string error = null)
        {
            return new DataLoadResult<T> { Success = false, Error = error };
        }
    }

    /// <summary>
    /// Сервис для загрузки новостей и статистики с сервера.
    /// </summary>
    public static class ContentService
    {
        /// <summary>
        /// Загружает список новостей с сервера.
        /// </summary>
        public static DataLoadResult<List<NewsItem>> LoadNews()
        {
            try
            {
                string newsUrl = LauncherConfig.GetInstance.SocialConfig.news_link;
                string data = Utils.DownloadDataFromFile(newsUrl);

                if (string.IsNullOrEmpty(data))
                {
                    return DataLoadResult<List<NewsItem>>.Fail("Empty response");
                }

                var newsList = JsonConvert.DeserializeObject<List<NewsItem>>(data);
                
                if (newsList == null)
                {
                    return DataLoadResult<List<NewsItem>>.Fail("Failed to parse news");
                }

                return DataLoadResult<List<NewsItem>>.Ok(newsList);
            }
            catch (Exception ex)
            {
                return DataLoadResult<List<NewsItem>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Загружает статистику Chip War с сервера.
        /// </summary>
        public static DataLoadResult<ChipWarStatistics> LoadStatistics()
        {
            try
            {
                string statUrl = LauncherConfig.GetInstance.SocialConfig.stat_link;
                string data = Utils.DownloadDataFromFile(statUrl);

                if (string.IsNullOrEmpty(data))
                {
                    return DataLoadResult<ChipWarStatistics>.Fail("Empty response");
                }

                var statistics = JsonConvert.DeserializeObject<ChipWarStatistics>(data);
                
                if (statistics == null)
                {
                    return DataLoadResult<ChipWarStatistics>.Fail("Failed to parse statistics");
                }

                return DataLoadResult<ChipWarStatistics>.Ok(statistics);
            }
            catch (Exception ex)
            {
                return DataLoadResult<ChipWarStatistics>.Fail(ex.Message);
            }
        }
    }
}

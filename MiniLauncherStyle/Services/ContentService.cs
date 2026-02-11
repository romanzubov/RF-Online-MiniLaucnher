using MiniLauncher.Data;
using MiniLauncher.Utils;
using MiniLauncherStyle.Data;
using MiniLauncherStyle.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MiniLauncherStyle.Services
{
    /// <summary>
    /// Результат загрузки данных с сервера (для обратной совместимости).
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
    /// Реализует IContentService с поддержкой асинхронной загрузки через BackgroundWorker.
    /// </summary>
    public class ContentService : IContentService
    {
        #region Static Methods (для обратной совместимости)

        /// <summary>
        /// Загружает список новостей с сервера (синхронно).
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
        /// Загружает статистику Chip War с сервера (синхронно).
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

        #endregion

        #region IContentService Implementation

        /// <summary>
        /// Асинхронно загружает новости через BackgroundWorker.
        /// </summary>
        /// <param name="onComplete">Callback при завершении</param>
        public void LoadNewsAsync(Action<AsyncResult<List<NewsItem>>> onComplete)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                e.Result = LoadNewsInternal();
            };
            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    onComplete(new AsyncResult<List<NewsItem>> 
                    { 
                        Success = false, 
                        ErrorMessage = e.Error.Message 
                    });
                }
                else
                {
                    var result = (AsyncResult<List<NewsItem>>)e.Result;
                    onComplete(result);
                }
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Внутренний метод загрузки новостей (синхронный).
        /// </summary>
        private AsyncResult<List<NewsItem>> LoadNewsInternal()
        {
            try
            {
                string newsUrl = LauncherConfig.GetInstance.SocialConfig.news_link;
                string data = Utils.DownloadDataFromFile(newsUrl);

                if (string.IsNullOrEmpty(data))
                {
                    return new AsyncResult<List<NewsItem>> { Success = false, ErrorMessage = "Empty response" };
                }

                var newsList = JsonConvert.DeserializeObject<List<NewsItem>>(data);
                
                if (newsList == null)
                {
                    return new AsyncResult<List<NewsItem>> { Success = false, ErrorMessage = "Failed to parse news" };
                }

                return new AsyncResult<List<NewsItem>> { Success = true, Data = newsList };
            }
            catch (Exception ex)
            {
                return new AsyncResult<List<NewsItem>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        #endregion
    }
}

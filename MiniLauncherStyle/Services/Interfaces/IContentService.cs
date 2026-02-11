using System;
using System.Collections.Generic;
using MiniLauncherStyle.Data;

namespace MiniLauncherStyle.Services.Interfaces
{
    /// <summary>
    /// Результат асинхронной операции загрузки данных.
    /// </summary>
    /// <typeparam name="T">Тип данных</typeparam>
    public class AsyncResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Интерфейс сервиса загрузки контента (новости, статистика).
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        /// Асинхронно загружает новости.
        /// </summary>
        /// <param name="onComplete">Callback при завершении</param>
        void LoadNewsAsync(Action<AsyncResult<List<NewsItem>>> onComplete);

        /// <summary>
        /// Асинхронно загружает статистику Chip War.
        /// </summary>
        /// <param name="onComplete">Callback при завершении</param>
        void LoadStatisticsAsync(Action<AsyncResult<ChipWarStatistics>> onComplete);
    }
}

using System;

namespace MiniLauncherStyle.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса диалоговых окон.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Показывает информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="title">Заголовок</param>
        void ShowInfo(string message, string title);

        /// <summary>
        /// Показывает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="title">Заголовок</param>
        void ShowError(string message, string title);

        /// <summary>
        /// Показывает диалог подтверждения.
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="title">Заголовок</param>
        /// <returns>True если пользователь подтвердил</returns>
        bool Confirm(string message, string title);

        /// <summary>
        /// Открывает URL в браузере.
        /// </summary>
        /// <param name="url">URL для открытия</param>
        void OpenUrl(string url);
    }
}

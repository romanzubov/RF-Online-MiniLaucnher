using System;
using System.Windows;

namespace MiniLauncherStyle.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса навигации между окнами.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Устанавливает ссылку на главное окно.
        /// </summary>
        void SetMainWindow(Window mainWindow);

        /// <summary>
        /// Показывает окно настроек.
        /// </summary>
        void ShowSettings();

        /// <summary>
        /// Показывает окно настроек с callback при закрытии.
        /// </summary>
        void ShowSettings(Action<bool> onClosed);

        /// <summary>
        /// Показывает окно обновления.
        /// </summary>
        /// <param name="onClosed">Callback при закрытии окна</param>
        void ShowUpdateWindow(Action onClosed);

        /// <summary>
        /// Закрывает текущее окно.
        /// </summary>
        void CloseCurrentWindow();

        /// <summary>
        /// Перезапускает приложение.
        /// </summary>
        void RestartApplication();

        /// <summary>
        /// Выходит из приложения.
        /// </summary>
        void ExitApplication();
    }
}

using MiniLauncherStyle.Services.Interfaces;
using MiniLauncherStyle.Views.Settings;
using System;
using System.Windows;

namespace MiniLauncherStyle.Services
{
    /// <summary>
    /// Реализация сервиса навигации между окнами.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private Window _currentMainWindow;
        private Action<bool> _settingsClosedCallback;

        /// <summary>
        /// Устанавливает ссылку на главное окно.
        /// </summary>
        public void SetMainWindow(Window mainWindow)
        {
            _currentMainWindow = mainWindow;
        }

        /// <summary>
        /// Показывает окно настроек.
        /// </summary>
        public void ShowSettings()
        {
            ShowSettings(null);
        }

        /// <summary>
        /// Показывает окно настроек с callback при закрытии.
        /// </summary>
        public void ShowSettings(Action<bool> onClosed)
        {
            _settingsClosedCallback = onClosed;
            
            double top = 0;
            double left = 0;
            
            if (_currentMainWindow != null)
            {
                top = _currentMainWindow.Top;
                left = _currentMainWindow.Left;
            }
            
            var settingsWindow = new SettingsWindow(top, left);
            settingsWindow.Closing += (sender, e) =>
            {
                var window = sender as SettingsWindow;
                _settingsClosedCallback?.Invoke(window?.ClientUpdateRequired ?? false);
            };
            settingsWindow.Show();
        }

        /// <summary>
        /// Показывает окно обновления.
        /// </summary>
        public void ShowUpdateWindow(Action onClosed)
        {
            // Реализация для окна обновления
            onClosed?.Invoke();
        }

        /// <summary>
        /// Закрывает текущее окно.
        /// </summary>
        public void CloseCurrentWindow()
        {
            _currentMainWindow?.Close();
        }

        /// <summary>
        /// Выходит из приложения.
        /// </summary>
        public void ExitApplication()
        {
            Environment.Exit(0);
        }
    }
}

using MiniLauncherStyle.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Windows;

namespace MiniLauncherStyle.Services
{
    /// <summary>
    /// Реализация сервиса диалоговых окон.
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// Показывает информационное сообщение.
        /// </summary>
        public void ShowInfo(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Показывает сообщение об ошибке.
        /// </summary>
        public void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Показывает диалог подтверждения.
        /// </summary>
        public bool Confirm(string message, string title)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Открывает URL в браузере.
        /// </summary>
        public void OpenUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;

            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                // Игнорируем ошибки открытия URL
            }
        }
    }
}

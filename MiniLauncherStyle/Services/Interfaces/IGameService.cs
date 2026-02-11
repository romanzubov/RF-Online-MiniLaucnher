using System;
using System.ComponentModel;
using MiniLauncher.Network.Packets;

namespace MiniLauncherStyle.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса игрового клиента.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Запускает игровой клиент.
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль (зашифрованный)</param>
        /// <param name="rememberLogin">Запомнить логин</param>
        /// <param name="onComplete">Callback при завершении запуска</param>
        void LaunchGame(string login, string password, bool rememberLogin, Action<bool, string> onComplete);

        /// <summary>
        /// Запускает клиент игры с данными сессии.
        /// </summary>
        /// <param name="defaultSet">Данные сессии от сервера</param>
        void RunGameClient(Default_Set defaultSet);

        /// <summary>
        /// Проверяет, нужно ли закрыть лаунчер после входа.
        /// </summary>
        bool ShouldCloseLauncherAfterLogin();

        /// <summary>
        /// Получает BackgroundWorker для асинхронного запуска игры.
        /// </summary>
        /// <returns>BackgroundWorker для запуска игры</returns>
        BackgroundWorker CreateLaunchWorker();
    }
}

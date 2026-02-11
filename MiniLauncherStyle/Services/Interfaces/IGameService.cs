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
        /// Запускает клиент игры с данными сессии.
        /// </summary>
        /// <param name="defaultSet">Данные сессии от сервера</param>
        void RunGameClient(Default_Set defaultSet);

        /// <summary>
        /// Проверяет, нужно ли закрыть лаунчер после входа.
        /// </summary>
        bool ShouldCloseLauncherAfterLogin();
    }
}

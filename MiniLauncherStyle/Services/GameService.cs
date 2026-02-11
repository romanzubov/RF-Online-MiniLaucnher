using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Network.Packets;
using MiniLauncherStyle.Services.Interfaces;
using System;
using System.ComponentModel;
using System.IO;

namespace MiniLauncherStyle.Services
{
    /// <summary>
    /// Реализация сервиса игрового клиента.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly IDialogService _dialogService;

        public GameService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        /// <summary>
        /// Запускает клиент игры с данными сессии.
        /// </summary>
        public void RunGameClient(Default_Set defaultSet)
        {
            var clientCfg = LauncherConfig.GetInstance.ClientConfig;
            ClientRunHelper.WriteTmp(clientCfg.DefaultSetTmpPath, defaultSet);
            ClientRunHelper.RunClient(clientCfg.ClientBinaryPath);
        }

        /// <summary>
        /// Проверяет, нужно ли закрыть лаунчер после входа.
        /// </summary>
        public bool ShouldCloseLauncherAfterLogin()
        {
            if (File.Exists(".\\R3Engine.ini"))
            {
                var ini = new IniFile(".\\R3Engine.ini");
                if (ini.KeyExists("close_launcher_after_login", "Launcher"))
                {
                    return bool.Parse(ini.ReadReverse("Launcher", "close_launcher_after_login").ToLower());
                }
                else
                {
                    ini.Write("close_launcher_after_login", "FALSE", "Launcher");
                }
            }
            return false;
        }
    }
}

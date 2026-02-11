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
        /// Запускает игровой клиент.
        /// </summary>
        public void LaunchGame(string login, string password, bool rememberLogin, Action<bool, string> onComplete)
        {
            try
            {
                var clientCfg = LauncherConfig.GetInstance.ClientConfig;
                
                // Проверяем существование клиента
                if (!File.Exists(clientCfg.ClientBinaryPath))
                {
                    onComplete?.Invoke(false, "Client executable not found");
                    return;
                }

                onComplete?.Invoke(true, null);
            }
            catch (Exception ex)
            {
                onComplete?.Invoke(false, ex.Message);
            }
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

        /// <summary>
        /// Получает BackgroundWorker для асинхронного запуска игры.
        /// </summary>
        public BackgroundWorker CreateLaunchWorker()
        {
            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            return worker;
        }
    }
}

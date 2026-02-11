using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.View;
using MiniLauncherStyle.Services;
using System;
using System.Net;
using System.Windows;

namespace MiniLauncherStyle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string ConfigFilePath = ".\\MiniLauncher.ini";

        public void App_Startup(object sender, StartupEventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 65000;

            if (LauncherUpdateService.ProcessUpdate(e.Args))
            {
                Environment.Exit(0);
            }

            if (!InitConfig())
            {
                return;
            }

            if (!InitLocalization())
            {
                return;
            }

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var updateView = new Update();
            System.Windows.Forms.Application.Run(updateView);

            if (updateView.continiue)
            {
                new MainWindow().Show();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private static bool InitConfig()
        {
            var configLoader = new LoadConfigData(ConfigFilePath);
            
            if (!configLoader.IsExist())
            {
                ShowConfigError("ConfigurationNotFound");
                return false;
            }

            if (!configLoader.Load())
            {
                ShowConfigError("ConfigurationFileReadError");
                return false;
            }

            return true;
        }

        private static void ShowConfigError(string messageKey)
        {
            MessageBox.Show(
                LocalizationManager.GetInstance.GetString(messageKey),
                LocalizationManager.GetInstance.GetString("Error"));
        }

        private static bool InitLocalization()
        {
            var localizationManager = LocalizationManager.GetInstance;
            var nationalConfig = LauncherConfig.GetInstance.NationalConfig;

            return localizationManager.Init(nationalConfig.NationCode);
        }
    }
}

using MiniLauncher.Data;
using MiniLauncher.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MiniLauncher
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!InitConfig())
                return;

            if (!InitLocalization())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        private static bool InitLocalization()
        {
            var localizationManager = LocalizationManager.GetInstance;
            var nationalConfig = LauncherConfig.GetInstance.NationalConfig;

            return localizationManager.Init(nationalConfig.NationCode);
        }
        private static bool InitConfig()
        {
            var configLoader = new LoadConfigData(".\\MiniLauncher.ini");
            if (!configLoader.IsExist())
            {
                MessageBox.Show(LocalizationManager.GetInstance.GetString("ConfigurationNotFound"), 
                    LocalizationManager.GetInstance.GetString("Error"));
                return false;
            }

            if (!configLoader.Load())
            {
                MessageBox.Show(LocalizationManager.GetInstance.GetString("ConfigurationFileReadError"),
                    LocalizationManager.GetInstance.GetString("Error"));
                return false;
            }

            return true;
        }
    }
}

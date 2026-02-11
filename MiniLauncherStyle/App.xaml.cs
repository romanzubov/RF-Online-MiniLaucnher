using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;

namespace MiniLauncherStyle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_Startup(object sender, StartupEventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 65000;
            string[] args = Environment.GetCommandLineArgs();
            if (ProcessUpdate(e.Args))
            {
                Environment.Exit(0);
            }

            if (!InitConfig())
                return;

            if (!InitLocalization())
                return;

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

        private static bool ProcessUpdate(string[] args)
        {
            if (args.Length <= 0)
            {
                return false;
            }
            if (String.IsNullOrEmpty(args[0]))
                return false;

            if (args[0] == "update1")
            {
                try
                {
                    var process = Process.GetProcessById(Int32.Parse(args[2]));
                    process.WaitForExit();
                }
                catch (Exception)
                {

                }

                File.Move($".\\{args[1]}", $".\\tmp_{args[1]}");

                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = $".\\tmp_{args[1]}";
                    myProcess.StartInfo.Arguments = $"update2 \"{args[1]}\" {Process.GetCurrentProcess().Id}";
                    myProcess.Start();
                }
            }

            if (args[0] == "update2")
            {
                try
                {
                    var process = Process.GetProcessById(Int32.Parse(args[2]));
                    process.WaitForExit();
                }
                catch (Exception)
                {
                }

                File.Move("MiniLauncherNew.exe", $".\\{args[1]}");

                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = $".\\{args[1]}";
                    myProcess.StartInfo.Arguments = $"update3  \"{System.AppDomain.CurrentDomain.FriendlyName}\" {Process.GetCurrentProcess().Id}";
                    myProcess.Start();
                }
            }

            if (args[0] == "update3")
            {

                try
                {
                    var process = Process.GetProcessById(Int32.Parse(args[2]));
                    process.WaitForExit();
                }
                catch (Exception)
                {
                }

                File.Delete($".\\{args[1]}");

                return false;
            }

            return true;
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

        private static bool InitLocalization()
        {
            var localizationManager = LocalizationManager.GetInstance;
            var nationalConfig = LauncherConfig.GetInstance.NationalConfig;

            return localizationManager.Init(nationalConfig.NationCode);
        }

    }
}

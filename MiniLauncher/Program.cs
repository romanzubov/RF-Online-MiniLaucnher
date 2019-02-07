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
            InitConfig();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        private static bool InitConfig()
        {
            var configLoader = new LoadConfigData(".\\MiniLauncher.ini");
            if (!configLoader.IsExist())
            {
                MessageBox.Show("Файл конфигурации не найден!", "Ошибка!");
                return false;
            }

            if (!configLoader.Load())
            {
                MessageBox.Show("Ошибка чтения файла конфигурации!", "Ошибка!");
                return false;
            }

            return true;
        }
    }
}

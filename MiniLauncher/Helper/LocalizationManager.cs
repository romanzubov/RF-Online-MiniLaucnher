using MiniLauncher.Data;
using MiniLauncher.Network.Packets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MiniLauncher.Helper
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private IniFile LocalozationFile;
        public bool Init(e_nation_code nationCode)
        {
            string path = GetNationFilePath(nationCode);
            if (!Directory.Exists(path))
                return false;

            if (!File.Exists(path + "\\MiniLauncher.Localization.ini"))
                return false;

            LocalozationFile = new IniFile(path + "\\MiniLauncher.Localization.ini");

            return true;
        }

        private string GetNationFilePath(e_nation_code nationCode)
        {
            var clientConfig = LauncherConfig.GetInstance.ClientConfig;
            string nationName = nationCode.ToString();
            if (nationName == "NUM")
                nationName = "ru_ru";

            return clientConfig.ClientWorkingDirectory + "DataTable\\" + nationName.Replace('_', '-');
        }

        public string GetString(string key)
        {
            if (LocalozationFile.KeyExists(key, "Localization"))
            {
                return LocalozationFile.Read(key, "Localization");
            } else {
                return string.Empty;
            }
        }
    }
}

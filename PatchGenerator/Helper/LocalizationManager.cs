using PatchGenerator.Data;
using System.IO;
using System.Windows.Forms;

namespace PatchGenerator.Helper
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private IniFile LocalozationFile;
        public LocalizationManager() {
            Init(e_nation_code.NUM);
        }
        public bool Init(e_nation_code nationCode)
        {
            string path = GetNationFilePath(nationCode);
            if (!Directory.Exists(path))
                return false;

            if (!File.Exists(path + "\\PatchGenerator.Localization.ini"))
                return false;

            LocalozationFile = new IniFile(path + "\\PatchGenerator.Localization.ini");

            return true;
        }

        private string GetNationFilePath(e_nation_code nationCode)
        {
            string nationName = nationCode.ToString();
            if (nationName == "NUM")
                nationName = "ru_ru";

            return System.AppDomain.CurrentDomain.BaseDirectory + "Localization\\" + nationName;
        }

        public string GetString(string key)
        {
            if (LocalozationFile.KeyExists(key, "Localization"))
            {
                return LocalozationFile.Read(key, "Localization");
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

using PatchGenerator.Data;
using System.IO;

namespace PatchGenerator.Helper
{
    public class LoadConfigData
    {
        public LoadConfigData(string ConfgiPath)
        {
            _configPath = ConfgiPath;
        }

        public bool IsExist()
        {
            return File.Exists(_configPath);
        }
        public bool Load()
        {
            bool result = true;
            var data = PatchGeneratorConfig.GetInstance;
            var iniParser = new IniFile(_configPath);
            // Section NationalSetting
            result = iniParser.KeyExists("Language", "Localization");

            if (result)
            {
                // Section NationalSetting
                data.NationalConfig.NationCode = DecodeNationData(iniParser.Read("Language", "Localization"));
            }
            return result;
        }
        private e_nation_code DecodeNationData(string language)
        {
            switch (language)
            {
                case "Europe":
                    return e_nation_code.en_gb;
                case "Russia":
                    return e_nation_code.ru_ru;
                default:
                    return e_nation_code.NUM;
            }
        }
        private string _configPath;
    }
}

using MiniLauncher.Data;
using MiniLauncher.Network.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiniLauncher.Helper
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
            var data = LauncherConfig.GetInstance;
            var r3Engine = new IniFile(".\\R3Engine.ini");
            var iniParser = new IniFile(_configPath);
            //Section Server
            string[] keys = new string[5] { "LogginAddress", "OverrideServerAddress", "ServerAddress", "OverrideServerSelection", "ServerIndexSelect" };
            result = iniParser.KeysExists(keys, "Server");
            // Section NationalSetting
            result = iniParser.KeyExists("OverrideNationalCode", "NationalSetting");
            // Section Client
            keys = new string[5] { "DefaultSetTmpPath", "ClientBinaryPath", "ClientWorkingDirectory", "ClientLogin", "ClientPassword" };
            result = iniParser.KeysExists(keys, "ClientSetting");
            // Logins Data
            result = iniParser.KeyExists("LoginNum", "LoginsSetting");

            if (result)
            {
                //Section Server
                data.ServerConfig.LogginAddress = iniParser.Read("LogginAddress", "Server");
                data.ServerConfig.ServerAddress = iniParser.Read("ServerAddress", "Server");
                data.ServerConfig.OverrideServerAddress = iniParser.Read("OverrideServerAddress", "Server").ToLower() == "true" ? true : false;
                data.ServerConfig.OverrideServerSelection = iniParser.Read("OverrideServerSelection", "Server").ToLower() == "true" ? true : false;
                data.ServerConfig.ServerIndexSelect = int.Parse(iniParser.Read("ServerIndexSelect", "Server"));
                // Section NationalSetting
                data.NationalConfig.OverrideNationalCode = iniParser.Read("OverrideNationalCode", "NationalSetting").ToLower() == "true" ? true : false;
                data.NationalConfig.NationCode = data.NationalConfig.OverrideNationalCode ? DecodeNationData(r3Engine.Read("Language", "Setup")) : e_nation_code.NUM;
                // Section Client
                data.ClientConfig.DefaultSetTmpPath = iniParser.Read("DefaultSetTmpPath", "ClientSetting");
                data.ClientConfig.ClientBinaryPath = iniParser.Read("ClientBinaryPath", "ClientSetting");
                data.ClientConfig.ClientWorkingDirectory = iniParser.Read("ClientWorkingDirectory", "ClientSetting");
                data.ClientConfig.ClientLogin = iniParser.Read("ClientLogin", "ClientSetting");
                data.ClientConfig.ClientPassword = iniParser.Read("ClientPassword", "ClientSetting");
                // Logins Data
                data.LoginsConfig.LoginNum = Convert.ToInt32(iniParser.Read("LoginNum", "LoginsSetting"));
                if (data.LoginsConfig.LoginNum > 0)
                {
                    data.LoginsConfig.ClientLogin = new string[10];
                    data.LoginsConfig.ClientPassword = new string[10];
                    int i;
                    for (i = 0; i < data.LoginsConfig.LoginNum; i++)
                    {
                        string IsFillLogin = string.Format("{0}{1}", "ClientLogin", i);
                        data.LoginsConfig.ClientLogin[i] = iniParser.Read(IsFillLogin, "LoginsSetting");
                        if (data.LoginsConfig.ClientLogin[i] != "")
                        {
                            string Login = string.Format("{0}{1}", "ClientLogin", i);
                            data.LoginsConfig.ClientLogin[i] = iniParser.Read(Login, "LoginsSetting");
                            string Password = string.Format("{0}{1}", "ClientPassword", i);
                            data.LoginsConfig.ClientPassword[i] = iniParser.Read(Password, "LoginsSetting");
                        }
                    }

                }
            }

            return result;

        }
        private e_nation_code DecodeNationData(string language)
        {
            switch (language)
            {
                case "Korea":
                    return e_nation_code.ko_kr;
                case "Brazil":
                    return e_nation_code.pt_br;
                case "China":
                    return e_nation_code.zn_cn;
                case "Europe":
                    return e_nation_code.en_gb;
                case "Indonesia":
                    return e_nation_code.en_id;
                case "Japan":
                    return e_nation_code.ja_jp;
                case "Philippines":
                    return e_nation_code.en_ph;
                case "Russia":
                    return e_nation_code.ru_ru;
                case "Taiwan":
                    return e_nation_code.zh_tw;
                case "Spain":
                    return e_nation_code.es_es;
                case "Thailand":
                    return e_nation_code.th_th;
                default:
                    return e_nation_code.NUM;
            }
        }
        private string _configPath;

    }
}

using MiniLauncher.Data;
using MiniLauncher.Network.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiniLauncher.Helper
{
    class LoadLoginsData
    {
        public LoadLoginsData(string ConfgiPath)
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
            var data = LoginConfig.GetInstance;
            var iniParser = new IniFile(_configPath);

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

            return result;
        }



        private string _configPath;
    }
}

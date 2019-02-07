using MiniLauncher.Helper;
using MiniLauncher.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniLauncher.Data
{


    public class LauncherConfig : Singleton<LauncherConfig>
    {
        public ServerSetting ServerConfig { get; set; }
        public NationalSetting NationalConfig { get; set; }
        public ClientSetting ClientConfig { get; set; }
        public LoginsSetting LoginsConfig { get; set; }

        public LauncherConfig()
        {
            ServerConfig = new ServerSetting();
            NationalConfig = new NationalSetting();
            ClientConfig = new ClientSetting();
            LoginsConfig = new LoginsSetting();
        }
    }

    public class ServerSetting
    {
        public string LogginAddress { get; set; }
        public string ServerAddress { get; set; }
        public bool OverrideServerAddress { get; set; }
        public bool OverrideServerSelection { get; set; }
        public int ServerIndexSelect { get; set; }
    }
    public class NationalSetting
    {
        public bool OverrideNationalCode { get; set; }
        public e_nation_code NationCode { get; set; }
    }
    public class ClientSetting
    {
        public string DefaultSetTmpPath { get; set; }
        public string ClientBinaryPath { get; set; }
        public string ClientWorkingDirectory { get; set; }
        public string ClientLogin { get; set; }
        public string ClientPassword { get; set; }
    }

    public class LoginsSetting
    {
        public int LoginNum { get; set; }
        public string[] ClientLogin { get; set; }
        public string[] ClientPassword { get; set; }
    }
}

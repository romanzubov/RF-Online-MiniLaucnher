using MiniLauncher.Helper;
using MiniLauncher.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniLauncher.Data
{
    class LoginConfig : Singleton<LoginConfig>
    {
        public LoginsSetting LoginsConfig { get; set; }

        public LoginConfig()
        {
          LoginsConfig = new LoginsSetting();
        }
    }

    class LoginsSetting 
    {
        public int LoginNum { get; set; }
        public string[] ClientLogin { get; set; }
        public string[] ClientPassword { get; set; }
    }
}

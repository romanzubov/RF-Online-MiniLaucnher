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
        public UpdateSetting UpdateConfig { get; set; }
        public SocialSetting SocialConfig { get; set; }
        public LauncherConfig()
        {
            ServerConfig = new ServerSetting();
            NationalConfig = new NationalSetting();
            ClientConfig = new ClientSetting();
            UpdateConfig = new UpdateSetting();
            SocialConfig = new SocialSetting();
        }
    }

    public class ServerSetting
    {
        public string Title { get; set; }
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
    }
    public enum UpdateType
    {
        HTTP,
        TORRENT
    }
    public class UpdateSetting
    {
        public bool ClientUpdateEnable { get; set; }
        public bool PatchUpdateEnable { get; set; }
        public string UpdateServerClient { get; set; }
        public string UpdateServerPatch { get; set; }
        public string UpdateLauncherUrl { get; set; }
        public string UpdateUIUrl { get; set; }
        public UpdateType UpdateType { get; set; }
        public int CountParallelDownload { get; set; }
    }
    public class SocialSetting
    {
        public string forum_link { get; set; }
        public string bd_link { get; set; }
        public string vk_link { get; set; }
        public string craft_link { get; set; }
        public string register_link { get; set; }
        public string news_link { get; set; }
        public string stat_link { get; set; }
    }
}

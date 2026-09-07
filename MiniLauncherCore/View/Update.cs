using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Updater;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace MiniLauncher.View
{
    public partial class Update : Form
    {
        private LocalizationManager Lm;
        private UpdateManager UpdateManager { get; set; }
        public bool continiue { get; set; }
        public Update()
        {
            Lm = LocalizationManager.GetInstance;
            InitializeComponent();
            continiue = true;
        }
        private void Update_Load(object sender, EventArgs e)
        {
            statusLable.Text = String.Format(Lm.GetString("launcher_update_status"), Lm.GetString("Проверка версии..."));
            LauncherUpdate();
        }

        private void LauncherUpdate()
        {
            if(string.IsNullOrEmpty(LauncherConfig.GetInstance.UpdateConfig.UpdateLauncherUrl))
            {
                Close();
                return;
            }

            Version currentVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            Version remouteVersion = GetRemouteVersion(LauncherConfig.GetInstance.UpdateConfig.UpdateLauncherUrl + "version.txt");
            if(remouteVersion > currentVersion)
            {
                using (var wcUpdate = new WebClient())
                {
                    wcUpdate.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24");
                    wcUpdate.Proxy = null;
                    wcUpdate.DownloadProgressChanged += WcUpdate_DownloadProgressChanged;
                    wcUpdate.DownloadFileCompleted += WcUpdate_DownloadFileCompleted;
                    wcUpdate.DownloadFileAsync(new Uri(LauncherConfig.GetInstance.UpdateConfig.UpdateLauncherUrl + $"{remouteVersion.ToString()}.exe"), 
                        AppDomain.CurrentDomain.BaseDirectory + "\\MiniLauncherNew.exe");
                }
            }
            if(currentVersion >= remouteVersion)
            {
                Close();
            }
        }

        private void WcUpdate_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.FileName = ".\\MiniLauncherNew.exe";
                    myProcess.StartInfo.Arguments = $"update1 \"{System.AppDomain.CurrentDomain.FriendlyName}\" {Process.GetCurrentProcess().Id}";
                    if (System.Environment.OSVersion.Version.Major >= 6)
                    {
                        myProcess.StartInfo.Verb = "runas";
                    }
                    myProcess.Start();
                }
                statusLable.Text = String.Format(Lm.GetString("launcher_update_status"), Lm.GetString("launcher_update"));
                continiue = false;
                Close();
            }
            catch (Exception p)
            {
                statusLable.Text = String.Format(Lm.GetString("launcher_update_status"), Lm.GetString("launcher_update_error"));
                MessageBox.Show(p.Message);
            }
        }

        private void WcUpdate_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progress.Invoke(new MethodInvoker(delegate
            {
                statusLable.Text = String.Format(Lm.GetString("launcher_update_status"), 
                    String.Format(Lm.GetString("launcher_update_download"), e.ProgressPercentage)); 
                progress.Value = e.ProgressPercentage;
            }));
        }

        private Version GetRemouteVersion(string updateLauncherUrl)
        {
            string version = Utils.Utils.DownloadDataFromFile(updateLauncherUrl);
            if (String.IsNullOrEmpty(version))
            {
                return System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            }
            else
            {
                return new Version(version);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Network;
using MiniLauncher.Network.Packets;
using MiniLauncher.Updater;
using MiniLauncher.Utils;
using MiniLauncher.View;

namespace MiniLauncher
{
    public partial class Main: Form
    {
        private UserCredential userCredential;
        private LocalizationManager Lm;
        private NetworkClient networkClient;
        private UpdateManager UpdateManager { get; set; }
        public Main()
        {
            Lm = LocalizationManager.GetInstance;

            InitializeComponent();
            InitLocalization();
        }

        private void InitUserCredential()
        {
            login_input.Items.Clear();
            login_input.Text = String.Empty;
            password_input.Text = String.Empty;
            userCredential = new UserCredential(".\\credential_storage.json");
            login_input.Items.AddRange(userCredential.LoadLogins());
            if (login_input.Items.Count > 0)
            {
                login_input.SelectedIndex = 0;
                string password = userCredential.ProcessLoginData(login_input.Text);
                if (!String.IsNullOrEmpty(password))
                {
                    password_input.Text = password;
                }
            }
        }
        private void InitLocalization()
        {
            Text = LauncherConfig.GetInstance.ServerConfig.Title;
            ForAllControls(this, control =>
            {
                control.Text = Lm.GetString(control.Name);
            });
            server_name.Text = Lm.GetString("server_name");
            status_name.Text = Lm.GetString("status_name");
        }
        private void InitializeUpdater()
        {
            update_box.Show();
            if (!LauncherConfig.GetInstance.UpdateConfig.ClientUpdateEnable && !LauncherConfig.GetInstance.UpdateConfig.PatchUpdateEnable)
            {
                update_box.Hide();
                return;
            }
            update_check_label.Text = String.Format(Lm.GetString("update_check_label"), 0, 0);
            update_apply_label.Text = String.Format(Lm.GetString("update_apply_label"), 0, 0);

            Queue<UpdateTask> updateTasks = new Queue<UpdateTask>();
            if (!File.Exists($".//{LauncherConfig.GetInstance.ServerConfig.Title}.lock") && LauncherConfig.GetInstance.UpdateConfig.ClientUpdateEnable)
            {
                updateTasks.Enqueue(new UpdateTask(Lm.GetString("update_box_client"),
                    LauncherConfig.GetInstance.UpdateConfig,
                    LauncherConfig.GetInstance.UpdateConfig.UpdateServerClient,
                    "client"));
            }

            if (LauncherConfig.GetInstance.UpdateConfig.PatchUpdateEnable)
            {
                updateTasks.Enqueue(new UpdateTask(Lm.GetString("update_box_patch "),
                    LauncherConfig.GetInstance.UpdateConfig,
                    LauncherConfig.GetInstance.UpdateConfig.UpdateServerPatch,
                    "patch"));
            }

            UpdateManager = new UpdateManager(updateTasks);
            UpdateManager.UpdateStart += UpdateManager_UpdateStart;
            UpdateManager.UpdateComplete += UpdateDownloader_UpdateComplete;
            UpdateManager.UpdateCheckProggress += UpdateCheckFiles_UpdateCheckProggress;
            UpdateManager.UpdateDownloadProgress += UpdateDownloader_UpdateDownloadProgress;
            UpdateManager.Start();
        }
        private void InitializeNetwork()
        {
            status_label.Text = Lm.GetString("StatusConnecting");
            var serverCfg = LauncherConfig.GetInstance.ServerConfig;
            networkClient = new NetworkClient(serverCfg.LogginAddress.Split(':')[0], int.Parse(serverCfg.LogginAddress.Split(':')[1]));
            networkClient.OnError += NetworkClient_OnError;
            networkClient.OnConnected += NetworkClient_OnConnected;
            networkClient.ClientEvents += NetworkClient_ClientEvents;
            (new Thread(() => {
                networkClient.StartClient();
            })).Start();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            InitializeNetwork();
            InitUserCredential();
            InitializeUpdater();
            if (LauncherConfig.GetInstance.ServerConfig.OverrideServerSelection)
            {
                MaximumSize = new System.Drawing.Size(Size.Width, 174);
                MinimumSize = new System.Drawing.Size(Size.Width, 174);
                server_list.Hide();
            }
        }


        private void UpdateManager_UpdateStart(object sender, UpdateStartedEventArgs e)
        {
            update_box.Invoke(new MethodInvoker(delegate
            {
                login_btn.Enabled = false;
                update_box.Text = e.TaskName;
            }));
        }
        private void UpdateDownloader_UpdateComplete(object sender, UpdateCompleteEventArgs e)
        {
            if (e.TaskName == Lm.GetString("update_box_client"))
            {
                File.Create($".//{LauncherConfig.GetInstance.ServerConfig.Title}.lock");
            }

            progress_apply.Invoke(new MethodInvoker(delegate
            {
                progress_apply.Maximum = 100;
                progress_apply.Value = 100;
                update_apply_label.Text = Lm.GetString("update_apply_label_done");
                update_box.Hide();
            }));
        }
        private void UpdateCheckFiles_UpdateCheckProggress(object sender, UpdateCheckEventArgs e)
        {
            progress_check.Invoke(new MethodInvoker(delegate
            {
                progress_check.Maximum = (int)e.TotalCount;
                progress_check.Value = (int)e.DoneCount;
                update_check_label.Text = String.Format(Lm.GetString("update_check_label"), e.DoneCount, e.TotalCount);
            }));
        }
        private void UpdateDownloader_UpdateDownloadProgress(object sender, UpdateDownloadEventArgs e)
        {
            progress_apply.Invoke(new MethodInvoker(delegate
            {
                progress_apply.Maximum = e.TotalCount;
                progress_apply.Value = e.DoneCount;
                update_apply_label.Text = String.Format(Lm.GetString("update_apply_label"), e.DoneCount, e.TotalCount);
            }));
        }


        private void NetworkClient_OnError(object sender, EventArgs e)
        {
            
            ChangeStatus(false);
        }

        private void NetworkClient_OnConnected(object sender, EventArgs e)
        {
            ChangeStatus(true);
        }

        private void NetworkClient_ClientEvents(object sender, NetworkClientEventArgs e)
        {
            switch (e.CState)
            {
                case NetworkClientEventArgs.Callback.CRYPTO_KEY_INFORM:
                    EnableLoginBtn(true);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_WRONG_LOGIN:
                    EnableLoginBtn(true);
                    MessageBox.Show(Lm.GetString("WrongLogin"),
                        Lm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_WRONG_PW:
                    EnableLoginBtn(true);
                    MessageBox.Show(Lm.GetString("WrongPassword"),
                        Lm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_SERVER_CLOSED:
                    EnableLoginBtn(true);
                    MessageBox.Show(Lm.GetString("ServerTechnicalWork"),
                        Lm.GetString("Error"), MessageBoxButtons.OK ,MessageBoxIcon.Error);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_BANNED:
                    MessageBox.Show(Lm.GetString("AccountBlocked"),
                        Lm.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case NetworkClientEventArgs.Callback.SERVER_LIST_INFORM:
                    if (save_checkbox.Enabled)
                    {
                        userCredential.ProcessLoginData(login_input.Text, password_input.Text);
                    }
                    FillServerList(e.Servers);
                    break;
                case NetworkClientEventArgs.Callback.SERVER_SESSION_RESULT:
                    RunGame(e.DefaultSet);
                    break;
            }
        }
        private void Login_input_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(login_input.Text))
            {
                return;
            }
            string password = userCredential.ProcessLoginData(login_input.Text);
            if (!String.IsNullOrEmpty(password))
            {
                password_input.Text = password;
            }
        }
        private void login_btn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(login_input.Text) && !string.IsNullOrEmpty(password_input.Text) ||
               login_input.Text.Length <= 13 && password_input.Text.Length <= 13)
            {
                networkClient.DoLogin(login_input.Text, password_input.Text);
                EnableLoginBtn(false);
            }
            else
            {
                MessageBox.Show(Lm.GetString("LoginPasswordCheck"),
                    Lm.GetString("Error"));
            }
        }
        private void Btn_settings_Click(object sender, EventArgs e)
        {
            var settings = new Settings();
            settings.FormClosing += Settings_FormClosing;
            settings.Show();
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = (Settings)sender;
            InitUserCredential();
            if (settings.clientUpdateRequeried)
            {
                InitializeUpdater();
            } 
        }

        private void ChangeStatus(bool ok)
        {
            Invoke(new Action(() => {
                status_label.Text = ok ? Lm.GetString("StatusConnected") : Lm.GetString("StatusDisconected");
                login_btn.Enabled = ok;
            }));
        }
        private void EnableLoginBtn(bool state)
        {
            Invoke(new Action(() => {
                login_btn.Enabled = state;
            }));
        }
        private void FillServerList(List<ServerState> _serverList)
        {
            Invoke(new Action(() => {
                var serverCfg = LauncherConfig.GetInstance.ServerConfig;
                if (!serverCfg.OverrideServerSelection)
                {
                    server_list.Enabled = true;
                    foreach (var server in _serverList)
                    {
                        string serverStatus = server.b_ServerState == 1 ? Lm.GetString("WorldOpen") : Lm.GetString("WorlClose");
                        server_list.Items.Add(new ListViewItem(new[] { server.s_ServerName, serverStatus }));
                    }
                }
                else
                {
                    networkClient.SelectWordlRequest(serverCfg.ServerIndexSelect);
                }
            }));
        }

        private void server_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (server_list.SelectedItems.Count > 0)
            {
                var item = server_list.SelectedItems[0];
                networkClient.SelectWordlRequest((short)item.Index);
                server_list.Enabled = false;
            }
        }

        private void RunGame(Default_Set defaultSet)
        {
            var clientCfg = LauncherConfig.GetInstance.ClientConfig;
            ClientRunHelper.WriteTmp(clientCfg.DefaultSetTmpPath, defaultSet);
            ClientRunHelper.RunClient(clientCfg.ClientBinaryPath);
            networkClient.StopListen();
            Environment.Exit(0);
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            networkClient.StopListen();
            Environment.Exit(0);
        }
        public static void ForAllControls(Control parent, Action<Control> action)
        {
            foreach (Control c in parent.Controls)
            {
                action(c);
                ForAllControls(c, action);
            }
        }
    }
}

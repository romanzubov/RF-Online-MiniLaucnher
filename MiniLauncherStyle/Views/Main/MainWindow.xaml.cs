using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Network;
using MiniLauncher.Network.Packets;
using MiniLauncher.Updater;
using MiniLauncher.Utils;
using MiniLauncherStyle.Data;
using MiniLauncherStyle.Views.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiniLauncherStyle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserCredential userCredential;
        private LocalizationManager Lm;
        private NetworkClient networkClient;
        private bool connection_status { get; set; }

        private UpdateManager UpdateManager { get; set; }
        List<News> listNews { get; set; }

        public MainWindow()
        {
            connection_status = false;
            Lm = LocalizationManager.GetInstance;
            InitializeComponent();
            InitializeNetwork();
            Title = LauncherConfig.GetInstance.ServerConfig.Title;
        }

        private void InitializeNetwork()
        {
            status_label.Text = Lm.GetString("StatusConnecting");
            status_label.Foreground = new SolidColorBrush(Colors.Orange);

            var serverCfg = LauncherConfig.GetInstance.ServerConfig;
            networkClient = new NetworkClient(serverCfg.LogginAddress.Split(':')[0], int.Parse(serverCfg.LogginAddress.Split(':')[1]));
            networkClient.OnError += NetworkClient_OnError;
            networkClient.OnConnected += NetworkClient_OnConnected;
            networkClient.ClientEvents += NetworkClient_ClientEvents;
            (new Thread(() => {
                networkClient.StartClient();
            })).Start();
        }
        private void NetworkClient_OnError(object sender, EventArgs e)
        {

            ChangeStatus(false);
        }
        private void NetworkClient_OnConnected(object sender, EventArgs e)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    var address = LauncherConfig.GetInstance.ServerConfig.ServerAddress.Split(':');
                    tcpClient.Connect(address[0], int.Parse(address[1]));
                    ChangeStatus(true);
                }
                catch (Exception)
                {
                    ChangeStatus(false);
                }
            }
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
                    System.Windows.MessageBox.Show(Lm.GetString("WrongLogin"),
                        Lm.GetString("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_WRONG_PW:
                    EnableLoginBtn(true);
                    System.Windows.MessageBox.Show(Lm.GetString("WrongPassword"),
                        Lm.GetString("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_SERVER_CLOSED:
                    EnableLoginBtn(true);
                    System.Windows.MessageBox.Show(Lm.GetString("ServerTechnicalWork"),
                        Lm.GetString("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_BANNED:
                    System.Windows.MessageBox.Show(Lm.GetString("AccountBlocked"),
                        Lm.GetString("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_ALREADY_IN_GAME:
                    networkClient.StopListen(false);
                    System.Windows.MessageBox.Show("Аккаунт уже в игре!",
                        Lm.GetString("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                    break;
                case NetworkClientEventArgs.Callback.SERVER_LIST_INFORM:
                    Dispatcher.Invoke((MethodInvoker)delegate
                    {
                        if (save_checkbox.IsChecked == true)
                        {
                            userCredential.ProcessLoginData(login_input.Text, password_input.Password);
                        }
                    });
                    FillServerList(e.Servers);
                    break;
                case NetworkClientEventArgs.Callback.SERVER_SESSION_RESULT:
                    RunGame(e.DefaultSet);
                    break;
            }
        }

        private void InitUserCredential()
        {
            login_input.Items.Clear();
            login_input.Text = String.Empty;
            password_input.Password = String.Empty;
            userCredential = new UserCredential(".\\credential_storage.json");
            foreach( var login in userCredential.LoadLogins())
            {
                login_input.Items.Add(login);
            }
            if (login_input.Items.Count > 0)
            {
                login_input.SelectedIndex = 0;
                string password = userCredential.ProcessLoginData(login_input.Text);
                if (!String.IsNullOrEmpty(password))
                {
                    password_input.Password = password;
                }
            }
        }

        private void InitializeUpdater()
        {
            Bottom.Visibility = Visibility.Visible;
            if (!LauncherConfig.GetInstance.UpdateConfig.ClientUpdateEnable && !LauncherConfig.GetInstance.UpdateConfig.PatchUpdateEnable)
            {
                Bottom.Visibility = Visibility.Hidden;
                return;
            }
            check_label.Text = String.Format(Lm.GetString("update_check_label"), 0, 0);
            apply_label.Text = String.Format(Lm.GetString("update_apply_label"), 0, 0);

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
                updateTasks.Enqueue(new UpdateTask(Lm.GetString("update_box_patch"),
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


        private void UpdateManager_UpdateStart(object sender, UpdateStartedEventArgs e)
        {
            Bottom.Dispatcher.Invoke(new MethodInvoker(delegate
            {
                EnableLoginBtn(false);
                //update_box.Text = e.TaskName; 
            }));
        }

        private void UpdateDownloader_UpdateComplete(object sender, UpdateCompleteEventArgs e)
        {
            if (e.TaskName == Lm.GetString("update_box_client"))
            {
                File.Create($".//{LauncherConfig.GetInstance.ServerConfig.Title}.lock");
            }
            Bottom.Dispatcher.Invoke(new MethodInvoker(delegate
            {
                proggres_apply.Maximum = 100;
                proggres_apply.Value = 100;
                apply_label.Text = Lm.GetString("update_apply_label_done");
                if (connection_status)
                {
                    EnableLoginBtn(true);
                }
            }));
        }

        private void UpdateCheckFiles_UpdateCheckProggress(object sender, UpdateCheckEventArgs e)
        {
            if (connection_status)
            {
                EnableLoginBtn(false);
            }

            Bottom.Dispatcher.Invoke(new MethodInvoker(delegate
            {
                proggres_check.Maximum = (int)e.TotalCount;
                proggres_check.Value = (int)e.DoneCount;
                check_label.Text = String.Format(Lm.GetString("update_check_label"),e.DoneCount, e.TotalCount);
            }));
        }

        private void UpdateDownloader_UpdateDownloadProgress(object sender, UpdateDownloadEventArgs e)
        {
            Bottom.Dispatcher.Invoke(new MethodInvoker(delegate
            {
                proggres_apply.Maximum = (int)e.TotalCount;
                proggres_apply.Value = (int)e.DoneCount;
                apply_label.Text = String.Format(Lm.GetString("update_apply_label"), e.DoneCount, e.TotalCount);
            }));
        }

        private void RunGame(Default_Set defaultSet)
        {
            var clientCfg = LauncherConfig.GetInstance.ClientConfig;
            ClientRunHelper.WriteTmp(clientCfg.DefaultSetTmpPath, defaultSet);
            ClientRunHelper.RunClient(clientCfg.ClientBinaryPath);

            bool is_need_to_close = false;
            if (File.Exists(".\\R3Engine.ini"))
            {
                var ini = new IniFile(".\\R3Engine.ini");
                if (ini.KeyExists("close_launcher_after_login", "Launcher"))
                {
                    is_need_to_close = bool.Parse(ini.ReadReverse("Launcher", "close_launcher_after_login").ToLower());
                }
                else
                {
                    ini.Write("close_launcher_after_login", "FALSE", "Launcher");
                }
            }
            EnableLoginBtn(true);
            if (is_need_to_close)
            {
                networkClient.StopListen();
                Environment.Exit(0);
            }
        }

        private void FillServerList(List<ServerState> _serverList)
        {
            Dispatcher.Invoke((MethodInvoker)delegate {
                var serverCfg = LauncherConfig.GetInstance.ServerConfig;
                networkClient.SelectWordlRequest(serverCfg.ServerIndexSelect);
            });
        }
        private void ChangeStatus(bool ok)
        {
            connection_status = ok;
            Dispatcher.Invoke((MethodInvoker)delegate {
                status_label.Text = ok ? Lm.GetString("StatusConnected") : Lm.GetString("StatusDisconected");
                if (ok)
                {
                    status_on.Visibility = Visibility.Visible;
                    status_off.Visibility = Visibility.Hidden;
                    status_label.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    status_off.Visibility = Visibility.Visible;
                    status_on.Visibility = Visibility.Hidden;
                    status_label.Foreground = new SolidColorBrush(Colors.Red);
                }
                play_btn.IsEnabled = ok;
            });
        }
        private void EnableLoginBtn(bool state)
        {
            Dispatcher.Invoke((MethodInvoker)delegate {
                play_btn.IsEnabled = state;
            });
        }
        object lock_load = new object();
        bool load_stat = true;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitLocalizationChanger();
            InitUserCredential();
            InitializeUpdater();
            LoadNews();
            
            TimeZoneInfo moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            DateTime today = DateTime.UtcNow + moscowTimeZone.BaseUtcOffset;
            int addToDay = 0;
            int addToMounth = 0;
            int addToYear = 0;



            if(today.Day == 30)
            {
                addToMounth = 1;
                addToDay = 0;
            }
            else
            {
                addToDay = 1;
            }

            try
            {
                if (today.Month == 12)
                {
                    addToMounth = 0;
                    addToYear = 1;
                }
                if (today.Hour >= 6)
                {
                    voteTime = new DateTime(today.Year + addToYear, today.Month + addToMounth, today.Day + addToDay, 6, 00, 00);
                }
                else
                {
                    voteTime = new DateTime(today.Year + addToYear, today.Month, today.Day, 6, 00, 00);
                }
                if (today.Hour >= 14)
                {
                    voteTime2 = new DateTime(today.Year + addToYear, today.Month, today.Day + addToDay, 14, 00, 00);
                }
                else
                {
                    voteTime2 = new DateTime(today.Year + addToYear, today.Month, today.Day, 14, 00, 00);
                }
                if (today.Hour >= 22)
                {
                    voteTime3 = new DateTime(today.Year, today.Month, today.Day + addToDay, 2, 00, 00);
                }
                else
                {
                    voteTime3 = new DateTime(today.Year, today.Month, today.Day, 22, 00, 00);
                }
            }
            catch (Exception e2) { }

            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += timer1_Tick;
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void InitLocalizationChanger()
        {
            var nationalities = Lm.GetNationalities();

            for(int i = 0; i < nationalities.Length; i++)
            {
                var nation = nationalities[i];
                var dataTablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataTable");
                var nationPath = Path.Combine(dataTablePath, nation);
                var iconPath = Path.Combine(nationPath, "lang.png");
                var image = new BitmapImage(new Uri(iconPath, UriKind.Absolute));

                if (!File.Exists(iconPath))
                {
                    continue;
                }
                
                var item = new ComboBoxItem
                {
                    Tag = nation,
                    Content = new Image
                    {
                        Source = image,
                        Width = 18,
                        Height = 18
                    }
                };
                
                language.Items.Add(item);
                if (LauncherConfig.GetInstance.NationalConfig.NationCode.ToString().Replace('_', '-') == nation)
                {
                    language.SelectedIndex = i;
                }
            }
        }

        DateTime voteTime;
        DateTime voteTime2;
        DateTime voteTime3;
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeZoneInfo moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            DateTime today = DateTime.UtcNow + moscowTimeZone.BaseUtcOffset;

            TimeSpan TimeRemaining = voteTime - today;
            TimeSpan TimeRemaining1 = voteTime2 - today;
            TimeSpan TimeRemaining2  = voteTime3 - today;
            chip_war_time_1.Text =  string.Format("{0:D2}:{1:D2}:{2:D2} |", TimeRemaining.Hours, TimeRemaining.Minutes, TimeRemaining.Seconds);
            chip_war_time_2.Text = string.Format("{0:D2}:{1:D2}:{2:D2} |", TimeRemaining1.Hours, TimeRemaining1.Minutes, TimeRemaining1.Seconds);
            chip_war_time_3.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", TimeRemaining2.Hours, TimeRemaining2.Minutes, TimeRemaining2.Seconds);
        }

        private void LoadStat()
        {

            string data = Utils.DownloadDataFromFile(LauncherConfig.GetInstance.SocialConfig.stat_link);
                
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    Stat statData = JsonConvert.DeserializeObject<Stat>(data);
                    Dispatcher.Invoke((MethodInvoker)delegate
                    {
                        label_win_race.Text = String.Format(Lm.GetString("win_race"), statData.destroed_race);
                        label_ore_percent.Text = String.Format(Lm.GetString("ore_percent"), statData.ore_percent);
                        acc_percent.Text = String.Format(Lm.GetString("acc_chip_percent"), statData.acc);
                        bcc_percent.Text = String.Format(Lm.GetString("bcc_chip_percent"), statData.bcc);
                        ccc_percent.Text = String.Format(Lm.GetString("ccc_chip_percent"), statData.ccc);
                    });
                }
                catch (Exception)
                {
                    Dispatcher.Invoke((MethodInvoker)delegate
                    {
                        menu_stat_btn.TextDecorations = TextDecorations.Strikethrough;
                        menu_login_btn.TextDecorations = TextDecorations.Underline;
                        menu_stat_btn.IsEnabled = false;
                        StatBlock.IsEnabled = false;
                        LoginBlock.IsEnabled = true;
                    });
                }
            }
            else
            {
                Dispatcher.Invoke((MethodInvoker)delegate
                {
                    menu_stat_btn.TextDecorations = TextDecorations.Strikethrough;
                    menu_login_btn.TextDecorations = TextDecorations.Underline;
                    menu_stat_btn.IsEnabled = false;
                    StatBlock.IsEnabled = false;
                    LoginBlock.IsEnabled = true;
                });
            }
        }
        private void LoadNews()
        {
            (new Thread(() => {
                string data = Utils.DownloadDataFromFile(LauncherConfig.GetInstance.SocialConfig.news_link);
                if (string.IsNullOrEmpty(data))
                {
                    Dispatcher.Invoke((MethodInvoker)delegate {
                        news_label.Visibility = Visibility.Hidden;
                        news1.Visibility = Visibility.Hidden;
                        news2.Visibility = Visibility.Hidden;
                        news3.Visibility = Visibility.Hidden;

                        menu_stat_btn.TextDecorations = TextDecorations.Strikethrough;
                        menu_login_btn.TextDecorations = TextDecorations.Underline;
                        menu_stat_btn.IsEnabled = false;
                        StatBlock.IsEnabled = false;
                        LoginBlock.IsEnabled = true;
                    });

                    return;
                }
                
                try { 
                    listNews = JsonConvert.DeserializeObject<List<News>>(data);
                    if(listNews == null)
                    {
                        Dispatcher.Invoke((MethodInvoker)delegate {
                            news_label.Visibility = Visibility.Hidden;
                            news1.Visibility = Visibility.Hidden;
                            news2.Visibility = Visibility.Hidden;
                            news3.Visibility = Visibility.Hidden;
                        });
                        return;
                    }
                    Dispatcher.Invoke((MethodInvoker)delegate {
                        if(listNews.Count == 1)
                        {
                            news_loader_1.Visibility = Visibility.Hidden;
                            news_head_1.Text = listNews[0].text;
                            news_author_1.Text = listNews[0].author + " | " + listNews[0].datetime;
                            news2.Visibility = Visibility.Hidden;
                            news3.Visibility = Visibility.Hidden;
                        }
                        if (listNews.Count == 2)
                        {
                            news_loader_1.Visibility = Visibility.Hidden;
                            news_loader_2.Visibility = Visibility.Hidden;
                            news_head_1.Text = listNews[0].text;
                            news_author_1.Text = listNews[0].author + " | " + listNews[0].datetime;
                            news_head_2.Text = listNews[1].text;
                            news_author_2.Text = listNews[1].author + " | " + listNews[1].datetime;
                            news3.Visibility = Visibility.Hidden;
                        }
                        if (listNews.Count >= 3)
                        {
                            news_loader_1.Visibility = Visibility.Hidden;
                            news_loader_2.Visibility = Visibility.Hidden;
                            news_loader_3.Visibility = Visibility.Hidden;
                            news_head_1.Text = listNews[0].text;
                            news_author_1.Text = listNews[0].author + " | " + listNews[0].datetime;
                            news_head_2.Text = listNews[1].text;
                            news_author_2.Text = listNews[1].author + " | " + listNews[1].datetime;
                            news_head_3.Text = listNews[2].text;
                            news_author_3.Text = listNews[2].author + " | " + listNews[2].datetime;
                        }
                    });
                    LoadStat();
                }
                catch (Exception)
                {
                    Dispatcher.Invoke((MethodInvoker)delegate {
                        news_label.Visibility = Visibility.Hidden;
                        news1.Visibility = Visibility.Hidden;
                        news2.Visibility = Visibility.Hidden;
                        news3.Visibility = Visibility.Hidden;
                    });
                    return;
                }
                
            })).Start();
        }

        private void Drag_Layout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Environment.Exit(0);
        }

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow(Top,Left);
            settings.Closing += Settings_Closing;
            settings.Show();
        }

        private void Settings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = (SettingsWindow)sender;
            InitUserCredential();
            if (settings.clientUpdateRequeried)
            {
                InitializeUpdater();
            }
        }

        private void Login_input_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count > 0)
            {
                if (String.IsNullOrEmpty(e.AddedItems[0].ToString()))
                {
                    return;
                }
                string password = userCredential.ProcessLoginData(e.AddedItems[0].ToString());
                if (!String.IsNullOrEmpty(password))
                {
                    password_input.Password = password;
                }
            }
        }

        private void Play_btn_Click(object sender, RoutedEventArgs e)
        {

            if (login_input.Text.Length > 13)
            {
                System.Windows.Forms.MessageBox.Show(Lm.GetString("LoginPasswordCheck"),
                    Lm.GetString("Error"));
                return;
            }

            if (password_input.Password.Length > 13)
            {
                System.Windows.Forms.MessageBox.Show(Lm.GetString("LoginPasswordCheck"),
                    Lm.GetString("Error"));
                return;
            }
            if (!string.IsNullOrEmpty(login_input.Text) && !string.IsNullOrEmpty(password_input.Password))
            {
                networkClient.DoLogin(login_input.Text, password_input.Password);
                EnableLoginBtn(false);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Lm.GetString("LoginPasswordCheck"),
                    Lm.GetString("Error"));
            }
        }
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            networkClient.StopListen(true);
            Environment.Exit(0);
        }

        private void News1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (listNews != null && listNews.Count >= 3)
                {
                    Process.Start(listNews[0].link);
                }
            }
        }

        private void News2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed) { 
                if (listNews != null && listNews.Count >= 3)
                {
                    Process.Start(listNews[1].link);
                }
            }
        }

        private void News3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (listNews != null && listNews.Count >= 3)
                {
                    Process.Start(listNews[2].link);
                }
            }
        }

        private void Menu_login_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LoginBlock.IsEnabled = true;
                StatBlock.IsEnabled = false;
                menu_login_btn.TextDecorations = TextDecorations.Underline;
                menu_stat_btn.TextDecorations = null;
            }
        }
        private void Menu_stat_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LoginBlock.IsEnabled = false;
                StatBlock.IsEnabled = true;
                menu_login_btn.TextDecorations = null;
                menu_stat_btn.TextDecorations = TextDecorations.Underline;
            }
        }

        private void Forum_btn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(LauncherConfig.GetInstance.SocialConfig.forum_link);
        }

        private void Bd_btn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(LauncherConfig.GetInstance.SocialConfig.bd_link);
        }

        private void Vk_btn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(LauncherConfig.GetInstance.SocialConfig.vk_link);
        }

        private void Craft_btn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(LauncherConfig.GetInstance.SocialConfig.craft_link);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Process.Start(LauncherConfig.GetInstance.SocialConfig.register_link);
            }
        }

        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var configuration = new IniFile(".\\R3Engine.ini");

            if (language.SelectedItem is ComboBoxItem item)
            {
                var nation = item.Tag.ToString().Replace('-', '_');
                
                var encoded = EncodeNationCode(nation);

                if (nation == LauncherConfig.GetInstance.NationalConfig.NationCode.ToString())
                {
                    return;
                }
                
                configuration.Write("Language", encoded, "Setup");
                
                string exePath = Process.GetCurrentProcess().MainModule?.FileName;

                Process.Start(new ProcessStartInfo(exePath)
                {
                    UseShellExecute = true
                });

                System.Windows.Application.Current.Shutdown();
            }
        }
        
        private string EncodeNationCode(string code)
        {
            switch (code)
            {
                case "ko_kr":
                    return "Korea";

                case "pt_br":
                    return "Brazil";

                case "zn_cn":
                    return "China";

                case "en_gb":
                    return "Europe";

                case "en_id":
                    return "Indonesia";

                case "ja_jp":
                    return "Japan";

                case "en_ph":
                    return "Philippines";

                case "ru_ru":
                    return "Russia";

                case "zh_tw":
                    return "Taiwan";

                case "es_es":
                    return "Spain";

                case "th_th":
                    return "Thailand";

                default:
                    return "Russia";
            }
        }
    }
}

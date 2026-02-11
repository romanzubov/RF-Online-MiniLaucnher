using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Network;
using MiniLauncher.Network.Packets;
using MiniLauncher.Updater;
using MiniLauncher.Utils;
using MiniLauncherStyle.Core;
using MiniLauncherStyle.Data;
using MiniLauncherStyle.Helper;
using MiniLauncherStyle.Services;
using MiniLauncherStyle.Services.Interfaces;
using MiniLauncherStyle.ViewModels;
using MiniLauncherStyle.Views.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Главное окно лаунчера.
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserCredential userCredential;
        private LocalizationManager Lm;
        private NetworkClient networkClient;
        private bool connectionStatus;

        private UpdateManager UpdateManager { get; set; }
        private List<NewsItem> newsList;
        
        private MainViewModel ViewModel { get; set; }
        
        // Сервисы
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IGameService _gameService;

        public MainWindow()
        {
            connectionStatus = false;
            Lm = LocalizationManager.GetInstance;
            
            // Получение сервисов из DI контейнера
            var contentService = ServiceLocator.Current.Get<IContentService>();
            _dialogService = ServiceLocator.Current.Get<IDialogService>();
            _navigationService = ServiceLocator.Current.Get<INavigationService>();
            _gameService = ServiceLocator.Current.Get<IGameService>();
            
            // Создание ViewModel
            ViewModel = new MainViewModel(contentService, _dialogService);
            
            InitializeComponent();
            
            // Установка DataContext для bindings
            DataContext = ViewModel;
            
            // Устанавливаем ссылку на главное окно в NavigationService
            _navigationService.SetMainWindow(this);
            
            InitializeNetwork();
        }

        private void InitializeNetwork()
        {
            ViewModel.ConnectionStatusText = Lm.GetString("StatusConnecting");
            ViewModel.StatusColor = "Orange";

            var serverCfg = LauncherConfig.GetInstance.ServerConfig;
            networkClient = new NetworkClient(serverCfg.LogginAddress.Split(':')[0], int.Parse(serverCfg.LogginAddress.Split(':')[1]));
            networkClient.OnError += NetworkClient_OnError;
            networkClient.OnConnected += NetworkClient_OnConnected;
            networkClient.ClientEvents += NetworkClient_ClientEvents;
            
            // Используем BackgroundWorker вместо raw Thread
            var worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += (sender, e) => networkClient.StartClient();
            worker.RunWorkerAsync();
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
                    _dialogService.ShowError(Lm.GetString("WrongLogin"), Lm.GetString("Error"));
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_WRONG_PW:
                    EnableLoginBtn(true);
                    _dialogService.ShowError(Lm.GetString("WrongPassword"), Lm.GetString("Error"));
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_SERVER_CLOSED:
                    EnableLoginBtn(true);
                    _dialogService.ShowError(Lm.GetString("ServerTechnicalWork"), Lm.GetString("Error"));
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_BANNED:
                    _dialogService.ShowError(Lm.GetString("AccountBlocked"), Lm.GetString("Error"));
                    break;
                case NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_ALREADY_IN_GAME:
                    networkClient.StopListen(false);
                    _dialogService.ShowError("Аккаунт уже в игре!", Lm.GetString("Error"));
                    _navigationService.ExitApplication();
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
            ViewModel.IsUpdateVisible = true;
            
            if (!LauncherConfig.GetInstance.UpdateConfig.ClientUpdateEnable && !LauncherConfig.GetInstance.UpdateConfig.PatchUpdateEnable)
            {
                ViewModel.IsUpdateVisible = false;
                return;
            }
            
            // Инициализация прогресса через ViewModel
            ViewModel.CheckProgressText = String.Format(Lm.GetString("update_check_label"), 0, 0);
            ViewModel.ApplyProgressText = String.Format(Lm.GetString("update_apply_label"), 0, 0);

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
                // Обновляем через ViewModel
                ViewModel.ApplyProgressMax = 100;
                ViewModel.ApplyProgressValue = 100;
                ViewModel.ApplyProgressText = Lm.GetString("update_apply_label_done");
                if (connectionStatus)
                {
                    EnableLoginBtn(true);
                }
            }));
        }

        private void UpdateCheckFiles_UpdateCheckProggress(object sender, UpdateCheckEventArgs e)
        {
            if (connectionStatus)
            {
                EnableLoginBtn(false);
            }

            Bottom.Dispatcher.Invoke(new MethodInvoker(delegate
            {
                // Обновляем через ViewModel
                ViewModel.UpdateCheckProgress(e.DoneCount, e.TotalCount);
            }));
        }

        private void UpdateDownloader_UpdateDownloadProgress(object sender, UpdateDownloadEventArgs e)
        {
            Bottom.Dispatcher.Invoke(new MethodInvoker(delegate
            {
                // Обновляем через ViewModel
                ViewModel.UpdateApplyProgress(e.DoneCount, e.TotalCount);
            }));
        }

        private void RunGame(Default_Set defaultSet)
        {
            // Запуск клиента через сервис
            _gameService.RunGameClient(defaultSet);

            EnableLoginBtn(true);
            
            // Проверяем, нужно ли закрыть лаунчер
            if (_gameService.ShouldCloseLauncherAfterLogin())
            {
                networkClient.StopListen();
                _navigationService.ExitApplication();
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
            connectionStatus = ok;
            Dispatcher.Invoke((MethodInvoker)delegate {
                // Обновляем ViewModel
                ViewModel.IsConnected = ok;
                ViewModel.ConnectionStatusText = ok ? Lm.GetString("StatusConnected") : Lm.GetString("StatusDisconected");
                ViewModel.IsLoginEnabled = ok;
                
                // Обновляем статус-иконки через ViewModel
                ViewModel.StatusOnVisible = ok;
                ViewModel.StatusOffVisible = !ok;
                ViewModel.StatusColor = ok ? "Green" : "Red";
            });
        }
        private void EnableLoginBtn(bool state)
        {
            Dispatcher.Invoke((MethodInvoker)delegate {
                ViewModel.IsLoginEnabled = state;
            });
        }

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
            catch (Exception)
            {
                // Игнорируем ошибки при расчете времени
            }

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
            ViewModel.ChipWarTime1 = string.Format("{0:D2}:{1:D2}:{2:D2} |", TimeRemaining.Hours, TimeRemaining.Minutes, TimeRemaining.Seconds);
            ViewModel.ChipWarTime2 = string.Format("{0:D2}:{1:D2}:{2:D2} |", TimeRemaining1.Hours, TimeRemaining1.Minutes, TimeRemaining1.Seconds);
            ViewModel.ChipWarTime3 = string.Format("{0:D2}:{1:D2}:{2:D2}", TimeRemaining2.Hours, TimeRemaining2.Minutes, TimeRemaining2.Seconds);
        }

        private void LoadStat()
        {
            var result = ContentService.LoadStatistics();
                
            if (result.Success)
            {
                var statData = result.Data;
                // Сохраняем в ViewModel
                ViewModel.Statistics = statData;
                
                Dispatcher.Invoke((MethodInvoker)delegate
                {
                    ViewModel.WinRaceText = String.Format(Lm.GetString("win_race"), statData.DestroyedRace);
                    ViewModel.OrePercentText = String.Format(Lm.GetString("ore_percent"), statData.OrePercent);
                    ViewModel.AccPercentText = String.Format(Lm.GetString("acc_chip_percent"), statData.AccPercent);
                    ViewModel.BccPercentText = String.Format(Lm.GetString("bcc_chip_percent"), statData.BccPercent);
                    ViewModel.CccPercentText = String.Format(Lm.GetString("ccc_chip_percent"), statData.CccPercent);
                });
            }
            else
            {
                DisableStatBlock();
            }
        }

        private void DisableStatBlock()
        {
            Dispatcher.Invoke((MethodInvoker)delegate
            {
                ViewModel.IsStatMenuEnabled = false;
                ViewModel.IsStatMenuUnderlined = false;
                ViewModel.IsLoginMenuUnderlined = true;
                ViewModel.IsStatBlockEnabled = false;
                ViewModel.IsLoginBlockEnabled = true;
            });
        }

        private void LoadNews()
        {
            var worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += (sender, e) => 
            {
                var result = ContentService.LoadNews();
                e.Result = result;
            };
            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    HideNewsSection();
                    DisableStatBlock();
                    return;
                }
                
                var result = (DataLoadResult<List<NewsItem>>)e.Result;
                if (!result.Success)
                {
                    HideNewsSection();
                    DisableStatBlock();
                    return;
                }
                
                newsList = result.Data;
                ViewModel.NewsList = newsList;
                DisplayNews(newsList);
                
                // Загружаем статистику в отдельном BackgroundWorker
                var statWorker = new System.ComponentModel.BackgroundWorker();
                statWorker.DoWork += (s, args) => LoadStat();
                statWorker.RunWorkerAsync();
            };
            worker.RunWorkerAsync();
        }

        private void HideNewsSection()
        {
            Dispatcher.Invoke((MethodInvoker)delegate 
            {
                ViewModel.NewsSectionVisible = false;
                ViewModel.News1Visible = false;
                ViewModel.News2Visible = false;
                ViewModel.News3Visible = false;
            });
        }

        private void DisplayNews(List<NewsItem> news)
        {
            Dispatcher.Invoke((MethodInvoker)delegate 
            {
                if (news.Count >= 1)
                {
                    ViewModel.News1LoaderVisible = false;
                    ViewModel.News1Head = news[0].Text;
                    ViewModel.News1Author = news[0].Author + " | " + news[0].DateTime;
                }
                if (news.Count >= 2)
                {
                    ViewModel.News2LoaderVisible = false;
                    ViewModel.News2Head = news[1].Text;
                    ViewModel.News2Author = news[1].Author + " | " + news[1].DateTime;
                }
                if (news.Count >= 3)
                {
                    ViewModel.News3LoaderVisible = false;
                    ViewModel.News3Head = news[2].Text;
                    ViewModel.News3Author = news[2].Author + " | " + news[2].DateTime;
                }
                
                // Скрыть неиспользуемые блоки через ViewModel
                if (news.Count < 3) ViewModel.News3Visible = false;
                if (news.Count < 2) ViewModel.News2Visible = false;
                if (news.Count < 1) ViewModel.News1Visible = false;
            });
        }

        private void Drag_Layout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            _navigationService.ExitApplication();
        }

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.ShowSettings((clientUpdateRequired) =>
            {
                InitUserCredential();
                if (clientUpdateRequired)
                {
                    InitializeUpdater();
                }
            });
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
                _dialogService.ShowError(Lm.GetString("LoginPasswordCheck"), Lm.GetString("Error"));
                return;
            }

            if (password_input.Password.Length > 13)
            {
                _dialogService.ShowError(Lm.GetString("LoginPasswordCheck"), Lm.GetString("Error"));
                return;
            }
            if (!string.IsNullOrEmpty(login_input.Text) && !string.IsNullOrEmpty(password_input.Password))
            {
                networkClient.DoLogin(login_input.Text, password_input.Password);
                EnableLoginBtn(false);
            }
            else
            {
                _dialogService.ShowError(Lm.GetString("LoginPasswordCheck"), Lm.GetString("Error"));
            }
        }
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            networkClient.StopListen(true);
            _navigationService.ExitApplication();
        }

        private void News1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (newsList != null && newsList.Count >= 1)
                {
                    _dialogService.OpenUrl(newsList[0].Link);
                }
            }
        }

        private void News2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (newsList != null && newsList.Count >= 2)
                {
                    _dialogService.OpenUrl(newsList[1].Link);
                }
            }
        }

        private void News3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (newsList != null && newsList.Count >= 3)
                {
                    _dialogService.OpenUrl(newsList[2].Link);
                }
            }
        }

        private void Menu_login_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ViewModel.IsLoginBlockEnabled = true;
                ViewModel.IsStatBlockEnabled = false;
                ViewModel.IsLoginMenuUnderlined = true;
                ViewModel.IsStatMenuUnderlined = false;
            }
        }
        private void Menu_stat_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ViewModel.IsLoginBlockEnabled = false;
                ViewModel.IsStatBlockEnabled = true;
                ViewModel.IsLoginMenuUnderlined = false;
                ViewModel.IsStatMenuUnderlined = true;
            }
        }

        private void Forum_btn_Click(object sender, RoutedEventArgs e)
        {
            _dialogService.OpenUrl(LauncherConfig.GetInstance.SocialConfig.forum_link);
        }

        private void Bd_btn_Click(object sender, RoutedEventArgs e)
        {
            _dialogService.OpenUrl(LauncherConfig.GetInstance.SocialConfig.bd_link);
        }

        private void Vk_btn_Click(object sender, RoutedEventArgs e)
        {
            _dialogService.OpenUrl(LauncherConfig.GetInstance.SocialConfig.vk_link);
        }

        private void Craft_btn_Click(object sender, RoutedEventArgs e)
        {
            _dialogService.OpenUrl(LauncherConfig.GetInstance.SocialConfig.craft_link);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _dialogService.OpenUrl(LauncherConfig.GetInstance.SocialConfig.register_link);
            }
        }

        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var configuration = new IniFile(".\\R3Engine.ini");

            if (language.SelectedItem is ComboBoxItem item)
            {
                var nation = item.Tag.ToString().Replace('-', '_');
                
                var encoded = NationCodeHelper.EncodeNationCode(nation);

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
    }
}

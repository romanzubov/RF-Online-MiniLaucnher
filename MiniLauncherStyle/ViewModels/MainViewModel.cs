using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Network;
using MiniLauncher.Network.Packets;
using MiniLauncher.Utils;
using MiniLauncherStyle.Core;
using MiniLauncherStyle.Data;
using MiniLauncherStyle.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace MiniLauncherStyle.ViewModels
{
    /// <summary>
    /// ViewModel для главного окна лаунчера.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContentService _contentService;
        private readonly IDialogService _dialogService;
        private readonly LocalizationManager _localization;
        
        private string _login;
        private string _password;
        private bool _rememberLogin;
        private bool _isConnected;
        private string _connectionStatusText;
        private bool _isLoginEnabled;
        private string _windowTitle;
        private List<NewsItem> _newsList;
        
        // Update progress
        private int _checkProgressMax;
        private int _checkProgressValue;
        private string _checkProgressText;
        private int _applyProgressMax;
        private int _applyProgressValue;
        private string _applyProgressText;
        private bool _isUpdateVisible;
        
        // News
        private string _news1Head;
        private string _news1Author;
        private bool _news1LoaderVisible = true;
        private bool _news1Visible = true;
        private string _news2Head;
        private string _news2Author;
        private bool _news2LoaderVisible = true;
        private bool _news2Visible = true;
        private string _news3Head;
        private string _news3Author;
        private bool _news3LoaderVisible = true;
        private bool _news3Visible = true;
        private bool _newsSectionVisible = true;
        
        // Status icons
        private bool _statusOnVisible;
        private bool _statusOffVisible = true;
        private string _statusColor = "Orange";

        #endregion

        #region Properties

        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value, "Login"); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value, "Password"); }
        }

        public bool RememberLogin
        {
            get { return _rememberLogin; }
            set { SetProperty(ref _rememberLogin, value, "RememberLogin"); }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (SetProperty(ref _isConnected, value, "IsConnected"))
                {
                    UpdateConnectionStatus();
                }
            }
        }

        public string ConnectionStatusText
        {
            get { return _connectionStatusText; }
            set { SetProperty(ref _connectionStatusText, value, "ConnectionStatusText"); }
        }

        public bool IsLoginEnabled
        {
            get { return _isLoginEnabled; }
            set { SetProperty(ref _isLoginEnabled, value, "IsLoginEnabled"); }
        }

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value, "WindowTitle"); }
        }

        public List<NewsItem> NewsList
        {
            get { return _newsList; }
            set { SetProperty(ref _newsList, value, "NewsList"); }
        }

        // Update progress properties
        public int CheckProgressMax
        {
            get { return _checkProgressMax; }
            set { SetProperty(ref _checkProgressMax, value, "CheckProgressMax"); }
        }

        public int CheckProgressValue
        {
            get { return _checkProgressValue; }
            set { SetProperty(ref _checkProgressValue, value, "CheckProgressValue"); }
        }

        public string CheckProgressText
        {
            get { return _checkProgressText; }
            set { SetProperty(ref _checkProgressText, value, "CheckProgressText"); }
        }

        public int ApplyProgressMax
        {
            get { return _applyProgressMax; }
            set { SetProperty(ref _applyProgressMax, value, "ApplyProgressMax"); }
        }

        public int ApplyProgressValue
        {
            get { return _applyProgressValue; }
            set { SetProperty(ref _applyProgressValue, value, "ApplyProgressValue"); }
        }

        public string ApplyProgressText
        {
            get { return _applyProgressText; }
            set { SetProperty(ref _applyProgressText, value, "ApplyProgressText"); }
        }

        public bool IsUpdateVisible
        {
            get { return _isUpdateVisible; }
            set { SetProperty(ref _isUpdateVisible, value, "IsUpdateVisible"); }
        }

        // News properties
        public string News1Head
        {
            get { return _news1Head; }
            set { SetProperty(ref _news1Head, value, "News1Head"); }
        }

        public string News1Author
        {
            get { return _news1Author; }
            set { SetProperty(ref _news1Author, value, "News1Author"); }
        }

        public bool News1LoaderVisible
        {
            get { return _news1LoaderVisible; }
            set { SetProperty(ref _news1LoaderVisible, value, "News1LoaderVisible"); }
        }

        public bool News1Visible
        {
            get { return _news1Visible; }
            set { SetProperty(ref _news1Visible, value, "News1Visible"); }
        }

        public string News2Head
        {
            get { return _news2Head; }
            set { SetProperty(ref _news2Head, value, "News2Head"); }
        }

        public string News2Author
        {
            get { return _news2Author; }
            set { SetProperty(ref _news2Author, value, "News2Author"); }
        }

        public bool News2LoaderVisible
        {
            get { return _news2LoaderVisible; }
            set { SetProperty(ref _news2LoaderVisible, value, "News2LoaderVisible"); }
        }

        public bool News2Visible
        {
            get { return _news2Visible; }
            set { SetProperty(ref _news2Visible, value, "News2Visible"); }
        }

        public string News3Head
        {
            get { return _news3Head; }
            set { SetProperty(ref _news3Head, value, "News3Head"); }
        }

        public string News3Author
        {
            get { return _news3Author; }
            set { SetProperty(ref _news3Author, value, "News3Author"); }
        }

        public bool News3LoaderVisible
        {
            get { return _news3LoaderVisible; }
            set { SetProperty(ref _news3LoaderVisible, value, "News3LoaderVisible"); }
        }

        public bool News3Visible
        {
            get { return _news3Visible; }
            set { SetProperty(ref _news3Visible, value, "News3Visible"); }
        }

        public bool NewsSectionVisible
        {
            get { return _newsSectionVisible; }
            set { SetProperty(ref _newsSectionVisible, value, "NewsSectionVisible"); }
        }

        // Status icon properties
        public bool StatusOnVisible
        {
            get { return _statusOnVisible; }
            set { SetProperty(ref _statusOnVisible, value, "StatusOnVisible"); }
        }

        public bool StatusOffVisible
        {
            get { return _statusOffVisible; }
            set { SetProperty(ref _statusOffVisible, value, "StatusOffVisible"); }
        }

        public string StatusColor
        {
            get { return _statusColor; }
            set { SetProperty(ref _statusColor, value, "StatusColor"); }
        }

        #endregion

        #region Commands

        public ICommand PlayCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand OpenUrlCommand { get; private set; }

        #endregion

        #region Constructor

        public MainViewModel(IContentService contentService, IDialogService dialogService)
        {
            _contentService = contentService;
            _dialogService = dialogService;
            _localization = LocalizationManager.GetInstance;

            InitializeCommands();
            InitializeDefaults();
        }

        #endregion

        #region Initialization

        private void InitializeCommands()
        {
            PlayCommand = new RelayCommand(ExecutePlay, CanExecutePlay);
            SettingsCommand = new RelayCommand(ExecuteSettings);
            ExitCommand = new RelayCommand(ExecuteExit);
            OpenUrlCommand = new RelayCommand<string>(ExecuteOpenUrl);
        }

        private void InitializeDefaults()
        {
            WindowTitle = LauncherConfig.GetInstance.ServerConfig.Title;
            ConnectionStatusText = _localization.GetString("StatusConnecting");
            IsLoginEnabled = false;
            IsConnected = false;
            IsUpdateVisible = true;
            
            CheckProgressText = string.Format(_localization.GetString("update_check_label"), 0, 0);
            ApplyProgressText = string.Format(_localization.GetString("update_apply_label"), 0, 0);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Загружает новости асинхронно.
        /// </summary>
        public void LoadNewsAsync()
        {
            _contentService.LoadNewsAsync(result =>
            {
                if (result.Success)
                {
                    NewsList = result.Data;
                }
            });
        }

        /// <summary>
        /// Обновляет прогресс проверки обновлений.
        /// </summary>
        public void UpdateCheckProgress(long done, long total)
        {
            CheckProgressMax = (int)total;
            CheckProgressValue = (int)done;
            CheckProgressText = string.Format(_localization.GetString("update_check_label"), done, total);
        }

        /// <summary>
        /// Обновляет прогресс применения обновлений.
        /// </summary>
        public void UpdateApplyProgress(long done, long total)
        {
            ApplyProgressMax = (int)total;
            ApplyProgressValue = (int)done;
            ApplyProgressText = string.Format(_localization.GetString("update_apply_label"), done, total);
        }

        /// <summary>
        /// Обработка завершения обновления.
        /// </summary>
        public void OnUpdateComplete()
        {
            ApplyProgressMax = 100;
            ApplyProgressValue = 100;
            ApplyProgressText = _localization.GetString("update_apply_label_done");
            
            if (IsConnected)
            {
                IsLoginEnabled = true;
            }
        }

        #endregion

        #region Private Methods

        private void UpdateConnectionStatus()
        {
            ConnectionStatusText = _isConnected
                ? _localization.GetString("StatusConnected")
                : _localization.GetString("StatusDisconected");
        }

        private bool CanExecutePlay(object parameter)
        {
            return IsLoginEnabled && IsConnected;
        }

        private void ExecutePlay(object parameter)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                _dialogService.ShowError(
                    _localization.GetString("EmptyCredentials"),
                    _localization.GetString("Error"));
                return;
            }

            IsLoginEnabled = false;
            // Событие для View - выполнить вход
            OnLoginRequested();
        }

        private void ExecuteSettings(object parameter)
        {
            // Событие для View - открыть настройки
            OnSettingsRequested();
        }

        private void ExecuteExit(object parameter)
        {
            // Событие для View - выход
            OnExitRequested();
        }

        private void ExecuteOpenUrl(string url)
        {
            _dialogService.OpenUrl(url);
        }

        #endregion

        #region Events for View

        public event EventHandler LoginRequested;
        public event EventHandler SettingsRequested;
        public event EventHandler ExitRequested;

        protected virtual void OnLoginRequested()
        {
            LoginRequested?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSettingsRequested()
        {
            SettingsRequested?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnExitRequested()
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}

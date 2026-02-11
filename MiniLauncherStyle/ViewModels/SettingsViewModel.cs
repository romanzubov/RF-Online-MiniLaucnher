using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncherStyle.Core;
using MiniLauncherStyle.Data;
using MiniLauncherStyle.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace MiniLauncherStyle.ViewModels
{
    /// <summary>
    /// ViewModel для окна настроек.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        #region Constants

        private const string ConfigFileName = ".\\R3Engine.ini";
        private const string CredentialStoragePath = ".\\credential_storage.json";
        
        private static readonly float[] GammaValues = { 0.8f, 1.0f, 1.2f, 1.4f, 1.6f, 1.8f };

        #endregion

        #region Fields

        private readonly IDialogService _dialogService;
        private readonly LocalizationManager _localization;
        private R3EngineSettings _config;

        // Video settings
        private List<string> _videoAdapters;
        private List<string> _resolutions;
        private int _selectedVideoAdapterIndex;
        private int _selectedResolutionIndex;
        
        // Quality settings
        private int _textureQuality;
        private int _lightningQuality;
        private int _shadowQuality;
        private int _illuminationQuality;
        private int _gammaIndex;
        
        // Labels
        private string _textureQualityLabel;
        private string _lightningQualityLabel;
        private string _shadowQualityLabel;
        private string _illuminationQualityLabel;
        private string _gammaLabel;
        
        // Checkboxes
        private bool _isWindowMode;
        private bool _isMouseAcceleration;
        private bool _isDetailTexture;
        private bool _isMusicEnabled;
        private bool _isSoundEnabled;
        private bool _closeLauncherAfterLogin;
        
        private bool _clientUpdateRequired;
        private bool _isRepairClientEnabled;

        #endregion

        #region Properties

        public List<string> VideoAdapters
        {
            get { return _videoAdapters; }
            set { SetProperty(ref _videoAdapters, value, "VideoAdapters"); }
        }

        public List<string> Resolutions
        {
            get { return _resolutions; }
            set { SetProperty(ref _resolutions, value, "Resolutions"); }
        }

        public int SelectedVideoAdapterIndex
        {
            get { return _selectedVideoAdapterIndex; }
            set { SetProperty(ref _selectedVideoAdapterIndex, value, "SelectedVideoAdapterIndex"); }
        }

        public int SelectedResolutionIndex
        {
            get { return _selectedResolutionIndex; }
            set { SetProperty(ref _selectedResolutionIndex, value, "SelectedResolutionIndex"); }
        }

        public int TextureQuality
        {
            get { return _textureQuality; }
            set
            {
                if (SetProperty(ref _textureQuality, value, "TextureQuality"))
                {
                    TextureQualityLabel = TextureQualityLabels[value];
                    if (_config != null) _config.TextureDetail = value;
                }
            }
        }

        public int LightningQuality
        {
            get { return _lightningQuality; }
            set
            {
                if (SetProperty(ref _lightningQuality, value, "LightningQuality"))
                {
                    LightningQualityLabel = LightingShadowQualityLabels[value];
                    if (_config != null) _config.DynamicLight = value;
                }
            }
        }

        public int ShadowQuality
        {
            get { return _shadowQuality; }
            set
            {
                if (SetProperty(ref _shadowQuality, value, "ShadowQuality"))
                {
                    ShadowQualityLabel = LightingShadowQualityLabels[value];
                    if (_config != null) _config.ShadowDetail = value;
                }
            }
        }

        public int IlluminationQuality
        {
            get { return _illuminationQuality; }
            set
            {
                if (SetProperty(ref _illuminationQuality, value, "IlluminationQuality"))
                {
                    IlluminationQualityLabel = IlluminationQualityLabels[value];
                    if (_config != null) _config.BboShasi = value;
                }
            }
        }

        public int GammaIndex
        {
            get { return _gammaIndex; }
            set
            {
                if (SetProperty(ref _gammaIndex, value, "GammaIndex"))
                {
                    GammaLabel = GammaValues[value].ToString();
                    if (_config != null) _config.Gamma = GammaValues[value];
                }
            }
        }

        // Labels
        public string TextureQualityLabel
        {
            get { return _textureQualityLabel; }
            set { SetProperty(ref _textureQualityLabel, value, "TextureQualityLabel"); }
        }

        public string LightningQualityLabel
        {
            get { return _lightningQualityLabel; }
            set { SetProperty(ref _lightningQualityLabel, value, "LightningQualityLabel"); }
        }

        public string ShadowQualityLabel
        {
            get { return _shadowQualityLabel; }
            set { SetProperty(ref _shadowQualityLabel, value, "ShadowQualityLabel"); }
        }

        public string IlluminationQualityLabel
        {
            get { return _illuminationQualityLabel; }
            set { SetProperty(ref _illuminationQualityLabel, value, "IlluminationQualityLabel"); }
        }

        public string GammaLabel
        {
            get { return _gammaLabel; }
            set { SetProperty(ref _gammaLabel, value, "GammaLabel"); }
        }

        // Checkboxes
        public bool IsWindowMode
        {
            get { return _isWindowMode; }
            set
            {
                if (SetProperty(ref _isWindowMode, value, "IsWindowMode"))
                {
                    if (_config != null) _config.IsFullScreen = value;
                }
            }
        }

        public bool IsMouseAcceleration
        {
            get { return _isMouseAcceleration; }
            set
            {
                if (SetProperty(ref _isMouseAcceleration, value, "IsMouseAcceleration"))
                {
                    if (_config != null) _config.IsMouseAccelerationEnabled = value;
                }
            }
        }

        public bool IsDetailTexture
        {
            get { return _isDetailTexture; }
            set
            {
                if (SetProperty(ref _isDetailTexture, value, "IsDetailTexture"))
                {
                    if (_config != null) _config.IsDetailTextureEnabled = value;
                }
            }
        }

        public bool IsMusicEnabled
        {
            get { return _isMusicEnabled; }
            set
            {
                if (SetProperty(ref _isMusicEnabled, value, "IsMusicEnabled"))
                {
                    if (_config != null) _config.IsMusicEnabled = value;
                }
            }
        }

        public bool IsSoundEnabled
        {
            get { return _isSoundEnabled; }
            set
            {
                if (SetProperty(ref _isSoundEnabled, value, "IsSoundEnabled"))
                {
                    if (_config != null) _config.IsSoundEnabled = value;
                }
            }
        }

        public bool CloseLauncherAfterLogin
        {
            get { return _closeLauncherAfterLogin; }
            set
            {
                if (SetProperty(ref _closeLauncherAfterLogin, value, "CloseLauncherAfterLogin"))
                {
                    if (_config != null) _config.CloseLauncherAfterLogin = value;
                }
            }
        }

        public bool ClientUpdateRequired
        {
            get { return _clientUpdateRequired; }
            set { SetProperty(ref _clientUpdateRequired, value, "ClientUpdateRequired"); }
        }

        public bool IsRepairClientEnabled
        {
            get { return _isRepairClientEnabled; }
            set { SetProperty(ref _isRepairClientEnabled, value, "IsRepairClientEnabled"); }
        }

        // Localized labels
        public string[] IlluminationQualityLabels { get; private set; }
        public string[] TextureQualityLabels { get; private set; }
        public string[] LightingShadowQualityLabels { get; private set; }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand RepairClientCommand { get; private set; }
        public ICommand ClearCredentialsCommand { get; private set; }

        #endregion

        #region Constructor

        public SettingsViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _localization = LocalizationManager.GetInstance;

            InitializeLabels();
            InitializeCommands();
            InitializeDefaults();
        }

        #endregion

        #region Initialization

        private void InitializeLabels()
        {
            IlluminationQualityLabels = new[]
            {
                _localization.GetString("settings_label_disable"),
                _localization.GetString("settings_label_low"),
                _localization.GetString("settings_label_high")
            };

            TextureQualityLabels = new[]
            {
                _localization.GetString("settings_label_low"),
                _localization.GetString("settings_label_mean"),
                _localization.GetString("settings_label_high"),
                _localization.GetString("settings_label_ultra")
            };

            LightingShadowQualityLabels = new[]
            {
                _localization.GetString("settings_label_disable"),
                _localization.GetString("settings_label_low"),
                _localization.GetString("settings_label_mean"),
                _localization.GetString("settings_label_high")
            };
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(ExecuteSave);
            CloseCommand = new RelayCommand(ExecuteClose);
            RepairClientCommand = new RelayCommand(ExecuteRepairClient, CanExecuteRepairClient);
            ClearCredentialsCommand = new RelayCommand(ExecuteClearCredentials);
        }

        private void InitializeDefaults()
        {
            VideoAdapters = new List<string>();
            Resolutions = new List<string>();
            IsRepairClientEnabled = LauncherConfig.GetInstance.UpdateConfig.ClientUpdateEnable;
        }

        /// <summary>
        /// Инициализирует ViewModel данными из конфигурационного файла.
        /// </summary>
        public void LoadConfig(R3EngineSettings config)
        {
            _config = config;

            if (_config.IsDefault())
                return;

            // Quality settings
            TextureQuality = _config.TextureDetail;
            LightningQuality = _config.DynamicLight;
            ShadowQuality = _config.ShadowDetail;
            IlluminationQuality = _config.BboShasi;

            // Find gamma index
            for (int i = 0; i < GammaValues.Length; i++)
            {
                if (Math.Abs(GammaValues[i] - _config.Gamma) < 0.01f)
                {
                    GammaIndex = i;
                    break;
                }
            }

            // Checkboxes
            IsWindowMode = _config.IsFullScreen;
            IsMouseAcceleration = _config.IsMouseAccelerationEnabled;
            IsDetailTexture = _config.IsDetailTextureEnabled;
            IsMusicEnabled = _config.IsMusicEnabled;
            IsSoundEnabled = _config.IsSoundEnabled;
            CloseLauncherAfterLogin = _config.CloseLauncherAfterLogin;
        }

        #endregion

        #region Command Handlers

        private void ExecuteSave(object parameter)
        {
            OnSaveRequested();
        }

        private void ExecuteClose(object parameter)
        {
            OnCloseRequested();
        }

        private bool CanExecuteRepairClient(object parameter)
        {
            return IsRepairClientEnabled;
        }

        private void ExecuteRepairClient(object parameter)
        {
            string lockFile = $".//{LauncherConfig.GetInstance.ServerConfig.Title}.lock";
            if (File.Exists(lockFile))
            {
                File.Delete(lockFile);
            }
            ClientUpdateRequired = true;
            OnCloseRequested();
        }

        private void ExecuteClearCredentials(object parameter)
        {
            if (File.Exists(CredentialStoragePath))
            {
                File.Delete(CredentialStoragePath);
            }
            _dialogService.ShowInfo(
                _localization.GetString("settings_login_cleared"),
                _localization.GetString("settings_label"));
        }

        #endregion

        #region Events

        public event EventHandler SaveRequested;
        public event EventHandler CloseRequested;

        protected virtual void OnSaveRequested()
        {
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCloseRequested()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}

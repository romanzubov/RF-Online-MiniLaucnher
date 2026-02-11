using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Utils;
using MiniLauncherStyle.Data;
using System;
using System.IO;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static MiniLauncher.View.Settings;

namespace MiniLauncherStyle.Views.Settings
{
    /// <summary>
    /// Окно настроек игры.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private const string ConfigFileName = ".\\R3Engine.ini";
        private const string CredentialStoragePath = ".\\credential_storage.json";

        public bool ClientUpdateRequired { get; set; }
        
        private readonly LocalizationManager localization;
        private readonly R3EngineSettings config;
        
        private static readonly float[] GammaValues = { 0.8f, 1.0f, 1.2f, 1.4f, 1.6f, 1.8f };
        private string[] illuminationQualityLabels;
        private string[] textureQualityLabels;
        private string[] lightingShadowQualityLabels;

        private readonly double initialX;
        private readonly double initialY;

        public SettingsWindow(double x, double y)
        {
            initialX = x;
            initialY = y;
            ClientUpdateRequired = false;
            localization = LocalizationManager.GetInstance;
            
            InitializeComponent();
            InitLocalization();
            InitializeVideoAndResolution();
            
            config = LoadConfig();
            if (!config.IsDefault())
            {
                InitializeFormFromConfig();
            }

            if (!LauncherConfig.GetInstance.UpdateConfig.ClientUpdateEnable)
            {
                btn_repair_client.IsEnabled = false;
            }
        }

        private void InitLocalization()
        {
            Title = localization.GetString("settings_label");

            illuminationQualityLabels = new[]
            {
                localization.GetString("settings_label_disable"),
                localization.GetString("settings_label_low"),
                localization.GetString("settings_label_high")
            };

            textureQualityLabels = new[]
            {
                localization.GetString("settings_label_low"),
                localization.GetString("settings_label_mean"),
                localization.GetString("settings_label_high"),
                localization.GetString("settings_label_ultra")
            };

            lightingShadowQualityLabels = new[]
            {
                localization.GetString("settings_label_disable"),
                localization.GetString("settings_label_low"),
                localization.GetString("settings_label_mean"),
                localization.GetString("settings_label_high")
            };
        }

        private void InitializeVideoAndResolution()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    foreach (PropertyData property in mo.Properties)
                    {
                        if (property.Name == "Description" && property.Value != null)
                        {
                            video_adapter_list.Items.Add(property.Value.ToString());
                        }
                    }
                }
            }

            DEVMODE vDevMode = new DEVMODE();
            int i = 0;
            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                string resolution = $"{vDevMode.dmPelsWidth}x{vDevMode.dmPelsHeight}";
                if (!resolution_list.Items.Contains(resolution))
                {
                    resolution_list.Items.Add(resolution);
                }
                i++;
            }

            if (resolution_list.Items.Count > 0) resolution_list.SelectedIndex = 0;
            if (video_adapter_list.Items.Count > 0) video_adapter_list.SelectedIndex = 0;
        }

        private R3EngineSettings LoadConfig()
        {
            var cfg = new R3EngineSettings();
            
            if (!File.Exists(ConfigFileName))
            {
                cfg.SetDefault();
                return cfg;
            }

            try
            {
                var ini = new IniFile(ConfigFileName);
                
                // Render State
                cfg.ScreenXSize = int.Parse(ini.ReadReverse("RenderState", "ScreenXSize"));
                cfg.ScreenYSize = int.Parse(ini.ReadReverse("RenderState", "ScreenYSize"));
                cfg.RenderBits = int.Parse(ini.ReadReverse("RenderState", "RenderBits"));
                cfg.BboShasi = int.Parse(ini.ReadReverse("RenderState", "BboShasi"));
                cfg.Gamma = float.Parse(ini.ReadReverse("RenderState", "Gamma").Replace('.', ','));
                cfg.DynamicLight = int.Parse(ini.ReadReverse("RenderState", "DynamicLight"));
                cfg.ShadowDetail = int.Parse(ini.ReadReverse("RenderState", "ShadowDetail"));
                cfg.Adapter = ini.ReadReverse("RenderState", "Adapter");
                cfg.SeeDistance = int.Parse(ini.ReadReverse("RenderState", "SeeDistance"));
                cfg.TextureDetail = int.Parse(ini.ReadReverse("RenderState", "TextureDetail"));
                cfg.IsFullScreen = !bool.Parse(ini.ReadReverse("RenderState", "bFullScreen"));
                cfg.IsMouseAccelerationEnabled = bool.Parse(ini.ReadReverse("RenderState", "bMouseAccelation"));
                cfg.IsDetailTextureEnabled = bool.Parse(ini.ReadReverse("RenderState", "bDetailTexture"));
                
                // Launcher
                if (ini.KeyExists("close_launcher_after_login", "Launcher"))
                {
                    cfg.CloseLauncherAfterLogin = bool.Parse(ini.ReadReverse("Launcher", "close_launcher_after_login"));
                }
                else
                {
                    ini.Write("close_launcher_after_login", "FALSE", "Launcher");
                }
                
                // Sound
                cfg.IsSoundEnabled = bool.Parse(ini.ReadReverse("Sound", "Sound"));
                cfg.IsMusicEnabled = bool.Parse(ini.ReadReverse("Sound", "music"));
            }
            catch (Exception)
            {
                File.Delete(ConfigFileName);
                cfg.SetDefault();
                SaveConfig(cfg);
            }
            
            return cfg;
        }

        private void InitializeFormFromConfig()
        {
            // Video Adapter
            for (int i = 0; i < video_adapter_list.Items.Count; i++)
            {
                if (video_adapter_list.Items[i].ToString() == config.Adapter.Replace('%', ' '))
                {
                    video_adapter_list.SelectedIndex = i;
                    break;
                }
            }

            // Resolution
            string targetResolution = $"{config.ScreenXSize}x{config.ScreenYSize}";
            for (int i = 0; i < resolution_list.Items.Count; i++)
            {
                if (resolution_list.Items[i].ToString() == targetResolution)
                {
                    resolution_list.SelectedIndex = i;
                    break;
                }
            }

            texture_quality_bar.Value = config.TextureDetail;
            lightning_quality_bar.Value = config.DynamicLight;
            shadow_quality_bar.Value = config.ShadowDetail;

            for (int i = 0; i < GammaValues.Length; i++)
            {
                if (Math.Abs(GammaValues[i] - config.Gamma) < 0.01f)
                {
                    gamma_bar.Value = i;
                    gamma_label.Text = GammaValues[i].ToString();
                    break;
                }
            }

            illimination_quality_bar.Value = config.BboShasi;
            window_mode_check.IsChecked = config.IsFullScreen;
            mouse_acceleration_check.IsChecked = config.IsMouseAccelerationEnabled;
            texture_detalization_check.IsChecked = config.IsDetailTextureEnabled;
            music_check.IsChecked = config.IsMusicEnabled;
            effects_check.IsChecked = config.IsSoundEnabled;

            illimination_quality_label.Text = illuminationQualityLabels[(int)illimination_quality_bar.Value];
            lightning_quality_label.Text = lightingShadowQualityLabels[(int)lightning_quality_bar.Value];
            shadow_quality_label.Text = lightingShadowQualityLabels[(int)shadow_quality_bar.Value];
            texture_quality_label.Text = textureQualityLabels[(int)texture_quality_bar.Value];

            close_launcher_after_login.IsChecked = config.CloseLauncherAfterLogin;
        }

        private void SaveConfig(R3EngineSettings settings)
        {
            if (!File.Exists(ConfigFileName))
            {
                File.Create(ConfigFileName).Close();
            }

            var ini = new IniFile(ConfigFileName);

            // Setup
            ini.WriteNew("Setup", "Language", "Russia");
            
            // Render State
            string[] resolution = resolution_list.SelectedItem.ToString().Split('x');
            ini.WriteNew("RenderState", "ScreenXSize", resolution[0]);
            ini.WriteNew("RenderState", "ScreenYSize", resolution[1]);
            ini.WriteNew("RenderState", "RenderBits", settings.RenderBits.ToString());
            ini.WriteNew("RenderState", "BboShasi", settings.BboShasi.ToString());
            ini.WriteNew("RenderState", "Gamma", settings.Gamma.ToString().Replace(',', '.'));
            ini.WriteNew("RenderState", "DynamicLight", settings.DynamicLight.ToString());
            ini.WriteNew("RenderState", "ShadowDetail", settings.ShadowDetail.ToString());
            ini.WriteNew("RenderState", "Adapter", video_adapter_list.SelectedItem.ToString().Replace(' ', '%'));
            ini.WriteNew("RenderState", "SeeDistance", settings.SeeDistance.ToString());
            ini.WriteNew("RenderState", "TextureDetail", settings.TextureDetail.ToString());
            ini.WriteNew("RenderState", "bFullScreen", (!window_mode_check.IsChecked).ToString().ToUpper());
            ini.WriteNew("RenderState", "bMouseAccelation", mouse_acceleration_check.IsChecked.ToString().ToUpper());
            ini.WriteNew("RenderState", "bDetailTexture", texture_detalization_check.IsChecked.ToString().ToUpper());

            // Sound
            ini.WriteNew("Sound", "MusicVol", "0.2");
            ini.WriteNew("Sound", "SoundVol", "0.2");
            ini.WriteNew("Sound", "AmbVol", "0.2");
            ini.WriteNew("Sound", "Sound", effects_check.IsChecked.ToString().ToUpper());
            ini.WriteNew("Sound", "music", music_check.IsChecked.ToString().ToUpper());
            
            // AutoLogin
            ini.WriteNew("AutoLogin", "Use", "FALSE");
            
            // Launcher
            ini.WriteNew("Launcher", "close_launcher_after_login", close_launcher_after_login.IsChecked.ToString().ToUpper());
        }

        #region Event Handlers

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Top = initialX;
            Left = initialY;
        }

        private void Gamma_bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index = (int)gamma_bar.Value;
            gamma_label.Text = GammaValues[index].ToString();
            config.Gamma = GammaValues[index];
        }

        private void Shadow_quality_bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index = (int)shadow_quality_bar.Value;
            shadow_quality_label.Text = lightingShadowQualityLabels[index];
            config.ShadowDetail = index;
        }

        private void Illimination_quality_bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index = (int)illimination_quality_bar.Value;
            illimination_quality_label.Text = illuminationQualityLabels[index];
            config.BboShasi = index;
        }

        private void Lightning_quality_bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index = (int)lightning_quality_bar.Value;
            lightning_quality_label.Text = lightingShadowQualityLabels[index];
            config.DynamicLight = index;
        }

        private void Texture_quality_bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index = (int)texture_quality_bar.Value;
            texture_quality_label.Text = textureQualityLabels[index];
            config.TextureDetail = index;
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig(config);
            Close();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_clean_passwords_Click(object sender, RoutedEventArgs e)
        {
            UserCredential.CleanData(CredentialStoragePath);
        }

        private void Btn_repair_client_Click(object sender, RoutedEventArgs e)
        {
            string lockFile = $".\\{LauncherConfig.GetInstance.ServerConfig.Title}.lock";
            
            if (File.Exists(lockFile))
            {
                File.Delete(lockFile);
                ClientUpdateRequired = true;
                Close();
            }
        }

        #endregion
    }
}

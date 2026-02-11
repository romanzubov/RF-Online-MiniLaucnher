using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MiniLauncher.View
{
    public partial class Settings : Form
    {
        public bool clientUpdateRequeried { get; set; }
        private LocalizationManager Lm;
        private readonly R3EngineSettings config;
        private float[] gamma_array = { 0.8f, 1.0f, 1.2f, 1.4f, 1.6f, 1.8f };
        private string[] illimonation_quality_array;
        private string[] texture_quality_array;
        private string[] lightning_shadow_quality_array;
        public Settings()
        {
            clientUpdateRequeried = false;
            Lm = LocalizationManager.GetInstance;
            InitializeComponent();
            InitLocalization();
            InitializeVideoAndResolution();
            config = InitializeConfig();
            if (!config.IsDefault())
            {
                InitializeForm();
            }

            if (!LauncherConfig.GetInstance.UpdateConfig.ClientUpdateEnable)
            {
                btn_repair_client.Enabled = false;
            }
        }
        private void InitLocalization()
        {
            this.Text = Lm.GetString("settings_label");
            illimonation_quality_array = new string[3];
            illimonation_quality_array[0] = Lm.GetString("settings_label_disable");
            illimonation_quality_array[1] = Lm.GetString("settings_label_low");
            illimonation_quality_array[2] = Lm.GetString("settings_label_high");

            texture_quality_array = new string[4];
            texture_quality_array[0] = Lm.GetString("settings_label_low");
            texture_quality_array[1] = Lm.GetString("settings_label_mean");
            texture_quality_array[2] = Lm.GetString("settings_label_high");
            texture_quality_array[3] = Lm.GetString("settings_label_ultra");

            lightning_shadow_quality_array = new string[4];
            lightning_shadow_quality_array[0] = Lm.GetString("settings_label_disable");
            lightning_shadow_quality_array[1] = Lm.GetString("settings_label_low");
            lightning_shadow_quality_array[2] = Lm.GetString("settings_label_mean");
            lightning_shadow_quality_array[3] = Lm.GetString("settings_label_high");

            ForAllControls(this, control =>
            {
                control.Text = Lm.GetString(control.Name);
            });
        }
        private void InitializeVideoAndResolution()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");

            string graphicsCard = string.Empty;
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData property in mo.Properties)
                {
                    if (property.Name == "Description")
                    {
                        video_adapter_list.Items.Add(property.Value.ToString());
                    }
                }
            }

            DEVMODE vDevMode = new DEVMODE();
            int i = 0;
            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                if (!resolution_list.Items.Contains($"{vDevMode.dmPelsWidth}x{vDevMode.dmPelsHeight}"))
                {
                    resolution_list.Items.Add($"{vDevMode.dmPelsWidth}x{vDevMode.dmPelsHeight}");
                }
                i++;
            }

            resolution_list.SelectedIndex = 0;
            video_adapter_list.SelectedIndex = 0;
        }

        private R3EngineSettings InitializeConfig()
        {
            var cfg = new R3EngineSettings();
            if (File.Exists(".\\R3Engine.ini"))
            {
                try
                {
                    var ini = new IniFile(".\\R3Engine.ini");
                    // RENDER STATE //
                    cfg.ScreenXSize = Int32.Parse(ini.ReadReverse("RenderState", "ScreenXSize"));
                    cfg.ScreenYSize = Int32.Parse(ini.ReadReverse("RenderState", "ScreenYSize"));
                    cfg.RenderBits = Int32.Parse(ini.ReadReverse("RenderState", "RenderBits"));
                    cfg.BboShasi = Int32.Parse(ini.ReadReverse("RenderState", "BboShasi"));
                    cfg.Gamma = float.Parse(ini.ReadReverse("RenderState", "Gamma").Replace('.', ','));
                    cfg.DynamicLight = Int32.Parse(ini.ReadReverse("RenderState", "DynamicLight"));
                    cfg.ShadowDetail = Int32.Parse(ini.ReadReverse("RenderState", "ShadowDetail"));
                    cfg.Adapter = ini.ReadReverse("RenderState", "Adapter");
                    cfg.SeeDistance = Int32.Parse(ini.ReadReverse("RenderState", "SeeDistance"));
                    cfg.TextureDetail = Int32.Parse(ini.ReadReverse("RenderState", "TextureDetail"));
                    cfg.bFullScreen = !bool.Parse(ini.ReadReverse("RenderState", "bFullScreen"));
                    cfg.bMouseAccelation = bool.Parse(ini.ReadReverse("RenderState", "bMouseAccelation"));
                    cfg.bDetailTexture = bool.Parse(ini.ReadReverse("RenderState", "bDetailTexture"));

                    // Sound STATE //
                    cfg.Sound = bool.Parse(ini.ReadReverse("Sound", "Sound"));
                    cfg.music = bool.Parse(ini.ReadReverse("Sound", "music"));
                }
                catch (Exception)
                {
                    File.Delete(".\\R3Engine.ini");
                    cfg.SetDefault();
                    SaveConfig(cfg);
                }
            }
            else
            {
                cfg.SetDefault();
            }
            return cfg;
        }

        private void InitializeForm()
        {
            for (int i = 0; i < video_adapter_list.Items.Count; i++)
            {
                if (video_adapter_list.Items[i].ToString() == config.Adapter.Replace('%', ' '))
                {
                    video_adapter_list.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < resolution_list.Items.Count; i++)
            {
                if (resolution_list.Items[i].ToString() == String.Format("{0}x{1}", config.ScreenXSize, config.ScreenYSize))
                {
                    resolution_list.SelectedIndex = i;
                    break;
                }
            }
            texture_quality_bar.Value = config.TextureDetail;
            lightning_quality_bar.Value = config.DynamicLight;
            shadow_quality_bar.Value = config.ShadowDetail;

            for (int i = 0; i < gamma_array.Length; i++)
            {
                if (gamma_array[i] == config.Gamma)
                {
                    gamma_bar.Value = i;
                    gamma_label.Text = gamma_array[gamma_bar.Value].ToString();
                    break;
                }
            }
            illimination_quality_bar.Value = config.BboShasi;
            window_mode_check.Checked = config.bFullScreen;
            mouse_acceleration_check.Checked = config.bMouseAccelation;
            texture_detalization_check.Checked = config.bDetailTexture;
            music_check.Checked = config.music;
            effects_check.Checked = config.Sound;

            illimination_quality_label.Text = illimonation_quality_array[illimination_quality_bar.Value];
            lightning_quality_label.Text = lightning_shadow_quality_array[lightning_quality_bar.Value];
            shadow_quality_label.Text = lightning_shadow_quality_array[shadow_quality_bar.Value];
            texture_quality_label.Text = texture_quality_array[texture_quality_bar.Value];
        }

        private void SaveConfig(R3EngineSettings config)
        {
            if (!File.Exists(".\\R3Engine.ini"))
            {
                File.Create(".\\R3Engine.ini").Close();
            }

            var ini = new IniFile(".\\R3Engine.ini");

            // Setup //
            ini.WriteNew("Setup", "Language", "Russia");
            // RENDER STATE //
            ini.WriteNew("RenderState", "ScreenXSize", resolution_list.SelectedItem.ToString().Split('x')[0]);
            ini.WriteNew("RenderState", "ScreenYSize", resolution_list.SelectedItem.ToString().Split('x')[1]);
            ini.WriteNew("RenderState", "RenderBits", config.RenderBits.ToString());
            ini.WriteNew("RenderState", "BboShasi", config.BboShasi.ToString());
            ini.WriteNew("RenderState", "Gamma", config.Gamma.ToString().Replace(',', '.'));
            ini.WriteNew("RenderState", "DynamicLight", config.DynamicLight.ToString());
            ini.WriteNew("RenderState", "ShadowDetail", config.ShadowDetail.ToString());
            ini.WriteNew("RenderState", "Adapter", video_adapter_list.SelectedItem.ToString().Replace(' ', '%'));
            ini.WriteNew("RenderState", "SeeDistance", config.SeeDistance.ToString());
            ini.WriteNew("RenderState", "TextureDetail", config.TextureDetail.ToString());
            ini.WriteNew("RenderState", "bFullScreen", (!window_mode_check.Checked).ToString().ToUpper());
            ini.WriteNew("RenderState", "bMouseAccelation", mouse_acceleration_check.Checked.ToString().ToUpper());
            ini.WriteNew("RenderState", "bDetailTexture", texture_detalization_check.Checked.ToString().ToUpper());

            // Sound STATE //
            ini.WriteNew("Sound", "MusicVol", "0.2");
            ini.WriteNew("Sound", "SoundVol", "0.2");
            ini.WriteNew("Sound", "AmbVol", "0.2");
            ini.WriteNew("Sound", "Sound", effects_check.Checked.ToString().ToUpper());
            ini.WriteNew("Sound", "music", music_check.Checked.ToString().ToUpper());
            // AutoLogin //
            ini.WriteNew("AutoLogin", "Use", "FALSE");
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveConfig(config);
            Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Update Labels
        private void texture_quality_bar_Scroll(object sender, EventArgs e)
        {
            texture_quality_label.Text = texture_quality_array[texture_quality_bar.Value];
            config.TextureDetail = texture_quality_bar.Value;
        }

        private void lightning_quality_bar_Scroll(object sender, EventArgs e)
        {
            lightning_quality_label.Text = lightning_shadow_quality_array[lightning_quality_bar.Value];
            config.DynamicLight = lightning_quality_bar.Value;
        }

        private void illimination_quality_bar_Scroll(object sender, EventArgs e)
        {
            illimination_quality_label.Text = illimonation_quality_array[illimination_quality_bar.Value];
            config.BboShasi = illimination_quality_bar.Value;
        }

        private void shadow_quality_bar_Scroll(object sender, EventArgs e)
        {
            shadow_quality_label.Text = lightning_shadow_quality_array[shadow_quality_bar.Value];
            config.ShadowDetail = shadow_quality_bar.Value;
        }

        private void gamma_bar_Scroll(object sender, EventArgs e)
        {
            gamma_label.Text = gamma_array[gamma_bar.Value].ToString();
            config.Gamma = gamma_array[gamma_bar.Value];
        }

        public static void ForAllControls(Control parent, Action<Control> action)
        {
            foreach (Control c in parent.Controls)
            {
                action(c);
                ForAllControls(c, action);
            }
        }

        #endregion
        #region DllImport
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(
             string deviceName, int modeNum, ref DEVMODE devMode);
        const int ENUM_CURRENT_SETTINGS = -1;

        const int ENUM_REGISTRY_SETTINGS = -2;

        public object IniHelper { get; private set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {

            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;

        }
        #endregion

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void Btn_clean_passwords_Click(object sender, EventArgs e)
        {
            UserCredential.CleanData(".\\credential_storage.json");
        }

        private void Btn_repair_client_Click(object sender, EventArgs e)
        {
            if (File.Exists($".\\{LauncherConfig.GetInstance.ServerConfig.Title}.lock")){
                File.Delete($".\\{LauncherConfig.GetInstance.ServerConfig.Title}.lock");
                clientUpdateRequeried = true;
                Close();
            }
        }
    }

    public class R3EngineSettings
    {

        public R3EngineSettings()
        {
            isDefault = false;
        }
        public int ScreenXSize { get; set; }
        public int ScreenYSize { get; set; }
        public int RenderBits { get; set; }
        public int BboShasi { get; set; }
        public float Gamma { get; set; }
        public int DynamicLight { get; set; }
        public int ShadowDetail { get; set; }
        public string Adapter { get; set; }
        public int SeeDistance { get; set; }
        public int TextureDetail { get; set; }
        public bool bFullScreen { get; set; }
        public bool bMouseAccelation { get; set; }
        public bool bDetailTexture { get; set; }
        public bool Sound { get; set; }
        public bool music { get; set; }

        private bool isDefault { get; set; }
        internal void SetDefault()
        {
            isDefault = true;
            Gamma = 1.0f;
        }
        internal bool IsDefault()
        {
            return isDefault;
        }
    }
}

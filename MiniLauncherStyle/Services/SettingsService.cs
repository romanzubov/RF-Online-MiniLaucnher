using MiniLauncher.Helper;
using MiniLauncher.Utils;
using MiniLauncherStyle.Data;
using MiniLauncherStyle.Helper;
using MiniLauncherStyle.Services.Interfaces;
using System;
using System.IO;

namespace MiniLauncherStyle.Services
{
    /// <summary>
    /// Реализация сервиса настроек.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private const string ConfigFileName = ".\\R3Engine.ini";
        private const string CredentialStoragePath = ".\\credential_storage.json";

        private bool _clientUpdateRequired;

        /// <summary>
        /// Получает или устанавливает флаг необходимости обновления клиента.
        /// </summary>
        public bool ClientUpdateRequired
        {
            get { return _clientUpdateRequired; }
            set { _clientUpdateRequired = value; }
        }

        /// <summary>
        /// Загружает настройки рендера.
        /// </summary>
        public R3EngineSettings LoadSettings()
        {
            var settings = new R3EngineSettings();

            if (!File.Exists(ConfigFileName))
            {
                settings.SetDefault();
                return settings;
            }

            try
            {
                var configFile = new IniFile(ConfigFileName);

                // Render State
                if (configFile.KeyExists("ScreenXSize", "Render State"))
                    settings.ScreenXSize = int.Parse(configFile.ReadReverse("Render State", "ScreenXSize"));

                if (configFile.KeyExists("ScreenYSize", "Render State"))
                    settings.ScreenYSize = int.Parse(configFile.ReadReverse("Render State", "ScreenYSize"));

                if (configFile.KeyExists("RenderBits", "Render State"))
                    settings.RenderBits = int.Parse(configFile.ReadReverse("Render State", "RenderBits"));

                if (configFile.KeyExists("BBO_SHASI", "Render State"))
                    settings.BboShasi = int.Parse(configFile.ReadReverse("Render State", "BBO_SHASI"));

                if (configFile.KeyExists("Gamma", "Render State"))
                    settings.Gamma = float.Parse(configFile.ReadReverse("Render State", "Gamma").Replace(',', '.'), 
                        System.Globalization.CultureInfo.InvariantCulture);

                if (configFile.KeyExists("DynamicLight ", "Render State"))
                    settings.DynamicLight = int.Parse(configFile.ReadReverse("Render State", "DynamicLight "));

                if (configFile.KeyExists("ShadowDetail", "Render State"))
                    settings.ShadowDetail = int.Parse(configFile.ReadReverse("Render State", "ShadowDetail"));

                if (configFile.KeyExists("Adapter", "Render State"))
                    settings.Adapter = configFile.ReadReverse("Render State", "Adapter");

                if (configFile.KeyExists("SeeDistance", "Render State"))
                    settings.SeeDistance = int.Parse(configFile.ReadReverse("Render State", "SeeDistance"));

                if (configFile.KeyExists("TextureDetail", "Render State"))
                    settings.TextureDetail = int.Parse(configFile.ReadReverse("Render State", "TextureDetail"));

                if (configFile.KeyExists("FullScreen", "Render State"))
                    settings.IsFullScreen = configFile.ReadReverse("Render State", "FullScreen").ToLower() == "true";

                if (configFile.KeyExists("mouse_acceleration", "Render State"))
                    settings.IsMouseAccelerationEnabled = configFile.ReadReverse("Render State", "mouse_acceleration").ToLower() == "true";

                if (configFile.KeyExists("DetailTexture", "Render State"))
                    settings.IsDetailTextureEnabled = configFile.ReadReverse("Render State", "DetailTexture").ToLower() == "true";

                // Sound
                if (configFile.KeyExists("Sound", "Sound"))
                    settings.IsSoundEnabled = configFile.ReadReverse("Sound", "Sound").ToLower() == "true";

                if (configFile.KeyExists("Music", "Sound"))
                    settings.IsMusicEnabled = configFile.ReadReverse("Sound", "Music").ToLower() == "true";

                // Launcher
                if (configFile.KeyExists("close_launcher_after_login", "Launcher"))
                    settings.CloseLauncherAfterLogin = configFile.ReadReverse("Launcher", "close_launcher_after_login").ToLower() == "true";
            }
            catch (Exception)
            {
                settings.SetDefault();
            }

            return settings;
        }

        /// <summary>
        /// Сохраняет настройки рендера.
        /// </summary>
        public bool SaveSettings(R3EngineSettings settings)
        {
            try
            {
                var configFile = new IniFile(ConfigFileName);

                // Render State
                configFile.Write("ScreenXSize", settings.ScreenXSize.ToString(), "Render State");
                configFile.Write("ScreenYSize", settings.ScreenYSize.ToString(), "Render State");
                configFile.Write("RenderBits", settings.RenderBits.ToString(), "Render State");
                configFile.Write("BBO_SHASI", settings.BboShasi.ToString(), "Render State");
                configFile.Write("Gamma", settings.Gamma.ToString(System.Globalization.CultureInfo.InvariantCulture), "Render State");
                configFile.Write("DynamicLight ", settings.DynamicLight.ToString(), "Render State");
                configFile.Write("ShadowDetail", settings.ShadowDetail.ToString(), "Render State");
                configFile.Write("Adapter", settings.Adapter ?? "", "Render State");
                configFile.Write("SeeDistance", settings.SeeDistance.ToString(), "Render State");
                configFile.Write("TextureDetail", settings.TextureDetail.ToString(), "Render State");
                configFile.Write("FullScreen", (!settings.IsFullScreen).ToString().ToUpper(), "Render State");
                configFile.Write("mouse_acceleration", settings.IsMouseAccelerationEnabled.ToString().ToUpper(), "Render State");
                configFile.Write("DetailTexture", settings.IsDetailTextureEnabled.ToString().ToUpper(), "Render State");

                // Sound
                configFile.Write("Sound", settings.IsSoundEnabled.ToString().ToUpper(), "Sound");
                configFile.Write("Music", settings.IsMusicEnabled.ToString().ToUpper(), "Sound");

                // Launcher
                configFile.Write("close_launcher_after_login", settings.CloseLauncherAfterLogin.ToString().ToUpper(), "Launcher");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Получает сохраненный логин.
        /// </summary>
        public string GetSavedLogin()
        {
            try
            {
                var userCredential = new UserCredential(CredentialStoragePath);
                var logins = userCredential.LoadLogins();
                return logins.Length > 0 ? logins[0] : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Сохраняет логин.
        /// </summary>
        public void SaveLogin(string login)
        {
            // Логин сохраняется через UserCredential в другом месте
        }

        /// <summary>
        /// Очищает сохраненные учетные данные.
        /// </summary>
        public void ClearCredentials()
        {
            try
            {
                if (File.Exists(CredentialStoragePath))
                {
                    File.Delete(CredentialStoragePath);
                }
            }
            catch
            {
                // Игнорируем ошибки удаления
            }
        }

        /// <summary>
        /// Сохраняет язык (код нации).
        /// </summary>
        /// <param name="nationCode">Код нации (например, "en_gb")</param>
        public void SaveLanguage(string nationCode)
        {
            try
            {
                var configuration = new IniFile(ConfigFileName);
                var encoded = NationCodeHelper.EncodeNationCode(nationCode);
                configuration.Write("Language", encoded, "Setup");
            }
            catch
            {
                // Игнорируем ошибки записи
            }
        }
    }
}

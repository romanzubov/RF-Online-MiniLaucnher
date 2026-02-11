namespace MiniLauncherStyle.Data
{
    /// <summary>
    /// Настройки рендеринга игры (R3Engine.ini).
    /// </summary>
    public class R3EngineSettings
    {
        private bool isDefault;

        // Render State
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
        public bool IsFullScreen { get; set; }
        public bool IsMouseAccelerationEnabled { get; set; }
        public bool IsDetailTextureEnabled { get; set; }

        // Sound
        public bool IsSoundEnabled { get; set; }
        public bool IsMusicEnabled { get; set; }

        // Launcher
        public bool CloseLauncherAfterLogin { get; set; }

        public R3EngineSettings()
        {
            isDefault = false;
        }

        /// <summary>
        /// Устанавливает настройки по умолчанию.
        /// </summary>
        public void SetDefault()
        {
            isDefault = true;
            Gamma = 1.0f;
            RenderBits = 32;
            SeeDistance = 100;
            TextureDetail = 2;
            DynamicLight = 2;
            ShadowDetail = 2;
            BboShasi = 1;
        }

        /// <summary>
        /// Проверяет, являются ли настройки дефолтными.
        /// </summary>
        public bool IsDefault()
        {
            return isDefault;
        }
    }
}

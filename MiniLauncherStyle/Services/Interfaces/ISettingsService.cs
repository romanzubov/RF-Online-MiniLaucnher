using System;
using MiniLauncherStyle.Data;

namespace MiniLauncherStyle.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса настроек.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Загружает настройки рендера.
        /// </summary>
        /// <returns>Настройки R3Engine</returns>
        R3EngineSettings LoadSettings();

        /// <summary>
        /// Сохраняет настройки рендера.
        /// </summary>
        /// <param name="settings">Настройки для сохранения</param>
        /// <returns>True если успешно</returns>
        bool SaveSettings(R3EngineSettings settings);

        /// <summary>
        /// Получает сохраненный логин.
        /// </summary>
        /// <returns>Сохраненный логин или пустая строка</returns>
        string GetSavedLogin();

        /// <summary>
        /// Сохраняет логин.
        /// </summary>
        /// <param name="login">Логин для сохранения</param>
        void SaveLogin(string login);

        /// <summary>
        /// Очищает сохраненные учетные данные.
        /// </summary>
        void ClearCredentials();

        /// <summary>
        /// Получает флаг необходимости обновления клиента.
        /// </summary>
        bool ClientUpdateRequired { get; set; }
    }
}

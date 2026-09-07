using MiniLauncher.Data;
using MiniLauncher.Helper;
using MiniLauncher.Utils;
using MiniLauncher.View;
using MiniLauncherStyle.Core;
using MiniLauncherStyle.Services;
using MiniLauncherStyle.Services.Interfaces;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MiniLauncherStyle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string ConfigFilePath = ".\\MiniLauncher.ini";

        public void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // Регистрация глобальных обработчиков исключений (как можно раньше)
                SetupGlobalExceptionHandlers();

                /*  
                 *  System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072
                    | (System.Net.SecurityProtocolType)768
                    | (System.Net.SecurityProtocolType)192;
                */

                ServicePointManager.DefaultConnectionLimit = 65000;

                if (LauncherUpdateService.ProcessUpdate(e.Args))
                {
                    Environment.Exit(0);
                }

                if (!InitConfig())
                {
                    return;
                }

                if (!InitLocalization())
                {
                    return;
                }

                // Регистрация сервисов в DI контейнере
                RegisterServices();

                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                var updateView = new Update();
                System.Windows.Forms.Application.Run(updateView);

                if (updateView.continiue)
                {
                    new MainWindow().Show();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                // Логируем краш даже если обработчики ещё не установлены
                LogCrashDump("App_Startup", ex);
                ShowCrashMessage(ex);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Регистрирует все сервисы в DI контейнере.
        /// </summary>
        private static void RegisterServices()
        {
            var locator = ServiceLocator.Current;
            
            // Регистрация базовых сервисов
            var dialogService = new DialogService();
            locator.Register<IDialogService>(dialogService);
            
            // Регистрация остальных сервисов
            locator.Register<IContentService>(new ContentService());
            locator.Register<IGameService>(new GameService(dialogService));
            locator.Register<INavigationService>(new NavigationService());
            locator.Register<ISettingsService>(new SettingsService());
            locator.Register<IGatewayService>(new GatewayService());
        }

        private static bool InitConfig()
        {
            var configLoader = new LoadConfigData(ConfigFilePath);
            
            if (!configLoader.IsExist())
            {
                ShowConfigError("ConfigurationNotFound");
                return false;
            }

            if (!configLoader.Load())
            {
                ShowConfigError("ConfigurationFileReadError");
                return false;
            }

            return true;
        }

        private static void ShowConfigError(string messageKey)
        {
            MessageBox.Show(
                LocalizationManager.GetInstance.GetString(messageKey),
                LocalizationManager.GetInstance.GetString("Error"));
        }

        private static bool InitLocalization()
        {
            var localizationManager = LocalizationManager.GetInstance;
            var nationalConfig = LauncherConfig.GetInstance.NationalConfig;

            return localizationManager.Init(nationalConfig.NationCode);
        }

        /// <summary>
        /// Настраивает глобальные обработчики необработанных исключений.
        /// </summary>
        private void SetupGlobalExceptionHandlers()
        {
            // Обработчик исключений в UI потоке WPF
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;

            // Обработчик необработанных исключений в AppDomain
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// Обработчик исключений в Dispatcher (UI поток).
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogCrashDump("DispatcherUnhandledException", e.Exception);
            e.Handled = true;
            
            ShowCrashMessage(e.Exception);
            Environment.Exit(1);
        }

        /// <summary>
        /// Обработчик необработанных исключений в AppDomain.
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            LogCrashDump("UnhandledException", exception);
            
            if (e.IsTerminating)
            {
                ShowCrashMessage(exception);
            }
        }

        /// <summary>
        /// Записывает информацию о краше в лог NetLog.
        /// </summary>
        private static void LogCrashDump(string source, Exception exception)
        {
            try
            {
                var logger = SimpleLogger.GetInstance;
                var crashInfo = new StringBuilder();

                crashInfo.AppendLine("========== CRASH DUMP ==========");
                crashInfo.AppendLine($"Source: {source}");
                crashInfo.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                crashInfo.AppendLine($"OS Version: {Environment.OSVersion}");
                crashInfo.AppendLine($"CLR Version: {Environment.Version}");
                crashInfo.AppendLine($"Working Set: {Environment.WorkingSet / 1024 / 1024} MB");

                if (exception != null)
                {
                    crashInfo.AppendLine();
                    crashInfo.AppendLine("--- Exception Details ---");
                    LogExceptionDetails(crashInfo, exception);
                }

                crashInfo.AppendLine("========== END CRASH DUMP ==========");

                logger.Fatal(crashInfo.ToString());
            }
            catch
            {
                // Игнорируем ошибки при записи лога, чтобы не скрыть оригинальную ошибку
            }
        }

        /// <summary>
        /// Рекурсивно записывает детали исключения включая внутренние исключения.
        /// </summary>
        private static void LogExceptionDetails(StringBuilder sb, Exception ex, int depth = 0)
        {
            string indent = new string(' ', depth * 2);

            sb.AppendLine($"{indent}Exception Type: {ex.GetType().FullName}");
            sb.AppendLine($"{indent}Message: {ex.Message}");
            sb.AppendLine($"{indent}Source: {ex.Source}");
            sb.AppendLine($"{indent}TargetSite: {ex.TargetSite}");
            sb.AppendLine($"{indent}Stack Trace:");
            sb.AppendLine(ex.StackTrace);

            if (ex.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendLine($"{indent}--- Inner Exception ---");
                LogExceptionDetails(sb, ex.InnerException, depth + 1);
            }
        }

        /// <summary>
        /// Показывает сообщение о краше пользователю.
        /// </summary>
        private static void ShowCrashMessage(Exception exception)
        {
            try
            {
                string message = "Произошла критическая ошибка приложения.\n" +
                                "Информация о краше записана в папку NetLog.\n\n" +
                                $"Ошибка: {exception?.Message ?? "Unknown error"}";

                MessageBox.Show(message, "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                // Игнорируем ошибки при показе сообщения
            }
        }
    }
}

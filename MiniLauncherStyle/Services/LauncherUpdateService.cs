using System;
using System.Diagnostics;
using System.IO;

namespace MiniLauncherStyle.Services
{
    /// <summary>
    /// Сервис для обработки самообновления лаунчера.
    /// Обрабатывает трёхэтапный процесс обновления исполняемого файла.
    /// </summary>
    public static class LauncherUpdateService
    {
        private const string UpdateArg1 = "update1";
        private const string UpdateArg2 = "update2";
        private const string UpdateArg3 = "update3";
        private const string NewLauncherFileName = "MiniLauncherNew.exe";
        private const string TempFilePrefix = "tmp_";

        /// <summary>
        /// Обрабатывает аргументы командной строки для процесса обновления.
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        /// <returns>true если требуется завершить приложение после обработки; false для продолжения запуска</returns>
        public static bool ProcessUpdate(string[] args)
        {
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                return false;
            }

            switch (args[0])
            {
                case UpdateArg1:
                    return ProcessUpdateStage1(args);

                case UpdateArg2:
                    return ProcessUpdateStage2(args);

                case UpdateArg3:
                    return ProcessUpdateStage3(args);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Этап 1: Переименовать текущий файл и запустить временную копию.
        /// </summary>
        private static bool ProcessUpdateStage1(string[] args)
        {
            if (args.Length < 3)
            {
                return false;
            }

            WaitForProcessExit(args[2]);

            string currentExecutable = args[1];
            string tempExecutable = $"{TempFilePrefix}{currentExecutable}";

            try
            {
                File.Move($".\\{currentExecutable}", $".\\{tempExecutable}");

                StartProcess(tempExecutable, $"{UpdateArg2} \"{currentExecutable}\" {Process.GetCurrentProcess().Id}");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Этап 2: Переместить новый файл на место старого и запустить его.
        /// </summary>
        private static bool ProcessUpdateStage2(string[] args)
        {
            if (args.Length < 3)
            {
                return false;
            }

            WaitForProcessExit(args[2]);

            string originalExecutable = args[1];

            try
            {
                File.Move(NewLauncherFileName, $".\\{originalExecutable}");

                StartProcess(originalExecutable, $"{UpdateArg3} \"{AppDomain.CurrentDomain.FriendlyName}\" {Process.GetCurrentProcess().Id}");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Этап 3: Удалить временный файл.
        /// </summary>
        private static bool ProcessUpdateStage3(string[] args)
        {
            if (args.Length < 3)
            {
                return false;
            }

            WaitForProcessExit(args[2]);

            string tempExecutable = args[1];

            try
            {
                if (File.Exists($".\\{tempExecutable}"))
                {
                    File.Delete($".\\{tempExecutable}");
                }
            }
            catch (Exception)
            {
                // Игнорируем ошибки при удалении временного файла
            }

            return false; // Продолжить нормальный запуск лаунчера
        }

        /// <summary>
        /// Ожидает завершения процесса по его ID.
        /// </summary>
        private static void WaitForProcessExit(string processIdStr)
        {
            if (!int.TryParse(processIdStr, out int processId))
            {
                return;
            }

            try
            {
                var process = Process.GetProcessById(processId);
                process.WaitForExit();
            }
            catch (Exception)
            {
                // Процесс уже завершён или не найден
            }
        }

        /// <summary>
        /// Запускает новый процесс.
        /// </summary>
        private static void StartProcess(string fileName, string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = $".\\{fileName}";
                process.StartInfo.Arguments = arguments;
                process.Start();
            }
        }
    }
}

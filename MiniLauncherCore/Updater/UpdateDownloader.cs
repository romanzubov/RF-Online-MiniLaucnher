using MiniLauncher.Data;
using MiniLauncher.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FileInfo = MiniLauncher.Data.FileInfo;
namespace MiniLauncher.Updater
{
    public class UpdateDownloader
    {
        // Cancel //
        private CancellationTokenSource _cts;
        // Sync vars //
        private readonly object _sync = new object();
        private readonly object _syncProcessedFiles = new Object();
        private readonly object _syncTotalThreadCount = new Object();
        // Events //
        public event UpdateDownloadEventHandler UpdateDownloadProgress;
        public event UpdateCompleteEventHandler UpdateComplete;
        // Manager and queue //
        private readonly UpdateTask _updateTask;
        public readonly Queue<FileInfo> BrokenFileInfos;
        // Local var //
        private string UpdateFileDir { get; }
        private int TotalFiles { get; set; }
        private int TotalThreadCount { get; set; }
        private List<Task> UpdateTasks { get; set; }
        private int CountProcessedFiles { get; set; }
        private bool UseProxyForDownload { get; set; }

        public UpdateDownloader(UpdateTask updateManager, string updateFileDir)
        {
            UseProxyForDownload = false;
            TotalFiles = 0;
            TotalThreadCount = 0;
            CountProcessedFiles = 0;

            _updateTask = updateManager;
            _cts = new CancellationTokenSource();
            BrokenFileInfos = new Queue<FileInfo>();
            UpdateFileDir = updateFileDir;
        }
        public void BeginDequeue()
        {
            UpdateTasks = new List<Task>();
            int CountParallelDownload = _updateTask.UpdateConfig.CountParallelDownload;
            if (LauncherConfig.GetInstance.UpdateConfig.CountParallelDownload != CountParallelDownload)
            {
                CountParallelDownload = LauncherConfig.GetInstance.UpdateConfig.CountParallelDownload;
            }
            for (int i = 0; i < CountParallelDownload; i++)
            {
                UpdateTasks.Add(Task.Factory.StartNew(AsyncBeginDownload));
            }
        }
        public void Stop()
        {
            lock (BrokenFileInfos)
            {
                Monitor.PulseAll(BrokenFileInfos);
            }
            //Task.WaitAll(UpdateTasks.ToArray());
        }

        private void AsyncBeginDownload()
        {
            while (!_updateTask.UpdateCheckFiles.IsDone() || !IsQueueEmpty())
            {

                lock (_sync)
                {
                    Monitor.Wait(_sync, 40);
                }
                FileInfo file;
                lock (BrokenFileInfos)
                {
                    if (BrokenFileInfos.Count == 0)
                        continue;

                    file = BrokenFileInfos.Dequeue();
                }
                DownloadFile(file);
                lock (_syncProcessedFiles)
                {
                    ++CountProcessedFiles;
                }
                OnUpdateDownloadProgress(new UpdateDownloadEventArgs { DoneCount = CountProcessedFiles, TotalCount = TotalFiles });
            }

            lock (_syncTotalThreadCount)
            {
                TotalThreadCount++;
            }

            if (TotalThreadCount == _updateTask.UpdateConfig.CountParallelDownload)
            {
                OnUpdateComplete(new UpdateCompleteEventArgs { TaskName = _updateTask.TaskName });
            }
        }

        private void DownloadFile(FileInfo file)
        {
            try
            {
                using (var wcUpdate = new WebClient())
                {
                    wcUpdate.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24");
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + file.Path))
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + file.Path);
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(".\\" + file.Path));
                    wcUpdate.Proxy = null;
                    string path = file.Path.Replace("\\", "/");
                    wcUpdate.DownloadFile(new Uri(UpdateFileDir + path), AppDomain.CurrentDomain.BaseDirectory + file.Path);
                }
            }
            catch (Exception e)
            {
                SimpleLogger.GetInstance.Error(e.ToString()+ " "+ UpdateFileDir + file.Path.Replace("\\", "/"));
            }
        }

        public bool IsQueueEmpty()
        {
            lock (BrokenFileInfos)
            {
                return BrokenFileInfos.Count == 0;
            }
        }

        public void AddToBroken(FileInfo file)
        {
            lock (BrokenFileInfos)
            {
                BrokenFileInfos.Enqueue(file);
                ++TotalFiles;
            }
            lock (_sync)
            {
                Monitor.Pulse(_sync);
            }
        }

        protected virtual void OnUpdateDownloadProgress(UpdateDownloadEventArgs e)
        {
            UpdateDownloadProgress?.Invoke(this, e);
        }

        protected virtual void OnUpdateComplete(UpdateCompleteEventArgs e)
        {
            UpdateComplete?.Invoke(this, e);
        }
    }
}

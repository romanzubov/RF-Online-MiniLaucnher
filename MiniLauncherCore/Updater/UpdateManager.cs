using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniLauncher.Updater
{
    public class UpdateManager
    {
        private readonly Queue<UpdateTask> _updateTasks;
        private UpdateTask _currentTask;
        public event UpdateStartedEventHandler UpdateStart;
        public event UpdateCompleteEventHandler UpdateComplete;
        public event UpdateCheckEventHandler UpdateCheckProggress;
        public event UpdateDownloadEventHandler UpdateDownloadProgress;
        public UpdateManager(Queue<UpdateTask> updateTasks)
        {
            _updateTasks = updateTasks;
        }

        public void Start()
        {
            if (_updateTasks.Any())
            {
                _currentTask = _updateTasks.Dequeue();
                OnUpdateStart(new UpdateStartedEventArgs { TaskName = _currentTask.TaskName });
                _currentTask.UpdateDownloader.UpdateComplete += UpdateDownloader_UpdateComplete;
                _currentTask.UpdateCheckFiles.UpdateCheckProggress += UpdateCheckFiles_UpdateCheckProggress;
                _currentTask.UpdateDownloader.UpdateDownloadProgress += UpdateDownloader_UpdateDownloadProgress;
                _currentTask.Start();
            }
            else
            {
                OnUpdateComplete(new UpdateCompleteEventArgs { TaskName = "all" });
            }
        }
        public void Stop()
        {
            _currentTask?.Stop();
        }
        private void UpdateDownloader_UpdateDownloadProgress(object sender, UpdateDownloadEventArgs e)
        {
            OnUpdateDownloadProgress(e);
        }

        private void UpdateCheckFiles_UpdateCheckProggress(object sender, UpdateCheckEventArgs e)
        {
            OnUpdateCheckProggress(e);
        }

        private void UpdateDownloader_UpdateComplete(object sender, UpdateCompleteEventArgs e)
        {
            if (_updateTasks.Any())
            {
                OnUpdateComplete(e);
                Start();
            }
            else
            {
                OnUpdateComplete(e);
            }
        }

        protected virtual void OnUpdateCheckProggress(UpdateCheckEventArgs e)
        {
            UpdateCheckProggress?.Invoke(this, e);
        }

        protected virtual void OnUpdateDownloadProgress(UpdateDownloadEventArgs e)
        {
            UpdateDownloadProgress?.Invoke(this, e);
        }

        protected virtual void OnUpdateComplete(UpdateCompleteEventArgs e)
        {
            UpdateComplete?.Invoke(this, e);
        }

        protected virtual void OnUpdateStart(UpdateStartedEventArgs e)
        {
            UpdateStart?.Invoke(this, e);
        }
    }
}

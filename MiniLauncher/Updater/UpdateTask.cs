using MiniLauncher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniLauncher.Updater
{
    class UpdateTask
    {
        public string TaskName { get; set; }
        public readonly UpdateSetting UpdateConfig;
        public readonly UpdateCheckFiles UpdateCheckFiles;
        public readonly UpdateDownloader UpdateDownloader;

        private bool IsUpdateDone { get; set; }
        public UpdateTask(string taskName, UpdateSetting updateConfig, string updateServer, string type)
        {
            TaskName = taskName;
            UpdateConfig = updateConfig;
            UpdateCheckFiles = new UpdateCheckFiles(this, $"{updateServer}/{type}/{@"updateInfo.json"}");
            UpdateDownloader = new UpdateDownloader(this, $"{updateServer}/{type}/{@"files"}");
        }
        public void Start()
        {
            if (UpdateCheckFiles.Prepare())
            {
                IsUpdateDone = false;
                UpdateCheckFiles.Start();
                UpdateDownloader.BeginDequeue();
            }
        }
        public void Stop()
        {
            UpdateDownloader.Stop();
        }
    }
}

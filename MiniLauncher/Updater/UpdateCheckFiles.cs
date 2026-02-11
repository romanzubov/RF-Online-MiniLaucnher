using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FileInfo = MiniLauncher.Data.FileInfo;

namespace MiniLauncher.Updater
{
    class UpdateCheckFiles
    {
        private object SyncState = new Object();
        public event UpdateCheckEventHandler UpdateCheckProggress;
        private readonly UpdateTask _updateManager;
        private List<FileInfo> UpdateList { get; set; }
        private string UpdateFileUrl { get; set; }
        private string UpdateLocalFolder { get; set; }
        private bool Running { get; set; }

        public UpdateCheckFiles(UpdateTask updateManager, string updateFileUrl)
        {
            _updateManager = updateManager;
            UpdateList = new List<FileInfo>();
            UpdateFileUrl = updateFileUrl;
            UpdateLocalFolder = $".\\";
        }

        internal bool IsDone()
        {
            lock (SyncState)
            {
                return Running;
            }
        }

        public bool Prepare()
        {
            return GetUpdateFile();
        }

        public void Start()
        {
            SwitchState(false);
            Task t = new Task(AsyncStart);
            t.Start();
        }
        private void AsyncStart()
        {
            long total = UpdateList.Count; // Increment from zero
            for (int i = 0; i < UpdateList.Count; i++)
            {
                bool result;
                FileInfo file = UpdateList[i];
                if (file == null)
                    continue;

                result = File.Exists(UpdateLocalFolder + file.Path);
                result = result && CheckMD5Hash(UpdateLocalFolder + file.Path) == file.Hash;
                if (!result)
                    AddToBroken(file);
                OnUpdateCheckProggress(new UpdateCheckEventArgs { DoneCount = i + 1, TotalCount = total });
            }
            SwitchState(true);
            UpdateList = null;
        }

        public void SwitchState(bool status)
        {
            lock (SyncState)
            {
                Running = status;
            }
        }

        private string CheckMD5Hash(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        private void AddToBroken(FileInfo file)
        {
            _updateManager.UpdateDownloader.AddToBroken(file);
        }

        private bool GetUpdateFile()
        {
            string data = Utils.Utils.DownloadDataFromFile(UpdateFileUrl);
            if (!String.IsNullOrEmpty(data))
            {
                try
                {
                    UpdateList = JsonConvert.DeserializeObject<List<FileInfo>>(data);
                    return true;
                }
                catch (Exception)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        protected virtual void OnUpdateCheckProggress(UpdateCheckEventArgs e)
        {
            UpdateCheckProggress?.Invoke(this, e);
        }
    }
}

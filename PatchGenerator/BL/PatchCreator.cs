using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using PatchGenerator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace PatchGenerator.BL
{
    class PatchCreator : IDisposable
    {
        public event PatchCreatorProgressEventHandler PatchCreatorProgress;
        public event PatchCreatorStatusEventHandler PatchCreatorStatus;

        private UpdateType UpdateType { get; set; }
        public PatchCreator(UpdateType updateType) => UpdateType = updateType;
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void CreatePatch()
        {
            (new Thread(() => {
                OnPatchCreatorStatus(new PatchCreatorStatusEventArgs{PatchCreateStatus = Data.PatchCreatorStatus.PREPARE});
                if (File.Exists(CurrentOutUpdateFolder() + "updateInfo.json"))
                {
                    string inFolder = CurrentInUpdateFolder();
                    string outFolder = CurrentOutUpdateFolder();
                    Tuple<List<Data.FileInfo>, List<Data.FileInfo>> filesInfos = ComparerUpdateInfo();
                    int done = 0;
                    OnPatchCreatorStatus(new PatchCreatorStatusEventArgs { PatchCreateStatus = Data.PatchCreatorStatus.COPYING });
                    foreach (var file in filesInfos.Item1)
                    {
                        FileSystem.CopyFile(inFolder + file.Path, outFolder + "files\\" + file.Path, true);
                        OnPatchCreatorProgress(new PatchCreatorProgressEventArgs { TotalProgress = filesInfos.Item1.Count, CurrentProgress = done++ });
                    }
                    SaveUpdateInfo(filesInfos.Item2);
                    OnPatchCreatorStatus(new PatchCreatorStatusEventArgs { PatchCreateStatus = Data.PatchCreatorStatus.DONE, TotalChanged = filesInfos.Item1.Count });
                }
                else
                {
                    FileSystem.CopyDirectory(CurrentInUpdateFolder(), CurrentOutUpdateFolder() + "files\\", UIOption.AllDialogs);
                    List<Data.FileInfo> filesInfo = ScanInputFolder();
                    SaveUpdateInfo(filesInfo);
                    OnPatchCreatorStatus(new PatchCreatorStatusEventArgs { PatchCreateStatus = Data.PatchCreatorStatus.DONE, TotalChanged = filesInfo.Count });
                }

                OpenFolder();
            })).Start();
        }

        private void OpenFolder()
        {
            Process.Start(CurrentOutUpdateFolder()+"..\\");
        }

        private void SaveUpdateInfo(List<Data.FileInfo> filesInfo)
        {
            string updateInfoFile = JsonConvert.SerializeObject(filesInfo);
            StreamWriter fileWriter = new StreamWriter(CurrentOutUpdateFolder() + "\\updateInfo.json");
            fileWriter.Write(updateInfoFile);
            fileWriter.Close();
        }



        private Tuple<List<Data.FileInfo>, List<Data.FileInfo>> ComparerUpdateInfo()
        {
            List<Data.FileInfo> newFilesInfo = ScanInputFolder();
            List<Data.FileInfo> oldFilesInfo = ParseExsistInfoFile();
            List<string> hashes = new List<string>();
            foreach (var file in oldFilesInfo)
            {
                hashes.Add(file.Hash);
            }
            Tuple< List < Data.FileInfo >, List<Data.FileInfo>> tupple = new Tuple<List<Data.FileInfo>, List<Data.FileInfo>>
                (newFilesInfo.Except(hashes).ToList(), 
                newFilesInfo.Except(oldFilesInfo).ToList());
            return tupple;
        }

        private List<Data.FileInfo> ParseExsistInfoFile()
        {
            return JsonConvert.DeserializeObject<List<Data.FileInfo>>(File.ReadAllText(CurrentOutUpdateFolder() + "updateInfo.json"));
        }
        private List<Data.FileInfo> ScanInputFolder()
        {
            string curDir = CurrentInUpdateFolder();
            List<Data.FileInfo> outData = new List<Data.FileInfo>();
            string[] files = Directory.GetFiles(curDir, "*", System.IO.SearchOption.AllDirectories);

            OnPatchCreatorStatus(new PatchCreatorStatusEventArgs { PatchCreateStatus = Data.PatchCreatorStatus.CHECKING });
            int done = 0;
            foreach (var file in files)
            {
                string hash = GetMd5HashFromFile(file);
                outData.Add(new Data.FileInfo
                {
                    Path = "\\"+file.Substring((curDir).Length),
                    Hash = hash
                });
                OnPatchCreatorProgress(new PatchCreatorProgressEventArgs{TotalProgress = files.Length, CurrentProgress = done++ });
            }
            return outData;
        }
        protected string GetMd5HashFromFile(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }
        private string CurrentOutUpdateFolder()
        {
            return UpdateType == UpdateType.CLIENT ? ".\\out\\client\\" : ".\\out\\patch\\";
        }
        private string CurrentInUpdateFolder()
        {
            return UpdateType == UpdateType.CLIENT ? ".\\in\\client\\" : ".\\in\\patch\\";
        }
        protected virtual void OnPatchCreatorProgress(PatchCreatorProgressEventArgs e)
        {
            PatchCreatorProgress?.Invoke(this, e);
        }
        protected virtual void OnPatchCreatorStatus(PatchCreatorStatusEventArgs e)
        {
            PatchCreatorStatus?.Invoke(this, e);
        }
    }
}

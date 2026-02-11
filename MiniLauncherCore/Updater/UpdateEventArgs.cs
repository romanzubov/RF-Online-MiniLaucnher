using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniLauncher.Updater
{
    public delegate void UpdateCheckEventHandler(object sender, UpdateCheckEventArgs e);
    public class UpdateCheckEventArgs
    {
        public long TotalCount { get; set; }
        public long DoneCount { get; set; }
    }
    public delegate void UpdateDownloadEventHandler(object sender, UpdateDownloadEventArgs e);
    public class UpdateDownloadEventArgs
    {
        public int TotalCount { get; set; }
        public int DoneCount { get; set; }
    }
    public delegate void UpdateCompleteEventHandler(object sender, UpdateCompleteEventArgs e);
    public class UpdateCompleteEventArgs
    {
        public string TaskName { get; set; }
    }
    public delegate void UpdateStartedEventHandler(object sender, UpdateStartedEventArgs e);
    public class UpdateStartedEventArgs
    {
        public string TaskName { get; set; }
    }
}

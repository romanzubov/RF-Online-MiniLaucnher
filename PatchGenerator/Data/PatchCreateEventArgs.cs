
namespace PatchGenerator.Data
{

    public delegate void PatchCreatorProgressEventHandler(object sender, PatchCreatorProgressEventArgs e);
    public class PatchCreatorProgressEventArgs
    {
        public int CurrentProgress { get; set; }
        public int TotalProgress { get; set; }
    }
    public delegate void PatchCreatorStatusEventHandler(object sender, PatchCreatorStatusEventArgs e);
    public class PatchCreatorStatusEventArgs
    {
        public int TotalChanged { get; set; }
        public PatchCreatorStatus PatchCreateStatus { get; set; }
    }
    public enum PatchCreatorStatus
    {
        COPYING,
        CHECKING,
        DONE,
        PREPARE
    }
}

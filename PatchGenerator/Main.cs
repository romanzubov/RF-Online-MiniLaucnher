using PatchGenerator.BL;
using PatchGenerator.Data;
using PatchGenerator.Helper;
using System;
using System.IO;
using System.Windows.Forms;

namespace PatchGenerator
{
    public partial class Main : Form
    {
        private LocalizationManager Lm;
        public Main()
        {
            Lm = LocalizationManager.GetInstance;
            InitializeComponent();
            InitLocalization();
            CreateFolders();
        }
        private void InitLocalization()
        {
            ForAllControls(this, control =>
            {
                control.Text = Lm.GetString(control.Name);
            });
        }
        private void CreateFolders()
        {
            if (!Directory.Exists(".\\out"))
            {
                Directory.CreateDirectory(".\\out");
            }
            if (!Directory.Exists(".\\out\\client"))
            {
                Directory.CreateDirectory(".\\out\\client");
            }
            if (!Directory.Exists(".\\out\\patch"))
            {
                Directory.CreateDirectory(".\\out\\patch");
            }
            if (!Directory.Exists(".\\out\\client\\files"))
            {
                Directory.CreateDirectory(".\\out\\client\\files");
            }
            if (!Directory.Exists(".\\out\\patch\\files"))
            {
                Directory.CreateDirectory(".\\out\\patch\\files");
            }

            if (!Directory.Exists(".\\in"))
            {
                Directory.CreateDirectory(".\\in");
            }
            if (!Directory.Exists(".\\in\\client"))
            {
                Directory.CreateDirectory(".\\in\\client");
            }
            if (!Directory.Exists(".\\in\\patch"))
            {
                Directory.CreateDirectory(".\\in\\patch");
            }
        }

        private void doPatch_Click(object sender, EventArgs e)
        {
            doPatch.Enabled = false;
            ControlBox = false;
            using (var patchCreator = new PatchCreator(GetUpdateType()))
            {
                patchCreator.PatchCreatorProgress += PatchCreator_PatchCreatorProgress;
                patchCreator.PatchCreatorStatus += PatchCreator_PatchCreatorStatus;
                patchCreator.CreatePatch();
            }
        }

        private void PatchCreator_PatchCreatorStatus(object sender, PatchCreatorStatusEventArgs e)
        {
            status_label.Invoke(new MethodInvoker(delegate
            {
                switch (e.PatchCreateStatus)
                {
                    case PatchCreatorStatus.COPYING:
                        status_label.Text = Lm.GetString("progress_copy");
                        break;
                    case PatchCreatorStatus.CHECKING:
                        status_label.Text = Lm.GetString("progress_scan");
                        break;
                    case PatchCreatorStatus.PREPARE:
                        status_label.Text = Lm.GetString("progress_prepare");
                        break;
                    case PatchCreatorStatus.DONE:
                        status_label.Text = String.Format(Lm.GetString("progress_done"),e.TotalChanged);
                        doPatch.Enabled = true;
                        ControlBox = true;
                        createProgress.Maximum = 100;
                        createProgress.Value = 100;
                        break;
                }
            }));
        }

        private void PatchCreator_PatchCreatorProgress(object sender, PatchCreatorProgressEventArgs e)
        {
            createProgress.Invoke(new MethodInvoker(delegate
            {
                createProgress.Maximum = e.TotalProgress;
                createProgress.Value = e.CurrentProgress;
            }));
        }

        private UpdateType GetUpdateType()
        {
            return rbClient.Checked ? UpdateType.CLIENT : UpdateType.PATCH;
        }

        private void clearIn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Lm.GetString("delete_directory_request"), Lm.GetString("delete_in_directory_request"), MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(".\\in\\client", true);
                    Directory.Delete(".\\in\\patch", true);
                    CreateFolders();
                }
                catch (Exception)
                {
                    // ignored
                }

                MessageBox.Show(Lm.GetString("Success"), Lm.GetString("delete_in_directory_success"), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void clearOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Lm.GetString("delete_directory_request"), Lm.GetString("delete_out_directory_request"), MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(".\\out\\client", true);
                    Directory.Delete(".\\out\\patch", true);
                    CreateFolders();
                }
                catch (Exception)
                {
                    // ignored
                }

                MessageBox.Show(Lm.GetString("Success"), Lm.GetString("delete_out_directory_success"), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        public static void ForAllControls(Control parent, Action<Control> action)
        {
            foreach (Control c in parent.Controls)
            {
                action(c);
                ForAllControls(c, action);
            }
        }
    }
}

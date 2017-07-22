using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using SoftuniAutozip.Interfaces;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace SoftuniAutozip.Services
{
    public class DialogManagers : IOpenFileDialogManager, IOpenFolderDialogManager
    {
        IEnumerable<string> IOpenFileDialogManager.ShowDialog(Window owner, string initialDirectory = "")
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = true,
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (!string.IsNullOrEmpty(initialDirectory))
            {
                ofd.InitialDirectory = initialDirectory;
            }
            if (ofd.ShowDialog(owner) == true)
            {
                return ofd.FileNames;
            }
            return null;
        }
        string IOpenFolderDialogManager.ShowDialog(string initialDirectory = "")
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    dialog.SelectedPath = initialDirectory;
                }
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }
            return "";
        }
    }
}
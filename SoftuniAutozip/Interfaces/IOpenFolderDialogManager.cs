using System.Collections.Generic;

namespace SoftuniAutozip.Interfaces
{
    public interface IOpenFolderDialogManager
    {
        string ShowDialog(string initialDirectory="");
    }
}
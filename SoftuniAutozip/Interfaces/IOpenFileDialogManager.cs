using System.Collections.Generic;
using System.Windows;

namespace SoftuniAutozip.Interfaces
{
    public interface IOpenFileDialogManager
    {
        IEnumerable<string> ShowDialog(Window owner,string initialDirectory="");
    }
}
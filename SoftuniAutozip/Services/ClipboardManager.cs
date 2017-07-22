using System.Collections.Specialized;
using System.IO;
using System.Windows;
using SoftuniAutozip.Interfaces;

namespace SoftuniAutozip.Services
{
    public class ClipboardManager : IClipboardManager
    {
        public bool CopyFile(string path)
        {
            StringCollection fileCollection = new StringCollection();
            if (!Directory.Exists(Path.GetDirectoryName(path)) || !File.Exists(path))
            {
                return false;
            }
            fileCollection.Add(path);
            Clipboard.SetFileDropList(fileCollection);
            return true;
        }
        public void CopyText(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
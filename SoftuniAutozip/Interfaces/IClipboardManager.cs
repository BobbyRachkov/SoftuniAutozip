namespace SoftuniAutozip.Interfaces
{
    public interface IClipboardManager
    {
        bool CopyFile(string path);
        void CopyText(string text);
    }
}
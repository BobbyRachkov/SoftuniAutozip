using System.Collections;
using System.Collections.Generic;

namespace SoftuniAutozip.Interfaces
{
    public interface IZipArchiver
    {
        IEnumerable<string> ExcludedDirectories { get; set; }
        IEnumerable<string> ExcludedFiles { get; set; }
        string BaseDirectory { get; set; }
        bool Archive(string archiveName, out string archiveFullName);
        bool Archive(string archiveName);
    }
}
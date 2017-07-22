using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using SoftuniAutozip.Interfaces;

namespace SoftuniAutozip.Services
{
    public class ZipArchiver : IZipArchiver
    {
        private readonly IClipboardManager _clipboardManager;
        public ZipArchiver(IClipboardManager clipboardManager)
        {
            _clipboardManager = clipboardManager;
        }
        public IEnumerable<string> ExcludedDirectories { get; set; }
        public IEnumerable<string> ExcludedFiles { get; set; }
        public string BaseDirectory { get; set; }

        public bool Archive(string archiveName)
        {
            return Archive(archiveName, out string garbage);
        }
        public bool Archive(string archiveName, out string archiveFullName)
        {
            archiveFullName = Path.GetFullPath($"{Path.Combine(BaseDirectory, "..\\")}{archiveName}.zip");
            ClearExistingArchive(archiveFullName);

            ZipFile.CreateFromDirectory(BaseDirectory, archiveFullName);
            ExcludeFilesAndDirectories(archiveFullName);

            return true;
        }

        private void ExcludeFilesAndDirectories(string archiveFullName)
        {
            using (ZipArchive archive = ZipFile.Open(archiveFullName, ZipArchiveMode.Update))
            {
                DeleteExcludedDirectories(archive);
                DeleteExcludedFiles(archive);
            }
        }
        private void DeleteExcludedDirectories(ZipArchive archive)
        {
            foreach (string excludedDirectory in ExcludedDirectories)
            {
                string exclDirectory = excludedDirectory.Substring(1);
                archive.Entries
                    .Where(x => x.FullName.StartsWith(exclDirectory))
                    .ToList()
                    .ForEach(x => x.Delete());
            }
        }
        private void DeleteExcludedFiles(ZipArchive archive)
        {
            foreach (string excludedFile in ExcludedFiles)
            {
                string exclDirectory = excludedFile.Substring(1);
                archive.Entries
                    .Where(x => x.FullName == exclDirectory)
                    .ToList()
                    .ForEach(x => x.Delete());
            }
        }

        private void ClearExistingArchive(string archiveFullName)
        {
            if (File.Exists(archiveFullName))
            {
                File.Delete(archiveFullName);
            }
        }
    }
}
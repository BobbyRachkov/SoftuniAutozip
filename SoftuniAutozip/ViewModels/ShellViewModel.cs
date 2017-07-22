using System;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Windows;
using Caliburn.Micro;
using SoftuniAutozip.Interfaces;
using SoftuniAutozip.Views;

namespace SoftuniAutozip.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        private ShellView _view;
        private string _archiveName;
        private bool _isFirstCall = true;
        private IObservableCollection<string> _excludedDirectories;
        private IObservableCollection<string> _excludedFiles;
        private string _baseArchiveDirectory;
        private bool _isBaseDirectorySelected;


        private readonly INameResolver _nameResolver;
        private readonly IOpenFileDialogManager _openFileDialogManager;
        private readonly IOpenFolderDialogManager _openFolderDialogManager;

        public ShellViewModel(INameResolver nameResolver, IOpenFolderDialogManager openFolderDialogManager,
            IOpenFileDialogManager openFileDialogManager)
        {
            DisplayName = "Softuni Autozip Configurator";

            _nameResolver = nameResolver;
            _openFolderDialogManager = openFolderDialogManager;
            _openFileDialogManager = openFileDialogManager;

            IsBaseDirectorySelected = false;

            ExcludedDirectories = new BindableCollection<string>();
            ExcludedFiles = new BindableCollection<string>();
        }

        public string BaseArchiveDirectory
        {
            get => _baseArchiveDirectory;
            set
            {
                _baseArchiveDirectory = value;
                NotifyOfPropertyChange();
            }
        }
        public string ArchiveName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_archiveName) && !_isFirstCall)
                {
                    _archiveName = _nameResolver.NewName();
                    _isFirstCall = false;
                }
                return _archiveName;
            }
            set
            {
                _archiveName = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsBaseDirectorySelected
        {
            get => _isBaseDirectorySelected;
            set
            {
                _isBaseDirectorySelected = value;
                NotifyOfPropertyChange();
            }
        }
        public IObservableCollection<string> ExcludedDirectories
        {
            get => _excludedDirectories;
            set
            {
                _excludedDirectories = value;
                NotifyOfPropertyChange();
            }
        }
        public IObservableCollection<string> ExcludedFiles
        {
            get => _excludedFiles;
            set
            {
                _excludedFiles = value;
                NotifyOfPropertyChange();
            }
        }
        public ShellView View
        {
            get
            {
                if (_view == null)
                {
                    _view = GetView() as ShellView;
                }
                return _view;
            }
        }


        public void Hide()
        {
            View.Hide();
        }
        public void Configure()
        {
            View.Show();
        }
        public void Exit()
        {
            View.TaskbarIcon.Dispose();
            TryClose();
        }

        public void ChooseBaseFolder()
        {
            string newPath= _openFolderDialogManager.ShowDialog(BaseArchiveDirectory);
            if (!string.IsNullOrWhiteSpace(newPath))
            {
                BaseArchiveDirectory = newPath;
                IsBaseDirectorySelected = true;

                ClearExcludedDirectories();
                ClearExcludedFiles();
            }
        }
        public void ExcludeFile()
        {
            var files = _openFileDialogManager
                .ShowDialog(View, BaseArchiveDirectory)?
                .Select(x =>
                {
                    if (!IsInBaseDirectory(x))
                    {
                        throw new ArgumentException(
                            "The files you exclude must be in the directory you want to archive!");
                    }
                    return x.Replace(BaseArchiveDirectory, "");
                });
            if (files == null || !files.Any())
            {
                return;
            }
            ExcludedFiles.AddRange(files);
        }
        public void ExcludeDirectory()
        {
            var directory = _openFolderDialogManager
                .ShowDialog(BaseArchiveDirectory);

            if (string.IsNullOrWhiteSpace(directory))
            {
                return;
            }
            if (!IsInBaseDirectory(directory))
            {
                throw new ArgumentException(
                    "The directory you exclude must be in the directory you want to archive!");
            }

            directory.Replace(BaseArchiveDirectory, "");
            ExcludedDirectories.Add(directory);
        }
        public void ClearExcludedDirectories()
        {
            ExcludedDirectories.Clear();
        }
        public void ClearExcludedFiles()
        {
            ExcludedFiles.Clear();
        }


        public bool IsInBaseDirectory(string fileOrFolder)
        {
            return !string.IsNullOrWhiteSpace(BaseArchiveDirectory) && fileOrFolder.Contains(BaseArchiveDirectory);
        }
    }
}
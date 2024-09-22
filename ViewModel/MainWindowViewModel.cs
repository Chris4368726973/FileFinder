﻿using FileFinder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileFinder
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        ObservableCollection<SearchResult> searchResults;
        public ObservableCollection<SearchResult> SearchResults {
            get => searchResults;
            set {

                searchResults = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchResults)));

                this.OpenSelectedFilesCommand.RaiseCanExecuteChanged();
                this.CopySelectedFilesCommand.RaiseCanExecuteChanged();

            }
        }

        ObservableCollection<string> savedSearchPaths;
        public ObservableCollection<string> SavedSearchPaths {
            get => savedSearchPaths;
            set
            {
                savedSearchPaths = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SavedSearchPaths)));
            }
        }

        string searchPath;     
        public string SearchPath {
            get => searchPath;
            set {        
                if (searchPath != value)
                {
                    searchPath = value;

                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchPath)));

                    this.SaveSearchPathCommand.RaiseCanExecuteChanged();
                    this.SearchFilecontentCommand.RaiseCanExecuteChanged();
                    this.SearchFilenameCommand.RaiseCanExecuteChanged();
                    SearchPathIsValid = FileService.PathisValid(SearchPath);
                }            
            }
        }

        bool searchPathIsValid;
        public bool SearchPathIsValid
        {
            get => searchPathIsValid;
            set
            {
                if (searchPathIsValid != value)
                {
                    searchPathIsValid = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchPathIsValid)));
                }
            }
        }

        string searchText;
        public string SearchText {
            get => searchText;
            set {
                if (searchText != value)
                {
                    searchText = value;                  
                    this.SearchFilecontentCommand.RaiseCanExecuteChanged();
                    this.SearchFilenameCommand.RaiseCanExecuteChanged();
                }
            }
        }

        bool filterPopupOpen;
        public bool FilterPopupOpen
        {
            get => filterPopupOpen;
            set
            {
                if (filterPopupOpen != value)
                {
                    filterPopupOpen = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterPopupOpen)));
                }
            }
        }

        bool isSearching;
        public bool IsSearching
        {
            get => isSearching;
            set
            {
                if (isSearching != value)
                {
                    isSearching = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSearching)));
                }
            }
        }

        public Filter Filter { get; set; }

        public bool FilterSearchSubfolders
        {
            get => Filter.SearchSubfolders;
            set
            {
                if (Filter.SearchSubfolders != value)
                {
                    Filter.SearchSubfolders = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterSearchSubfolders)));
                }
            }
        }

        public bool FilterSearchAllFiletypes
        {
            get => Filter.SearchAllFiletypes;
            set
            {
                if (FilterSearchAllFiletypes != value)
                {
                    Filter.SearchAllFiletypes = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterSearchSpecificFiletypes)));
                }

            }
        }

        public bool FilterSearchSpecificFiletypes
        {
            get => !Filter.SearchAllFiletypes;
            set
            {
                if (FilterSearchSpecificFiletypes != value)
                {
                    Filter.SearchAllFiletypes = !value;                  
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterSearchAllFiletypes)));
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterSearchSpecificFiletypes)));
                }
            }
        }

        public DelegateCommand SaveSearchPathCommand { get; set; }

        public DelegateCommand SelectSearchPathCommand { get; set; }

        public DelegateCommand OpenFilterPopupCommand { get; set; }

        public DelegateCommand CloseFilterPopupCommand { get; set; }

        public DelegateCommand SearchFilecontentCommand { get; set; }

        public DelegateCommand SearchFilenameCommand { get; set; }

        public DelegateCommand OpenSelectedFilesCommand { get; set; }

        public DelegateCommand CopySelectedFilesCommand { get; set; }

        public MainWindowViewModel()
        {            

            InitializeCommands();
            InitializeFilter();
            InitializeSettings();         

        }      

        public void SaveSearchPath()
        {

            SavedSearchPaths.Add(SearchPath);
            SettingsManager.SaveSettings(new Settings { SearchPaths = SavedSearchPaths });

        }

        public async void SearchFilesByFilecontent()
        {

            IsSearching = true;

            var results = await Task.Run(() =>
            {

                return FileSearcher.SearchFilesByFilecontent(searchText, searchPath, Filter);

            });

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SearchResults = results;
            });

            IsSearching = false;

        }

        public async void SearchFilesByFilename()
        {

            IsSearching = true;

            var results = await Task.Run(() =>
            {

                return FileSearcher.SearchFilesByFilename(searchText, searchPath, Filter);

            });

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SearchResults = results;
            });

            IsSearching = false;         

        }

        public void OpenSelectedFiles()
        {
            foreach (var file in SearchResults)
            {

                if (file.Selected)
                {

                    FileService.OpenFile(file.Path);

                }
                
            }
        }

        public void CopySelectedFiles()
        {

            String folderPath = FileDialog.OpenFileDialog();

            SearchText = folderPath;

            foreach (var file in SearchResults)
            {

                if (file.Selected)
                {

                    FileService.MoveFile(file.Name, file.Path, folderPath);

                }            

            }

        }

        private void InitializeCommands()
        {

            this.SaveSearchPathCommand = new DelegateCommand(
                (o) => FileService.PathisValid(SearchPath),
                (o) => { SaveSearchPath(); }
            );

            this.SelectSearchPathCommand = new DelegateCommand(
                (o) => { SearchPath = FileDialog.OpenFileDialog(); 
            });

            this.SearchFilecontentCommand = new DelegateCommand(
                (o) => FileService.PathisValid(SearchPath) && searchText?.Length > 0,
                (o) => { SearchFilesByFilecontent(); }
            );

            this.SearchFilenameCommand = new DelegateCommand(
                (o) => FileService.PathisValid(SearchPath) && searchText?.Length > 0,
                (o) => { SearchFilesByFilename(); }
            );

            this.OpenFilterPopupCommand = new DelegateCommand(
                (o) => { FilterPopupOpen = true; 
            });
            this.CloseFilterPopupCommand = new DelegateCommand(
                (o) => { FilterPopupOpen = false; 
            });

            this.OpenSelectedFilesCommand = new DelegateCommand(
                (o) => SearchResults?.Count > 0,
                (o) => { OpenSelectedFiles(); }
            );

            this.CopySelectedFilesCommand = new DelegateCommand(
                (o) => SearchResults?.Count > 0,
                (o) => { CopySelectedFiles(); }
            );

        }

        private void InitializeFilter()
        {

            this.Filter = new Filter { SearchSubfolders = true, SearchAllFiletypes = true, Filetypes = ".txt;" };

        }

        private void InitializeSettings()
        {

            var settings = SettingsManager.LoadSettings();

            this.SavedSearchPaths = settings.SearchPaths;

        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}

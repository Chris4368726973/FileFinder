﻿using FileFinder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<SearchResult> SearchResults { get; set; }

        string searchPath;     
        public string SearchPath {
            get => searchPath;
            set {        
                if (searchPath != value)
                {
                    searchPath = value;

                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(searchPath)));

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
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(searchPathIsValid)));
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
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(filterPopupOpen)));
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
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isSearching)));
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

        public DelegateCommand SelectSearchPathCommand { get; set; }

        public DelegateCommand OpenFilterPopupCommand { get; set; }

        public DelegateCommand CloseFilterPopupCommand { get; set; }

        public DelegateCommand SearchFilecontentCommand { get; set; }

        public DelegateCommand SearchFilenameCommand { get; set; }

        public DelegateCommand OpenSelectedFilesCommand { get; set; }

        public DelegateCommand CopySelectedFilesCommand { get; set; }

        public MainWindowViewModel()
        {
            this.Filter = new Filter { SearchSubfolders=true, SearchAllFiletypes=true, Filetypes=".txt;" };

            this.SelectSearchPathCommand = new DelegateCommand((o) => { SearchPath = FileDialog.OpenFileDialog(); });

            this.SearchFilecontentCommand = new DelegateCommand(
                (o) => FileService.PathisValid(SearchPath),
                (o) => { SearchFilesByFilecontent(); }
            );

            this.SearchFilenameCommand = new DelegateCommand(
                (o) => FileService.PathisValid(SearchPath),
                (o) => {  SearchFilesByFilename(); }
            );

            this.OpenFilterPopupCommand = new DelegateCommand((o) => { FilterPopupOpen = true; });
            this.CloseFilterPopupCommand = new DelegateCommand((o) => { FilterPopupOpen = false; });

            this.OpenSelectedFilesCommand = new DelegateCommand(
                (o) => SearchResults?.Count > 0,
                (o) => { OpenSelectedFiles(); }
            );

            this.CopySelectedFilesCommand = new DelegateCommand(
                (o) => SearchResults?.Count > 0,
                (o) => { CopySelectedFiles(); }
            );

        }      

        public async void SearchFilesByFilecontent()
        {

            if (SearchText?.Length > 0) {

                IsSearching = true;        

                await Task.Run(() =>
                {

                    SearchResults = FileSearcher.SearchFilesByFilecontent(searchText, searchPath, Filter);

                });               

                IsSearching = false;

            } else {

                SearchResults = new ObservableCollection<SearchResult>();

            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchResults)));
            this.OpenSelectedFilesCommand.RaiseCanExecuteChanged();
            this.CopySelectedFilesCommand.RaiseCanExecuteChanged();

        }

        public async void SearchFilesByFilename()
        {

            if (SearchText?.Length > 0)
            {

                IsSearching = true;

                await Task.Run(() =>
                {

                    SearchResults = FileSearcher.SearchFilesByFilename(searchText, searchPath, Filter);

                });                

                IsSearching = false;

            } else {

                SearchResults = new ObservableCollection<SearchResult>();

            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchResults)));
            this.OpenSelectedFilesCommand.RaiseCanExecuteChanged();
            this.CopySelectedFilesCommand.RaiseCanExecuteChanged();

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

        public event PropertyChangedEventHandler PropertyChanged;

    }
}

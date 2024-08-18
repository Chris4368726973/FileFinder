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
                    this.SearchCommand.RaiseCanExecuteChanged();
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
                    this.SearchCommand.RaiseCanExecuteChanged();
                    SearchTextIsValid = !String.IsNullOrEmpty(SearchText);
                }
            }
        }

        bool searchTextIsValid;
        public bool SearchTextIsValid
        {
            get => searchTextIsValid;
            set
            {
                if (searchTextIsValid != value)
                {
                    searchTextIsValid = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(searchTextIsValid)));
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

        public DelegateCommand OpenFilterPopupCommand { get; set; }

        public DelegateCommand CloseFilterPopupCommand { get; set; }

        public DelegateCommand SearchCommand { get; set; }

        public DelegateCommand OpenSelectedFilesCommand { get; set; }

        public MainWindowViewModel()
        {
            this.Filter = new Filter { SearchSubfolders=true, SearchAllFiletypes=true, Filetypes=".txt;" };

            this.SearchCommand = new DelegateCommand(
                (o) => FileService.PathisValid(SearchPath) && !String.IsNullOrEmpty(SearchText),
                (o) => { SearchFiles(); }
            );

            this.OpenFilterPopupCommand = new DelegateCommand((o) => { FilterPopupOpen = true; });
            this.CloseFilterPopupCommand = new DelegateCommand((o) => { FilterPopupOpen = false; });

            this.OpenSelectedFilesCommand = new DelegateCommand(
                (o) => SearchResults?.Count > 0,
                (o) => { OpenFiles(); }
            );

        }      

        public void SearchFiles()
        {
            SearchResults = FileSearcher.SearchFiles(searchText, searchPath, Filter);

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchResults)));
            this.OpenSelectedFilesCommand.RaiseCanExecuteChanged();

        }

        public void OpenFiles()
        {
            foreach (var file in SearchResults)
            {

                if (file.Selected)
                {

                    FileService.OpenFile(file.Path);

                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}

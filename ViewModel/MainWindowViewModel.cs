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

        public DelegateCommand SearchCommand { get; set; }

        public MainWindowViewModel()
        {

            this.SearchCommand = new DelegateCommand(
                (o) => FileService.PathisValid(SearchPath) && !String.IsNullOrEmpty(SearchText),
                (o) => { SearchFiles(); }
            );

        }      

        public void SearchFiles()
        {
            SearchResults = FileSearcher.SearchFiles(searchText, searchPath);

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchResults)));

        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}

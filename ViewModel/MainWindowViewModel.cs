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
                }
            }
        }

        public DelegateCommand SearchCommand { get; set; }

        public MainWindowViewModel()
        {

            this.SearchCommand = new DelegateCommand(
                (o) => !String.IsNullOrEmpty(SearchPath) && !String.IsNullOrEmpty(SearchText),
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

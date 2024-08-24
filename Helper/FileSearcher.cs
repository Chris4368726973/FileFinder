using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace FileFinder
{
    public static class FileSearcher
    {

        private static readonly object LockObject = new object();

        public static ObservableCollection<SearchResult> SearchFilesByFilecontent(string textToSearch, string pathToSearch, Filter filter)
        {
            ObservableCollection<SearchResult> searchResults = new ObservableCollection<SearchResult>();

            Regex regex = new Regex(Regex.Escape(textToSearch), RegexOptions.IgnoreCase);

            Parallel.ForEach(FileService.GetFiles(pathToSearch, filter), fileObject =>
            {

                if (regex.IsMatch(FileService.GetFileContent(fileObject)))
                {
                    lock (LockObject)
                    {

                        searchResults.Add(new SearchResult { Name = fileObject.Name, Path = fileObject.FilePath, Selected = false });

                    }
                    
                }

            });

            return searchResults;

        }

        public static ObservableCollection<SearchResult> SearchFilesByFilename(string textToSearch, string pathToSearch, Filter filter)
        {

            ObservableCollection<SearchResult> searchResults = new ObservableCollection<SearchResult>();

            Regex regex = new Regex(Regex.Escape(textToSearch), RegexOptions.IgnoreCase);

            Parallel.ForEach(FileService.GetFiles(pathToSearch, filter), fileObject =>
            {

                if (regex.IsMatch(fileObject.Name))
                {
                    lock (LockObject)
                    {

                        searchResults.Add(new SearchResult { Name = fileObject.Name, Path = fileObject.FilePath, Selected = false });

                    }

                }

            });

            return searchResults;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;

namespace FileFinder
{
    public static class FileSearcher
    {

        public static ObservableCollection<SearchResult> SearchFilesByFilecontent(string textToSearch, string pathToSearch, Filter filter)
        {

            if (textToSearch?.Length == 0)
            {

                return new ObservableCollection<SearchResult>();

            }

            var searchResults = new ConcurrentBag<SearchResult>();

            Regex regex = new Regex(Regex.Escape(textToSearch), RegexOptions.IgnoreCase);

            Parallel.ForEach(FileService.GetFiles(pathToSearch, filter), fileObject =>
            {

                if (regex.IsMatch(FileService.GetFileContent(fileObject)))
                {

                    searchResults.Add(new SearchResult
                    {
                        Name = fileObject.Name,
                        Path = fileObject.FilePath,
                        Selected = false
                    });

                }

            });

            return new ObservableCollection<SearchResult>(searchResults);

        }

        public static ObservableCollection<SearchResult> SearchFilesByFilename(string textToSearch, string pathToSearch, Filter filter)
        {

            if (textToSearch?.Length == 0)
            {

                return new ObservableCollection<SearchResult>();

            }

            var searchResults = new ConcurrentBag<SearchResult>();

            Regex regex = new Regex(Regex.Escape(textToSearch), RegexOptions.IgnoreCase);

            Parallel.ForEach(FileService.GetFiles(pathToSearch, filter), fileObject =>
            {

                if (regex.IsMatch(fileObject.Name))
                {
                    searchResults.Add(new SearchResult
                    {
                        Name = fileObject.Name,
                        Path = fileObject.FilePath,
                        Selected = false
                    });
                }

            });

            return new ObservableCollection<SearchResult>(searchResults);

        }

    }
}

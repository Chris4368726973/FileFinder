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

        public static ObservableCollection<SearchResult> SearchFiles(string textToSearch, string pathToSearch)
        {
            ObservableCollection<SearchResult> searchResults = new ObservableCollection<SearchResult>();

            Regex regex = new Regex(Regex.Escape(textToSearch), RegexOptions.IgnoreCase);

            List<FileObject> fileObjects = FileService.GetFiles(pathToSearch);

            Parallel.ForEach(fileObjects, fileObject =>
            {

                if (TextInContentFinder.IsRegexInContent(regex, fileObject.Content))
                {
                    lock (LockObject)
                    {

                        searchResults.Add(new SearchResult { Name = fileObject.Name, Pfad = fileObject.FilePath });

                    }
                    
                }

            });

            return searchResults;

        }

    }
}

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

        public static ObservableCollection<SearchResult> SearchFiles(string textToSearch, string pathToSearch)
        {
            ObservableCollection<SearchResult> searchResults = new ObservableCollection<SearchResult>();

            Regex regex = new Regex(Regex.Escape(textToSearch), RegexOptions.IgnoreCase);

            List<FileObject> fileObjects = FileService.GetFiles(pathToSearch);

            foreach (var fileObject in fileObjects)
            {

                if (TextInContentFinder.IsRegexInContent(regex, fileObject.Content))
                {
                    searchResults.Add(new SearchResult { Name = fileObject.Name, Pfad = fileObject.FilePath });
                }

            }

            return searchResults;

        }

    }
}

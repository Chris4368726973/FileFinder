using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace FileFinder
{
    public static class TextInContentFinder
    {

        public static bool IsRegexInContent(Regex regex, string content) => regex.IsMatch(content);

    }
}

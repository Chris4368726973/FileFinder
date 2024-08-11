using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class Filter
    {

        public bool SearchSubfolders { get; set; }

        public bool SearchAllFiletypes { get; set; }

        public string Filetypes { get; set; }

        public bool IsFiletypeIncluded(string filetype)
        {
            if (SearchAllFiletypes) { 
                return true; 
            } else {
                return Filetypes.Contains(filetype + ";");
            }
        }

    }
}

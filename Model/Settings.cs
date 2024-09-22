using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class Settings
    {

        public ObservableCollection<string> SearchPaths { get; set; }

        public Settings() { 
        
            SearchPaths = new ObservableCollection<string>();
        
        }

    }
}

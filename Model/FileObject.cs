using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class FileObject
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string Content { get; set; }

    }
}

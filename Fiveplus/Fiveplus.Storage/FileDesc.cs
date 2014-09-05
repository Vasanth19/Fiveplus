using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiveplus.Storage
{
    public class FileDesc
    {
        public string UserName { get; set; }
        public string GigId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public FileDesc()
        { }
    }
}

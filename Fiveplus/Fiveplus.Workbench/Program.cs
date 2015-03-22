using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Storage;

namespace Fiveplus.Workbench
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fileStream = System.IO.File.OpenRead(@"C:\Projects\GitHub\Fiveplus\Fiveplus\Fiveplus.Kicker\assets\img\bg\7.jpg"))
            {
              //  StorageManager.UploadImage(fileStream, "7x.jpg","image/png");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnusDreams.Util
{
    class ChangeBg
    {
        static string startupPath = Environment.CurrentDirectory;
        public static void ChangeBag(Backgrounds backgrounds)
        {

            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new Backgrounds();

            //p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + backgrounds + ".jpg", UriKind.Relative));

        }
    }
}

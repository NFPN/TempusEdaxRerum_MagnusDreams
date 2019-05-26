using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MagnusDreams.Util
{
    public class Audio
    {
        static string  startupPath = Environment.CurrentDirectory;

        public  enum Audios
        {
            bgSoundsss, //.wav
            //"bgSoundsss.wav"

        };

        public static void PlayMusic(Enum audioEnum)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + audioEnum + ".wav", UriKind.Relative));
            p1.Volume = 0.5;
            p1.Play();
        }
    }
}

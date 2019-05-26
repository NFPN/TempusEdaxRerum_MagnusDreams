using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static MagnusDreams.Views.OptionsView;

namespace MagnusDreams.Util
{
    public class Audio
    {
        static string  startupPath = Environment.CurrentDirectory;


        public static void PlayMusic(Musicas musica)
        {
            
                DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + musica + ".wav", UriKind.Relative));
            p1.Volume = 0.5;
            p1.Play();
        }

        public static void PlayMusic(Efeitos efeitos)
        {
            
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + efeitos + ".wav", UriKind.Relative));
            p1.Volume = 0.5;
            p1.Play();


        }
    }
}
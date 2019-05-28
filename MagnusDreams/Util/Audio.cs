using MagnusDreams.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static MagnusDreams.Views.OptionsView;
using System.Media;


namespace MagnusDreams.Util
{
    public class Audio
    {
        static string  startupPath = Environment.CurrentDirectory;
        static Gameplay gameplay = new Gameplay();
        static OptionsView op = new OptionsView();


        public static void PlayMusic(Musicas musica)
        {
            
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + musica + ".wav", UriKind.Relative));
            if (gameplay.musicIsChecked.IsChecked == true || op.musicIsChecked.IsChecked == true)
            {  
                if (!p1.IsMuted)
                p1.Volume = 0.5;
                p1.Play();
            }
            else if (gameplay.musicIsChecked.IsChecked == false || op.musicIsChecked.IsChecked == false){
                MuteMusic(p1);
            }            
        }

        public static void PlayMusic(Efeitos efeitos)
        {
           
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + efeitos + ".wav", UriKind.Relative));
            p1.Volume = 0.25;
            
            p1.Play();


        }
        //acho que n precisa se o codigo acima funcionar
        public static void MuteMusic(MediaPlayer music)
        {
            music.Volume = 0;
            music.IsMuted = true;
        }
    }
}
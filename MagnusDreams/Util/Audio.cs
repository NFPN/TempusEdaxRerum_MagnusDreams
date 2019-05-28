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
        public static List<MediaPlayer> InitializeAudios = new List<MediaPlayer>();

        public static void bgMusic()
        {
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + Musicas.bgSoundsss + ".wav", UriKind.Relative));
           
            p1.Volume = 0.05;
            p1.Play();
            InitializeAudios.Add(p1);
            
        }
        public static void sfxAudio()
        {
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + Efeitos.shineselect + ".wav", UriKind.Relative));

            p1.Volume = 0.01;
            p1.Play();
            InitializeAudios.Add(p1);

        }

        public static void MuteBgMusic()
        {
            InitializeAudios[0].Volume = 0.0;
        }

        public static void DesmuteBgMusic()
        {
            //InitializeAudios[0].Volume = 0.5;
        }

        public static void MuteSfx()
        {
            InitializeAudios[1].Volume = 0.0;
        }

        public static void DesmuteSfx()
        {
            //InitializeAudios[0].Volume = 0.5;
        }





        //public static void PlayMusic(Musicas musica)
        //{
        //    
        //    DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);
        //
        //    Directory.GetDirectories(directoryInfo.FullName);
        //
        //    var p1 = new MediaPlayer();
        //
        //    p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + musica + ".wav", UriKind.Relative));
        //    
        //        
        //        p1.Volume = 0.25;
        //        p1.Play();
        //    
        //              
        //}
        //
        public static void PlayMusic(Efeitos efeitos)
        {
           
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);
        
            Directory.GetDirectories(directoryInfo.FullName);
        
            var p1 = new MediaPlayer();
        
            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + efeitos + ".wav", UriKind.Relative));
            p1.Volume = 0.01;
            
            p1.Play();
        
        
        }
        
    }
}
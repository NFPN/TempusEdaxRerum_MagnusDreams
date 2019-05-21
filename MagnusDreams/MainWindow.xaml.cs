using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MagnusDreams.Views;


namespace MagnusDreams
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public Window appWindow;
        public List<MediaPlayer> audios = new List<MediaPlayer>();
        string startupPath = Environment.CurrentDirectory;

        

        public MainWindow()
        {
            
            InitializeComponent();
            AudioGame("bgSoundsss.wav");
            contentControl.Content = new MenuView();
            //MenuOptions();

            appWindow = GetWindow(this);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new CreditosView();
            
        }

        public void ChangeBG()
        {
            Fundo.Source = new BitmapImage(new Uri("Images\\Sky.jpg", UriKind.Relative));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        public void ChangeVisibility(Control[] sender, bool isVisible)
        {
            foreach (var obj in sender)
            {
                obj.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public  void AudioGame(string audioName)
        {

            // Get application path and return 2 foldes (parents) 
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

            Directory.GetDirectories(directoryInfo.FullName);

            var p1 = new MediaPlayer();

            p1.Open(new Uri(directoryInfo.FullName + @"\Sounds\" + audioName , UriKind.Relative));
            p1.Play();
            //Add backgroung to yout list of audios if necessary
            audios.Add(p1);
            //MessageBox.Show(directoryInfo.FullName + @"\Sounds");

        }
    }
}

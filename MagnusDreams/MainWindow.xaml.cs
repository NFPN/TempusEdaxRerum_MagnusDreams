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
using MagnusDreams.Util;
using MagnusDreams.Views;
using static MagnusDreams.Util.Audio;

namespace MagnusDreams
{
    public partial class MainWindow : Window
    {
        static public Window appWindow;
        OptionsView op = new OptionsView();
        static string startupPath = Environment.CurrentDirectory;

        public MainWindow()
        {
            
            InitializeComponent();
            
            ChangeBG(Backgrounds.MENUPrancheta);
            contentControl.Content = new MenuView();
            //Chama a classe Audio criada recentemente
            
            //MenuOptions();

            appWindow = GetWindow(this);
            bgMusic();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new CreditosView();
            
        }
        public void ChangeBG(Backgrounds backgrounds)
        {
                DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(startupPath).FullName);

                Directory.GetDirectories(directoryInfo.FullName);
             
            //Try and done !!
            ImageBrush brush = new ImageBrush();
            BitmapImage bitmapImage = new BitmapImage(new Uri(directoryInfo.FullName + @"\Images\" + backgrounds + ".jpg"));
            brush.ImageSource = bitmapImage;
            GridMenu.Background = brush;
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
    }
}

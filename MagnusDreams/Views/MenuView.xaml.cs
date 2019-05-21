using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using MagnusDreams;
using Microsoft.Win32;

namespace MagnusDreams.Views
{
    /// <summary>
    /// Interação lógica para MenuView.xam
    /// </summary>
    public partial class MenuView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        //List<MediaPlayer> audios = new List<MediaPlayer>();
        //string startupPath = Environment.CurrentDirectory;

        public MenuView()
        {
            
            //AudioGame("bgSound.mp4");
            InitializeComponent();
            ShowMenu();

        }

        private void CreditsButton_Clicked(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new CreditosView();
            HiddenMenu();
            //Teste de som
            main.AudioGame("shineselect.wav");
        }

        private void ExitButton_Clicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            //Teste de som
            main.AudioGame("select.wav");
        }

        private void OptionsButton_Clicked(object sender, RoutedEventArgs e)
        {
            //Cast app window for proper acess (new MainWindow doesn't work)
            contentControl.Content = new OptionsView();
            HiddenMenu();

            //Teste de som
            main.AudioGame("click.wav");
            

        }

        private void NewGame_Clicked(object sender, RoutedEventArgs e)
        {
            //---- For testing -----
            contentControl.Content = new Gameplay();
            //----------------------
            HiddenMenu();

            //Teste de som
            main.AudioGame("click.wav");
        }

        public void HiddenMenu()
        {
            main.ChangeVisibility(new Control[] {
            btnCredits, btnOptions, btn_Close,
            btnNewGame }, false);
        }

        public void ShowMenu()
        {
            main.ChangeVisibility(new Control[] {
            btnCredits, btnOptions, btn_Close,
            btnNewGame }, true);
        }        
    }
}

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
using MagnusDreams.Util;
using static MagnusDreams.Util.Audio;

namespace MagnusDreams.Views
{
    public partial class MenuView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;     
        
        public MenuView()
        {                     
            InitializeComponent();
            ShowMenu();
        }

        private void CreditsButton_Clicked(object sender, RoutedEventArgs e)
        {
            sfxAudio();
            main.ChangeBG(Backgrounds.LivroCreditosPrancheta);
            contentControl.Content = new CreditosView();
            HiddenMenu();
            
            
        }

        private void ExitButton_Clicked(object sender, RoutedEventArgs e)
        {
            sfxAudio(); PlayMusic(Efeitos.shineselect);
            Environment.Exit(0);
            
        }

        private void OptionsButton_Clicked(object sender, RoutedEventArgs e)
        {
            sfxAudio();
            main.ChangeBG(Backgrounds.MontainBg);
            //Cast app window for proper acess (new MainWindow doesn't work)
            contentControl.Content = new OptionsView();
            HiddenMenu();

            
        }

        private void NewGame_Clicked(object sender, RoutedEventArgs e)
        {
            sfxAudio();
            main.ChangeBG(Backgrounds.fundo);

            //---- For testing -----
            contentControl.Content = new Gameplay();
            //----------------------
            HiddenMenu();
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

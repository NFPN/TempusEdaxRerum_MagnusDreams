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
using MagnusDreams;

namespace MagnusDreams.Views
{
    /// <summary>
    /// Interação lógica para MenuView.xam
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void CreditsButton_Clicked(object sender, RoutedEventArgs e)
        {
           
        }

        private void ExitButton_Clicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OptionsButton_Clicked(object sender, RoutedEventArgs e)
        {
            //Cast app window for proper acess (new MainWindow doesn't work)
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.ChangeBG();
            MessageBox.Show("Funciona!");
        }

        private void NewGame_Clicked(object sender, RoutedEventArgs e)
        {
            //---- For testing -----
            contentControl.Content = new Gameplay();
            //----------------------
            ChangeVisibility(new Control[] {
            btnCredits, btnOptions, btn_Close,
            btnNewGame }, false);
        }

        private void ChangeVisibility(Control[] sender, bool isVisible)
        {
            foreach (var obj in sender)
            {
                obj.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}

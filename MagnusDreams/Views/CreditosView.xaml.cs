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

namespace MagnusDreams.Views
{
    /// <summary>
    /// Interaction logic for CreditosView.xaml
    /// </summary>
    public partial class CreditosView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;

        public CreditosView()
        {
            InitializeComponent();
        }

        private void ReturnButton_Clicked(object sender, RoutedEventArgs e)
        {
            contentControlCredits.Content = new MenuView();

            main.ChangeVisibility(new Control[]{
                textDescriptionAudio,
                textDescriptionDesigners,
                textDescriptionEquip,
                textDescriptionProgrammers,
                btnReturn,
                textNamesAudio,
                textNamesDesingners,
                textNamesProgrammers               
            },false);
        }

        private void OpenGamePause(object sender, RoutedEventArgs e)
        {
            //contentControlCredits.Content = new InGamePauseView();
            main.ChangeVisibility(new Control[] {
                btnPause
            },false);
        }
    }
}

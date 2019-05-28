using MagnusDreams.Util;
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
using static MagnusDreams.Util.Audio;

namespace MagnusDreams.Views
{
    public partial class CreditosView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;

        public CreditosView()
        {
            InitializeComponent();
        }

        private void ReturnButton_Clicked(object sender, RoutedEventArgs e)
        {
            sfxAudio();
            main.ChangeBG(Backgrounds.MENUPrancheta);
            contentControlCredits.Content = new MenuView();
            //backgroundFundo.Visibility = Visibility.Hidden;
            

            main.ChangeVisibility(new Control[]{
               
                textDescriptionAudio,
                textDescriptionDesigners,
                textDescriptionEquip,
                textDescriptionProgrammers,
                btnReturn,
                textNamesAudio,
                textNamesDesingners,
                textNamesProgrammers
                
            }, false);
        }
    }
}

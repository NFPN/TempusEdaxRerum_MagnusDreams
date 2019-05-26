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
    public partial class OptionsView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
       
        public OptionsView()
        {
            InitializeComponent();
        }

        private void sfxOff(object sender, System.EventArgs e)
        {
            //faz com q os barulhos do bottão fiquem off
        }

        private void musicOn(object sender, System.EventArgs e)
        {
            if(main != null)
            {
                //main.audios[0].Volume = 0.5;
            }
            
        }

        private void sfxOn(object sender, System.EventArgs e)
        {
            //faz com q os barulhos do bottão fiquem on
        }

        private void musicOff(object sender, System.EventArgs e)
        {
           // main.audios[0].Volume = 0;
        }

        private void ReturnButtonOp_Clicked(object sender, RoutedEventArgs e)
        {
            //Teste de som
            //main.AudioGame("click.wav");

            contentControl.Content = new MenuView();
            backgroundFundo.Visibility = Visibility.Hidden;
            main.ChangeVisibility(new Control[]{
                txtmusicVolume,
                txtSfxVolume,
                musicIsChecked,
                sfxIsChecked,
                btnReturn
            }, false);


        }        
    }
}
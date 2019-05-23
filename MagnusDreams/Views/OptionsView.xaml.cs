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
    /// Interaction logic for OptionsView.xaml
    /// </summary>
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
            //main.audios[0].Volume = 0.5f;
        }

        private void sfxOn(object sender, System.EventArgs e)
        {
            //faz com q os barulhos do bottão fiquem on
        }

        private void musicOff(object sender, System.EventArgs e)
        {
            main.audios[0].Volume = 0;
        }

        private void ReturnButtonOp_Clicked(object sender, RoutedEventArgs e)
        {
            //Teste de som
            main.AudioGame("click.wav");

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

        //private void focustest(object sender, RoutedEventArgs e)
        // {
        //
        //    Button tb = e.Source as Button;
        //    tb.Background = Brushes.Red;
        //    tb.Foreground = Brushes.Yellow;
        //    
        //}
        //
        //private void BtnReturn_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    Button tb = e.Source as Button;
        //    tb.Background = Brushes.Aquamarine;
        //}

        private void testemouseEnter(object sender, MouseEventArgs e)
        {
            Button tb = e.Source as Button;
            tb.Background = Brushes.Aquamarine;
            
        }
    }
}
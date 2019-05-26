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
    /// Interação lógica para InGamePauseView.xam
    /// </summary>
    public partial class InGamePauseView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;

        public InGamePauseView()
        {
            InitializeComponent();
        }

        private void ReturnMainMenuClicked(object sender, RoutedEventArgs e)
        {
            //Teste de som
           //main.AudioGame("shineselect.wav");

            contentControlPaused.Content = new MenuView();

            bgPauseGame.Visibility = Visibility.Hidden;
            ImgPause.Visibility = Visibility.Hidden;

            main.ChangeVisibility(new Control[]
            {
                txtmusicVolume,
                txtSfxVolume,
                musicIsChecked,
                sfxIsChecked,
                btnReturnMenu,
                btnReturnToGame

            }, false);
        }

        private void musicOn(object sender, RoutedEventArgs e)
        {
            //main.AudioGame("bgSoundsss.wav");
        }

        private void musicOff(object sender, System.EventArgs e)
        {
            //main.audios[0].Volume = 0;
        }
    }
}

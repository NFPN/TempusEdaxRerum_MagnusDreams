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
    public partial class InGamePauseView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;

        public InGamePauseView()
        {
            InitializeComponent();
        }

        private void ReturnMainMenuClicked(object sender, RoutedEventArgs e)
        {
            //-----para voltar pro jogo------
            contentControlPaused.Content = Gameplay.thiscontentControl;
            //------------------------------

            PlayMusic(Efeitos.select);

            //contentControlPaused.Visibility = Visibility.Collapsed;
            //contentControlPaused.Content = new MenuView();

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
            PlayMusic(Efeitos.select);
        }

        private void musicOff(object sender, System.EventArgs e)
        {
            //main.audios[0].Volume = 0;
        }
    }
}

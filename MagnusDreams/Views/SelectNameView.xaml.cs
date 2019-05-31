using MagnusDreams.Util;
using MagnusDrems.DAO;//para pegar o comando sql
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
using System.Windows.Shapes;
using static MagnusDreams.Util.Audio;

namespace MagnusDreams.Views
{
    /// <summary>
    /// Interação lógica para SelectNameView.xam
    /// </summary>
    public partial class SelectNameView : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        ComandosSQL comandos = new ComandosSQL();

        public SelectNameView()
        {
            InitializeComponent();
        }
        private void StartGame_Clicked(object sender, RoutedEventArgs e)
        {
            //sfxAudio();
            //Insere o texto inserido do player no Banco de dados
            string NomeJogador = lbTextNamePlayer.Text;
           comandos.InsertData(NomeJogador);
           main.ChangeBG(Backgrounds.fundo);

            main.ChangeVisibility(new Control[]{lbTextNamePlayer,btnBegin,textDescription
            },false);
           contentControl.Content = new Gameplay();
            //((Gameplay)contentControl.Content).gamePlayClass = (Gameplay)contentControl.Content;
        }

        private void CleanLabel(object sender, RoutedEventArgs e)
        {
            lbTextNamePlayer.Text = string.Empty;
        }
    }
}

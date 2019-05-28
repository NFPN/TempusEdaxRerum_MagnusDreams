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
    public partial class SelectName : Window
    {
        ComandosSQL comandos = new ComandosSQL();

        public SelectName()
        {
            InitializeComponent();
        }

        private void StartGame_Clicked(object sender, RoutedEventArgs e)
        {
            sfxAudio();
            //Insere o texto inserido do player no Banco de dados
            string NomeJogador = lbTextNamePlayer.Text;
            comandos.InsertData(NomeJogador);
            contentControl.Content = new Gameplay();
        }
    }
}

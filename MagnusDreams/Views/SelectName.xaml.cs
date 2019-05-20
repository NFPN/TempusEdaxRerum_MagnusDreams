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

namespace MagnusDreams.Views
{
    /// <summary>
    /// Lógica interna para SelectName.xaml
    /// </summary>

    public partial class SelectName : Window
    {
        ComandosSQL comandos = new ComandosSQL();
        public SelectName()
        {
            InitializeComponent();
        }

        private void StartGame_Clicked(object sender, RoutedEventArgs e)
        {

            //Insere o texto inserido do player no Banco de dados
            comandos.InsertData(lbTextNamePlayer.Text);
        }
    }
}

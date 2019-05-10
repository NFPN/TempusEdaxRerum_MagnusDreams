using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagnusDreams
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();


            changeVisibility(new Control[]{
                btnBegin,
                lbTextNamePlayer,
                textDescription,
                btnReturn,
                textDescriptionEquip,
                textDescriptionAudio,
                textDescriptionDesigners,
                textDescriptionProgrammers,
                lbTextNamePlayer,
                textNamesDesingners,
                textNamesAudio,
                textNamesProgrammers}, false);
            

        }

        private void changeVisibility(Control[] sender,bool isVisible )
        {
            foreach (var obj in sender)
            {
                obj.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }

            

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            btnCredits.Visibility = Visibility.Hidden;
            btnOptions.Visibility = Visibility.Hidden;
            btn_Close.Visibility = Visibility.Hidden;
            btnNewGame.Visibility = Visibility.Hidden;
            btnBegin.Visibility = Visibility.Visible;
            lbTextNamePlayer.Visibility = Visibility.Visible;
            textDescription.Visibility = Visibility.Visible;

            Background.Source = new BitmapImage(new Uri("Sky.jpg", UriKind.Relative));
        }

        private void BtnOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCredits_Click(object sender, RoutedEventArgs e)
        {
            btnCredits.Visibility = Visibility.Hidden;
            btnOptions.Visibility = Visibility.Hidden;
            btn_Close.Visibility = Visibility.Hidden;
            btnNewGame.Visibility = Visibility.Hidden;
            btnReturn.Visibility = Visibility.Visible;
            lbTextNamePlayer.Visibility = Visibility.Hidden;

            textDescriptionEquip.Visibility = Visibility.Visible;
            textDescriptionAudio.Visibility = Visibility.Visible;
            textDescriptionDesigners.Visibility = Visibility.Visible;
            textDescriptionProgrammers.Visibility = Visibility.Visible;
            textNamesDesingners.Visibility = Visibility.Visible;
            textNamesAudio.Visibility = Visibility.Visible;
            textNamesProgrammers.Visibility = Visibility.Visible;

        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            btnCredits.Visibility = Visibility.Visible;
            btnOptions.Visibility = Visibility.Visible;
            btn_Close.Visibility = Visibility.Visible;
            btnNewGame.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Hidden;

            textDescriptionEquip.Visibility = Visibility.Hidden;
            textDescriptionAudio.Visibility = Visibility.Hidden;
            textDescriptionDesigners.Visibility = Visibility.Hidden;
            textDescriptionProgrammers.Visibility = Visibility.Hidden;
            lbTextNamePlayer.Visibility = Visibility.Hidden;
            textNamesDesingners.Visibility = Visibility.Hidden;
            textNamesAudio.Visibility = Visibility.Hidden;
            textNamesProgrammers.Visibility = Visibility.Hidden;
        }
    }
}

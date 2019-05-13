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

            ChangeVisibility(new Control[]{
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

        private void ChangeVisibility(Control[] sender, bool isVisible)
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
            ChangeVisibility(new Control[] { btnCredits, btnOptions, btn_Close, btnNewGame }, false);
            ChangeVisibility(new Control[] { btnBegin, lbTextNamePlayer, textDescription }, true);

            Background.Source = new BitmapImage(new Uri("Sky.jpg", UriKind.Relative));
        }

        private void BtnOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCredits_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibility(new Control[] {
            btnCredits, btnOptions, btn_Close,
            btnNewGame, btnReturn, lbTextNamePlayer }, false);

            ChangeVisibility(new Control[] {
            textDescriptionEquip, textDescriptionAudio,
            textDescriptionDesigners, textDescriptionProgrammers,
            textNamesDesingners, textNamesAudio, textNamesProgrammers }, true);
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibility(new Control[] { btnCredits, btnOptions, btn_Close, btnNewGame, btnReturn }, false);

            ChangeVisibility(new Control[] {
            textDescriptionEquip, textDescriptionAudio, textDescriptionDesigners, textDescriptionProgrammers,
            lbTextNamePlayer, textNamesDesingners, textNamesAudio, textNamesProgrammers }, true);
        }
    }
}

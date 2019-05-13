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
using MagnusDreams.ViewModels;

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
            MenuOptions();
        }

        private void ChangeVisibility(Control[] sender, bool isVisible)
        {
            foreach (var obj in sender)
            {
                obj.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public void MenuOptions()
        {
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

            ChangeVisibility(new Control[]{btnNewGame,btnOptions,btnCredits,btn_Close}, true);
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
        

        private void CreditsButton_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new CreditosViewModel();

            ChangeVisibility(new Control[] {
            btnCredits, btnOptions, btn_Close,
            btnNewGame, lbTextNamePlayer }, false);

            ChangeVisibility(new Control[] {
            textDescriptionEquip, textDescriptionAudio,
            textDescriptionDesigners, textDescriptionProgrammers,
            textNamesDesingners, textNamesAudio, textNamesProgrammers, btnReturn }, true);
        }

        private void ExitButton_Clicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OptionsButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ChangeVisibility(new Control[] { btnCredits, btnOptions, btn_Close, btnNewGame }, false);
            ChangeVisibility(new Control[] { btnBegin, lbTextNamePlayer, textDescription }, true);

            Background.Source = new BitmapImage(new Uri("Sky.jpg", UriKind.Relative));
        }

        private void NewGame_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void StartGame_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnButton_Clicked(object sender, RoutedEventArgs e)
        {
            MenuOptions();
        }
    }
}

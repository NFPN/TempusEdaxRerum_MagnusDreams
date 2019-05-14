﻿using System;
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
using MagnusDreams.Views;

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
            this.contentControl.Content = new MenuView();
            //MenuOptions();
        }

        /*private void ChangeVisibility(Control[] sender, bool isVisible)
        {
            foreach (var obj in sender)
            {
                obj.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }*/

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new CreditosView();

        }

        public void ChangeBG()
        {
            Background.Source = new BitmapImage(new Uri("Images\\Sky.jpg", UriKind.Relative));
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        /*public void MenuOptions()
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
            Background.Source = new BitmapImage(new Uri("Images\\Sky.jpg", UriKind.Relative));
            Button btn = (Button)sender;
            ChangeVisibility(new Control[] { btnCredits, btnOptions, btn_Close, btnNewGame }, false);
            ChangeVisibility(new Control[] { btnBegin, lbTextNamePlayer, textDescription }, true);

            
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
        }*/
    }
}

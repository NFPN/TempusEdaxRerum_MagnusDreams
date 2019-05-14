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
            contentControl.Content = new MenuView();
            //MenuOptions();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new CreditosView();

        }

        public void ChangeBG()
        {
            Fundo.Source = new BitmapImage(new Uri("Images\\Sky.jpg", UriKind.Relative));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        public void ChangeVisibility(Control[] sender, bool isVisible)
        {
            foreach (var obj in sender)
            {
                obj.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}

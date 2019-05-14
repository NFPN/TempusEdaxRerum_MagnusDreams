using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
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
    /// Interaction logic for Gameplay.xaml
    /// </summary>
    public partial class Gameplay : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();



        public Gameplay()
        {
            InitializeComponent();
            timer.Tick += Timer;
            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.Start();
        }

        private void Timer(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {
            Log.Content = $"" +
                $"Top:{Canvas.GetTop(PlayerTest)} " +
                $"Left:{Canvas.GetLeft(PlayerTest)} "; 

            if (Keyboard.IsKeyDown(Key.Down) && Canvas.GetTop(PlayerTest) < 720-PlayerTest.ActualHeight)
            {
                Canvas.SetTop(PlayerTest, Canvas.GetTop(PlayerTest) + 10);
            }
            if (Keyboard.IsKeyDown(Key.Up) && Canvas.GetTop(PlayerTest) > 0)
            {
                Canvas.SetTop(PlayerTest, Canvas.GetTop(PlayerTest) - 10);
            }
            if (Keyboard.IsKeyDown(Key.Left)&& Canvas.GetLeft(PlayerTest) > 0)
            {
                Canvas.SetLeft(PlayerTest, Canvas.GetLeft(PlayerTest) - 10);
            }
            if (Keyboard.IsKeyDown(Key.Right) && Canvas.GetLeft(PlayerTest) < 1280 - PlayerTest.ActualWidth)
            {
                Canvas.SetLeft(PlayerTest, Canvas.GetLeft(PlayerTest) + 10);
            }


        }
    }
}

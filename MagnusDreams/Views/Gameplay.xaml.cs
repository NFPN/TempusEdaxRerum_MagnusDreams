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
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Start();

        }

        private void TimerTick(object sender, EventArgs e)
        {

        }
    }
}

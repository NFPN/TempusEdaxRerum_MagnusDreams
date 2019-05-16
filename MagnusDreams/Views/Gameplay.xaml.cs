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
        bool shouldMakeNewbullet = false;

        List<Image> bulletPool = new List<Image>();

        DispatcherTimer mainTime = new DispatcherTimer(), dangerTimer = new DispatcherTimer();

        BitmapSource bitmap = new BitmapImage(new Uri("Images/fundo.jpg", UriKind.Relative));

        DateTime startingTime = DateTime.Now;

        TimeSpan deltaTime = new TimeSpan(), shotTime = new TimeSpan();

        double elapsedSeconds, elapseMilidSeconds, timeToShoot, speed, fireRate;



        public Gameplay()
        {
            InitializeComponent();
            PlayerBullet.Visibility = Visibility.Hidden;
            
            elapsedSeconds = 0;
            elapseMilidSeconds = 0;
            fireRate = 2;
            timeToShoot = 0;
            speed = 10;

            mainTime.Tick += MainTimeController;
            mainTime.Tick += SecondaryTimeController;
            mainTime.Interval = TimeSpan.FromMilliseconds(5);
            mainTime.Start();


            dangerTimer.Tick += DangerTick;
            dangerTimer.Interval = TimeSpan.FromMilliseconds(1);
            dangerTimer.Start();
        }

        private void MainTimeController(object sender, EventArgs e)
        {
            Update();
        }

        private void SecondaryTimeController(object sender, EventArgs e)
        {
            AutoUpdate();
        }

        private void DangerTick(object sender, EventArgs e)
        {
            deltaTime = DateTime.Now - startingTime;
            elapseMilidSeconds = deltaTime.Milliseconds;
            elapsedSeconds = deltaTime.Seconds;
            //visual debug
            Log.Content = $"{bulletPool.Count}" +
                $" - {string.Format("{0:0}", elapsedSeconds)}" +
                $" - {string.Format("{0:0.00}", elapseMilidSeconds)}";
        }

        private void AutoUpdate()
        {
            
            if (bulletPool.Count > 0)
                foreach (var bullet in bulletPool)
                {
                    if (Canvas.GetTop(bullet) > 720 ||
                        Canvas.GetTop(bullet) < 0 ||
                        Canvas.GetLeft(bullet) > 1280 ||
                        Canvas.GetLeft(bullet) < 0)//need collision verification
                    {
                        bullet.Visibility = Visibility.Hidden;
                    }
                    else if (bullet.Visibility == Visibility.Visible)
                    {
                        Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + speed);
                    }
                }
        }



        private void Update()
        {
            if (Keyboard.IsKeyDown(Key.Down) && Canvas.GetTop(PlayerTest) < 720 - PlayerTest.ActualHeight)
            {
                Canvas.SetTop(PlayerTest, Canvas.GetTop(PlayerTest) + 10);
            }
            if (Keyboard.IsKeyDown(Key.Up) && Canvas.GetTop(PlayerTest) > 0)
            {
                Canvas.SetTop(PlayerTest, Canvas.GetTop(PlayerTest) - 10);
            }
            if (Keyboard.IsKeyDown(Key.Left) && Canvas.GetLeft(PlayerTest) > 0)
            {
                Canvas.SetLeft(PlayerTest, Canvas.GetLeft(PlayerTest) - 10);
            }
            if (Keyboard.IsKeyDown(Key.Right) && Canvas.GetLeft(PlayerTest) < 1280 - PlayerTest.ActualWidth)
            {
                Canvas.SetLeft(PlayerTest, Canvas.GetLeft(PlayerTest) + 10);
            }
            if (Keyboard.IsKeyDown(Key.A) && elapsedSeconds > timeToShoot)
            {
                timeToShoot = elapsedSeconds + fireRate;
                shouldMakeNewbullet = true;

                if (bulletPool.Count > 0)
                    for (int i = 0; i < bulletPool.Count; i++)
                    {
                        if (bulletPool[i].Visibility == Visibility.Hidden)
                        {
                            SetBullet(bulletPool[i]);
                            shouldMakeNewbullet = false;
                        }
                    }

                if (shouldMakeNewbullet)
                {
                    bulletPool.Add(new Image()
                    {
                        Height = PlayerBullet.Height,
                        Width = PlayerBullet.Width,
                        Source = PlayerBullet.Source,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Visibility = Visibility.Hidden,
                        Tag = "PlayerBullet"
                    });
                    GameCanvas.Children.Add(bulletPool.LastOrDefault());
                    SetBullet(bulletPool.LastOrDefault());
                }
            }
        }

        public void SetBullet(Image bullet)
        {
            //GameCanvas.Children.Add(bullet);
            Canvas.SetTop(bullet, Canvas.GetTop(PlayerTest));
            Canvas.SetLeft(bullet, Canvas.GetLeft(PlayerTest) + PlayerTest.ActualWidth);
            bullet.Visibility = Visibility.Visible;
            bullet.Refresh();
        }


    }

    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };


        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}

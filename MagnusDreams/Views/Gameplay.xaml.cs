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
using System.Timers;

namespace MagnusDreams.Views
{
    /// <summary>
    /// Interaction logic for Gameplay.xaml
    /// </summary>
    public partial class Gameplay : UserControl
    {
        #region Global Variables


        DateTime startingTime = DateTime.Now;

        List<Image> bulletPool = new List<Image>();

        bool canShoot = false, shouldMakeNewBullet = false, canMove = true;

        TimeSpan deltaTime = new TimeSpan(), shotTime = new TimeSpan();

        double elapsedSeconds, elapsedMiliSeconds, timeToShoot, speed, fireRate;

        DispatcherTimer mainTime = new DispatcherTimer(), fastTimer = new DispatcherTimer();

        BitmapSource bitmap = new BitmapImage(new Uri("Images/fundo.jpg", UriKind.Relative));


        #endregion


        public Gameplay()
        {
            InitializeComponent();

            fastTimer.Tick += GlobalTick;
            fastTimer.Interval = TimeSpan.FromMilliseconds(1);

            mainTime.Tick += SlowTimeTick;
            mainTime.Interval = TimeSpan.FromMilliseconds(5);

            Start();
            fastTimer.Start();
            mainTime.Start();
        }

        private void SlowTimeTick(object sender, EventArgs e)
        {
            Update();
        }

        private void GlobalTick(object sender, EventArgs e)
        {
            //TimeControl
            deltaTime = DateTime.Now - startingTime;
            elapsedMiliSeconds = deltaTime.Milliseconds;
            elapsedSeconds = deltaTime.Seconds;
            timeToShoot = elapsedMiliSeconds % 100;


            //Loop Methods
            FastUpdate();
        }

        private void Start()
        {
            PlayerBullet.Visibility = Visibility.Hidden;
            elapsedMiliSeconds = 0;
            elapsedSeconds = 0;
            timeToShoot = 0;
            fireRate = 2;
            speed = 10;
        }

        private void FastUpdate()
        {
            if (elapsedMiliSeconds > 100)
                canShoot = true;

            if (Keyboard.IsKeyDown(Key.A))
            {
                if (canShoot && timeToShoot >= 85)
                {
                    canShoot = false;
                    if (bulletPool.Count > 0)
                        GetExistingBullet();
                    else
                        NewBullet();
                }
            }
        }



        private void Update()
        {
            #region Movement Logic
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
            #endregion

            //Bullet pooling logic [need collision verification]
            if (bulletPool.Count > 0)
            {
                foreach (var bullet in bulletPool)
                {
                    if (Canvas.GetTop(bullet) > 720 ||
                        Canvas.GetTop(bullet) < 0 ||
                        Canvas.GetLeft(bullet) > 1280 ||
                        Canvas.GetLeft(bullet) < 0)
                    {
                        bullet.Visibility = Visibility.Hidden;
                    }
                    else if (bullet.Visibility == Visibility.Visible)
                    {
                        Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + speed);
                    }
                }
            }
        }


        public void NewBullet()
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

        public void GetExistingBullet()
        {
            shouldMakeNewBullet = true;
            for (int i = 0; i < bulletPool.Count; i++)
            {
                if (bulletPool[i].Visibility == Visibility.Hidden)
                {
                    SetBullet(bulletPool[i]);
                    shouldMakeNewBullet = false;
                    break;
                }
            }
            if (shouldMakeNewBullet)
                NewBullet();
        }

        public void SetBullet(Image bullet)
        {
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

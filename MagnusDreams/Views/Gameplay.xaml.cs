using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using MagnusDreams.Extensions;

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

        TimeSpan deltaTime = new TimeSpan();

        double elapsedMiliSeconds, timeToShoot, speed, playerStartingLeft, playerStartingTop;

        DispatcherTimer mainTime = new DispatcherTimer(), fastTimer = new DispatcherTimer();


        //BitmapSource bitmap = new BitmapImage(new Uri("Images/fundo.jpg", UriKind.Relative));


        #endregion


        public Gameplay()
        {
            InitializeComponent();

            //Sets player respawn position
            playerStartingLeft = Canvas.GetLeft(PlayerImage);
            playerStartingTop = Canvas.GetTop(PlayerImage);

            //Low input lag controller( Com erro!)
            //KeyboardController kbControl = new KeyboardController(Window.GetWindow(this));
            //kbControl.timer.Interval = TimeSpan.FromMilliseconds(1);
            //kbControl.KeyboardTick += Movement;

            fastTimer.Tick += GlobalTick;
            fastTimer.Interval = TimeSpan.FromMilliseconds(1);

            mainTime.Tick += SlowTimeTick;
            mainTime.Interval = TimeSpan.FromMilliseconds(5);

            Start();
            mainTime.Start();
            fastTimer.Start();
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
            timeToShoot = elapsedMiliSeconds % 100;


            //Loop Methods
            FastUpdate();
        }

        private void Start()
        {
            PlayerBullet.Visibility = Visibility.Hidden;
            elapsedMiliSeconds = 0;
            timeToShoot = 0;
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


        /*private void Movement(object sender, EventArgs e)
        {
            Log.Content = e;
        }*/


        private void Update()
        {
            if (Keyboard.IsKeyDown(Key.Escape))
                Log.Content = "Pause";
            #region Movement Logic
            if (canMove)
            {
                if (Keyboard.IsKeyDown(Key.Down) && Canvas.GetTop(PlayerImage) < 720 - PlayerImage.ActualHeight)
                {
                    Canvas.SetTop(PlayerImage, Canvas.GetTop(PlayerImage) + 10);
                }
                if (Keyboard.IsKeyDown(Key.Up) && Canvas.GetTop(PlayerImage) > 0)
                {
                    Canvas.SetTop(PlayerImage, Canvas.GetTop(PlayerImage) - 10);
                }
                if (Keyboard.IsKeyDown(Key.Left) && Canvas.GetLeft(PlayerImage) > 0)
                {
                    Canvas.SetLeft(PlayerImage, Canvas.GetLeft(PlayerImage) - 10);
                }
                if (Keyboard.IsKeyDown(Key.Right) && Canvas.GetLeft(PlayerImage) < 1280 - PlayerImage.ActualWidth)
                {
                    Canvas.SetLeft(PlayerImage, Canvas.GetLeft(PlayerImage) + 10);
                }
            }
            #endregion

            //Bullet pooling logic [need collision verification]
            if (bulletPool.Count > 0)
            {
                foreach (var bullet in bulletPool)
                {
                    if (bullet.Visibility == Visibility.Visible)
                    {
                        Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + speed);
                    }
                    CollisionCheck(bullet, true);
                }
            }
            CollisionCheck(PlayerImage, false);
        }

        public void CollisionCheck(Image obj, bool isBullet)
        {
            foreach (Image img in GameCanvas.Children.OfType<Image>())
            {
                if (img.Tag != null && img.Tag.ToString() == "Enemy")
                {
                    //Draw rectantgles on top of all
                    var x1 = Canvas.GetLeft(img);
                    var y1 = Canvas.GetTop(img);
                    var x2 = Canvas.GetLeft(obj);
                    var y2 = Canvas.GetTop(obj);
                    Rect r1 = new Rect(x1, y1, img.ActualWidth, img.ActualHeight);
                    Rect r2 = new Rect(x2, y2, obj.ActualWidth, obj.ActualHeight);

                    //Check rectangles collision
                    if (r1.IntersectsWith(r2))
                    {
                        if (obj.Tag.ToString() != null && obj.Tag.ToString() == "Player")
                        {
                            Canvas.SetLeft(PlayerImage, playerStartingLeft);
                            Canvas.SetTop(PlayerImage, playerStartingTop);
                            return;
                        }
                        obj.Visibility = Visibility.Hidden;
                        return;
                    }
                }
                else if ((Canvas.GetTop(obj) > 720 ||
                          Canvas.GetTop(obj) < 0 ||
                          Canvas.GetLeft(obj) > 1280 ||
                          Canvas.GetLeft(obj) < 0) && isBullet)
                {
                    obj.Visibility = Visibility.Hidden;
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
            bullet.Refresh();
            bullet.Visibility = Visibility.Visible;
            Canvas.SetLeft(bullet, Canvas.GetLeft(PlayerImage) + PlayerImage.ActualWidth);
            Canvas.SetTop(bullet, Canvas.GetTop(PlayerImage) + PlayerImage.ActualHeight - (bullet.ActualHeight * 2));
            bullet.Refresh();
        }
    }
}

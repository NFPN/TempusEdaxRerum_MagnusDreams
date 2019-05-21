using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using MagnusDreams.Util;
using System.Windows.Media;

namespace MagnusDreams.Views
{
    public partial class Gameplay : UserControl
    {
        #region Global Variables

        public enum ObjType { Player, Enemy, PlayerBullet }

        //Time Info
        double elapsedMiliSeconds;
        TimeSpan deltaTime = new TimeSpan();
        DateTime startingTime = DateTime.Now;
        DispatcherTimer mainTime = new DispatcherTimer(), fastTimer = new DispatcherTimer();

        //Bullet Info
        ImageSource bulletSource;
        double bulletHeight, bulletWidth;

        //Enemy Info
        ImageSource enemySource;
        double enemyBaseHeight, enemyBaseWidth;
        List<EntityObject> enemyPool = new List<EntityObject>();

        //Player Info
        EntityObject player;
        List<Image> playerBulletPool = new List<Image>();
        bool shouldMakeNewBullet, canMove = true;
        double playerInitialLeftPosition, playerInitialTopPosition, playerSpeed, timeToShootPlayerBullets;

        #endregion

        
        public Gameplay()
        {
            InitializeComponent();
            
            KeyboardController kbcontrol = new KeyboardController(MainWindow.appWindow);
            kbcontrol.timer.Interval = TimeSpan.FromMilliseconds(1);
            kbcontrol.KeyboardTick += Movement;

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
            timeToShootPlayerBullets = elapsedMiliSeconds % 100;

            //Loop Methods
            FastUpdate();
        }

        // Executes once when the game scene is loaded
        private void Start()
        {
            //Initializing timers
            elapsedMiliSeconds = 0;
            timeToShootPlayerBullets = 0;

            //Player Setup
            playerInitialLeftPosition = Canvas.GetLeft(PlayerImage);
            playerInitialTopPosition = Canvas.GetTop(PlayerImage);
            player = new EntityObject(3, PlayerImage);
            playerSpeed = 20;
            
            //Scene Cleanup
            PlayerBullet.Visibility = Visibility.Hidden;
            GameCanvas.Children.Remove(PlayerBullet);
            
            //Test purpose only(change o update later)
            foreach (var img in GameCanvas.Children.OfType<Image>())
            {
                if (img.Tag != null && img.Tag.ToString() == "Enemy")
                {
                    enemyPool.Add(new EntityObject(3, img));
                }
            }
        }

        private void FastUpdate()
        {

        }


        private void Movement(object sender, EventArgs e)
        {
            if (((KeyboardController)sender).KeyDown(Key.A) || ((KeyboardController)sender).KeyDown(Key.Space))
                if (timeToShootPlayerBullets >= 85)
                    if (playerBulletPool.Count > 0)
                        GetExistingBullet();
                    else
                        NewBullet();
        }


        private void Update()
        {
            if (Keyboard.IsKeyDown(Key.Escape))
                Log.Content = "Pause";

            //Movement Logic
            if (canMove)
            {
                if (Keyboard.IsKeyDown(Key.Down) && Canvas.GetTop(player.Image) < 720 - player.Image.ActualHeight)
                {
                    Canvas.SetTop(player.Image, Canvas.GetTop(player.Image) + 10);
                }
                if (Keyboard.IsKeyDown(Key.Up) && Canvas.GetTop(player.Image) > 0)
                {
                    Canvas.SetTop(player.Image, Canvas.GetTop(player.Image) - 10);
                }
                if (Keyboard.IsKeyDown(Key.Left) && Canvas.GetLeft(player.Image) > 0)
                {
                    Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) - 10);
                }
                if (Keyboard.IsKeyDown(Key.Right) && Canvas.GetLeft(player.Image) < 1280 - player.Image.ActualWidth)
                {
                    Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) + 10);
                }
            }
            
            //Player collision check
            CollisionCheck(player.Image, ObjType.Player);

            //Bullet pooling and collision
            if (playerBulletPool.Count > 0)
            {
                foreach (var bullet in playerBulletPool)
                {
                    if (bullet.Visibility == Visibility.Visible)
                    {
                        Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + playerSpeed);
                        CollisionCheck(bullet, ObjType.PlayerBullet);
                    }
                }
            }
        }

        public void CollisionCheck(Image imgObj, ObjType type)
        {
            foreach (Image img in GameCanvas.Children.OfType<Image>())
            {
                //Draw rectantgles on top of all
                var x1 = Canvas.GetLeft(img);
                var y1 = Canvas.GetTop(img);
                var x2 = Canvas.GetLeft(imgObj);
                var y2 = Canvas.GetTop(imgObj);
                Rect r1 = new Rect(x1, y1, img.ActualWidth, img.ActualHeight);
                Rect r2 = new Rect(x2, y2, imgObj.ActualWidth, imgObj.ActualHeight);

                //Check rectangles collision
                if (r1.IntersectsWith(r2) && img.Tag != null)
                {
                    if (type == ObjType.Player && img.Tag.ToString() == "Enemy")
                    {
                        player.Life--;
                        if (player.Life <= 0)
                        {
                            //gameover
                        }
                    }
                    else if (type == ObjType.Enemy)
                    {
                        CheckOutOfBounds(imgObj);
                    }
                    else if (type == ObjType.PlayerBullet)
                    {
                        CheckOutOfBounds(imgObj);
                    }

                    if (imgObj.Tag.ToString() != null && imgObj.Tag.ToString() == "Player")
                    {
                        Canvas.SetLeft(PlayerImage, playerInitialLeftPosition);
                        Canvas.SetTop(PlayerImage, playerInitialTopPosition);
                        return;
                    }
                    img.Visibility = Visibility.Hidden;
                    imgObj.Visibility = Visibility.Hidden;
                    GameCanvas.Children.Remove(img);
                    GameCanvas.Children.Remove(imgObj);
                    return;
                }
            }
        }

        public void CheckOutOfBounds(Image imgObj)
        {
            if (Canvas.GetTop(imgObj) > 720 || Canvas.GetTop(imgObj) < 0 || Canvas.GetLeft(imgObj) > 1280 || Canvas.GetLeft(imgObj) < 0)
            {
                Canvas.SetLeft(imgObj, -100);
                imgObj.Visibility = Visibility.Hidden;
            }
        }


        public void NewBullet()
        {
            playerBulletPool.Add(new Image()
            {
                Height = PlayerBullet.Height,
                Width = PlayerBullet.Width,
                Source = PlayerBullet.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                Tag = "PlayerBullet"
            });
            //GameCanvas.Children.Add(bulletPool.LastOrDefault());
            SetBullet(playerBulletPool.LastOrDefault());
        }

        public void GetExistingBullet()
        {
            shouldMakeNewBullet = true;
            for (int i = 0; i < playerBulletPool.Count; i++)
            {
                if (playerBulletPool[i].Visibility == Visibility.Hidden)
                {
                    SetBullet(playerBulletPool[i]);
                    shouldMakeNewBullet = false;
                    break;
                }
            }
            if (shouldMakeNewBullet)
                NewBullet();
        }

        public void SetBullet(Image bullet)
        {
            if (!GameCanvas.Children.Contains(bullet))
                GameCanvas.Children.Add(bullet);
            bullet.Refresh();
            bullet.Visibility = Visibility.Visible;
            Canvas.SetLeft(bullet, Canvas.GetLeft(PlayerImage) + PlayerImage.ActualWidth);
            Canvas.SetTop(bullet, Canvas.GetTop(PlayerImage) + PlayerImage.ActualHeight - (bullet.ActualHeight * 2));
            bullet.Refresh();
        }
    }
}

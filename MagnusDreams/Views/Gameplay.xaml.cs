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

        //The hidden position of player and playerBullet objects, enemy and enemyBullet objects
        double[,] hiddenPos = new double[2, 2] 
        { 
            { -250, 0 }, 
            { -250, 250 }
        }; 

        //Time Info
        double elapsedMiliSeconds;
        TimeSpan deltaTime = new TimeSpan();
        DateTime startingTime = DateTime.Now;
        DispatcherTimer mainTime = new DispatcherTimer(), fastTimer = new DispatcherTimer();

        //Enemy Info
        //ImageSource enemySource;
        //double enemyBaseHeight, enemyBaseWidth;
        //List<EntityObject> enemyPool = new List<EntityObject>();

        //Player Info
        EntityObject player;
        List<EntityObject> playerBulletPool = new List<EntityObject>();
        bool shouldMakeNewBullet, canMove;
        double playerInitialLeftPosition, playerInitialTopPosition, playerSpeed, timeToShootPlayerBullets;

        List<EntityObject> allObjects = new List<EntityObject>();

        #endregion

        
        public Gameplay()
        {
            InitializeComponent();
            
            KeyboardController kbcontrol = new KeyboardController(MainWindow.appWindow);
            kbcontrol.timer.Interval = TimeSpan.FromMilliseconds(1);
            kbcontrol.KeyboardTick += InputChecker;

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
            allObjects.Add(player);
            
            //Initializing timers
            elapsedMiliSeconds = 0;
            timeToShootPlayerBullets = 0;

            //Player Setup
            playerInitialLeftPosition = Canvas.GetLeft(PlayerImage);
            playerInitialTopPosition = Canvas.GetTop(PlayerImage);
            player = new EntityObject(3, PlayerImage, ObjType.Player);
            playerSpeed = 20;
            canMove = true;

            //Enemy Setup

            //Scene Cleanup
            PlayerBullet.Visibility = Visibility.Hidden;
            GameCanvas.Children.Remove(PlayerBullet);
            
            //Test purpose only(change o update later)
            foreach (var img in GameCanvas.Children.OfType<Image>())
            {
                if (img.Tag != null && img.Tag.ToString() == "Enemy")
                {
                    allObjects.Add(new EntityObject(3, img, ObjType.Enemy));
                }
            }
        }

        private void FastUpdate()
        {

        }

        private void InputChecker(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
                Log.Content = "Pause";

            //Shooting Logic
            if (((KeyboardController)sender).KeyDown(Key.A) || ((KeyboardController)sender).KeyDown(Key.Space))
                if (timeToShootPlayerBullets >= 85)
                    if (playerBulletPool.Count > 0) GetExistingBullet();
                    else NewBullet();

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
        }

        private void Update()
        {
            CollisionCheck();

            //Bullet pooling and collision
            if (playerBulletPool.Count > 0)
            {
                foreach (var bullet in playerBulletPool)
                {
                    if (bullet.Image.Visibility == Visibility.Visible)
                    {
                        Canvas.SetLeft(bullet.Image, Canvas.GetLeft(bullet.Image) + playerSpeed);
                    }
                }
            }
        }

        public void CollisionUpdate(EntityObject obj)
        {
            var x = Canvas.GetLeft(obj.Image);
            var y = Canvas.GetTop(obj.Image);
            obj.Rect =  new Rect(x, y, obj.Image.ActualWidth, obj.Image.ActualHeight);
        }

        public void CollisionCheck()
        {
            foreach(var obj1 in allObjects)
            foreach (var obj2 in allObjects)
            {
                //Check rectangles collision
                if (obj1.Rect.IntersectsWith(obj2.Rect))
                {
                    //PlayerCollided
                    if (obj1.Type == ObjType.Player && obj2.Type != ObjType.Player && obj2.Type != ObjType.PlayerBullet)
                    {
                            Log.Content = "PlayerCollided";
                        //player.Life--;
                        if (player.Life <= 0)
                        {
                            //gameover
                        }

                        return;
                    }
                    else if(obj1.Type == ObjType.Enemy && obj2.Type != ObjType.Enemy)
                    GameCanvas.Children.Remove(obj1.Image);
                    return;
                }
            }
        }

        public void ClearFromScreen(EntityObject entity)
        {
            //Verify if its player and resets position or hide and remove obj from the interaction area
            entity.Image.Visibility = Visibility.Hidden;
            if (entity.Type == ObjType.Player || entity.Type == ObjType.PlayerBullet)
            {
                bool check = entity.Type == ObjType.PlayerBullet ?
                    GoToHiddenPos(entity.Image, hiddenPos[1, 0], hiddenPos[1, 1]) :
                    GoToHiddenPos(entity.Image, hiddenPos[1, 0], hiddenPos[1, 1], false);
            }
            else if (entity.Type == ObjType.Enemy || entity.Type == ObjType.EnemyBullet)
            {
                GoToHiddenPos(entity.Image, hiddenPos[1, 0],hiddenPos[1, 1]);
            }
        }

        public bool GoToHiddenPos(Image img, double posX, double posY, bool hide = true)
        {
            if (hide)
                img.Visibility = Visibility.Hidden;
            Canvas.SetLeft(img, posX);
            Canvas.SetTop(img, posY);
            return hide;
        }

        public bool CheckOutOfBounds(Image imgObj)
        {
            if (Canvas.GetTop(imgObj) > 720 || Canvas.GetTop(imgObj) < 0 || Canvas.GetLeft(imgObj) > 1280 || Canvas.GetLeft(imgObj) < 0)
                return true;
            return false;
        }

        public void NewBullet()
        {
            playerBulletPool.Add(new EntityObject(1, new Image()
            {
                Height = PlayerBullet.Height,
                Width = PlayerBullet.Width,
                Source = PlayerBullet.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                Tag = "PlayerBullet"
            }, ObjType.PlayerBullet));
            SetBullet(playerBulletPool.LastOrDefault().Image);
        }

        public void GetExistingBullet()
        {
            shouldMakeNewBullet = true;
            for (int i = 0; i < playerBulletPool.Count; i++)
            {
                if (playerBulletPool[i].Image.Visibility == Visibility.Hidden)
                {
                    SetBullet(playerBulletPool[i].Image);
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

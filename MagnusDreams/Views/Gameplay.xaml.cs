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
using System.Windows.Shapes;

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
            //Test purpose only
            foreach (var img in GameCanvas.Children.OfType<Image>())
            {
                if (img.Tag != null && img.Tag.ToString() == "Enemy")
                {
                    allObjects.Add(new EntityObject(3, img, ObjType.Enemy));
                }
            }
            allObjects.Add(player);
            
            //Initializing timers
            elapsedMiliSeconds = 0;
            timeToShootPlayerBullets = 0;

            //Player Setup
            playerInitialLeftPosition = Canvas.GetLeft(PlayerImage);
            playerInitialTopPosition = Canvas.GetTop(PlayerImage);
            player = new EntityObject(3, PlayerImage, ObjType.Player);
            playerSpeed = 10;
            canMove = true;

            //Enemy Setup

            //Scene Cleanup
            PlayerBullet.Visibility = Visibility.Hidden;
            GameCanvas.Children.Remove(PlayerBullet);
            
            
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
                    Canvas.SetTop(player.Image, Canvas.GetTop(player.Image) + playerSpeed);
                }
                if (Keyboard.IsKeyDown(Key.Up) && Canvas.GetTop(player.Image) > 0)
                {
                    Canvas.SetTop(player.Image, Canvas.GetTop(player.Image) - playerSpeed);
                }
                if (Keyboard.IsKeyDown(Key.Left) && Canvas.GetLeft(player.Image) > 0)
                {
                    Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) - playerSpeed);
                }
                if (Keyboard.IsKeyDown(Key.Right) && Canvas.GetLeft(player.Image) < 1280 - player.Image.ActualWidth)
                {
                    Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) + playerSpeed);
                }
            }
        }

        private void FastUpdate()
        {
            CollisionUpdate(player);
            CollisionCheck();

            //Bullet pooling and collision
            if (playerBulletPool.Count > 0)
            {
                for (int i = 0; i < playerBulletPool.Count;i++)
                {
                    if (playerBulletPool[i].Image.Visibility == Visibility.Visible && !CheckOutOfBounds(playerBulletPool[i].Image))
                    {
                        Canvas.SetLeft(playerBulletPool[i].Image, Canvas.GetLeft(playerBulletPool[i].Image) + 15);
                        playerBulletPool[i].Rect = new Rect(
                            Canvas.GetLeft(playerBulletPool[i].Image),
                            Canvas.GetTop(playerBulletPool[i].Image),
                            playerBulletPool[i].Image.Width,
                            playerBulletPool[i].Image.Height);
                        //Log.Content = $"{bullet.Rect.Left} - {bullet.Rect.Top}";
                        CollisionUpdate(playerBulletPool[i]);
                    }
                    else //if(GameCanvas.Children.Contains(bullet.Image))
                    {
                        if (allObjects.Contains(playerBulletPool[i]))
                            allObjects.Remove(playerBulletPool[i]);
                        Canvas.SetLeft(playerBulletPool[i].Image, hiddenPos[0, 0]);
                        Canvas.SetTop(playerBulletPool[i].Image, hiddenPos[0, 1]);
                    }
                }
            }
        }

        private void Update()
        {

            if (!allObjects.Contains(player))
                allObjects.Add(player);

            /*if (allObjects.Count > 0)
            {
                foreach (var entity in allObjects)
                {
                    if (entity == null)
                        continue;
                    CollisionUpdate(entity);
                }
            }
            CollisionUpdate(player);
            CollisionCheck();
            /*if (playerBulletPool.Count > 0)
            {
                foreach (var entity in playerBulletPool)
                {
                    if (entity == null)
                        continue;
                    CollisionUpdate(entity);
                    //Log.Content = $"{entity.Rect.Left}";
                }
            }*/
        }

        public void CollisionUpdate(EntityObject obj)
        {
            obj.Rect =  new Rect(Canvas.GetLeft(obj.Image), Canvas.GetTop(obj.Image), obj.Image.ActualWidth, obj.Image.ActualHeight);
        }


        public void CollisionCheck()
        {
            for (int obj1index = 0;obj1index< allObjects.Count;obj1index++)
            {
                for (int obj2index = 0; obj2index < allObjects.Count; obj2index++)
                {
                    if (allObjects[obj1index] == null || allObjects[obj2index] == null || 
                        allObjects[obj1index].Type == allObjects[obj2index].Type)
                        continue;

                    if (allObjects[obj1index].Rect.IntersectsWith(allObjects[obj2index].Rect))
                    {
                        //PlayerCollided with enemy or enemy bullet
                        if (allObjects[obj1index].Type == ObjType.Player && (allObjects[obj2index].Type == ObjType.Enemy ||
                            allObjects[obj2index].Type == ObjType.EnemyBullet))
                        {
                            Log.Content = $"{allObjects[obj1index].Image.Name} with {allObjects[obj2index].Image.Name}";

                            //obj1.Life--;
                            if (allObjects[obj1index].Life <= 0)
                            {
                                //gameover here
                            }

                            //clear remaining bullets here

                            //Reset Player position
                            Canvas.SetLeft(allObjects[obj1index].Image, playerInitialLeftPosition);
                            Canvas.SetTop(allObjects[obj1index].Image, playerInitialTopPosition);
                            continue;
                        }
                        //playerBullet collided with Enemy
                        else if (allObjects[obj1index].Type == ObjType.PlayerBullet && allObjects[obj2index].Type == ObjType.Enemy)
                        {
                            Log.Content = $"{allObjects[obj1index].Image.Name} with {allObjects[obj2index].Image.Name}";

                            allObjects[obj2index].Life--;
                            
                            ClearFromScreen(allObjects[obj1index]);
                            if (allObjects[obj2index].Life <= 0)
                            {
                                ClearFromScreen(allObjects[obj2index]);
                            }
                        }
                    }
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
                if (!check)
                    return;
            }
            else if (entity.Type == ObjType.Enemy || entity.Type == ObjType.EnemyBullet)
            {
                GoToHiddenPos(entity.Image, hiddenPos[1, 0],hiddenPos[1, 1]);
            }
            GameCanvas.Children.Remove(entity.Image);
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

            allObjects.Add(playerBulletPool.LastOrDefault());
            SetBulletPosition(playerBulletPool.LastOrDefault());
        }

        public void GetExistingBullet()
        {
            shouldMakeNewBullet = true;
            for (int i = 0; i < playerBulletPool.Count; i++)
            {
                if (playerBulletPool[i].Image.Visibility == Visibility.Hidden)
                {
                    SetBulletPosition(playerBulletPool[i]);
                    shouldMakeNewBullet = false;
                    break;
                }
            }
            if (shouldMakeNewBullet)
                NewBullet();
        }

        public void SetBulletPosition(EntityObject bullet)
        {
            if (!GameCanvas.Children.Contains(bullet.Image))
                GameCanvas.Children.Add(bullet.Image);

            bullet.Image.Refresh();
            bullet.Image.Visibility = Visibility.Visible;
            Canvas.SetLeft(bullet.Image, Canvas.GetLeft(PlayerImage) + PlayerImage.ActualWidth);
            Canvas.SetTop(bullet.Image, Canvas.GetTop(PlayerImage) + PlayerImage.ActualHeight - (bullet.Image.ActualHeight * 2));
            bullet.Image.Refresh();
        }
    }
}

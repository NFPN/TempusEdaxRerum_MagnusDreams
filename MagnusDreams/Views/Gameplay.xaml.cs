using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using static MagnusDreams.Util.Audio;
using MagnusDreams.Util;
using MagnusDrems.DAO;


namespace MagnusDreams.Views
{
    public partial class Gameplay : UserControl
    {

        #region Global Variables
        //MainWindow reference audios list
        public MainWindow main = (MainWindow)Application.Current.MainWindow;
        ComandosSQL comandos = new ComandosSQL();

        //The hidden position of playerBullet objects, enemy and enemyBullet objects
        double[,] hiddenPos = new double[2, 2]
        {
            { -250, 0 },  //player bullets 
            { -250, 250 } // enemy hidden
        };

        //Time Info
        double elapsedMiliSeconds;
        TimeSpan deltaTime = new TimeSpan();
        DateTime startingTime = DateTime.Now;
        DispatcherTimer mainTime = new DispatcherTimer(),
            fastTimer = new DispatcherTimer(),
            secondsTimer = new DispatcherTimer();


        //Enemy Info
        //ImageSource enemySource;
        //double enemyBaseHeight, enemyBaseWidth;
        List<Bullet> enemyBulletPool = new List<Bullet>();
        double angle;

        //Player Info
        IObjController player;
        List<Bullet> playerBulletPool = new List<Bullet>();
        bool canMove;
        double playerInitialLeftPosition, playerInitialTopPosition, timeToShootPlayerBullets;
        long score;

        //List<BaseObject> allObjects = new List<BaseObject>();

        int frames;
        Random random = new Random();
        List<IObjController> allObjs = new List<IObjController>();
        public static ContentControl thiscontentControl = new ContentControl();

        #endregion



        public Gameplay()
        {
            InitializeComponent();
            //Test of btn Pause
            InitialStateGameplay();

            CompositionTarget.Rendering += (s, a) =>
            {
                ++frames;
            };

            DispatcherTimer fpsTimer = new DispatcherTimer();
            fpsTimer.Interval = TimeSpan.FromSeconds(0.1);
            fpsTimer.Tick += (s, a) =>
            {
                //Log.Content = string.Format("FPS:{0}", frames);

                frames = 0;
            };
            fpsTimer.Start();

            KeyboardController kbcontrol = new KeyboardController(MainWindow.appWindow);
            kbcontrol.timer.Interval = TimeSpan.FromMilliseconds(1);
            kbcontrol.KeyboardTick += InputUpdate;

            secondsTimer.Tick += SecondsTick;
            secondsTimer.Interval = TimeSpan.FromSeconds(1);

            fastTimer.Tick += GlobalTick;
            fastTimer.Interval = TimeSpan.FromMilliseconds(1);

            mainTime.Tick += SlowTimeTick;
            mainTime.Interval = TimeSpan.FromMilliseconds(5);

            Start();
            mainTime.Start();
            fastTimer.Start();
            secondsTimer.Start();
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

        private void SecondsTick(object sender, EventArgs e)
        {
            Log.Content = "Ué";
            EnemySpawner();
        }

        // Executes once when the game scene is loaded
        private void Start()
        {
            score = 0;

            //Test purpose only
            //foreach (var img in GameCanvas.Children.OfType<Image>())
            //{
            //    if (img.Tag != null && img.Tag.ToString() == "Enemy")
            //    {
            //        allObjs.Add(new Enemy(3, 3, img, ObjType.Enemy));
            //    }
            //}

            player = new Player(10, 3, PlayerImage, ObjType.Player);

            allObjs.Add(player);

            //Initializing timers
            elapsedMiliSeconds = 0;
            timeToShootPlayerBullets = 0;

            //Player Setup
            playerInitialLeftPosition = Canvas.GetLeft(PlayerImage);
            playerInitialTopPosition = Canvas.GetTop(PlayerImage);
            canMove = true;

            //Enemy Spawner Setup

            //Scene Cleanup
            PlayerBullet.Visibility = Visibility.Hidden;
            EnemyImage.Visibility = Visibility.Hidden;
            EnemyBullet.Visibility = Visibility.Hidden;

            GameCanvas.Children.Remove(EnemyImage);
            GameCanvas.Children.Remove(EnemyBullet);
            GameCanvas.Children.Remove(PlayerBullet);

        }

        private void InputUpdate(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
                Log.Content = "Pause";

            //Shooting Logic
            if (((KeyboardController)sender).KeyDown(Key.A) || ((KeyboardController)sender).KeyDown(Key.Space))
                if (timeToShootPlayerBullets >= 85)
                {
                    score++;
                    txtPlayerScore.Content = $"Score: {score.ToString()}";

                    if (playerBulletPool.Count > 0)
                        GetExistingBullet();
                    else
                        NewBullet();
                }

            //Movement Logic Down Up Left Right
            if (canMove)
            {
                if (Keyboard.IsKeyDown(Key.Down) && Canvas.GetTop(player.Image) < 720 - player.Image.ActualHeight)
                {
                    Canvas.SetTop(player.Image, Canvas.GetTop(player.Image) + player.Speed);
                }
                if (Keyboard.IsKeyDown(Key.Up) && Canvas.GetTop(player.Image) > 0)
                {
                    Canvas.SetTop(player.Image, Canvas.GetTop(player.Image) - player.Speed);
                }
                if (Keyboard.IsKeyDown(Key.Left) && Canvas.GetLeft(player.Image) > 0)
                {
                    Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) - player.Speed);
                }
                if (Keyboard.IsKeyDown(Key.Right) && Canvas.GetLeft(player.Image) < 1280 - player.Image.ActualWidth)
                {
                    Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) + player.Speed);
                }
            }
        }

        private void FastUpdate()
        {
            RectUpdate(player);
            CheckCollision();

            MoveObjects(ref allObjs);


            for (int i = 0; i < allObjs.Count; i++)
            {
                if (allObjs[i] == null)
                    continue;

                if (allObjs[i].Type == ObjType.Enemy || allObjs[i].Type == ObjType.EnemyBullet)
                {
                    if (allObjs[i].Image.Visibility == Visibility.Hidden)
                    {
                        Canvas.SetLeft(allObjs[i].Image, hiddenPos[1, 0]);
                        Canvas.SetTop(allObjs[i].Image, hiddenPos[1, 1]);
                    }
                    else
                    {
                        if (allObjs[i].Type == ObjType.Enemy)
                        {
                            var randNum = random.Next(0, 1001);

                            if (randNum < 10)
                                NewEnemyBullet(allObjs[i]);
                        }
                    }
                }
            }
        }
        private void Update()
        {
            angle += 0.1;
            angle %= 360;
            if (!allObjs.Contains(player))
                allObjs.Add(player);
        }
        public void RectUpdate(IObjController obj)
        {
            obj.Rect = new Rect(Canvas.GetLeft(obj.Image), Canvas.GetTop(obj.Image), obj.Image.ActualWidth, obj.Image.ActualHeight);
        }


        public void CheckCollision()
        {
            for (int obj1index = 0; obj1index < allObjs.Count; obj1index++)
            {
                for (int obj2index = 0; obj2index < allObjs.Count; obj2index++)
                {
                    if (allObjs[obj1index] == null || allObjs[obj2index] == null ||
                        allObjs[obj1index].Type == allObjs[obj2index].Type)
                        continue;

                    if (allObjs[obj1index].Rect.IntersectsWith(allObjs[obj2index].Rect))
                    {
                        //PlayerCollided with enemy or enemy bullet
                        if (allObjs[obj1index].Type == ObjType.Player && (allObjs[obj2index].Type == ObjType.Enemy ||
                            allObjs[obj2index].Type == ObjType.EnemyBullet))
                        {
                            sfxAudio();
                            Rosto.IsEnabled = false;

                            //Rosto.Resources = new Uri("smiley_stackpanel.PNG", UriKind.Relative);

                            //obj1.Life--;
                            if (allObjs[obj1index].Life <= 0)
                            {
                                //comandos.InsertData();
                                contentControlGamePlay.Content = new GameOverView();
                            }

                            //clear remaining bullets here
                            for (int i = 0; i < allObjs.Count; i++)
                            {
                                if (allObjs[i].Type != ObjType.Player)
                                    ClearFromScreen(allObjs[i]);
                            }
                            //Reset Player position
                            Canvas.SetLeft(allObjs[obj1index].Image, playerInitialLeftPosition);
                            Canvas.SetTop(allObjs[obj1index].Image, playerInitialTopPosition);
                            continue;
                        }
                        //playerBullet collided with Enemy
                        else if (allObjs[obj1index].Type == ObjType.PlayerBullet && allObjs[obj2index].Type == ObjType.Enemy)
                        {
                            score += 10;
                            score = (long)(score * 1.1);
                            txtPlayerScore.Content = $"Score: {score.ToString()}";

                            allObjs[obj2index].Life--;

                            ClearFromScreen(allObjs[obj1index]);
                            if (allObjs[obj2index].Life <= 0)
                            {
                                ClearFromScreen(allObjs[obj2index]);
                            }
                        }
                    }
                }
            }
        }
        public bool CheckOutOfBounds(Image imgObj)
        {
            if (Canvas.GetTop(imgObj) > 770 || Canvas.GetTop(imgObj) < -50 || Canvas.GetLeft(imgObj) > 1330 || Canvas.GetLeft(imgObj) < -50)
                return true;
            return false;
        }

        // Remove obj from the interaction area
        public void ClearFromScreen(IObjController entity)
        {
            bool check;

            if (entity.Type == ObjType.Player || entity.Type == ObjType.PlayerBullet)
            {
                check = entity.Type == ObjType.PlayerBullet ?
                    GoToHiddenPos(entity.Image, hiddenPos[1, 0], hiddenPos[1, 1]) :
                    GoToHiddenPos(entity.Image, hiddenPos[1, 0], hiddenPos[1, 1]);
                if (!check)
                    return;
            }
            else if (entity.Type == ObjType.Enemy || entity.Type == ObjType.EnemyBullet)
            {
                check = GoToHiddenPos(entity.Image, hiddenPos[1, 0], hiddenPos[1, 1]);
            }
            if (GameCanvas.Children.Contains(entity.Image))
                GameCanvas.Children.Remove(entity.Image);
        }
        public bool GoToHiddenPos(Image img, double posX, double posY)
        {
            bool hide = true;
            Canvas.SetLeft(img, posX);
            Canvas.SetTop(img, posY);
            return hide;
        }

        /// <summary>
        /// Move objects on the list or Hide them
        /// </summary>
        public void MoveObjects(ref List<IObjController> list)
        {
            if (list.Count > 0)
            {

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].GetType().ToString().Contains("Player") || list[i] == null)
                        continue;

                    if (!CheckOutOfBounds(list[i].Image))
                    {
                        if (list[i].Type == ObjType.Enemy)
                        {
                            Log.Content = "Tentando se mover";
                            ((Enemy)list[i]).WaveMovement(angle);
                        }
                        else if (list[i].Type == ObjType.EnemyBullet)
                            Canvas.SetLeft(list[i].Image, Canvas.GetLeft(list[i].Image) - list[i].Speed);
                        else
                            Canvas.SetLeft(list[i].Image, Canvas.GetLeft(list[i].Image) + list[i].Speed);

                        list[i].Rect = new Rect(Canvas.GetLeft(list[i].Image), Canvas.GetTop(list[i].Image),
                            list[i].Image.Width, list[i].Image.Height);
                        RectUpdate(list[i]);
                    }
                    else
                    {
                        if (list[i].Type == ObjType.Enemy || list[i].Type == ObjType.EnemyBullet)
                        {
                            Canvas.SetLeft(list[i].Image, hiddenPos[1, 0]);
                            Canvas.SetTop(list[i].Image, hiddenPos[1, 1]);
                        }
                        else if (list[i].Type == ObjType.PlayerBullet)
                        {
                            Canvas.SetLeft(list[i].Image, hiddenPos[0, 0]);
                            Canvas.SetTop(list[i].Image, hiddenPos[0, 1]);
                        }

                        if (allObjs.Contains(list[i]))
                            allObjs.Remove(list[i]);
                    }
                }
            }
        }

        public void EnemySpawner()
        {
            for (int i = 0; i < allObjs.Count; i++)
            {
                if (allObjs[i].Type == ObjType.Enemy && CheckOutOfBounds(allObjs[i].Image))
                {
                    Canvas.SetLeft(allObjs[i].Image, 1280);
                    Canvas.SetTop(allObjs[i].Image, random.Next(0, 720));
                    return;
                }
            }

            allObjs.Add(new Enemy(random.Next(5, 16), 3, new Image()
            {
                Height = EnemyImage.Height,
                Width = EnemyImage.Width,
                Source = EnemyImage.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Visible,
                Tag = EnemyImage.Tag,
                Margin = new Thickness(1200, random.Next((int)EnemyImage.ActualHeight, 700), 0, 0)

            }, ObjType.Enemy));
            GameCanvas.Children.Add(allObjs.LastOrDefault().Image);
            allObjs.LastOrDefault().Image.Refresh();
        }

        public void NewEnemyBullet(IObjController enemyWhoShot)
        {
            enemyBulletPool.Add(new Bullet(15, 1, new Image()
            {
                Height = EnemyBullet.Height,
                Width = EnemyBullet.Width,
                Source = EnemyBullet.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                Tag = EnemyBullet.Tag
            }, ObjType.EnemyBullet));

            allObjs.Add(enemyBulletPool.LastOrDefault());
            SetBulletPosition(enemyBulletPool.LastOrDefault(), enemyWhoShot);
        }

        public void GetExistingEnemyBullet(IObjController enemy)
        {
            for (int i = 0; i < playerBulletPool.Count; i++)
            {
                if (playerBulletPool[i].Image.Visibility == Visibility.Hidden)
                {
                    SetBulletPosition(playerBulletPool[i]);
                    break;
                }
            }
            NewEnemyBullet(enemy);
        }

        #region Player Bullet Methods
        public void NewBullet()
        {
            sfxAudio();
            playerBulletPool.Add(new Bullet(15, 1, new Image()
            {
                Height = PlayerBullet.Height,
                Width = PlayerBullet.Width,
                Source = PlayerBullet.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                Tag = "PlayerBullet"
            }, ObjType.PlayerBullet));

            allObjs.Add(playerBulletPool.LastOrDefault());
            SetBulletPosition(playerBulletPool.LastOrDefault());
        }

        public void GetExistingBullet()
        {
            for (int i = 0; i < playerBulletPool.Count; i++)
            {
                if (CheckOutOfBounds(playerBulletPool[i].Image))
                {
                    SetBulletPosition(playerBulletPool[i]);
                    return;
                }
            }
            NewBullet();
        }
        #endregion

        public void SetBulletPosition(IObjController bullet, IObjController whoShot = null)
        {
            if (!GameCanvas.Children.Contains(bullet.Image))
                GameCanvas.Children.Add(bullet.Image);

            bullet.Image.Refresh();
            bullet.Image.Visibility = Visibility.Visible;

            if (bullet.Type == ObjType.PlayerBullet)
            {
                Canvas.SetLeft(bullet.Image, Canvas.GetLeft(PlayerImage) + PlayerImage.ActualWidth);
                Canvas.SetTop(bullet.Image, Canvas.GetTop(PlayerImage) + PlayerImage.ActualHeight - (bullet.Image.ActualHeight * 2));
            }
            else if (bullet.Type == ObjType.EnemyBullet && whoShot != null)
            {
                Canvas.SetLeft(bullet.Image, Canvas.GetLeft(whoShot.Image) + whoShot.Image.ActualWidth);
                Canvas.SetTop(bullet.Image, Canvas.GetTop(whoShot.Image) + whoShot.Image.ActualHeight - (bullet.Image.ActualHeight));
            }
            bullet.Image.Refresh();
        }

        private void InitialStateGameplay()
        {
            bgPauseGame.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
            
            main.ChangeVisibility(new Control[] { txtmusicVolume, txtSfxVolume, btnReturnToGame, musicIsChecked, sfxIsChecked, }, false);
        }



        private void OpenGamePause(object sender, RoutedEventArgs e)
        {
            thiscontentControl.Content = contentControlGamePlay.Content;
            //contentControlGamePlay.Content = new InGamePauseView();

            main.ChangeVisibility(new Control[] { btnPause }, false);
            bgPauseGame.Visibility = Visibility.Visible;
           
            main.ChangeVisibility(new Control[] { txtmusicVolume, txtSfxVolume, btnReturnToGame, musicIsChecked, sfxIsChecked, }, true);

            sfxAudio();

        }



        private void CloseGamePause(object sender, RoutedEventArgs e)
        {
            sfxAudio();
            InitialStateGameplay();
        }

        public void ClearGrid()
        {
            GameGrid.Children.Clear();
        }

        #region Audio Methods
        private void musicOn(object sender, RoutedEventArgs e)
        {
            DesmuteBgMusic();
        }

        private void musicOff(object sender, RoutedEventArgs e)
        {
            MuteBgMusic();
        }
        private void sfxOff(object sender, RoutedEventArgs e)
        {
            MuteSfx();
        }

        private void sfxOn(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}

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

//using static MagnusDreams.Util.Audio;
using MagnusDreams.Util;
using MagnusDrems.DAO;


namespace MagnusDreams.Views
{
    public partial class Gameplay : UserControl
    {

        #region Global Variables

        //MainWindow reference audios list
        public MainWindow main = (MainWindow)Application.Current.MainWindow;
        public Audio audio = new Audio();

        //Database command
        ComandosSQL comandos = new ComandosSQL();

        //The position PlayerBullet, Enemy and EnemyBullet objects will be hidden at
        readonly double[,] hiddenPos = new double[2, 2]
        {
            { -500, 0 },  // Player
            { -500, 500 } // Enemy
        };

        //Time Info
        double elapsedMiliSeconds;
        TimeSpan deltaTime = new TimeSpan();
        DateTime startingTime = DateTime.Now;
        DispatcherTimer mainTime = new DispatcherTimer(), fastTimer = new DispatcherTimer(), secondsTimer = new DispatcherTimer();

        //Enemy Info
        int currentEnemyBullets = 0;
        double angle;

        //Player Info
        IObjController player;
        int currentBullets = 0;
        bool canMove;
        double playerInitialLeftPosition, playerInitialTopPosition, timeToShootPlayerBullets;
        long score;

        //Utilities
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
            secondsTimer.Interval = TimeSpan.FromSeconds(1.5);

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
        private void SecondsTick(object sender, EventArgs e)
        {
            EnemySpawner();
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


        // Executes Before all updates
        private void Start()
        {
            score = 0;

            //Test purpose only
            foreach (var img in GameCanvas.Children.OfType<Image>())
            {
                if (img.Tag != null && img.Tag.ToString() == "Enemy")
                {
                    allObjs.Add(new Enemy(3, 3, img, ObjType.Enemy));
                }
            }

            player = new Player(10, 3, PlayerImage, ObjType.Player);
            lbLifePlayer.Content = $"Vidas: {player.Life}";

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
            //EnemyImage.Visibility = Visibility.Hidden;
            //EnemyBullet.Visibility = Visibility.Hidden;

            //GameCanvas.Children.Remove(EnemyImage);
            //GameCanvas.Children.Remove(EnemyBullet);
            GameCanvas.Children.Remove(PlayerBullet);

            if (!allObjs.Contains(player))
                allObjs.Add(player);
        }

        #region Update methods

        private void InputUpdate(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
                //Log.Content = "Pause";

            //Shooting Logic
            if (((KeyboardController)sender).KeyDown(Key.A) || ((KeyboardController)sender).KeyDown(Key.Space))
                if (timeToShootPlayerBullets >= 85)
                {
                    score++;
                    txtPlayerScore.Content = $"Score: {score.ToString()}";

                    if (currentBullets > 0)
                        GetExistingBullet();
                    else
                        NewBullet();
                }

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
            MoveObjects();


            for (int i = 0; i < allObjs.Count; i++)
            {
                if (CheckNull(allObjs[i]))
                    continue;

                if (allObjs[i].Type == ObjType.Enemy || allObjs[i].Type == ObjType.EnemyBullet)
                {
                    if (allObjs[i].Image.Visibility == Visibility.Hidden)
                    {
                        Canvas.SetLeft(allObjs[i].Image, hiddenPos[1, 0]);
                        Canvas.SetTop(allObjs[i].Image, hiddenPos[1, 1]);
                    }
                }
            }
        }
        private void Update()
        {
            for (int i = 0; i < allObjs.Count; i++)
            {
                if (allObjs[i] == null)
                    continue;
                //Enemy Shot
                if (allObjs[i].Type == ObjType.Enemy)
                {
                    if (random.Next(0, 1001) < 10)
                        GetExistingEnemyBullet(allObjs[i]);
                }
            }
        }
        public void RectUpdate(IObjController obj)
        {
            obj.Rect = new Rect(Canvas.GetLeft(obj.Image), Canvas.GetTop(obj.Image), obj.Image.ActualWidth, obj.Image.ActualHeight);
        }

        public void MoveObjects()
        {
            if (allObjs.Count > 0)
            {
                for (int i = 0; i < allObjs.Count; i++)
                {
                    if (allObjs[i].GetType().ToString().Contains("Player") || CheckNull(allObjs[i]))
                        continue;

                    RectUpdate(allObjs[i]);

                    if (!CheckOutOfBounds(allObjs[i].Image))
                    {
                        if (allObjs[i].Type == ObjType.PlayerBullet)
                        {
                            Canvas.SetLeft(allObjs[i].Image, Canvas.GetLeft(allObjs[i].Image) + allObjs[i].Speed);
                        }
                        else
                        {
                            //allObjs[i].Image.Margin = new Thickness(allObjs[i].Image.Margin.Left - allObjs[i].Speed,allObjs[i].Image.Margin.Top,0,0);
                            if (allObjs[i].Type == ObjType.Enemy)
                                (allObjs[i] as Enemy).WaveMovement();
                            //Log.Content = "moving everything else?";
                            Canvas.SetLeft(allObjs[i].Image, Canvas.GetLeft(allObjs[i].Image) - allObjs[i].Speed);
                        }

                        allObjs[i].Rect = new Rect(Canvas.GetLeft(allObjs[i].Image), Canvas.GetTop(allObjs[i].Image), allObjs[i].Image.Width, allObjs[i].Image.Height);
                        
                    }
                    else
                    {
                        if (allObjs[i].Type == ObjType.Enemy || allObjs[i].Type == ObjType.EnemyBullet)
                        {
                            Canvas.SetLeft(allObjs[i].Image, hiddenPos[1, 0]);
                            Canvas.SetTop(allObjs[i].Image, hiddenPos[1, 1]);
                        }
                        else if (allObjs[i].Type == ObjType.PlayerBullet)
                        {
                            Canvas.SetLeft(allObjs[i].Image, hiddenPos[0, 0]);
                            Canvas.SetTop(allObjs[i].Image, hiddenPos[0, 1]);
                        }

                        //if (allObjs.Contains(allObjs[i]))
                          //  allObjs.Remove(allObjs[i]);
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

            allObjs.Add(new Enemy(random.Next(2, 5), 3, new Image()
            {
                Name = EnemyImage.Name,
                Height = EnemyImage.Height,
                Width = EnemyImage.Width,
                Source = EnemyImage.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Visible,
                Tag = EnemyImage.Tag
                
                
                //Margin = new Thickness(1200, random.Next((int)EnemyImage.ActualHeight, 700), 0, 0),
                

            }, ObjType.Enemy));

            if (!GameCanvas.Children.Contains(allObjs.LastOrDefault().Image))
                GameCanvas.Children.Add(allObjs.LastOrDefault().Image);

            allObjs.LastOrDefault().Image.Visibility = Visibility.Visible;

            RectUpdate(allObjs.LastOrDefault());
            Canvas.SetLeft(allObjs.LastOrDefault().Image, 1200);
            Canvas.SetTop(allObjs.LastOrDefault().Image, random.Next((int)EnemyImage.ActualHeight, 700));
            RectUpdate(allObjs.LastOrDefault());

            allObjs.LastOrDefault().Image.Refresh();
        }

        #endregion

        #region UI methods

        public void CheckCollision()
        {
            for (int obj1index = 0; obj1index < allObjs.Count; obj1index++)
            {
                for (int obj2index = 0; obj2index < allObjs.Count; obj2index++)
                {
                    if (CheckNull(allObjs[obj1index]) || CheckNull(allObjs[obj2index]) ||
                        allObjs[obj1index].Type == allObjs[obj2index].Type)
                        continue;

                    if (allObjs[obj1index].Rect.IntersectsWith(allObjs[obj2index].Rect))
                    {
                        //PlayerCollided with enemy or enemy bullet
                        if (allObjs[obj1index].Type == ObjType.Player && (allObjs[obj2index].Type == ObjType.Enemy ||
                            allObjs[obj2index].Type == ObjType.EnemyBullet))
                        {
                            //sfxAudio();
                            Audio.InitializeAudios[1].Play();

                            Rosto.IsEnabled = false;

                            //Rosto.Resources = new Uri("smiley_stackpanel.PNG", UriKind.Relative);

                            allObjs[obj1index].Life--;
                            //lbLifePlayer.Content = $"Vidas : {allObjs[obj1index].Life--.ToString()}";
                            lbLifePlayer.Content = $"Vidas: {player.Life}";
                            if (allObjs[obj1index].Life < 1)
                            {

                                //comandos.InsertData();
                                GameCanvas.Children.Clear();
                                main.ChangeVisibility(new Control[] { lbLifePlayer },false);
                                contentControlGamePlay.Content = new GameOverView();
                                //ClearGrid();
                                //ClearFromScreen(allObjs[obj1index]);
                            }

                            //clear screen
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

        static public bool CheckNull(object obj)
        {
            return obj == null ? true : false;
        }
        public bool CheckOutOfBounds(Image imgObj)
        {
            if (Canvas.GetTop(imgObj) > 770 || Canvas.GetTop(imgObj) < -50 || Canvas.GetLeft(imgObj) > 1330 || Canvas.GetLeft(imgObj) < -50)
                return true;
            return false;
        }

        // Remove obj from the interaction area
        public bool ClearFromScreen(IObjController entity)
        {
            if (entity.Type == ObjType.Player || entity.Type == ObjType.PlayerBullet)
            {
                return entity.Type == ObjType.PlayerBullet ?
                    GoToHiddenPos(entity.Image, hiddenPos[0, 0], hiddenPos[0, 1]) :
                    GoToHiddenPos(entity.Image, hiddenPos[1, 0], hiddenPos[1, 1]);
            }
            else if (entity.Type == ObjType.Enemy || entity.Type == ObjType.EnemyBullet)
            {
                return GoToHiddenPos(entity.Image, hiddenPos[1, 0], hiddenPos[1, 1]);
            }
            return false;
        }

        public bool GoToHiddenPos(Image img, double posX, double posY)
        {
            bool hide = true;
            Canvas.SetLeft(img, posX);
            Canvas.SetTop(img, posY);
            return hide;
        }

        #endregion

        #region Bullet Methods

        public void NewEnemyBullet(IObjController enemyWhoShot)
        {
            allObjs.Add(new Bullet(15, 1, new Image()
            {
                Height = EnemyBullet.Height,
                Width = EnemyBullet.Width,
                Source = EnemyBullet.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                Tag = EnemyBullet.Tag
            }, ObjType.EnemyBullet));
            SetBulletPosition(allObjs.LastOrDefault(), enemyWhoShot);
            currentEnemyBullets++;
        }

        public void GetExistingEnemyBullet(IObjController enemy)
        {
            for (int i = 0; i < allObjs.Count; i++)
            {
                if (allObjs[i].Type == ObjType.EnemyBullet)
                {
                    SetBulletPosition(allObjs[i]);
                    break;
                }
            }
            NewEnemyBullet(enemy);
        }



        public void NewBullet()
        {
            allObjs.Add(new Bullet(15, 1, new Image()
            {
                Height = PlayerBullet.Height,
                Width = PlayerBullet.Width,
                Source = PlayerBullet.Source,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden,
                Tag = "PlayerBullet"
            }, ObjType.PlayerBullet));

            SetBulletPosition(allObjs.LastOrDefault());
            currentBullets++;
        }

        public void GetExistingBullet()
        {
            for (int i = 0; i < allObjs.Count; i++)
            {
                if (allObjs[i].Type == ObjType.PlayerBullet && CheckOutOfBounds(allObjs[i].Image))
                {
                    SetBulletPosition(allObjs[i]);
                    return;
                }
            }
            NewBullet();
        }


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
            //sfxAudio();
            Audio.InitializeAudios[1].Play();
        }

        private void InitialStateGameplay()
        {
            //lbLifePlayer.Content = $"Vidas :";
            bgPauseGame.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
            //PauseInPause.Visibility = Visibility.Hidden;
            main.ChangeVisibility(new Control[] { txtmusicVolume, txtSfxVolume, btnReturnToGame, /*musicIsChecked, sfxIsChecked,*/ }, false);
        }



         private void OpenGamePause(object sender, RoutedEventArgs e)
         {
             thiscontentControl.Content = contentControlGamePlay.Content;
             //contentControlGamePlay.Content = new InGamePauseView();

            main.ChangeVisibility(new Control[] { btnPause }, false);
            bgPauseGame.Visibility = Visibility.Visible;
            //PauseInPause.Visibility = Visibility.Visible;
            main.ChangeVisibility(new Control[] { txtmusicVolume, txtSfxVolume, btnReturnToGame, /*musicIsChecked, sfxIsChecked,*/ }, true);

            //sfxAudio();
            Audio.InitializeAudios[1].Play();
        }



         private void CloseGamePause(object sender, RoutedEventArgs e)
         {
            //sfxAudio();
            Audio.InitializeAudios[1].Play();
            InitialStateGameplay();
         }

        public void ClearGrid()
        {
            GameGrid.Children.Clear();
        }

        #endregion

        /*audio  
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
           */
    }
}

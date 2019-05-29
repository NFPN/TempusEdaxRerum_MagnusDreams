using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;

namespace MagnusDreams.Util
{
    public interface IObjController
    {
         int Life { get; set; }
         int Speed { get; set; }
         Rect Rect { get; set; }
         Image Image { get; set; }
         ObjType Type { get; set; }
    }

    class Enemy : BaseObject, IObjController
    {
        int initialTop, waveLimit;
        bool isGoingUp;
        public Enemy() { }
        public Enemy(int speed, int life, Image image, ObjType type)
        {
            Type = type;
            Life = life;
            Speed = speed;
            Image = image;
            SetRect();

            isGoingUp = true;
            waveLimit = 1;
            initialTop = (int)Canvas.GetTop(Image);
        }


        public void WaveMovement(double angle)
        {
            Canvas.SetLeft(Image, Canvas.GetLeft(Image) - Speed);

            if (isGoingUp)
            {
                Canvas.SetTop(Image, Canvas.GetTop(Image) + Math.Sin(angle / Math.PI));
                //Canvas.SetTop(Image, Canvas.GetTop(Image) + (Speed * 0.5));
                if (Canvas.GetTop(Image) >= waveLimit + initialTop)
                    isGoingUp = false;
            }
            else if (!isGoingUp)
            {
                Canvas.SetTop(Image, Canvas.GetTop(Image) - Math.Sin(angle / Math.PI));
                //Canvas.SetTop(Image, Canvas.GetTop(Image) - (Speed * 0.5));
                if (Canvas.GetTop(Image) <= waveLimit - initialTop)
                    isGoingUp = true;
            }
        }
    }
    
    class Boss : Enemy
    {
        //constructor and extra atk patterns
    }

    class Bullet : BaseObject, IObjController
    {
        //public int Speed { get; set; }
        public Bullet(int speed, int life, Image image, ObjType type)
        {
            this.Type = type;
            this.Life = life;
            this.Speed = speed;
            this.Image = image;
            this.SetRect();
        }
    }

    class Player : BaseObject, IObjController
    {
        public Player(int speed, int life, Image image, ObjType type)
        {
            this.Type = type;
            this.Life = life;
            this.Speed = speed;
            this.Image = image;
            this.SetRect();
        }
    }
}

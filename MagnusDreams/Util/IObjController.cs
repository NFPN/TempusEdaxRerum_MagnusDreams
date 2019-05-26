using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

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
        
        public Enemy() { }
        public Enemy(int speed, int life, Image image, ObjType type)
        {
            this.Type = type;
            this.Life = life;
            this.Speed = speed; 
            this.Image = image;
            this.SetRect();
        }
        
        public void WaveMovement()
        {
            Canvas.SetLeft(this.Image, Canvas.GetLeft(this.Image) - this.Speed);
        }
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
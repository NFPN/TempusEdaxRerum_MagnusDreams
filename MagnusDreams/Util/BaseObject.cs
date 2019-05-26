using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

namespace MagnusDreams.Util
{
    public class BaseObject
    {
        /*public BaseObject() { }
        public BaseObject(int life, Image image, ObjType type)
        {
            Type = type;
            Life = life;
            Image = image;
            Rect = new Rect
            (
                Canvas.GetLeft(image),
                Canvas.GetTop(image),
                image.Width ,
                image.Height
            );
        }*/

        public void SetRect()
        {
            Rect = new Rect
            (
                Canvas.GetLeft(this.Image),
                Canvas.GetTop(this.Image),
                this.Image.Width,
                this.Image.Height
            );
        }

        public int Life { get; set; }
        public int Speed { get; set; }
        public Rect Rect { get; set; }
        public Image Image { get; set; }
        public ObjType Type { get; set; }
    }
}

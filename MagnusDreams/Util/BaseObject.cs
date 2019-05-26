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
        public BaseObject() { }
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
        }

        public BaseObject(int life, Image image, ObjType type, double customRectHeight, double customRectWidth)
        {
            Type = type;
            Life = life;
            Image = image;
            Rect = new Rect
            (
                Canvas.GetLeft(image),
                Canvas.GetTop(image),
                image.Width,
                image.Height
            );
        }

        public int Life { get; set; }
        public Image Image { get; }
        public Rect Rect { get; set; }
        public ObjType Type { get; }
    }
}

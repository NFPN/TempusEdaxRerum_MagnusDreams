using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

namespace MagnusDreams.Util
{
    public class EntityObject
    {
        public EntityObject() { }
        public EntityObject(int life, Image entityImage, ObjType type)
        {
            Type = type;
            Life = life;
            Image = entityImage;
            Rect = new Rect
            (
                Canvas.GetLeft(entityImage),
                Canvas.GetTop(entityImage),
                entityImage.ActualWidth,
                entityImage.ActualHeight
            );
        }

        public int Life { get; set; }
        public Image Image { get; }

        public Rect Rect { get; set; }

        public ObjType Type { get; }
    }
}

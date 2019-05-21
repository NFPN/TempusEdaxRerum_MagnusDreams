using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;

namespace MagnusDreams.Util
{
    class EntityObject
    {
        public EntityObject() { }
        public EntityObject(int life, Image entityObjectGraphic)
        {
            Life = life;
            Image = entityObjectGraphic;
        }

        public int Life { get; set; }
        public Image Image { get; }
    }
}

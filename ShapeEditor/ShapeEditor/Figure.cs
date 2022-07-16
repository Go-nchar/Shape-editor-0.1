using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public abstract class Figure
    {
        protected Random Random = new Random();
        protected List<Button> Buttons = new List<Button>();
        protected Button CenterButton;
        protected PictureBox PictureBox;

        public abstract void Create(PictureBox pictureBox);

        public abstract bool Update(Point diff, Button button);

        public abstract void Draw();
    }
}

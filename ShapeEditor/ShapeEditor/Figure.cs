using System;
using System.Collections.Generic;
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

        public abstract void Create(PictureBox pictureBox);

        public abstract void Draw();
    }
}

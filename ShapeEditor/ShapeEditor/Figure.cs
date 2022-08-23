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
        public List<BaseButton> Buttons = new List<BaseButton>();
        public BaseButton CenterButton;
        public TextBox textBox;
        protected PictureBox PictureBox;

        public int Indexe;

        public int number;

        public event Action Updated;

        public abstract void Create(PictureBox pictureBox);

        public abstract bool Update(Point diff, BaseButton button);

        public abstract void Draw();

        public abstract List<string> GetData();

        public abstract void SetData(Point centerPoint, List<Point> points, PictureBox pictureBox);

        public abstract bool IsValidate(Point center, List<Point> points);

        protected void OnUpdate()
        {
            Updated?.Invoke();
        }

    }
}

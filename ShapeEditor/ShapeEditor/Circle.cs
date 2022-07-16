using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public class Circle : Figure
    {
        private Rectangle _rect;

        public override void Create(PictureBox pictureBox)
        {
            var leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
            var radius = Random.Next(0, Math.Min(pictureBox.Size.Width - leftUpPoint.X, pictureBox.Size.Height - leftUpPoint.Y));
            _rect = new Rectangle(leftUpPoint.X + 4, leftUpPoint.Y + 4, radius, radius);

            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = new Point(leftUpPoint.X + radius / 2, leftUpPoint.Y + radius / 2);

            var b = new Button();
            b.Size = new Size(8, 8);
            b.Location = new Point(leftUpPoint.X + radius / 2, leftUpPoint.Y);
            Buttons.Add(b);

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
        }

        public override void Draw()
        {
            GraphicsManager.Graphics.DrawEllipse(GraphicsManager.Pen, _rect);
        }
    }
}

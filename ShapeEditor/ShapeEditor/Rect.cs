using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public class Rect : Figure
    {
        private Rectangle _rect;

        public override void Create(PictureBox pictureBox)
        {
            var leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
            var width = Random.Next(0, pictureBox.Size.Width - leftUpPoint.X);
            var height = Random.Next(0, pictureBox.Size.Height - leftUpPoint.Y);
            _rect = new Rectangle(leftUpPoint.X + 4, leftUpPoint.Y + 4, width, height);

            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = new Point(leftUpPoint.X + width / 2, leftUpPoint.Y + height / 2);

            var points = new List<Point>()
            {
                leftUpPoint,
                new Point(leftUpPoint.X + width, leftUpPoint.Y),
                new Point(leftUpPoint.X + width, leftUpPoint.Y + height),
                new Point(leftUpPoint.X, leftUpPoint.Y + height)
            };

            foreach (var p in points)
            {
                var b = new Button();
                b.Size = new Size(8, 8);
                b.Location = p;
                Buttons.Add(b);
            }

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
        }

        public override void Draw()
        {
            GraphicsManager.Graphics.DrawRectangle(GraphicsManager.Pen, _rect);
        }
    }
}

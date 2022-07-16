using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public class Pentagon : Figure
    {
        public override void Create(PictureBox pictureBox)
        {
            var centerPoint = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = centerPoint;

            var radiuses = new List<int>()
            {
                centerPoint.X,
                centerPoint.Y,
                pictureBox.Size.Width - centerPoint.X,
                pictureBox.Size.Height - centerPoint.Y
            };

            var radius = Random.Next(0, radiuses.Min());

            var angle = 2.0 * Math.PI / 5.0;
            IEnumerable<PointF> points = Enumerable.Range(0, 5).Select(i => PointF.Add((PointF)centerPoint,
                new SizeF(
                (float)(Math.Sin(i * angle) * radius),
                (float)(Math.Cos(i * angle) * radius))));

            foreach (var p in points)
            {
                var b = new Button();
                b.Size = new Size(8, 8);
                b.Location = new Point((int) p.X, (int) p.Y);
                Buttons.Add(b);
            }

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
        }

        public override void Draw()
        {
            var points = new List<Point>();
            foreach (var b in Buttons)
            {
                points.Add(new Point(b.Location.X + 4, b.Location.Y + 4));
            }
            GraphicsManager.Graphics.DrawPolygon(GraphicsManager.Pen, points.ToArray());
        }
    }
}

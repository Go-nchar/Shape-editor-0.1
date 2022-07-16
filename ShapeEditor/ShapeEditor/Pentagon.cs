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
            PictureBox = pictureBox;

            var centerPoint = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = centerPoint;
            CenterButton.HandleMove(pictureBox);

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
                b.HandleMove(pictureBox);
                Buttons.Add(b);
            }

            ButtonManager.Dragged += Update;

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

        public override bool Update(Point diff, Button button)
        {
            if (button == CenterButton)
            {
                foreach (var b in Buttons)
                {
                    int x = b.Location.X + diff.X;
                    int y = b.Location.Y + diff.Y;

                    if (x < 0 || y < 0 || x > PictureBox.Width || y > PictureBox.Height)
                    {
                        return false;
                    }
                }

                foreach (var b in Buttons)
                {
                    int x = b.Location.X + diff.X;
                    int y = b.Location.Y + diff.Y;

                    b.Location = new Point(x, y);
                }

                Program.MainForm.DrawFigures();
            }
            else if (Buttons.Contains(button))
            {
                var x = Math.Abs(CenterButton.Location.X - button.Location.X);
                var y = Math.Abs(CenterButton.Location.Y - button.Location.Y);
                var radius = Math.Sqrt(x * x + y * y);

                var angle = 2.0 * Math.PI / 5.0;
                var points = Enumerable.Range(0, 5).Select(i => PointF.Add((PointF)CenterButton.Location,
                    new SizeF(
                    (float)(Math.Sin(i * angle) * radius),
                    (float)(Math.Cos(i * angle) * radius)))).ToList();

                for (var i = 0; i < points.Count; i++)
                {

                    if (points[i].X < 0 || points[i].Y < 0 || points[i].X > PictureBox.Width || points[i].Y > PictureBox.Height)
                    {
                        return false;
                    }
                }

                for (var i = 0; i < points.Count; i++)
                {
                    Buttons[i].Location = new Point((int)points[i].X, (int)points[i].Y);
                }

                Program.MainForm.DrawFigures();
            }

            return true;
        }
    }
}

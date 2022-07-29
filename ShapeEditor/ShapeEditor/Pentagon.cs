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

            var radiuses = new List<int>()
            {
                centerPoint.X,
                centerPoint.Y,
                pictureBox.Size.Width - centerPoint.X,
                pictureBox.Size.Height - centerPoint.Y
            };

            var radius = Random.Next(0, radiuses.Min());

            var angle = 2.0 * Math.PI / 5.0;
            IEnumerable<Point> points = Enumerable.Range(0, 5).Select(i => Point.Add((Point)centerPoint,
                new Size(
                (int)(Math.Sin(i * angle) * radius),
                (int)(Math.Cos(i * angle) * radius))));

            SetData(centerPoint, points.ToList(), pictureBox);
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

        public override List<string> GetData()
        {
            var strs = new List<string>();
            strs.Add("Pentagon");
            strs.Add(CenterButton.Location.X + " " + CenterButton.Location.Y);
            foreach (var b in Buttons)
            {
                strs.Add(b.Location.X + " " + b.Location.Y);
            }

            return strs;
        }

        public override void SetData(Point centerPoint, List<Point> points, PictureBox pictureBox)
        {
            PictureBox = pictureBox;

            CenterButton = new BaseButton();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = centerPoint;
            CenterButton.SetPictBox(pictureBox);
            CenterButton.Dragged += Update;

            foreach (var p in points)
            {
                var b = new BaseButton();
                b.Size = new Size(8, 8);
                b.Location = new Point(p.X, p.Y);
                b.SetPictBox(pictureBox);
                b.Dragged += Update;
                Buttons.Add(b);
            }

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
        }

        public override bool Update(Point diff, BaseButton button)
        {
            if (button == CenterButton)
            {
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
                    Buttons[i].Location = new Point((int)points[i].X, (int)points[i].Y);
                }

                Program.MainForm.DrawFigures();
            }

            OnUpdate();
            return IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList());
        }

        protected override bool IsValidate(Point center, List<Point> points)
        {
            var isValidate = true;
            if (center.X < 0 || center.X > PictureBox.Width ||
                center.Y < 0 || center.Y > PictureBox.Height)
            {
                isValidate = false;
            }
            foreach (var p in points)
            {
                if (p.X < 0 || p.X > PictureBox.Width || p.Y < 0 || p.Y > PictureBox.Height)
                {
                    isValidate = false;
                }
            }
            return isValidate;
        }
    }
}

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
        private Point centerPoint;
        public override void Create(PictureBox pictureBox)
        {
            centerPoint = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));

            var radiuses = new List<int>()
            {
                centerPoint.X,
                centerPoint.Y,
                pictureBox.Size.Width - centerPoint.X,
                pictureBox.Size.Height - centerPoint.Y
            };

            while (radiuses.Min() < 30)
            {
                radiuses.Clear();

                centerPoint = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));

                radiuses.AddRange(new[]{
                centerPoint.X,
                centerPoint.Y,
                pictureBox.Size.Width - centerPoint.X,
                pictureBox.Size.Height - centerPoint.Y});
            }

            int radius = Random.Next(30, radiuses.Min());

            var angle = 2.0 * Math.PI / 5.0;
            IEnumerable<Point> points = Enumerable.Range(0, 5).Select(i => Point.Add((Point)centerPoint,
                new Size(
                (int)(Math.Sin(i * angle) * radius),
                (int)(Math.Cos(i * angle) * radius))));

            SetData(centerPoint, points.ToList(), pictureBox);
        }

        public override void Draw()
        {
            List<Point> points = new List<Point>();
            foreach (var b in Buttons)
            {
                points.Add(new Point(b.Location.X + 4, b.Location.Y + 4));
            }
            GraphicsManager.bufferedGraphics.Graphics.DrawPolygon(GraphicsManager.Pen, points.ToArray());
        }

        public override List<string> GetData()
        {
            List<string> strs = new List<string>
            {
                CenterButton.Location.X + " " + CenterButton.Location.Y
            };

            foreach (var b in Buttons)
            {
                strs[0] += " " + b.Location.X.ToString() + " " + b.Location.Y.ToString();
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

            BaseButton b;

            textBox = new TextBox
            {
                Size = new Size(20, 5),
                Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8),
                ReadOnly = true
            };

            foreach (var p in points)
            {
                b = new BaseButton
                {
                    Size = new Size(8, 8),
                    Location = new Point(p.X, p.Y)
                };
                b.SetPictBox(pictureBox);
                b.Dragged += Update;
                Buttons.Add(b);
            }

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
            pictureBox.Controls.Add(textBox);
        }

        public override bool Update(Point diff, BaseButton button)
        {
            if (button == CenterButton)
            {
                textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                foreach (var b in Buttons)
                {
                    b.Location = new Point(b.Location.X + diff.X, b.Location.Y + diff.Y);
                }

                if (false == IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList()))
                {
                    CenterButton.Location = new Point(CenterButton.Location.X - diff.X, CenterButton.Location.Y - diff.Y);
                    textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                    foreach (var b in Buttons)
                    {
                        b.Location = new Point(b.Location.X - diff.X, b.Location.Y - diff.Y);
                    }

                    return true;
                }
            }
            else if (Buttons.Contains(button))
            {
                int x = Math.Abs(CenterButton.Location.X - button.Location.X);
                int y = Math.Abs(CenterButton.Location.Y - button.Location.Y);
                var radius = Math.Sqrt(x * x + y * y);

                var angle = 2.0 * Math.PI / 5.0;
                var points = Enumerable.Range(0, 5).Select(i => PointF.Add((PointF)CenterButton.Location,
                    new SizeF(
                    (float)(Math.Sin(i * angle) * radius),
                    (float)(Math.Cos(i * angle) * radius)))).ToList();

                textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                if (false == IsValidate(CenterButton.Location, points.ConvertAll(new Converter<PointF, Point>(p => new Point((int)p.X, (int)p.Y)))))
                {
                    button.Location = new Point(button.Location.X - diff.X, button.Location.Y - diff.Y);

                    x = Math.Abs(button.Location.X - CenterButton.Location.X);
                    y = Math.Abs(button.Location.Y - CenterButton.Location.Y);
                    radius = Math.Sqrt(x * x + y * y);

                    points = Enumerable.Range(0, 5).Select(i => PointF.Add((PointF)CenterButton.Location,
                       new SizeF(
                       (float)(Math.Sin(i * angle) * radius),
                       (float)(Math.Cos(i * angle) * radius)))).ToList();

                    textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                    for (var i = 0; i < points.Count; i++)
                    {
                        Buttons[i].Location = new Point((int)points[i].X, (int)points[i].Y);
                    }

                    return true;
                }

                for (var i = 0; i < points.Count; i++)
                {
                    Buttons[i].Location = new Point((int)points[i].X, (int)points[i].Y);
                }
            }
            Program.MainForm.DrawFigures();
            OnUpdate();
            return IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList());
        }

        public override bool IsValidate(Point center, List<Point> points)
        {
            bool isValidate = true;
            if (center.X < 0 || center.X > PictureBox.Width ||
                center.Y < 0 || center.Y > PictureBox.Height)
            {
                isValidate = false;
            }
            foreach (var p in points)
            {
                if (p.X < 0 || p.X > PictureBox.Width - 6 || p.Y < 0 || p.Y > PictureBox.Height - 6)
                {
                    isValidate = false;
                }
            }
            return isValidate;
        }
    }
}

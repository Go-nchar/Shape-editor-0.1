using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public class Triangle : Figure
    {
        public override void Create(PictureBox pictureBox)
        {
            List<Point> points = new List<Point>();
            Point p;

            for (int i = 0; i < 3; ++i)
            {
                p = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));
                points.Add(p);
            }

            int a = Convert.ToInt32(Math.Sqrt((points[0].X - points[1].X) * (points[0].X - points[1].X) +
                (points[0].Y - points[1].Y) * (points[0].Y - points[1].Y)));
            int b = Convert.ToInt32(Math.Sqrt((points[1].X - points[2].X) * (points[1].X - points[2].X) +
                (points[1].Y - points[2].Y) * (points[1].Y - points[2].Y)));
            int c = Convert.ToInt32(Math.Sqrt((points[0].X - points[2].X) * (points[0].X - points[2].X) +
                (points[0].Y - points[2].Y) * (points[0].Y - points[2].Y)));

            while (a < 30 || b < 30 || c < 30 || a != b || b != c || a != c )
            {
                points.Clear();

                for (int i = 0; i < 3; ++i)
                {
                    p = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));
                    points.Add(p);
                }

                a = Convert.ToInt32(Math.Sqrt((points[0].X - points[1].X) * (points[0].X - points[1].X) +
                    (points[0].Y - points[1].Y) * (points[0].Y - points[1].Y)));
                b = Convert.ToInt32(Math.Sqrt((points[1].X - points[2].X) * (points[1].X - points[2].X) +
                    (points[1].Y - points[2].Y) * (points[1].Y - points[2].Y)));
                c = Convert.ToInt32(Math.Sqrt((points[0].X - points[2].X) * (points[0].X - points[2].X) +
                    (points[0].Y - points[2].Y) * (points[0].Y - points[2].Y)));
            }

            SetData(new Point(), points, pictureBox);
        }

        public override void Draw()
        {
            var points = new List<Point>();
            foreach (var b in Buttons)
            {
                points.Add(new Point(b.Location.X + 4, b.Location.Y + 4));
            }
            GraphicsManager.bufferedGraphics.Graphics.DrawPolygon(GraphicsManager.Pen, points.ToArray());
        }

        public override List<string> GetData()
        {
            var strs = new List<string>
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

            int x = 0;
            int y = 0;
            BaseButton button;

            foreach (var p in points)
            {
                button = new BaseButton
                {
                    Size = new Size(8, 8),
                    Location = p
                };
                x += button.Location.X / 3;
                y += button.Location.Y / 3;
                button.SetPictBox(pictureBox);
                button.Dragged += Update;
                Buttons.Add(button);
            }

            textBox = new TextBox
            {
                Size = new Size(20, 5),
                Location = new Point(x + 8, y + 8),
                ReadOnly = true
            };

            CenterButton = new BaseButton
            {
                Size = new Size(8, 8),
                Location = new Point(x, y)
            };
            
            CenterButton.SetPictBox(pictureBox);
            CenterButton.Dragged += Update;

            pictureBox.Controls.AddRange(Buttons.ToArray());
            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.Add(textBox);
        }

        public override bool Update(Point diff, Point location, BaseButton button)
        {
            if (button == CenterButton)
            {
                var locations = new List<Point>();
                locations.AddRange(Buttons.Select(b => new Point(b.Location.X + diff.X, b.Location.Y + diff.Y)));

                if (IsValidate(location, locations))
                {
                    foreach (var b in Buttons)
                    {
                        b.Location = new Point(b.Location.X + diff.X, b.Location.Y + diff.Y);
                    }
                    CenterButton.Location = location;
                    textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                }
            }
            else if (Buttons.Contains(button))
            {
                if (IsValidate(location))
                {
                    button.Location = location;
                    var (x, y) = (0, 0);
                    foreach (var b in Buttons)
                    {
                        x += b.Location.X / 3;
                        y += b.Location.Y / 3;
                    }

                    CenterButton.Location = new Point(x, y);
                    textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);
                }
            }
            Program.MainForm.DrawFigures();
            OnUpdate();
            return IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList());
        }

        public override bool IsValidate(Point center, List<Point> points)
        {
            bool isValidate = true;
            if (center.X < 0 || center.Y < 0 || center.X > PictureBox.Width || center.Y > PictureBox.Height)
            {
                isValidate = false;
            }
            foreach (var p in points)
            {
                if (p.X < 0 || p.Y < 0 || p.X > PictureBox.Width - 6 || p.Y > PictureBox.Height - 6)
                {
                    isValidate = false;
                }
            }

            return isValidate;
        }

        private bool IsValidate(Point point)
        {
            return !(point.X < 0 || point.Y < 0 || point.X > PictureBox.Width - 6 || point.Y > PictureBox.Height - 6);
        }
    }
}

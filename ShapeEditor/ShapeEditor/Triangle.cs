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
            var points = new List<Point>();
            Point p;
            var (x, y) = (0, 0);
            Point center;

            for (int i = 0; i < 3; ++i)
            {
                p = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));
                points.Add(p);
            }

            foreach(var point in points)
            {
                x += point.X / 3;
                y += point.Y / 3;
            }

            center = new Point(x, y);

            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = 0; j < points.Count; ++j)
                {
                    if (points[i] == points[j])
                        continue;

                    while (Math.Abs(points[i].X - points[j].X) < 150 || Math.Abs(points[i].Y - points[j].Y) < 150 && Math.Abs(center.X - points[i].X) < 150 || Math.Abs(center.Y - points[i].Y) < 150)
                    {
                        points.Clear();

                        for (int k = 0;  k < 3; ++k)
                        {
                            p = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));
                            points.Add(p);
                        }
                    }
                }
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
            GraphicsManager.bufferedGraphics.Render();
        }

        public override List<string> GetData()
        {
            var strs = new List<string>();
            strs.Add(CenterButton.Location.X + " " + CenterButton.Location.Y);
            string x;
            string y;
            string xy;

            foreach (var b in Buttons)
            {
                x = " " + b.Location.X.ToString();
                y = b.Location.Y.ToString();
                xy = x + " " + y;

                strs[0] += xy;
            }

            return strs;
        }

        public override void SetData(Point centerPoint, List<Point> points, PictureBox pictureBox)
        {
            PictureBox = pictureBox;

            var x = 0;
            var y = 0;
            BaseButton button;

            foreach (var p in points)
            {
                button = new BaseButton();
                button.Size = new Size(8, 8);
                button.Location = p;
                x += button.Location.X / 3;
                y += button.Location.Y / 3;
                button.SetPictBox(pictureBox);
                button.Dragged += Update;
                Buttons.Add(button);
            }

            CenterButton = new BaseButton();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = new Point(x, y);
            CenterButton.SetPictBox(pictureBox);
            CenterButton.Dragged += Update;

            pictureBox.Controls.AddRange(Buttons.ToArray());
            pictureBox.Controls.Add(CenterButton);
        }

        public override bool Update(Point diff, BaseButton button)
        {
            if (button == CenterButton)
            {
                foreach (var b in Buttons)
                {
                    b.Location = new Point(b.Location.X + diff.X, b.Location.Y + diff.Y);
                }

                if (false == IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList()))
                {
                    CenterButton.Location = new Point(CenterButton.Location.X - diff.X, CenterButton.Location .Y - diff.Y);

                    foreach (var b in Buttons)
                    {
                        b.Location = new Point(b.Location.X - diff.X, b.Location.Y - diff.Y);
                    }
                    return true;
                }
            }
            else if (Buttons.Contains(button))
            {
                var (x, y) = (0, 0);
                foreach (var b in Buttons)
                {
                    x += b.Location.X / 3;
                    y += b.Location.Y / 3;
                }

                CenterButton.Location = new Point(x, y);

                if (false == IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList()))
                {
                    button.Location = new Point(button.Location.X - diff.X, button.Location.Y - diff.Y);
                   
                    return true;
                }
            }
            Program.MainForm.DrawFigures();
            OnUpdate();
            return IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList());
        }

        public override bool IsValidate(Point center, List<Point> points)
        {
            var isValidate = true;
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
    }
}

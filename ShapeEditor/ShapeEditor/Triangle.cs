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
            for (int i = 0; i < 3; ++i)
            {
                p = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
                points.Add(p);
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
            strs.Add("Triangle");
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
                    MessageBox.Show("size exceeded!");
                    CenterButton.Location = new Point(CenterButton.Location.X - diff.X, CenterButton.Location .Y - diff.Y);

                    foreach (var b in Buttons)
                    {
                        b.Location = new Point(b.Location.X - diff.X, b.Location.Y - diff.Y);
                    }
                    return false;
                }

                Program.MainForm.DrawFigures();
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

                Program.MainForm.DrawFigures();
            }

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
                if (p.X < 0 || p.Y < 0 || p.X > PictureBox.Width || p.Y > PictureBox.Height)
                {
                    isValidate = false;
                }
            }

            return isValidate;
        }
    }
}

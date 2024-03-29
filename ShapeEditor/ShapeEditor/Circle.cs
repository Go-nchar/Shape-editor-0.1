﻿using System;
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
        private Point leftUpPoint;
        private int diameter;

        public override void Create(PictureBox pictureBox)
        {
            leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));
            while (Math.Min(pictureBox.Size.Width - 6 - leftUpPoint.X, pictureBox.Size.Height - 6 - leftUpPoint.Y) < 30)
            {
                leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));
            }

            diameter = Random.Next(30, Math.Min(pictureBox.Size.Width - 6 - leftUpPoint.X, pictureBox.Size.Height - 6 - leftUpPoint.Y));
            _rect = new Rectangle(leftUpPoint.X + 4, leftUpPoint.Y + 4, diameter, diameter);

            SetData(new Point(leftUpPoint.X + diameter / 2, leftUpPoint.Y + diameter / 2),
                new List<Point> { new Point(leftUpPoint.X + diameter / 2, leftUpPoint.Y) }, pictureBox);
        }

        public override void Draw()
        {
            GraphicsManager.bufferedGraphics.Graphics.DrawEllipse(GraphicsManager.Pen, _rect);
        }

        public override List<string> GetData()
        {
            List<string> strs = new List<string>
            {
                CenterButton.Location.X + " " + CenterButton.Location.Y
            };

            foreach (var b in Buttons)
            {
                strs[0] += " " + b.Location.X + " " + b.Location.Y;
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
                    Location = p
                };
                b.SetPictBox(pictureBox);
                b.Dragged += Update;
                Buttons.Add(b);
            }

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
            pictureBox.Controls.Add(textBox);

            var y = Math.Abs(Buttons[0].Location.Y - CenterButton.Location.Y);
            var x = Math.Abs(Buttons[0].Location.X - CenterButton.Location.X);
            var radius = (int)Math.Sqrt(x * x + y * y);

            _rect = new Rectangle(CenterButton.Location.X - radius + 4,
                    CenterButton.Location.Y - radius + 4, radius * 2, radius * 2);
        }

        public override bool Update(Point diff, Point location, BaseButton button)
        {
            if (button == CenterButton)
            {
                var locations = new List<Point>();
                locations.AddRange(Buttons.Select(b => new Point(b.Location.X + diff.X, b.Location.Y + diff.Y)));

                if (IsValidate(location, locations))
                {
                    CenterButton.Location = location;
                    textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                    foreach (var b in Buttons)
                    {
                        b.Location = new Point(b.Location.X + diff.X, b.Location.Y + diff.Y);
                    }

                    _rect.X += diff.X;
                    _rect.Y += diff.Y;
                }
            }
            else if (Buttons.Contains(button))
            {
                if (IsValidate(CenterButton.Location, location))
                {
                    button.Location = location;

                    var y = Math.Abs(location.Y - CenterButton.Location.Y);
                    var x = Math.Abs(location.X - CenterButton.Location.X);
                    var radius = (int)Math.Sqrt(x * x + y * y);

                    _rect = new Rectangle(CenterButton.Location.X - radius + 4,
                        CenterButton.Location.Y - radius + 4, radius * 2, radius * 2);
                }
            }
            Program.MainForm.DrawFigures();
            OnUpdate();
            return IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList());
        }

        public override bool IsValidate(Point center, List<Point> points)
        {
            var isValidate = true;
            int y;
            int x;
            int radius;

            if (center.X < 0 || center.X > PictureBox.Width ||
                center.Y < 0 || center.Y > PictureBox.Height)
            {
                isValidate = false;
            }
            foreach (var p in points)
            {
                y = Math.Abs(p.Y - center.Y);
                x = Math.Abs(p.X - center.X);
                radius = (int)Math.Sqrt(x * x + y * y);

                if (center.X - radius < 0 || center.X + radius > PictureBox.Width - 6 ||
                    center.Y - radius < 0 || center.Y + radius > PictureBox.Height - 6)
                {
                    isValidate = false;
                }
            }
            return isValidate;
        }

        public bool IsValidate(Point center, Point point)
        {
            int y = Math.Abs(point.Y - center.Y);
            int x = Math.Abs(point.X - center.X);
            int radius = (int)Math.Sqrt(x * x + y * y);

            return !(center.X - radius < 0 || center.X + radius > PictureBox.Width - 6 ||
                center.Y - radius < 0 || center.Y + radius > PictureBox.Height - 6);
        }
    }
}

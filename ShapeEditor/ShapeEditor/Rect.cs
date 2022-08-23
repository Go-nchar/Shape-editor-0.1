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
        private Point leftUpPoint;
        private int width;
        private int height;

        public override void Create(PictureBox pictureBox)
        {
            leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));

            while (pictureBox.Size.Width - 6 - leftUpPoint.X < 50 || pictureBox.Size.Height - 6 - leftUpPoint.Y < 50)
            {
                leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width - 6), Random.Next(0, pictureBox.Size.Height - 6));
            }

            width = Random.Next(50, pictureBox.Size.Width - 6 - leftUpPoint.X);
            height = Random.Next(50, pictureBox.Size.Height - 6 - leftUpPoint.Y);

            var points = new List<Point>()
            {
                leftUpPoint,
                new Point(leftUpPoint.X + width, leftUpPoint.Y),
                new Point(leftUpPoint.X + width, leftUpPoint.Y + height),
                new Point(leftUpPoint.X, leftUpPoint.Y + height)
            };

            Point centerPoint = new Point(leftUpPoint.X + width / 2, leftUpPoint.Y + height / 2);

            SetData(centerPoint, points, pictureBox);
        }

        public override void Draw()
        {
            GraphicsManager.bufferedGraphics.Graphics.DrawRectangle(GraphicsManager.Pen, _rect);
            GraphicsManager.bufferedGraphics.Render();
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

            var width = Buttons[1].Location.X - Buttons[0].Location.X;
            var height = Buttons[3].Location.Y - Buttons[0].Location.Y;
            CenterButton.Location = new Point(Buttons[0].Location.X + width / 2,
                Buttons[0].Location.Y + height / 2);

            width = Math.Abs(width);
            height = Math.Abs(height);

            var leftUpButton = new Point(CenterButton.Location.X - width / 2, CenterButton.Location.Y - height / 2);

            _rect = new Rectangle(leftUpButton.X + 4, leftUpButton.Y + 4, width, height);
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

                _rect.X += diff.X;
                _rect.Y += diff.Y;

                if (false == IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList()))
                {
                    CenterButton.Location = new Point(CenterButton.Location.X - diff.X, CenterButton.Location.Y - diff.Y);
                    textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                    foreach (var b in Buttons)
                    {
                        b.Location = new Point(b.Location.X - diff.X, b.Location.Y - diff.Y);
                    }

                    _rect.X -= diff.X;
                    _rect.Y -= diff.Y;

                    return true;
                }
            }
            else if (Buttons.Contains(button))
            {
                var i = Buttons.IndexOf(button);
                if (i == 0)
                {
                    Buttons[1].Location = new Point(Buttons[1].Location.X, Buttons[1].Location.Y + diff.Y);
                    Buttons[3].Location = new Point(Buttons[3].Location.X + diff.X, Buttons[3].Location.Y);
                }
                else if (i == 1)
                {
                    Buttons[2].Location = new Point(Buttons[2].Location.X + diff.X, Buttons[2].Location.Y);
                    Buttons[0].Location = new Point(Buttons[0].Location.X, Buttons[0].Location.Y + diff.Y);
                }
                else if (i == 2)
                {
                    Buttons[1].Location = new Point(Buttons[1].Location.X + diff.X, Buttons[1].Location.Y);
                    Buttons[3].Location = new Point(Buttons[3].Location.X, Buttons[3].Location.Y + diff.Y);
                }
                else if (i == 3)
                {
                    Buttons[2].Location = new Point(Buttons[2].Location.X, Buttons[2].Location.Y + diff.Y);
                    Buttons[0].Location = new Point(Buttons[0].Location.X + diff.X, Buttons[0].Location.Y);
                }

                var width = Buttons[1].Location.X - Buttons[0].Location.X;
                var height = Buttons[3].Location.Y - Buttons[0].Location.Y;
                CenterButton.Location = new Point(Buttons[0].Location.X + width / 2, 
                    Buttons[0].Location.Y + height / 2);

                textBox.Location = new Point(CenterButton.Location.X + 8, CenterButton.Location.Y + 8);

                width = Math.Abs(width);
                height = Math.Abs(height);

                var leftUpButton = new Point(CenterButton.Location.X - width / 2, CenterButton.Location.Y - height / 2);

                _rect = new Rectangle(leftUpButton.X + 4, leftUpButton.Y + 4, width, height);
            }
            Program.MainForm.DrawFigures();
            OnUpdate();
            return IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList());
        }

        public override bool IsValidate(Point center, List<Point> points)
        {
            var isValidate = true;
            if (center.X < 0 || center.X > PictureBox.Width ||
                center.Y < 0 || center.Y > PictureBox.Height)
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

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
            PictureBox = pictureBox;

            var leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
            var width = Random.Next(0, pictureBox.Size.Width - leftUpPoint.X);
            var height = Random.Next(0, pictureBox.Size.Height - leftUpPoint.Y);
            _rect = new Rectangle(leftUpPoint.X + 4, leftUpPoint.Y + 4, width, height);

            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = new Point(leftUpPoint.X + width / 2, leftUpPoint.Y + height / 2);
            CenterButton.HandleMove(pictureBox);

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
                b.HandleMove(pictureBox);
                Buttons.Add(b);
            }

            ButtonManager.Dragged += Update;

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
        }

        public override void Draw()
        {
            GraphicsManager.Graphics.DrawRectangle(GraphicsManager.Pen, _rect);
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

                _rect.X += diff.X;
                _rect.Y += diff.Y;

                Program.MainForm.DrawFigures();
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

                width = Math.Abs(width);
                height = Math.Abs(height);

                var leftUpButton = new Point(CenterButton.Location.X - width / 2, CenterButton.Location.Y - height / 2);

                _rect = new Rectangle(leftUpButton.X + 4, leftUpButton.Y + 4, width, height);

                Program.MainForm.DrawFigures();
            }

            return true;
        }
    }
}

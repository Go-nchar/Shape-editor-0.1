using System;
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

        public override void Create(PictureBox pictureBox)
        {
            PictureBox = pictureBox;

            var leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
            var diameter = Random.Next(0, Math.Min(pictureBox.Size.Width - leftUpPoint.X, pictureBox.Size.Height - leftUpPoint.Y));
            _rect = new Rectangle(leftUpPoint.X + 4, leftUpPoint.Y + 4, diameter, diameter);

            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = new Point(leftUpPoint.X + diameter / 2, leftUpPoint.Y + diameter / 2);
            CenterButton.HandleMove(pictureBox);

            var b = new Button();
            b.Size = new Size(8, 8);
            b.Location = new Point(leftUpPoint.X + diameter / 2, leftUpPoint.Y);
            b.HandleMove(pictureBox);
            Buttons.Add(b);

            ButtonManager.Dragged += Update;

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());
        }

        public override void Draw()
        {
            GraphicsManager.Graphics.DrawEllipse(GraphicsManager.Pen, _rect);
        }

        public override bool Update(Point diff, Button button)
        {
            if (button == CenterButton)
            {
                foreach (var b in Buttons)
                {
                    int x = b.Location.X + diff.X;
                    int y = b.Location.Y + diff.Y;

                    var deltaY = Math.Abs(button.Location.Y - y);
                    var deltaX = Math.Abs(button.Location.X - x);
                    var radius = (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                    if (CenterButton.Location.X - radius < 0 || CenterButton.Location.X + radius > PictureBox.Width ||
                        CenterButton.Location.Y - radius < 0 || CenterButton.Location.Y + radius > PictureBox.Height)
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
                var y = Math.Abs(button.Location.Y - CenterButton.Location.Y);
                var x = Math.Abs(button.Location.X - CenterButton.Location.X);
                var radius = (int) Math.Sqrt(x * x + y * y);

                if (CenterButton.Location.X - radius < 0 || CenterButton.Location.X + radius > PictureBox.Width || 
                    CenterButton.Location.Y - radius < 0 || CenterButton.Location.Y + radius > PictureBox.Height)
                {
                    return false;
                }

                _rect = new Rectangle(CenterButton.Location.X - radius + 4, 
                    CenterButton.Location.Y - radius + 4, radius * 2, radius * 2);

                Program.MainForm.DrawFigures();
            }

            return true;
        }
    }
}

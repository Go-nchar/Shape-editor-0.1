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
            PictureBox = pictureBox;

            var x = 0;
            var y = 0;
            for (int i = 0; i < 3; ++i)
            {
                var button = new Button();
                button.Size = new Size(8, 8);
                button.Location = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
                x += button.Location.X / 3;
                y += button.Location.Y / 3;
                button.HandleMove(pictureBox);
                Buttons.Add(button);
            }

            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = new Point(x, y);
            CenterButton.HandleMove(pictureBox);

            ButtonManager.Dragged += Update;

            pictureBox.Controls.AddRange(Buttons.ToArray());
            pictureBox.Controls.Add(CenterButton);
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
                var (x, y) = (0, 0);
                foreach (var b in Buttons)
                {
                    x += b.Location.X / 3;
                    y += b.Location.Y / 3;
                }

                CenterButton.Location = new Point(x, y);
                Program.MainForm.DrawFigures();
            }

            return true;
        }
    }
}

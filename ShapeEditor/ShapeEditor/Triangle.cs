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
            var x = 0;
            var y = 0;
            for (int i = 0; i < 3; ++i)
            {
                var button = new Button();
                button.Size = new Size(8, 8);
                button.Location = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
                x += button.Location.X / 3;
                y += button.Location.Y / 3;
                Buttons.Add(button);
            }

            CenterButton = new Button();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = new Point(x, y);

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
    }
}

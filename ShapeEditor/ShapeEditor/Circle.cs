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
            var leftUpPoint = new Point(Random.Next(0, pictureBox.Size.Width), Random.Next(0, pictureBox.Size.Height));
            var diameter = Random.Next(0, Math.Min(pictureBox.Size.Width - leftUpPoint.X, pictureBox.Size.Height - leftUpPoint.Y));
            _rect = new Rectangle(leftUpPoint.X + 4, leftUpPoint.Y + 4, diameter, diameter);

            SetData(new Point(leftUpPoint.X + diameter / 2, leftUpPoint.Y + diameter / 2),
                new List<Point> { new Point(leftUpPoint.X + diameter / 2, leftUpPoint.Y) }, pictureBox);
        }

        public override void Draw()
        {
            GraphicsManager.Graphics.DrawEllipse(GraphicsManager.Pen, _rect);
        }

        public override List<string> GetData()
        {
            var strs = new List<string>();
            strs.Add("Circle");
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

            CenterButton = new BaseButton();
            CenterButton.Size = new Size(8, 8);
            CenterButton.Location = centerPoint;
            CenterButton.SetPictBox(pictureBox);
            CenterButton.Dragged += Update;

            foreach (var p in points)
            {
                var b = new BaseButton();
                b.Size = new Size(8, 8);
                b.Location = p;
                b.SetPictBox(pictureBox);
                b.Dragged += Update;
                Buttons.Add(b);
            }

            pictureBox.Controls.Add(CenterButton);
            pictureBox.Controls.AddRange(Buttons.ToArray());

            var y = Math.Abs(Buttons[0].Location.Y - CenterButton.Location.Y);
            var x = Math.Abs(Buttons[0].Location.X - CenterButton.Location.X);
            var radius = (int)Math.Sqrt(x * x + y * y);

            _rect = new Rectangle(CenterButton.Location.X - radius + 4,
                    CenterButton.Location.Y - radius + 4, radius * 2, radius * 2);
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
                var y = Math.Abs(p.Y - center.Y);
                var x = Math.Abs(p.X - center.X);
                var radius = (int)Math.Sqrt(x * x + y * y);

                if (center.X - radius < 0 || center.X + radius > PictureBox.Width ||
                    center.Y - radius < 0 || center.Y + radius > PictureBox.Height)
                {
                    isValidate = false;
                }
            }
            return isValidate;
        }

        public override bool Update(Point diff, BaseButton button)
        {
            if (button == CenterButton)
            {
                foreach (var b in Buttons)
                {
                    b.Location = new Point(b.Location.X + diff.X, b.Location.Y + diff.Y);
                }

                _rect.X += diff.X;
                _rect.Y += diff.Y;

                if (false == IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList()))
                {
                    MessageBox.Show("size exceeded!");

                    CenterButton.Location = new Point(CenterButton.Location.X - diff.X, CenterButton.Location.Y - diff.Y);

                    foreach (var b in Buttons)
                    {
                        b.Location = new Point(b.Location.X - diff.X, b.Location.Y - diff.Y);
                    }

                    _rect.X -= diff.X;
                    _rect.Y -= diff.Y;

                    return false;
                }

                Program.MainForm.DrawFigures();
            }
            else if (Buttons.Contains(button))
            {
                var y = Math.Abs(button.Location.Y - CenterButton.Location.Y);
                var x = Math.Abs(button.Location.X - CenterButton.Location.X);
                var radius = (int) Math.Sqrt(x * x + y * y);

                _rect = new Rectangle(CenterButton.Location.X - radius + 4, 
                    CenterButton.Location.Y - radius + 4, radius * 2, radius * 2);

                if (false == IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList()))
                {
                    MessageBox.Show("size exceeded!");

                    button.Location = new Point(button.Location.X - diff.X, button.Location.Y - diff.Y);

                    y = Math.Abs(button.Location.Y - CenterButton.Location.Y);
                    x = Math.Abs(button.Location.X - CenterButton.Location.X);
                    radius = (int)Math.Sqrt(x * x + y * y);

                    _rect = new Rectangle(CenterButton.Location.X - radius + 4,
                        CenterButton.Location.Y - radius + 4, radius * 2, radius * 2);

                    return false;
                }

                Program.MainForm.DrawFigures();
            }

            OnUpdate();
            return IsValidate(CenterButton.Location, Buttons.Select(b => b.Location).ToList());
        }
    }
}

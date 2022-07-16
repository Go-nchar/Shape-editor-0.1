using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public static class ButtonManager
    {
        private static bool _isDragged;
        private static Point _downPoint;

        public static event Func<Point, Button, bool> Dragged;

        public static void HandleMove(this Button button, PictureBox pictureBox)
        {
            button.MouseDown += OnMouseDown;
            button.MouseUp += OnMouseUp;
            button.MouseMove += (s, e) => OnMouseMove(e, button, pictureBox);
        }

        private static void OnMouseUp(object s, MouseEventArgs e)
        {
            _isDragged = false;
        }

        private static void OnMouseDown(object s, MouseEventArgs e)
        {
            _downPoint = e.Location;
            _isDragged = true;
        }

        private static void OnMouseMove(MouseEventArgs e, Button b, PictureBox pictureBox)
        {
            if (!_isDragged)
                return;

            Point diff = new Point(e.Location.X - _downPoint.X, e.Location.Y - _downPoint.Y);

            if (pictureBox.Width > b.Location.X + diff.X && pictureBox.Height > b.Location.Y + diff.Y)
            {
                if (b.Location.X + diff.X > 0 && b.Location.Y + diff.Y > 0)
                {
                    int x = b.Location.X + diff.X;
                    int y = b.Location.Y + diff.Y;
                    b.Location = new Point(x, y);
                    _isDragged = Dragged?.Invoke(diff, b) ?? true;
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public class BaseButton : Button
    {
        private bool _isDragged;
        private Point _downPoint;
        private PictureBox _pictureBox;
        private Point diff;
        private int x;
        private int y;
        private Point _loc;

        public event Func<Point, Point, BaseButton, bool> Dragged;

        public void SetPictBox(PictureBox pBox) => _pictureBox = pBox;

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _isDragged = false;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _downPoint = mevent.Location;
            _isDragged = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_isDragged)
                return;

             diff = new Point(e.Location.X - _downPoint.X, e.Location.Y - _downPoint.Y);

            
            if (_pictureBox.Width - 6 > Location.X + diff.X && _pictureBox.Height - 6 > Location.Y + diff.Y)
            {
                if (Location.X + diff.X > 0 && Location.Y + diff.Y > 0)
                {
                    x = Location.X + diff.X;
                    y = Location.Y + diff.Y;
                    _loc = new Point(x, y);
                    _isDragged = Dragged?.Invoke(diff, _loc, this) ?? true;
                }
            }
            
        }
    }
}

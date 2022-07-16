using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public static class GraphicsManager
    {
        public static Graphics Graphics { get; private set; }
        public static SolidBrush Brush { get; private set; }
        public static Pen Pen { get; private set; }

        public static void Init(PictureBox p)
        {
            Graphics = p.CreateGraphics();
            Brush = new SolidBrush(Color.DarkMagenta);
            Pen = new Pen(Color.Black, 2);
        }
    }
}

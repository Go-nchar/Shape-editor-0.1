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
        public static BufferedGraphics bufferedGraphics;
        public static BufferedGraphicsContext bufferedGraphicsContext;
        public static Pen Pen { get; private set; }

        public static void Init(PictureBox p)
        {
            bufferedGraphicsContext = new BufferedGraphicsContext();

            Graphics = p.CreateGraphics();
            bufferedGraphics = bufferedGraphicsContext.Allocate(Graphics, new Rectangle(0, 0, p.Width, p.Height));

            p.SizeChanged += (s, e) => Graphics = p.CreateGraphics();
            p.SizeChanged += (s, e) => bufferedGraphics = bufferedGraphicsContext.Allocate(Graphics, new Rectangle(0, 0, p.Width, p.Height));

            Pen = new Pen(Color.Black, 2);
            
        }

        public static void ClearBuffered()
        {
            bufferedGraphics.Graphics.Clear(Color.LightBlue);
        }

        public static void ClearGraphics()
        {
            Graphics.Clear(Color.LightBlue);
        }
    }
}

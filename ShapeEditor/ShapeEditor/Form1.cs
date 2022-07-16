using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public partial class Form1 : Form
    {
        private Random _random = new Random();
        private List<Figure> _figures = new List<Figure>();

        public Form1()
        {
            InitializeComponent();
            GraphicsManager.Init(pictureBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OnTriangleButtonClick(object sender, EventArgs e)
        {
            var t = new Triangle();
            t.Create(pictureBox);
            t.Draw();
            _figures.Add(t);
        }

        private void OnRectButtonClick(object sender, EventArgs e)
        {
            var r = new Rect();
            r.Create(pictureBox);
            r.Draw();
            _figures.Add(r);
        }

        private void OnPentagonButtonClick(object sender, EventArgs e)
        {
            var p = new Pentagon();
            p.Create(pictureBox);
            p.Draw();
            _figures.Add(p);
        }

        private void OnCircleButtonClick(object sender, EventArgs e)
        {
            var c = new Circle();
            c.Create(pictureBox);
            c.Draw();
            _figures.Add(c);
        }

        public void DrawFigures()
        {
            GraphicsManager.Clear();
            foreach (Figure f in _figures)
            {
                f.Draw();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            t.Updated += () => textBox1.SetData(_figures);
            _figures.Add(t);
            textBox1.SetData(_figures);
        }

        private void OnRectButtonClick(object sender, EventArgs e)
        {
            var r = new Rect();
            r.Create(pictureBox);
            r.Draw();
            r.Updated += () => textBox1.SetData(_figures);
            _figures.Add(r);
            textBox1.SetData(_figures);
        }

        private void OnPentagonButtonClick(object sender, EventArgs e)
        {
            var p = new Pentagon();
            p.Create(pictureBox);
            p.Draw();
            p.Updated += () => textBox1.SetData(_figures);
            _figures.Add(p);
            textBox1.SetData(_figures);
        }

        private void OnCircleButtonClick(object sender, EventArgs e)
        {
            var c = new Circle();
            c.Create(pictureBox);
            c.Draw();
            c.Updated += () => textBox1.SetData(_figures);
            _figures.Add(c);
            textBox1.SetData(_figures);
        }

        public void DrawFigures()
        {
            GraphicsManager.Clear();
            foreach (Figure f in _figures)
            {
                f.Draw();
            }
        }

        public void DrawFigures(object sender, EventArgs e)
        {
            GraphicsManager.Clear();
            foreach (Figure f in _figures)
            {
                f.Draw();
            }
        }

        private void OnClearButtonClick(object sender, EventArgs e)
        {
            GraphicsManager.Clear();
            _figures.Clear();
            pictureBox.Controls.Clear();
        }

        private bool _isUser;

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox textBox) || !_isUser)
                return;

            _isUser = false;
            OnClearButtonClick(sender, e);

            string target = " ";
            Regex regex = new Regex(@"\b\d{1,3} \d{1,3}\b");

            var strs = new List<List<string>>();
            var lines = new List<string>();
            foreach (var l in textBox.Lines)
            {
                if (l == "Circle" || l == "Triangle" ||
                    l == "Rect" || l == "Pentagon")
                {
                    if (lines.Count != 0)
                        strs.Add(lines);

                    lines = new List<string>();
                }

                lines.Add(l);
            }
            strs.Add(lines);

            for (var i = 0; i < strs.Count; i++)
            {
                var center = new Point();
                var points = new List<Point>();
                for (var j = 0; j < strs[i].Count; j++)
                {
                    string l = strs[i][j];
                    if (l == "Triangle" || l == "Circle")
                        continue;

                    if (!regex.IsMatch(l))
                        l = "0 0";

                    var nums = l.Split(' ').Select(snum => int.Parse(snum)).ToArray();
                    if (j == 1 && !strs[i].Contains("Triangle"))
                        center = new Point(nums[0], nums[1]);
                    else
                        points.Add(new Point(nums[0], nums[1]));
                }

                if (strs[i].Contains("Circle"))
                {
                    var c = new Circle();
                    c.SetData(center, points, pictureBox);
                    c.Draw();
                    _figures.Add(c);
                }

                if (strs[i].Contains("Triangle"))
                {
                    var c = new Triangle();
                    c.SetData(center, points, pictureBox);
                    c.Draw();
                    _figures.Add(c);
                }
            }

            textBox.SetData(_figures);
        }

        private void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;

            if (char.IsDigit(keyChar) || keyChar == '\b' || keyChar == ' ')
            {
                _isUser = true;
                return;
            }

            e.Handled = true;
        }
    }
}

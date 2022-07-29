using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
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
            textBox1.Clear();
        }

        private void OnClearButtonClick()
        {
            GraphicsManager.Clear();
            _figures.Clear();
            pictureBox.Controls.Clear();
            textBox1.Clear();
        }

        private bool _isUser;

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox textBox) || !_isUser)
                return;

            _isUser = false;

            Regex regex = new Regex(@"\b\d{1,4} \d{1,4}\b");

            foreach (var f in _figures)
            {
                var center = new Point();
                var points = new List<Point>();

                foreach (var j in f.Indexes)
                {
                    if (j == f.Indexes.First())
                        continue;

                    var str = textBox.Lines[j];
                    if (!regex.IsMatch(str))
                        str = "0 0";

                    var nums = str.Split(' ').Select(snum => int.Parse(snum)).ToArray();
                    if (j == f.Indexes[1])
                    {
                        center = new Point(nums[0], nums[1]);
                        if (center.X > pictureBox.Width)
                        {
                            center.X = pictureBox.Width - 4;
                            MessageBox.Show("size exceeded");
                        }
                        else if (center.Y > pictureBox.Height)
                        {
                            center.Y = pictureBox.Height - 4;
                            MessageBox.Show("size exceeded");
                        }
                    }
                    else
                    {
                        points.Add(new Point(nums[0], nums[1]));
                        if (points.Last().X > pictureBox.Width)
                        {
                            points.Remove(points.Last());
                            points.Add(new Point(pictureBox.Width - 4, pictureBox.Height/2));
                            MessageBox.Show("size exceeded");
                        }
                        else if (points.Last().Y > pictureBox.Height)
                        {
                            points.Remove(points.Last());
                            points.Add(new Point(pictureBox.Width / 2, pictureBox.Height - 4));
                            MessageBox.Show("size exceeded");
                        }
                    }
                }

                var centerDiff = new Point(center.X - f.CenterButton.Location.X, center.Y - f.CenterButton.Location.Y);
                var pointDiffs = new List<Point>();
                for (var k = 0; k < points.Count; k++)
                {
                    pointDiffs.Add(new Point(points[k].X - f.Buttons[k].Location.X, points[k].Y - f.Buttons[k].Location.Y));
                }

                if (centerDiff.X != 0 || centerDiff.Y != 0)
                {
                    f.CenterButton.Location = center;
                    f.Update(centerDiff, f.CenterButton);
                }

                for (var p = 0; p < pointDiffs.Count; p++)
                {
                    if (pointDiffs[p].X == 0 && pointDiffs[p].Y == 0)
                        continue;

                    f.Buttons[p].Location = points[p];
                    f.Update(pointDiffs[p], f.Buttons[p]);
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

        private bool Check(Point center, List<Point> points)
        {
            if (center.X < 0 || center.Y < 0 || center.X > pictureBox.Width || center.Y > pictureBox.Height)
            {
                OnClearButtonClick();
                MessageBox.Show("The file does not fit!");
                return true;
            }
            for (int c = 0; c < points.Count; c++)
            {
                if (points[c].X < 0 || points[c].Y < 0 || points[c].X > pictureBox.Width || points[c].Y > pictureBox.Height)
                {
                    OnClearButtonClick();
                    MessageBox.Show("The file does not fit!");
                    return true;
                }
            }
            return false;
        }

        private void SettingTextboxInitial(List<string> fileText)
        {
            if (fileText.First() != "Triangle" && fileText.First() != "Circle" && fileText.First() != "Pentagon" && fileText.First() != "Rect")
            { 
                MessageBox.Show("The file does not fit!");
                return;
            }

            for (var i = 0; i < fileText.Count; i++)
            { 

                if (fileText[i] == "Triangle")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 1; j < i + 5; j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        if (j == i + 1)
                            center = new Point(nums[0], nums[1]);
                        else
                            points.Add(new Point(nums[0], nums[1]));
                    }

                    if (Check(center, points)) 
                        return;

                    var t = new Triangle();
                    t.SetData(center, points, pictureBox);
                    t.Updated += () => textBox1.SetData(_figures);
                    _figures.Add(t);
                    textBox1.SetData(_figures);
                }

                if (fileText[i] == "Circle")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 1; j < i + 3; j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        if (j == i + 1)
                            center = new Point(nums[0], nums[1]);
                        else
                            points.Add(new Point(nums[0], nums[1]));
                    }

                    if (Check(center, points))
                        return;

                    var c = new Circle();
                    c.SetData(center, points, pictureBox);
                    c.Updated += () => textBox1.SetData(_figures);
                    _figures.Add(c);
                    textBox1.SetData(_figures);
                }

                if (fileText[i] == "Pentagon")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 1; j < i + 7; j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        if (j == i + 1)
                            center = new Point(nums[0], nums[1]);
                        else
                            points.Add(new Point(nums[0], nums[1]));
                    }

                    if (Check(center, points))
                        return;

                    var p = new Pentagon();
                    p.SetData(center, points, pictureBox);
                    p.Updated += () => textBox1.SetData(_figures);
                    _figures.Add(p);
                    textBox1.SetData(_figures);
                }

                if (fileText[i] == "Rect")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 1; j < i + 6; j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        if (j == i + 1)
                            center = new Point(nums[0], nums[1]);
                        else
                            points.Add(new Point(nums[0], nums[1]));
                    }

                    if (Check(center, points))
                        return;

                    var r = new Rect();
                    r.SetData(center, points, pictureBox);
                    r.Updated += () => textBox1.SetData(_figures);
                    _figures.Add(r);
                    textBox1.SetData(_figures);
                }
            }
            MessageBox.Show("File open");
            DrawFigures();
        }

        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = saveFileDialog1.FileName;

            File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("File saved");
        }

        private void OnLoadButtonClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = openFileDialog1.FileName;

            OnClearButtonClick();
            List<string> fileText = File.ReadAllLines(filename).ToList();

            try
            {
                SettingTextboxInitial(fileText);
                return;
            }
            catch
            {
                MessageBox.Show("The file does not fit!");
                return;
            }
        }
    }
}

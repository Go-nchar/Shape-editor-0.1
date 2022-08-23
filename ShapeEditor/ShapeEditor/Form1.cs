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
        private List<Figure> _figures = new List<Figure>();
        private ToolTip _tooltip = new ToolTip();

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
            _tooltip.RemoveAll();
            GraphicsManager.ClearBuffered();
            foreach (Figure f in _figures)
            {
                f.Draw();

                //_tooltip.SetToolTip(f.CenterButton, f.number.ToString());
                f.textBox.Text = f.number.ToString();

                foreach (var b in f.Buttons)
                {
                    _tooltip.SetToolTip(b, b.Location.ToString());
                }
            }

            GraphicsManager.bufferedGraphics.Render();
        }

        public void DrawFigures(object sender, EventArgs e)
        {
            _tooltip.RemoveAll();
            GraphicsManager.ClearBuffered();
            foreach (Figure f in _figures)
            {
                f.Draw();

                //_tooltip.SetToolTip(f.CenterButton, f.number.ToString());
                f.textBox.Text = f.number.ToString();

                foreach (var b in f.Buttons)
                {
                    _tooltip.SetToolTip(b, b.Location.ToString());
                }
            }

            GraphicsManager.bufferedGraphics.Render();
        }

        private void OnClearButtonClick(object sender, EventArgs e)
        {
            GraphicsManager.ClearGraphics();
            _figures.Clear();
            pictureBox.Controls.Clear();
            textBox1.Clear();
            _tooltip.RemoveAll();
        }

        private void OnClearButtonClick()
        {
            GraphicsManager.ClearGraphics();
            _figures.Clear();
            pictureBox.Controls.Clear();
            textBox1.Clear();
            _tooltip.RemoveAll();
        }

        private bool _isUser;
        private Point _center;
        private List<Point> _points;
        private Point _centerDiff;
        private List<Point> _pointDiffs;

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox textBox) || !_isUser)
                return;

            _isUser = false;
            foreach (var f in _figures)
            {
                _center = new Point();
                _points = new List<Point>();

                try
                {
                    var str = textBox.Lines[f.Indexe];
                    var nums = str.Split(' ').Select(snum => int.Parse(snum)).ToArray();
                    
                    for (int i = 0; i < nums.Length; i += 2)
                    {
                        _center = new Point(nums[0], nums[1]);
                        if (_center.X > pictureBox.Width)
                        {
                            _center.X = f.CenterButton.Location.X;
                        }
                        else if (_center.Y > pictureBox.Height)
                        {
                            _center.Y = f.CenterButton.Location.Y;
                        }

                        if (i > 1)
                        {
                            _points.Add(new Point(nums[i], nums[i + 1]));
                            if (_points.Last().X > pictureBox.Width)
                            {
                                _points.Remove(_points.Last());
                                _points.Add(new Point(f.Buttons[_points.Count].Location.X, f.Buttons[_points.Count].Location.Y));
                            }
                            else if (_points.Last().Y > pictureBox.Height)
                            {
                                _points.Remove(_points.Last());
                                _points.Add(new Point(f.Buttons[_points.Count].Location.X, f.Buttons[_points.Count].Location.Y));
                            }
                        }
                    }
                }
                catch
                {
                    textBox.SetData(_figures);
                }

                try
                {
                    _centerDiff = new Point(_center.X - f.CenterButton.Location.X, _center.Y - f.CenterButton.Location.Y);
                _pointDiffs = new List<Point>();
                for (var k = 0; k < _points.Count; k++)
                {
                    _pointDiffs.Add(new Point(_points[k].X - f.Buttons[k].Location.X, _points[k].Y - f.Buttons[k].Location.Y));
                }
                }
                catch
                {
                    textBox.SetData(_figures);
                }

                if (_centerDiff.X != 0 || _centerDiff.Y != 0)
                {
                    f.CenterButton.Location = _center;
                    f.Update(_centerDiff, f.CenterButton);
                }

                for (var p = 0; p < _pointDiffs.Count; p++)
                {
                    if (_pointDiffs[p].X == 0 && _pointDiffs[p].Y == 0)
                        continue;

                    f.Buttons[p].Location = _points[p];
                    f.Update(_pointDiffs[p], f.Buttons[p]);
                }
            }

            textBox.SetData(_figures);
        }

        public void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;

            if (char.IsDigit(keyChar) || keyChar == '\b')
            {
                _isUser = true;
                return;
            }

            e.Handled = true;
        }

        private void SettingTextboxInitial(List<string> fileText)
        {
            if (fileText.Count == 0)
            {
                OnClearButtonClick();
                MessageBox.Show("The file does not fit!");
                return;
            }

            for (var i = 0; i < fileText.Count; i++)
            {
                if (fileText[i] != "Triangle" && fileText[i] != "Circle" && fileText[i] != "Pentagon" && fileText[i] != "Rect")
                {
                    OnClearButtonClick();
                    MessageBox.Show("The file does not fit!");
                    return;
                }

                if (fileText[i] == "Triangle")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 2; j < i + 2 + int.Parse(fileText[i + 1]); j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        for (int k = 0; k < nums.Length; k += 2)
                        {
                            center = new Point(nums[0], nums[1]);
                            if (k > 1)
                            {
                                points.Add(new Point(nums[k], nums[k + 1]));
                            }
                        }

                        var t = new Triangle();
                        t.SetData(center, points, pictureBox);
                        t.Updated += () => textBox1.SetData(_figures);
                        _figures.Add(t);
                        textBox1.SetData(_figures);

                        if (false == t.IsValidate(center, points))
                        {
                            OnClearButtonClick();
                            MessageBox.Show("The file does not fit!");
                            return;
                        }
                        points.Clear();
                    }
                    i += int.Parse(fileText[i + 1]) + 1;
                }

                if (fileText[i] == "Circle")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 2; j < i + 2 + int.Parse(fileText[i + 1]); j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        for (int k = 0; k < nums.Length; k += 2)
                        {
                            center = new Point(nums[0], nums[1]);
                            if (k > 1)
                            {
                                points.Add(new Point(nums[k], nums[k + 1]));
                            }
                        }

                        var c = new Circle();
                        c.SetData(center, points, pictureBox);
                        c.Updated += () => textBox1.SetData(_figures);
                        _figures.Add(c);
                        textBox1.SetData(_figures);

                        if (false == c.IsValidate(center, points))
                        {
                            OnClearButtonClick();
                            MessageBox.Show("The file does not fit!");
                            return;
                        }
                        points.Clear();
                    }
                    i += int.Parse(fileText[i + 1]) + 1;
                }

                if (fileText[i] == "Pentagon")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 2; j < i + 2 + int.Parse(fileText[i + 1]); j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        for (int k = 0; k < nums.Length; k += 2)
                        {
                            center = new Point(nums[0], nums[1]);
                            if (k > 1)
                            {
                                points.Add(new Point(nums[k], nums[k + 1]));
                            }
                        }

                        var p = new Pentagon();
                        p.SetData(center, points, pictureBox);
                        p.Updated += () => textBox1.SetData(_figures);
                        _figures.Add(p);
                        textBox1.SetData(_figures);

                        if (false == p.IsValidate(center, points))
                        {
                            OnClearButtonClick();
                            MessageBox.Show("The file does not fit!");
                            return;
                        }
                        points.Clear();
                    }
                    i += int.Parse(fileText[i + 1]) + 1;
                }

                if (fileText[i] == "Rect")
                {
                    var center = new Point();
                    var points = new List<Point>();

                    for (int j = i + 2; j < i + 2 + int.Parse(fileText[i + 1]); j++)
                    {
                        var nums = fileText[j].Split(' ').Select(snum => int.Parse(snum)).ToArray();
                        for (int k = 0; k < nums.Length; k += 2)
                        {
                            center = new Point(nums[0], nums[1]);
                            if (k > 1)
                            {
                                points.Add(new Point(nums[k], nums[k + 1]));
                            }
                        }

                        var r = new Rect();
                        r.SetData(center, points, pictureBox);
                        r.Updated += () => textBox1.SetData(_figures);
                        _figures.Add(r);
                        textBox1.SetData(_figures);

                        if (false == r.IsValidate(center, points))
                        {
                            OnClearButtonClick();
                            MessageBox.Show("The file does not fit!");
                            return;
                        }
                        points.Clear();
                    }
                    i += int.Parse(fileText[i + 1]) + 1;
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
                OnClearButtonClick();
                MessageBox.Show("The file does not fit!");
                return;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShapeEditor
{
    public static class TextBoxExtension
    {
        public static void SetData(this TextBox textBox, List<Figure> figures)
        {
            var strs = new List<string>();
            var i = 0;
            var d = 0;
            foreach(Figure f in figures)
            {
                var data = f.GetData();
                f.Indexes.Clear();
                d += data.Count;
                for (int j = i; j < d; j++)
                {
                    f.Indexes.Add(j);
                }
                i += data.Count;
                strs.AddRange(data);
            }
            textBox.Lines = strs.ToArray();
        }
    }
}

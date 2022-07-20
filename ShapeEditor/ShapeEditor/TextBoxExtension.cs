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
            foreach(Figure f in figures)
            {
                var data = f.GetData();
                strs.AddRange(data);
            }

            textBox.Lines = strs.ToArray();
        }
    }
}

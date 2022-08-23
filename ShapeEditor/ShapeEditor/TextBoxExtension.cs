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

            figures = figures.OrderBy(l => l.GetType().ToString()).ToList();

            var count = 0;
            var titleindex = 0;
            Type curTupe = null;
            string name = null;

            var i = 0;
            var d = 0;

            foreach (Figure f in figures)
            {
                if (curTupe != f.GetType())
                {
                    if (count != 0)
                    {
                        strs.Insert(titleindex, count.ToString());
                        strs.Insert(titleindex,name);
                    }

                    curTupe = f.GetType();
                    name = f.GetType().Name.ToString();

                    count = 0;
                    d += 2;
                    i += 2;
                    titleindex = strs.Count;
                }

                count++;

                f.number = count;

                var data = f.GetData();
                f.Indexe = 0;
                d += data.Count;
                for (int j = i; j < d; j++)
                {
                    f.Indexe = j;
                }
                i += data.Count;
                strs.AddRange(data);
            }

            if (count != 0)
            {
                strs.Insert(titleindex, count.ToString());
                strs.Insert(titleindex, name);
            }

            textBox.Lines = strs.ToArray();
        }
    }
}

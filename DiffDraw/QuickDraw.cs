using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace DiffDraw
{
    public class QuickDraw
    {
        private string _dataFolder = "D:/store/quickdraw/";
        private Dictionary<QuickDrawCategory, List<QuickDrawing>> _drawings = new Dictionary<QuickDrawCategory, List<QuickDrawing>>();
        private QuickDrawCategory _currentCategory;
        private int loadMax = 1000;

        public int CurrentIndex { get; set; }

        public QuickDraw()
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        public int Next()
        {
            CurrentIndex++;
            if (CurrentIndex >= _drawings[_currentCategory].Count)
            {
                CurrentIndex = 0;
            }
            return CurrentIndex;
        }
        public int Previous()
        {
            CurrentIndex--;
            if (CurrentIndex < 0)
            {
                CurrentIndex = _drawings[_currentCategory].Count - 1;
            }
            return CurrentIndex;
        }
        public bool IsCurrentRecognized() => _drawings[_currentCategory][CurrentIndex].Recognized;

        public void LoadCategory(QuickDrawCategory category)
        {
            _currentCategory = category;
            if (!_drawings.ContainsKey(category))
            {
                var result = new List<QuickDrawing>();
                string fileName = _dataFolder + QuickDrawing.CategoryToFileName(category);
                using (var stream = new StreamReader(fileName))
                {
                    string line;
                    int counter = 0;
                    while ((line = stream.ReadLine()) != null && counter < loadMax)
                    {
                        result.Add(JsonSerializer.Deserialize<QuickDrawing>(line));
                        counter++;
                    }
                }
                _drawings[category] = result;
            }
        }

        public void OnPaint(Graphics g)
        {
            var cat = _drawings[_currentCategory];

            var state = g.Save();
            float scale = 256f;
            g.ScaleTransform(scale, scale, System.Drawing.Drawing2D.MatrixOrder.Append);

            Pen p = new Pen(Color.Black, 1f / 256f);
            cat[CurrentIndex].OnPaint(g, p);
            g.Restore(state);
        }
    }

}

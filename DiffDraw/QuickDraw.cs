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
        private int _currentIndex = 0;

        public QuickDraw()
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        public int Next()
        {
            _currentIndex++;
            if (_currentIndex >= _drawings[_currentCategory].Count)
            {
                _currentIndex = 0;
            }
            return _currentIndex;
        }
        public int Previous()
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = _drawings[_currentCategory].Count - 1;
            }
            return _currentIndex;
        }
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
                    while ((line = stream.ReadLine()) != null && counter < 200)
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
            cat[_currentIndex].OnPaint(g, p);
            g.Restore(state);
        }
    }

    public struct Stroke
    {
        public QuickDrawing _parent;
        private int _index;
        public float[][] _stroke => _parent.Drawing[_index];

        public Stroke(QuickDrawing parent, int index)
        {
            _parent = parent;
            _index = index;
        }
        
        public PointF PointAt(int index) { return new PointF(_stroke[index][0], _stroke[index][0]); }
        public float XAt(int index) { return _stroke[index][0]; }
        public float YAt(int index) { return _stroke[index][1]; }
        public float TAt(int index) { return _stroke[index][2]; }
    }
    public class QuickDrawing
    {
        // {"word":"triangle","countrycode":"US","timestamp":"2017-03-06 17:09:47.79884 UTC","recognized":true,"key_id":"4676427809030144","drawing":
        //[[
        //[318,318,315,311,305,298,290,281,275,270,266,263,262,262,264,273,284,311,351,395,434,465,486,498,498,492,485,472,459,444,428,412,396,380,364,353,344,337,331,329],
        //[204,211,220,235,253,270,291,315,334,352,370,384,394,400,405,408,408,408,408,411,414,416,417,419,408,400,390,374,357,341,321,301,282,263,245,230,219,209,204,200],
        //[0,35,51,68,85,103,120,138,155,172,189,207,224,242,276,310,327,344,361,378,396,413,430,447,586,603,620,637,655,672,689,707,724,741,759,776,794,811,828,1032]
        //]]
        //}

        [JsonIgnore]
        public QuickDrawCategory Category { get; set; }

        [JsonPropertyName("word")]
        public string Word
        {
            get => CategoryToName(Category);
            set
            {
                Category = NameToCategory(value);
            }
        }

        [JsonPropertyName("countrycode")]
        public string CountryCode { get; set; }
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        [JsonPropertyName("recognized")]
        public bool Recognized { get; set; }
        private float[][][] _drawing;
        [JsonPropertyName("drawing")]
        public float[][][] Drawing
        {
            get
            {
                return _drawing;
            }
            set
            {
                _drawing = value;
                Normalize(_drawing);
            }
        }

        private void Normalize(float[][][] data)
        {
            if (data == null) return;
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float temp;
            foreach (var stroke in data)
            {
                temp = stroke[0].Min();
                minX = temp < minX ? temp : minX;
                temp = stroke[1].Min();
                minY = temp < minY ? temp : minY;
                temp = stroke[0].Max();
                maxX = temp > maxX ? temp : maxX;
                temp = stroke[1].Max();
                maxY = temp > maxY ? temp : maxY;
            }

            float size = 1f;
            float scale;
            float xOffset;
            float yOffset;
            var difX = maxX - minX;
            var difY = maxY - minY;
            var isVert = difY > difX;
            var nudge = .1f * size;
            if (isVert)
            {
                scale = (size / difY) * 0.8f;
                yOffset = nudge;
                xOffset = ((difY - difX) * scale) / 2f + nudge; //((1f - (difX / difY)) * size) / 2f;
            }
            else
            {
                scale = (size / difX) * 0.8f;
                xOffset = nudge;
                yOffset = ((difX - difY) * scale) / 2f + nudge;
            }
            foreach (var stroke in Drawing)
            {
                float startT = stroke[2][0];
                float maxT = stroke[2][stroke[2].Length - 1] - startT;
                for (int i = 0; i < stroke[0].Length; i++)
                {
                    stroke[0][i] = (stroke[0][i] - minX) * scale + xOffset;
                    stroke[1][i] = (stroke[1][i] - minY) * scale + yOffset;
                    stroke[2][i] = (stroke[2][i] - startT) / maxT;
                }
            }
        }

        public void OnPaint(Graphics g, Pen pen)
        {
            Pen p;
            float penWidth = pen.Width * 3f;
            foreach (var stroke in Drawing)
            {
                for (int i = 1; i < stroke[0].Length; i++)
                {
                    float t = stroke[2][i];
                    p = new Pen(Color.FromArgb(255, (int)(t * 255f), (int)((1f - t) * 255f), 0), penWidth);
                    g.DrawLine(p, stroke[0][i - 1], stroke[1][i - 1], stroke[0][i], stroke[1][i]);
                }
            }
        }

        public override string ToString()
        {
            return Word + ":" + Drawing;
        }

        public static string CategoryToName(QuickDrawCategory category)
        {
            string result = CategoryNames[(int)category];

            //switch (category)
            //{
            //    case QuickDrawCategory.Circle:
            //        result = "circle";
            //        break;
            //    case QuickDrawCategory.Donut:
            //        result = "donut";
            //        break;
            //    case QuickDrawCategory.Eye:
            //        result = "eye";
            //        break;
            //    case QuickDrawCategory.HockeyPuck:
            //        result = "hockey puck";
            //        break;
            //    case QuickDrawCategory.Square:
            //        result = "square";
            //        break;
            //    case QuickDrawCategory.Triangle:
            //        result = "triangle";
            //        break;
            //}
            return result;
        }

        public static string CategoryToFileName(QuickDrawCategory category, bool isRaw = true)
        {
            string result = isRaw ? "full_raw_" : "";
            return result + CategoryToName(category) + ".ndjson";
        }

        public static QuickDrawCategory NameToCategory(string name)
        {
            QuickDrawCategory result = QuickDrawCategory.Circle;
            int index = Array.IndexOf(CategoryNames, name.ToLowerInvariant());
            if (index > -1)
            {
                result = (QuickDrawCategory)index;
            }
            //switch (name.ToLowerInvariant())
            //{
            //    case "circle":
            //        result = QuickDrawCategory.Circle;
            //        break;
            //    case "donut":
            //        result = QuickDrawCategory.Donut;
            //        break;
            //    case "eye":
            //        result = QuickDrawCategory.Eye;
            //        break;
            //    case "hockey puck":
            //        result = QuickDrawCategory.HockeyPuck;
            //        break;
            //    case "square":
            //        result = QuickDrawCategory.Square;
            //        break;
            //    case "triangle":
            //        result = QuickDrawCategory.Triangle;
            //        break;
            //}
            return result;
        }
        public static string[] CategoryNames = new string[] { "circle", "donut", "eye", "hockey puck", "square", "triangle" };
    }
    public enum QuickDrawCategory
    {
        Circle,
        Donut,
        Eye,
        HockeyPuck,
        Square,
        Triangle,
    }

}

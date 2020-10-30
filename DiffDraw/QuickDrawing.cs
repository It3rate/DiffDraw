using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiffDraw
{
    public class QuickDrawing
    {

        //public static QuickDrawing Deserialize(BinaryReader reader)
        //{
        //   var result =  new QuickDrawing((QuickDrawCategory)reader.ReadInt16(), reader.ReadString())
        //    formatter.Serialize(stream, Category);
        //    formatter.Serialize(stream, Recognized);
        //    formatter.Serialize(stream, CountryCode);
        //    formatter.Serialize(stream, Drawing);
        //}

        //public void Serialize(BinaryWriter writer)
        //{
        //    formatter.Serialize(stream, Category);
        //    formatter.Serialize(stream, Recognized);
        //    formatter.Serialize(stream, CountryCode);
        //    formatter.Serialize(stream, Drawing);
        //}

        public QuickDrawing()
        {
        }
        public QuickDrawing(QuickDrawCategory category, string countryCode, bool recognized, float[][][] drawing)
        {
            Category = category;
            CountryCode = countryCode;
            Recognized = recognized;
           // _drawing = drawing;
        }


        // {"word":"triangle",
        //  "countrycode":"US",
        //  "timestamp":"2017-03-06 17:09:47.79884 UTC",
        //  "recognized":true,
        //  "key_id":"4676427809030144",
        //  "drawing": [Strokes  [  X    [318,318,...331,329],
        //                          Y    [204, 211, ...204, 200],
        //                          Time [0, 35, 51, ...828, 1032]]  ]}

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

        [JsonIgnore]
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("recognized")]
        public bool Recognized { get; set; }

        private List<QuickStroke> Strokes = new List<QuickStroke>();
        //private float[][][] _drawing;
        private static float[][][] _empty;
        [JsonPropertyName("drawing")]
        public float[][][] Drawing
        {
            get
            {
                return _empty;
            }
            set
            {
                //_drawing = value;
                ProcessDrawing(value);
            }
        }

        private void ProcessDrawing(float[][][] data)
        {
            if (data == null) return;
            Strokes.Clear();

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
                xOffset = ((difY - difX) * scale) / 2f + nudge;
            }
            else
            {
                scale = (size / difX) * 0.8f;
                xOffset = nudge;
                yOffset = ((difX - difY) * scale) / 2f + nudge;
            }

            for (int strokeIndex = 0; strokeIndex < data.Length; strokeIndex++)
            {
                var stroke = data[strokeIndex];
                float startT = stroke[2][0];
                float maxT = stroke[2][stroke[2].Length - 1] - startT;
                for (int i = 0; i < stroke[0].Length; i++)
                {
                    stroke[0][i] = (stroke[0][i] - minX) * scale + xOffset;
                    stroke[1][i] = (stroke[1][i] - minY) * scale + yOffset;
                    stroke[2][i] = (stroke[2][i] - startT) / maxT;
                }
                Strokes.Add(new QuickStroke(strokeIndex, stroke[0], stroke[1], stroke[2], startT, maxT));
            }
        }

        public void OnPaint(Graphics g, Pen pen)
        {
            Pen p;
            float penWidth = pen.Width * 3f;
            foreach (var stroke in Strokes)
            {
                for (int i = 1; i < stroke.Length; i++)
                {
                    var t = stroke.Times[i];
                    p = new Pen(Color.FromArgb(255, (int)(t * 255f), (int)((1f - t) * 255f), 0), penWidth);
                    g.DrawLine(p, stroke[i - 1], stroke[i]);
                }
            }
        }

        public override string ToString()
        {
            return Word + ":" + Drawing;
        }

        public static string[] CategoryNames = new string[] { "circle", "donut", "eye", "hockey puck", "square", "triangle" };

        public static string CategoryToName(QuickDrawCategory category)
        {
            return CategoryNames[(int)category];
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
            return result;
        }
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

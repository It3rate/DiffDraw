using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDraw
{
    public class QuickStroke
    {
        private int Index { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public PointF[] Points { get; set; } 
        public float[] Times { get; set; } //normalized 0-1 over stroke

        public QuickStroke(int index, float[] xs, float[] ys, float[] times, float startTime = 0, float endTime = 0)
        {
            Index = index;
            StartTime = startTime;
            EndTime = endTime;

            Debug.Assert(xs.Length == ys.Length && ys.Length == times.Length);
            int len = xs.Length;

            Points = new PointF[len];
            Times = new float[len];
            Array.Copy(times, Times, len);
            for (int i = 0; i < len; i++)
            {
                Points[i] = new PointF(xs[i], ys[i]);
            }
        }

        public int Length => Points.Length;
        public PointF this[int index] => Points[index];

        public void OnPaint(Graphics g, Pen pen)
        {
            g.DrawLines(pen, Points);
        }

    }

}

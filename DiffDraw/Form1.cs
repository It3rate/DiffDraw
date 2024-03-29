﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiffDraw
{
    public partial class Form1 : Form
    {
        QuickDraw _quickDraw;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _quickDraw = new QuickDraw();
            comboBox1.Items.AddRange(QuickDrawing.CategoryNames);
            LoadCategory(QuickDrawCategory.HockeyPuck);
            panel1.Paint += Panel1_Paint;
        }

        public void LoadCategory(QuickDrawCategory category)
        {
            comboBox1.SelectedIndex = (int)category;
            _quickDraw.LoadCategory(category);
            Update();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            _quickDraw.OnPaint(g);
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            _quickDraw.Next();
            Update();
        }
        private void btPrevious_Click(object sender, EventArgs e)
        {
            _quickDraw.Previous();
            Update();
        }
        public void Update()
        {
            lbIndex.Text = _quickDraw.CurrentIndex.ToString();
            if (_quickDraw.IsCurrentRecognized())
            {
                lbRecognized.Text = "✓ Recognized";
                lbRecognized.ForeColor = Color.Green;
            }
            else
            {
                lbRecognized.Text = "X Unrecognized";
                lbRecognized.ForeColor = Color.Red;
            }
            panel1.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCategory((QuickDrawCategory)comboBox1.SelectedIndex);
            Update();
            panel1.Invalidate();
        }

    }
}

namespace DiffDraw
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btNext = new System.Windows.Forms.Button();
            this.lbIndex = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btPrevious = new System.Windows.Forms.Button();
            this.lbRecognized = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.Location = new System.Drawing.Point(48, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 400);
            this.panel1.TabIndex = 0;
            // 
            // btNext
            // 
            this.btNext.Location = new System.Drawing.Point(348, 471);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(100, 31);
            this.btNext.TabIndex = 1;
            this.btNext.Text = "Next";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // label1
            // 
            this.lbIndex.Location = new System.Drawing.Point(348, 45);
            this.lbIndex.Name = "label1";
            this.lbIndex.Size = new System.Drawing.Size(100, 20);
            this.lbIndex.TabIndex = 2;
            this.lbIndex.Text = "0";
            this.lbIndex.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(48, 31);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(193, 28);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btPrevious
            // 
            this.btPrevious.Location = new System.Drawing.Point(48, 471);
            this.btPrevious.Name = "btPrevious";
            this.btPrevious.Size = new System.Drawing.Size(100, 31);
            this.btPrevious.TabIndex = 1;
            this.btPrevious.Text = "Previous";
            this.btPrevious.UseVisualStyleBackColor = true;
            this.btPrevious.Click += new System.EventHandler(this.btPrevious_Click);
            // 
            // label2
            // 
            this.lbRecognized.Location = new System.Drawing.Point(176, 476);
            this.lbRecognized.Name = "label2";
            this.lbRecognized.Size = new System.Drawing.Size(146, 26);
            this.lbRecognized.TabIndex = 4;
            this.lbRecognized.Text = "_ Recognized";
            this.lbRecognized.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 554);
            this.Controls.Add(this.lbRecognized);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lbIndex);
            this.Controls.Add(this.btPrevious);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Label lbIndex;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btPrevious;
        private System.Windows.Forms.Label lbRecognized;
    }
}


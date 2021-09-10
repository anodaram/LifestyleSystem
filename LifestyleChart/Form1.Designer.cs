namespace LifestyleChart
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
            this.components = new System.ComponentModel.Container();
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_draw_price = new System.Windows.Forms.Button();
            this.btn_draw_pnt = new System.Windows.Forms.Button();
            this.btn_draw_ind = new System.Windows.Forms.Button();
            this.cmb_price = new System.Windows.Forms.ComboBox();
            this.cmb_pnt = new System.Windows.Forms.ComboBox();
            this.cmb_ind = new System.Windows.Forms.ComboBox();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.ucSciStockChart1 = new WpfSciStockChart.ucSciStockChart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_clear = new System.Windows.Forms.Button();
            this.txt_bug = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(12, 12);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_draw_price
            // 
            this.btn_draw_price.Location = new System.Drawing.Point(320, 10);
            this.btn_draw_price.Name = "btn_draw_price";
            this.btn_draw_price.Size = new System.Drawing.Size(75, 23);
            this.btn_draw_price.TabIndex = 1;
            this.btn_draw_price.Text = "Draw";
            this.btn_draw_price.UseVisualStyleBackColor = true;
            this.btn_draw_price.Click += new System.EventHandler(this.btn_draw_price_Click);
            // 
            // btn_draw_pnt
            // 
            this.btn_draw_pnt.Location = new System.Drawing.Point(636, 10);
            this.btn_draw_pnt.Name = "btn_draw_pnt";
            this.btn_draw_pnt.Size = new System.Drawing.Size(75, 23);
            this.btn_draw_pnt.TabIndex = 2;
            this.btn_draw_pnt.Text = "Draw";
            this.btn_draw_pnt.UseVisualStyleBackColor = true;
            this.btn_draw_pnt.Click += new System.EventHandler(this.btn_draw_pnt_Click);
            // 
            // btn_draw_ind
            // 
            this.btn_draw_ind.Location = new System.Drawing.Point(949, 12);
            this.btn_draw_ind.Name = "btn_draw_ind";
            this.btn_draw_ind.Size = new System.Drawing.Size(75, 23);
            this.btn_draw_ind.TabIndex = 3;
            this.btn_draw_ind.Text = "Draw";
            this.btn_draw_ind.UseVisualStyleBackColor = true;
            this.btn_draw_ind.Click += new System.EventHandler(this.btn_draw_ind_Click);
            // 
            // cmb_price
            // 
            this.cmb_price.FormattingEnabled = true;
            this.cmb_price.Location = new System.Drawing.Point(125, 12);
            this.cmb_price.Name = "cmb_price";
            this.cmb_price.Size = new System.Drawing.Size(189, 21);
            this.cmb_price.TabIndex = 4;
            // 
            // cmb_pnt
            // 
            this.cmb_pnt.FormattingEnabled = true;
            this.cmb_pnt.Location = new System.Drawing.Point(435, 12);
            this.cmb_pnt.Name = "cmb_pnt";
            this.cmb_pnt.Size = new System.Drawing.Size(195, 21);
            this.cmb_pnt.TabIndex = 5;
            // 
            // cmb_ind
            // 
            this.cmb_ind.FormattingEnabled = true;
            this.cmb_ind.Location = new System.Drawing.Point(751, 14);
            this.cmb_ind.Name = "cmb_ind";
            this.cmb_ind.Size = new System.Drawing.Size(192, 21);
            this.cmb_ind.TabIndex = 6;
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(20, 41);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1123, 524);
            this.elementHost1.TabIndex = 7;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.ucSciStockChart1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(1068, 12);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 8;
            this.btn_clear.Text = "Clear Cache";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // txt_bug
            // 
            this.txt_bug.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_bug.Location = new System.Drawing.Point(20, 571);
            this.txt_bug.Name = "txt_bug";
            this.txt_bug.ReadOnly = true;
            this.txt_bug.Size = new System.Drawing.Size(100, 13);
            this.txt_bug.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 597);
            this.Controls.Add(this.txt_bug);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.cmb_ind);
            this.Controls.Add(this.cmb_pnt);
            this.Controls.Add(this.cmb_price);
            this.Controls.Add(this.btn_draw_ind);
            this.Controls.Add(this.btn_draw_pnt);
            this.Controls.Add(this.btn_draw_price);
            this.Controls.Add(this.btn_connect);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "LifestyleChart";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_draw_price;
        private System.Windows.Forms.Button btn_draw_pnt;
        private System.Windows.Forms.Button btn_draw_ind;
        private System.Windows.Forms.ComboBox cmb_price;
        private System.Windows.Forms.ComboBox cmb_pnt;
        private System.Windows.Forms.ComboBox cmb_ind;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private WpfSciStockChart.ucSciStockChart ucSciStockChart1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.TextBox txt_bug;
    }
}


namespace XMLtoXLSXcvt
{
    partial class PropertiesForm
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ICWRNumeric = new System.Windows.Forms.NumericUpDown();
            this.CRRNumeric = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ICWRNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CRRNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Image column width ratio";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Column row ratio";
            // 
            // ICWRNumeric
            // 
            this.ICWRNumeric.DecimalPlaces = 6;
            this.ICWRNumeric.Location = new System.Drawing.Point(15, 64);
            this.ICWRNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ICWRNumeric.Name = "ICWRNumeric";
            this.ICWRNumeric.Size = new System.Drawing.Size(120, 20);
            this.ICWRNumeric.TabIndex = 26;
            this.ICWRNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ICWRNumeric.ValueChanged += new System.EventHandler(this.ICWRNumeric_ValueChanged);
            // 
            // CRRNumeric
            // 
            this.CRRNumeric.DecimalPlaces = 6;
            this.CRRNumeric.Location = new System.Drawing.Point(15, 25);
            this.CRRNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CRRNumeric.Name = "CRRNumeric";
            this.CRRNumeric.Size = new System.Drawing.Size(120, 20);
            this.CRRNumeric.TabIndex = 25;
            this.CRRNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.CRRNumeric.ValueChanged += new System.EventHandler(this.CRRNumeric_ValueChanged);
            // 
            // PropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ICWRNumeric);
            this.Controls.Add(this.CRRNumeric);
            this.Name = "PropertiesForm";
            this.Text = "PropertiesForm";
            ((System.ComponentModel.ISupportInitialize)(this.ICWRNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CRRNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown ICWRNumeric;
        private System.Windows.Forms.NumericUpDown CRRNumeric;
    }
}
namespace OnScreenProtractor
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
            this.AngleLabel = new System.Windows.Forms.Label();
            this.QuickMessureBtn = new System.Windows.Forms.Button();
            this.PrecisionMessureBtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ConfrimBtn = new System.Windows.Forms.Button();
            this.AbortBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AngleLabel
            // 
            this.AngleLabel.AutoSize = true;
            this.AngleLabel.Location = new System.Drawing.Point(91, 70);
            this.AngleLabel.Name = "AngleLabel";
            this.AngleLabel.Size = new System.Drawing.Size(105, 13);
            this.AngleLabel.TabIndex = 0;
            this.AngleLabel.Text = "Pick A Measurement";
            // 
            // QuickMessureBtn
            // 
            this.QuickMessureBtn.Location = new System.Drawing.Point(25, 141);
            this.QuickMessureBtn.Name = "QuickMessureBtn";
            this.QuickMessureBtn.Size = new System.Drawing.Size(108, 23);
            this.QuickMessureBtn.TabIndex = 1;
            this.QuickMessureBtn.Text = "Quick Messure";
            this.QuickMessureBtn.UseVisualStyleBackColor = true;
            this.QuickMessureBtn.Click += new System.EventHandler(this.QuickMessureBtn_Click);
            // 
            // PrecisionMessureBtn
            // 
            this.PrecisionMessureBtn.Location = new System.Drawing.Point(154, 141);
            this.PrecisionMessureBtn.Name = "PrecisionMessureBtn";
            this.PrecisionMessureBtn.Size = new System.Drawing.Size(108, 23);
            this.PrecisionMessureBtn.TabIndex = 2;
            this.PrecisionMessureBtn.Text = "Precision Messure";
            this.PrecisionMessureBtn.UseVisualStyleBackColor = true;
            this.PrecisionMessureBtn.Click += new System.EventHandler(this.PrecisionMessureBtn_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // ConfrimBtn
            // 
            this.ConfrimBtn.Location = new System.Drawing.Point(10, 181);
            this.ConfrimBtn.Name = "ConfrimBtn";
            this.ConfrimBtn.Size = new System.Drawing.Size(123, 23);
            this.ConfrimBtn.TabIndex = 3;
            this.ConfrimBtn.Text = "Confim Measurement";
            this.ConfrimBtn.UseVisualStyleBackColor = true;
            this.ConfrimBtn.Click += new System.EventHandler(this.ConfrimBtn_Click);
            // 
            // AbortBtn
            // 
            this.AbortBtn.Location = new System.Drawing.Point(154, 181);
            this.AbortBtn.Name = "AbortBtn";
            this.AbortBtn.Size = new System.Drawing.Size(123, 23);
            this.AbortBtn.TabIndex = 4;
            this.AbortBtn.Text = "Abort Measurement";
            this.AbortBtn.UseVisualStyleBackColor = true;
            this.AbortBtn.Click += new System.EventHandler(this.AbortBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.AbortBtn);
            this.Controls.Add(this.ConfrimBtn);
            this.Controls.Add(this.PrecisionMessureBtn);
            this.Controls.Add(this.QuickMessureBtn);
            this.Controls.Add(this.AngleLabel);
            this.Name = "Form1";
            this.Text = "KK\'s Protractor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AngleLabel;
        private System.Windows.Forms.Button QuickMessureBtn;
        private System.Windows.Forms.Button PrecisionMessureBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button ConfrimBtn;
        private System.Windows.Forms.Button AbortBtn;
    }
}


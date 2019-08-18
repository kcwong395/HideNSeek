namespace HideNSeek
{
    partial class HideNSeek
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
            this.open = new System.Windows.Forms.Button();
            this.hide = new System.Windows.Forms.Button();
            this.seek = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Open
            // 
            this.open.Location = new System.Drawing.Point(480, 50);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(120, 40);
            this.open.TabIndex = 0;
            this.open.Text = "Open";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.Open_Click);
            // 
            // Hide
            // 
            this.hide.Location = new System.Drawing.Point(480, 155);
            this.hide.Name = "hide";
            this.hide.Size = new System.Drawing.Size(120, 40);
            this.hide.TabIndex = 1;
            this.hide.Text = "Hide";
            this.hide.UseVisualStyleBackColor = true;
            this.hide.Click += new System.EventHandler(this.Hide_Click);
            // 
            // Seek
            // 
            this.seek.Location = new System.Drawing.Point(480, 260);
            this.seek.Name = "seek";
            this.seek.Size = new System.Drawing.Size(120, 40);
            this.seek.TabIndex = 2;
            this.seek.Text = "Seek";
            this.seek.UseVisualStyleBackColor = true;
            this.seek.Click += new System.EventHandler(this.Seek_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(50, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(380, 250);
            this.textBox1.TabIndex = 3;
            // 
            // Status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(50, 350);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(380, 15);
            this.status.TabIndex = 4;
            this.status.Text = "Waiting for input...";
            // 
            // HideNSeek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 400);
            this.Controls.Add(this.status);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.seek);
            this.Controls.Add(this.hide);
            this.Controls.Add(this.open);
            this.Name = "HideNSeek";
            this.Text = "HideNSeek";
            this.Load += new System.EventHandler(this.HideNSeek_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button open;
        private System.Windows.Forms.Button hide;
        private System.Windows.Forms.Button seek;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label status;
    }
}
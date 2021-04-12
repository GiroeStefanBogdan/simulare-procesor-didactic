namespace Nume_proiect
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
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.ASMShowTextBox = new System.Windows.Forms.TextBox();
            this.ResultTextBox = new System.Windows.Forms.TextBox();
            this.ConvertBtn = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.BackColor = System.Drawing.Color.AliceBlue;
            this.FileNameTextBox.Location = new System.Drawing.Point(13, 40);
            this.FileNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FileNameTextBox.Multiline = true;
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(766, 46);
            this.FileNameTextBox.TabIndex = 4;
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseBtn.Location = new System.Drawing.Point(869, 38);
            this.BrowseBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(350, 48);
            this.BrowseBtn.TabIndex = 5;
            this.BrowseBtn.Text = "Browse ASM  File";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // ASMShowTextBox
            // 
            this.ASMShowTextBox.BackColor = System.Drawing.Color.SkyBlue;
            this.ASMShowTextBox.Location = new System.Drawing.Point(13, 154);
            this.ASMShowTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ASMShowTextBox.Multiline = true;
            this.ASMShowTextBox.Name = "ASMShowTextBox";
            this.ASMShowTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ASMShowTextBox.Size = new System.Drawing.Size(514, 481);
            this.ASMShowTextBox.TabIndex = 7;
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.BackColor = System.Drawing.Color.SkyBlue;
            this.ResultTextBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ResultTextBox.Location = new System.Drawing.Point(747, 154);
            this.ResultTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ResultTextBox.Multiline = true;
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResultTextBox.Size = new System.Drawing.Size(514, 481);
            this.ResultTextBox.TabIndex = 8;
            // 
            // ConvertBtn
            // 
            this.ConvertBtn.Enabled = false;
            this.ConvertBtn.Location = new System.Drawing.Point(82, 96);
            this.ConvertBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConvertBtn.Name = "ConvertBtn";
            this.ConvertBtn.Size = new System.Drawing.Size(384, 51);
            this.ConvertBtn.TabIndex = 9;
            this.ConvertBtn.Text = "Convert";
            this.ConvertBtn.UseVisualStyleBackColor = true;
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Location = new System.Drawing.Point(830, 94);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(406, 51);
            this.ExitButton.TabIndex = 10;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 665);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ConvertBtn);
            this.Controls.Add(this.ResultTextBox);
            this.Controls.Add(this.ASMShowTextBox);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.FileNameTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Button BrowseBtn;
        public System.Windows.Forms.TextBox ASMShowTextBox;
        public System.Windows.Forms.TextBox ResultTextBox;
        public System.Windows.Forms.Button ConvertBtn;
        private System.Windows.Forms.Button ExitButton;
    }
}


namespace CreateDB
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
            this.LblMsg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CmbServers = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.BtnCreateDb = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtFilePath = new System.Windows.Forms.TextBox();
            this.BtnBrows = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblMsg
            // 
            this.LblMsg.Location = new System.Drawing.Point(145, 124);
            this.LblMsg.Name = "LblMsg";
            this.LblMsg.Size = new System.Drawing.Size(300, 20);
            this.LblMsg.TabIndex = 15;
            this.LblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LblMsg.Visible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(60, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 26);
            this.label1.TabIndex = 14;
            this.label1.Text = "Server";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CmbServers
            // 
            this.CmbServers.FormattingEnabled = true;
            this.CmbServers.Location = new System.Drawing.Point(145, 24);
            this.CmbServers.Name = "CmbServers";
            this.CmbServers.Size = new System.Drawing.Size(330, 26);
            this.CmbServers.TabIndex = 13;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(145, 144);
            this.progressBar1.MarqueeAnimationSpeed = 20;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(300, 10);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Visible = false;
            // 
            // BtnCreateDb
            // 
            this.BtnCreateDb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCreateDb.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCreateDb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnCreateDb.Location = new System.Drawing.Point(145, 90);
            this.BtnCreateDb.Name = "BtnCreateDb";
            this.BtnCreateDb.Size = new System.Drawing.Size(130, 30);
            this.BtnCreateDb.TabIndex = 11;
            this.BtnCreateDb.Text = "Create Database";
            this.BtnCreateDb.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(60, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 26);
            this.label2.TabIndex = 10;
            this.label2.Text = "File path";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtFilePath
            // 
            this.TxtFilePath.Location = new System.Drawing.Point(145, 57);
            this.TxtFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.TxtFilePath.Name = "TxtFilePath";
            this.TxtFilePath.Size = new System.Drawing.Size(300, 26);
            this.TxtFilePath.TabIndex = 9;
            // 
            // BtnBrows
            // 
            this.BtnBrows.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BtnBrows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBrows.Location = new System.Drawing.Point(445, 57);
            this.BtnBrows.Name = "BtnBrows";
            this.BtnBrows.Size = new System.Drawing.Size(30, 26);
            this.BtnBrows.TabIndex = 16;
            this.BtnBrows.Text = "...";
            this.BtnBrows.UseVisualStyleBackColor = true;
            // 
            // BtnClose
            // 
            this.BtnClose.BackColor = System.Drawing.Color.Red;
            this.BtnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClose.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClose.ForeColor = System.Drawing.Color.White;
            this.BtnClose.Location = new System.Drawing.Point(281, 90);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(70, 30);
            this.BtnClose.TabIndex = 17;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(534, 161);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnBrows);
            this.Controls.Add(this.LblMsg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CmbServers);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.BtnCreateDb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtFilePath);
            this.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create DB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CmbServers;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button BtnCreateDb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtFilePath;
        private System.Windows.Forms.Button BtnBrows;
        private System.Windows.Forms.Button BtnClose;
    }
}


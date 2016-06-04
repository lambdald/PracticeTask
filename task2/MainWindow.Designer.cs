namespace task2
{
    partial class MainWindow
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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnFontDialog = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.datetimePicker = new System.Windows.Forms.DateTimePicker();
            this.picBoxDatetime = new System.Windows.Forms.PictureBox();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.MainPanel.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDatetime)).BeginInit();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ControlPanel);
            this.MainPanel.Controls.Add(this.picBoxDatetime);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(949, 573);
            this.MainPanel.TabIndex = 1;
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.button1);
            this.ControlPanel.Controls.Add(this.btnFontDialog);
            this.ControlPanel.Controls.Add(this.btnModify);
            this.ControlPanel.Controls.Add(this.label2);
            this.ControlPanel.Controls.Add(this.datetimePicker);
            this.ControlPanel.Location = new System.Drawing.Point(13, 410);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(924, 151);
            this.ControlPanel.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(553, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "选择背景";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnFontDialog
            // 
            this.btnFontDialog.Location = new System.Drawing.Point(330, 35);
            this.btnFontDialog.Name = "btnFontDialog";
            this.btnFontDialog.Size = new System.Drawing.Size(75, 23);
            this.btnFontDialog.TabIndex = 5;
            this.btnFontDialog.Text = "设置字体";
            this.btnFontDialog.UseVisualStyleBackColor = true;
            this.btnFontDialog.Click += new System.EventHandler(this.btnFontDialog_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(3, 80);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(107, 24);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "确定";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(-1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "时间设置";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // datetimePicker
            // 
            this.datetimePicker.Cursor = System.Windows.Forms.Cursors.Default;
            this.datetimePicker.CustomFormat = "yyyy年MM月dd日 dddd HH:mm:ss";
            this.datetimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datetimePicker.Location = new System.Drawing.Point(-1, 34);
            this.datetimePicker.Name = "datetimePicker";
            this.datetimePicker.Size = new System.Drawing.Size(218, 21);
            this.datetimePicker.TabIndex = 0;
            // 
            // picBoxDatetime
            // 
            this.picBoxDatetime.Location = new System.Drawing.Point(12, 12);
            this.picBoxDatetime.Name = "picBoxDatetime";
            this.picBoxDatetime.Size = new System.Drawing.Size(925, 392);
            this.picBoxDatetime.TabIndex = 0;
            this.picBoxDatetime.TabStop = false;
            this.picBoxDatetime.Paint += new System.Windows.Forms.PaintEventHandler(this.picBoxDatetime_Paint);
            // 
            // fontDialog
            // 
            this.fontDialog.ShowColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Image Files|*.PNG;*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.UpdateTime_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(949, 573);
            this.Controls.Add(this.MainPanel);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.MainPanel.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDatetime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.DateTimePicker datetimePicker;
        private System.Windows.Forms.PictureBox picBoxDatetime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFontDialog;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer timer;

    }
}
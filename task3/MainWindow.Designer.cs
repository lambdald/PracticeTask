namespace task3
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
            this.listViewFolder = new System.Windows.Forms.ListView();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.chooseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.panel = new System.Windows.Forms.Panel();
            this.radioBtnType = new System.Windows.Forms.RadioButton();
            this.radioBtnTime = new System.Windows.Forms.RadioButton();
            this.label = new System.Windows.Forms.Label();
            this.folder = new System.Windows.Forms.Label();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewFolder
            // 
            this.listViewFolder.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listViewFolder.Location = new System.Drawing.Point(12, 28);
            this.listViewFolder.Name = "listViewFolder";
            this.listViewFolder.Size = new System.Drawing.Size(447, 674);
            this.listViewFolder.TabIndex = 0;
            this.listViewFolder.UseCompatibleStateImageBehavior = false;
            this.listViewFolder.View = System.Windows.Forms.View.Details;
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Location = new System.Drawing.Point(522, 79);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(86, 27);
            this.btnChooseFolder.TabIndex = 1;
            this.btnChooseFolder.Text = "选择目录";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.radioBtnType);
            this.panel.Controls.Add(this.radioBtnTime);
            this.panel.Controls.Add(this.label);
            this.panel.Location = new System.Drawing.Point(509, 124);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(113, 99);
            this.panel.TabIndex = 2;
            // 
            // radioBtnType
            // 
            this.radioBtnType.AutoSize = true;
            this.radioBtnType.Location = new System.Drawing.Point(3, 69);
            this.radioBtnType.Name = "radioBtnType";
            this.radioBtnType.Size = new System.Drawing.Size(107, 16);
            this.radioBtnType.TabIndex = 2;
            this.radioBtnType.Text = "按文件类型分组";
            this.radioBtnType.UseVisualStyleBackColor = true;
            this.radioBtnType.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnTime
            // 
            this.radioBtnTime.AutoSize = true;
            this.radioBtnTime.Location = new System.Drawing.Point(3, 28);
            this.radioBtnTime.Name = "radioBtnTime";
            this.radioBtnTime.Size = new System.Drawing.Size(83, 16);
            this.radioBtnTime.TabIndex = 1;
            this.radioBtnTime.Text = "按时间排序";
            this.radioBtnTime.UseVisualStyleBackColor = true;
            this.radioBtnTime.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(1, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(77, 12);
            this.label.TabIndex = 0;
            this.label.Text = "选择分组方式";
            // 
            // folder
            // 
            this.folder.AutoSize = true;
            this.folder.Location = new System.Drawing.Point(35, 9);
            this.folder.Name = "folder";
            this.folder.Size = new System.Drawing.Size(0, 12);
            this.folder.TabIndex = 3;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 714);
            this.Controls.Add(this.folder);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.btnChooseFolder);
            this.Controls.Add(this.listViewFolder);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewFolder;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.FolderBrowserDialog chooseFolder;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.RadioButton radioBtnTime;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.RadioButton radioBtnType;
        private System.Windows.Forms.Label folder;
    }
}
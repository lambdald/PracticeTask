namespace task4Server
{
    partial class Server
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
            this.panelIP = new System.Windows.Forms.Panel();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.panelPort = new System.Windows.Forms.Panel();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.panelServerName = new System.Windows.Forms.Panel();
            this.textBoxServerName = new System.Windows.Forms.TextBox();
            this.labelServerName = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.listViewClient = new System.Windows.Forms.ListView();
            this.listViewChat = new System.Windows.Forms.ListView();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.btnSingle = new System.Windows.Forms.Button();
            this.btnGroup = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panelButton = new System.Windows.Forms.Panel();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnCloseServer = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panelIP.SuspendLayout();
            this.panelPort.SuspendLayout();
            this.panelServerName.SuspendLayout();
            this.panelButton.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelIP
            // 
            this.panelIP.Controls.Add(this.textBoxIP);
            this.panelIP.Controls.Add(this.lblIP);
            this.panelIP.Location = new System.Drawing.Point(17, 13);
            this.panelIP.Name = "panelIP";
            this.panelIP.Size = new System.Drawing.Size(219, 25);
            this.panelIP.TabIndex = 0;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxIP.Location = new System.Drawing.Point(62, -1);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(155, 26);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.CausesValidation = false;
            this.lblIP.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIP.Location = new System.Drawing.Point(3, 3);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(39, 19);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "IP:";
            // 
            // panelPort
            // 
            this.panelPort.Controls.Add(this.textBoxPort);
            this.panelPort.Controls.Add(this.lblPort);
            this.panelPort.Location = new System.Drawing.Point(17, 53);
            this.panelPort.Name = "panelPort";
            this.panelPort.Size = new System.Drawing.Size(219, 25);
            this.panelPort.TabIndex = 1;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxPort.Location = new System.Drawing.Point(62, 0);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(155, 26);
            this.textBoxPort.TabIndex = 2;
            this.textBoxPort.Text = "5000";
            this.textBoxPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.CausesValidation = false;
            this.lblPort.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPort.Location = new System.Drawing.Point(3, 4);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(59, 19);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port:";
            // 
            // panelServerName
            // 
            this.panelServerName.Controls.Add(this.textBoxServerName);
            this.panelServerName.Controls.Add(this.labelServerName);
            this.panelServerName.Location = new System.Drawing.Point(316, 12);
            this.panelServerName.Name = "panelServerName";
            this.panelServerName.Size = new System.Drawing.Size(219, 25);
            this.panelServerName.TabIndex = 2;
            // 
            // textBoxServerName
            // 
            this.textBoxServerName.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxServerName.Location = new System.Drawing.Point(80, -1);
            this.textBoxServerName.Name = "textBoxServerName";
            this.textBoxServerName.Size = new System.Drawing.Size(139, 27);
            this.textBoxServerName.TabIndex = 4;
            this.textBoxServerName.Text = "Server";
            this.textBoxServerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxServerName.TextChanged += new System.EventHandler(this.textBoxServerName_TextChanged);
            // 
            // labelServerName
            // 
            this.labelServerName.AutoSize = true;
            this.labelServerName.CausesValidation = false;
            this.labelServerName.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelServerName.Location = new System.Drawing.Point(0, 4);
            this.labelServerName.Name = "labelServerName";
            this.labelServerName.Size = new System.Drawing.Size(85, 19);
            this.labelServerName.TabIndex = 3;
            this.labelServerName.Text = "服务器名";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(316, 54);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(89, 23);
            this.btnStartServer.TabIndex = 3;
            this.btnStartServer.Text = "启动服务器";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // listViewClient
            // 
            this.listViewClient.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewClient.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewClient.FullRowSelect = true;
            this.listViewClient.HideSelection = false;
            this.listViewClient.Location = new System.Drawing.Point(316, 88);
            this.listViewClient.MultiSelect = false;
            this.listViewClient.Name = "listViewClient";
            this.listViewClient.Size = new System.Drawing.Size(237, 336);
            this.listViewClient.TabIndex = 4;
            this.listViewClient.UseCompatibleStateImageBehavior = false;
            this.listViewClient.View = System.Windows.Forms.View.Details;
            // 
            // listViewChat
            // 
            this.listViewChat.Location = new System.Drawing.Point(17, 88);
            this.listViewChat.Name = "listViewChat";
            this.listViewChat.Size = new System.Drawing.Size(279, 237);
            this.listViewChat.TabIndex = 5;
            this.listViewChat.UseCompatibleStateImageBehavior = false;
            this.listViewChat.View = System.Windows.Forms.View.Details;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(17, 331);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(279, 56);
            this.textBoxMessage.TabIndex = 6;
            // 
            // btnSingle
            // 
            this.btnSingle.Location = new System.Drawing.Point(0, 0);
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(65, 23);
            this.btnSingle.TabIndex = 7;
            this.btnSingle.Text = "单发";
            this.btnSingle.UseVisualStyleBackColor = true;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // btnGroup
            // 
            this.btnGroup.Location = new System.Drawing.Point(62, 0);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(65, 23);
            this.btnGroup.TabIndex = 8;
            this.btnGroup.Text = "群发";
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(211, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(65, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.btnSendFile);
            this.panelButton.Controls.Add(this.btnSingle);
            this.panelButton.Controls.Add(this.btnClear);
            this.panelButton.Controls.Add(this.btnGroup);
            this.panelButton.Location = new System.Drawing.Point(17, 402);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(279, 22);
            this.panelButton.TabIndex = 10;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(130, 0);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(65, 23);
            this.btnSendFile.TabIndex = 10;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnCloseServer
            // 
            this.btnCloseServer.Location = new System.Drawing.Point(446, 53);
            this.btnCloseServer.Name = "btnCloseServer";
            this.btnCloseServer.Size = new System.Drawing.Size(89, 23);
            this.btnCloseServer.TabIndex = 12;
            this.btnCloseServer.Text = "关闭服务器";
            this.btnCloseServer.UseVisualStyleBackColor = true;
            this.btnCloseServer.Click += new System.EventHandler(this.btnCloseServer_Click);
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 17);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip.Location = new System.Drawing.Point(0, 431);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(570, 22);
            this.statusStrip.TabIndex = 13;
            this.statusStrip.Text = "statusStrip1";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 453);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnCloseServer);
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.listViewChat);
            this.Controls.Add(this.listViewClient);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.panelServerName);
            this.Controls.Add(this.panelPort);
            this.Controls.Add(this.panelIP);
            this.Name = "Server";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Server_Load);
            this.panelIP.ResumeLayout(false);
            this.panelIP.PerformLayout();
            this.panelPort.ResumeLayout(false);
            this.panelPort.PerformLayout();
            this.panelServerName.ResumeLayout(false);
            this.panelServerName.PerformLayout();
            this.panelButton.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelIP;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Panel panelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Panel panelServerName;
        private System.Windows.Forms.Label labelServerName;
        private System.Windows.Forms.TextBox textBoxServerName;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.ListView listViewClient;
        private System.Windows.Forms.ListView listViewChat;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button btnSingle;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Button btnCloseServer;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
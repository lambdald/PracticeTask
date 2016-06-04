namespace task4Client
{
    partial class Client
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
            this.btnCloseConnection = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.listViewChat = new System.Windows.Forms.ListView();
            this.listViewOnline = new System.Windows.Forms.ListView();
            this.btnConnectServer = new System.Windows.Forms.Button();
            this.panelClientName = new System.Windows.Forms.Panel();
            this.textBoxClientName = new System.Windows.Forms.TextBox();
            this.labelClientName = new System.Windows.Forms.Label();
            this.panelPort = new System.Windows.Forms.Panel();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.panelIP = new System.Windows.Forms.Panel();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblOnlineFriends = new System.Windows.Forms.Label();
            this.panelChatType = new System.Windows.Forms.Panel();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.radioButtonSingle = new System.Windows.Forms.RadioButton();
            this.radioButtonGroup = new System.Windows.Forms.RadioButton();
            this.radioButtonServer = new System.Windows.Forms.RadioButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.status.SuspendLayout();
            this.panelClientName.SuspendLayout();
            this.panelPort.SuspendLayout();
            this.panelIP.SuspendLayout();
            this.panelChatType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCloseConnection
            // 
            this.btnCloseConnection.Location = new System.Drawing.Point(446, 47);
            this.btnCloseConnection.Name = "btnCloseConnection";
            this.btnCloseConnection.Size = new System.Drawing.Size(89, 23);
            this.btnCloseConnection.TabIndex = 22;
            this.btnCloseConnection.Text = "断开连接";
            this.btnCloseConnection.UseVisualStyleBackColor = true;
            this.btnCloseConnection.Click += new System.EventHandler(this.btnCloseConnection_Click);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.status.Location = new System.Drawing.Point(0, 481);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(568, 22);
            this.status.TabIndex = 21;
            this.status.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(12, 325);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(279, 56);
            this.textBoxMessage.TabIndex = 19;
            // 
            // listViewChat
            // 
            this.listViewChat.Location = new System.Drawing.Point(12, 82);
            this.listViewChat.Name = "listViewChat";
            this.listViewChat.Size = new System.Drawing.Size(279, 237);
            this.listViewChat.TabIndex = 18;
            this.listViewChat.UseCompatibleStateImageBehavior = false;
            this.listViewChat.View = System.Windows.Forms.View.Details;
            // 
            // listViewOnline
            // 
            this.listViewOnline.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewOnline.Location = new System.Drawing.Point(316, 105);
            this.listViewOnline.MultiSelect = false;
            this.listViewOnline.Name = "listViewOnline";
            this.listViewOnline.Size = new System.Drawing.Size(237, 373);
            this.listViewOnline.TabIndex = 17;
            this.listViewOnline.UseCompatibleStateImageBehavior = false;
            this.listViewOnline.View = System.Windows.Forms.View.Details;
            // 
            // btnConnectServer
            // 
            this.btnConnectServer.Location = new System.Drawing.Point(316, 48);
            this.btnConnectServer.Name = "btnConnectServer";
            this.btnConnectServer.Size = new System.Drawing.Size(89, 23);
            this.btnConnectServer.TabIndex = 16;
            this.btnConnectServer.Text = "连接服务器";
            this.btnConnectServer.UseVisualStyleBackColor = true;
            this.btnConnectServer.Click += new System.EventHandler(this.btnConnectServer_Click);
            // 
            // panelClientName
            // 
            this.panelClientName.Controls.Add(this.textBoxClientName);
            this.panelClientName.Controls.Add(this.labelClientName);
            this.panelClientName.Location = new System.Drawing.Point(316, 6);
            this.panelClientName.Name = "panelClientName";
            this.panelClientName.Size = new System.Drawing.Size(219, 25);
            this.panelClientName.TabIndex = 15;
            // 
            // textBoxClientName
            // 
            this.textBoxClientName.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxClientName.Location = new System.Drawing.Point(80, -1);
            this.textBoxClientName.Name = "textBoxClientName";
            this.textBoxClientName.Size = new System.Drawing.Size(139, 27);
            this.textBoxClientName.TabIndex = 4;
            this.textBoxClientName.Text = "User";
            this.textBoxClientName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelClientName
            // 
            this.labelClientName.AutoSize = true;
            this.labelClientName.CausesValidation = false;
            this.labelClientName.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelClientName.Location = new System.Drawing.Point(0, 4);
            this.labelClientName.Name = "labelClientName";
            this.labelClientName.Size = new System.Drawing.Size(66, 19);
            this.labelClientName.TabIndex = 3;
            this.labelClientName.Text = "用户名";
            // 
            // panelPort
            // 
            this.panelPort.Controls.Add(this.textBoxPort);
            this.panelPort.Controls.Add(this.lblPort);
            this.panelPort.Location = new System.Drawing.Point(12, 45);
            this.panelPort.Name = "panelPort";
            this.panelPort.Size = new System.Drawing.Size(219, 25);
            this.panelPort.TabIndex = 14;
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
            // panelIP
            // 
            this.panelIP.Controls.Add(this.textBoxIP);
            this.panelIP.Controls.Add(this.lblIP);
            this.panelIP.Location = new System.Drawing.Point(12, 5);
            this.panelIP.Name = "panelIP";
            this.panelIP.Size = new System.Drawing.Size(219, 25);
            this.panelIP.TabIndex = 13;
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
            // lblOnlineFriends
            // 
            this.lblOnlineFriends.AutoSize = true;
            this.lblOnlineFriends.Location = new System.Drawing.Point(314, 82);
            this.lblOnlineFriends.Name = "lblOnlineFriends";
            this.lblOnlineFriends.Size = new System.Drawing.Size(53, 12);
            this.lblOnlineFriends.TabIndex = 23;
            this.lblOnlineFriends.Text = "在线好友";
            // 
            // panelChatType
            // 
            this.panelChatType.Controls.Add(this.btnSendFile);
            this.panelChatType.Controls.Add(this.btnClear);
            this.panelChatType.Controls.Add(this.btnSend);
            this.panelChatType.Controls.Add(this.radioButtonSingle);
            this.panelChatType.Controls.Add(this.radioButtonGroup);
            this.panelChatType.Controls.Add(this.radioButtonServer);
            this.panelChatType.Location = new System.Drawing.Point(30, 387);
            this.panelChatType.Name = "panelChatType";
            this.panelChatType.Size = new System.Drawing.Size(237, 91);
            this.panelChatType.TabIndex = 24;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(141, 36);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 5;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(141, 68);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(141, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // radioButtonSingle
            // 
            this.radioButtonSingle.AutoSize = true;
            this.radioButtonSingle.Location = new System.Drawing.Point(0, 16);
            this.radioButtonSingle.Name = "radioButtonSingle";
            this.radioButtonSingle.Size = new System.Drawing.Size(47, 16);
            this.radioButtonSingle.TabIndex = 2;
            this.radioButtonSingle.Text = "单聊";
            this.radioButtonSingle.UseVisualStyleBackColor = true;
            // 
            // radioButtonGroup
            // 
            this.radioButtonGroup.AutoSize = true;
            this.radioButtonGroup.Location = new System.Drawing.Point(0, 39);
            this.radioButtonGroup.Name = "radioButtonGroup";
            this.radioButtonGroup.Size = new System.Drawing.Size(47, 16);
            this.radioButtonGroup.TabIndex = 1;
            this.radioButtonGroup.Text = "群聊";
            this.radioButtonGroup.UseVisualStyleBackColor = true;
            // 
            // radioButtonServer
            // 
            this.radioButtonServer.AutoSize = true;
            this.radioButtonServer.Checked = true;
            this.radioButtonServer.Location = new System.Drawing.Point(0, 62);
            this.radioButtonServer.Name = "radioButtonServer";
            this.radioButtonServer.Size = new System.Drawing.Size(59, 16);
            this.radioButtonServer.TabIndex = 0;
            this.radioButtonServer.TabStop = true;
            this.radioButtonServer.Text = "服务器";
            this.radioButtonServer.UseVisualStyleBackColor = true;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 503);
            this.Controls.Add(this.panelChatType);
            this.Controls.Add(this.lblOnlineFriends);
            this.Controls.Add(this.btnCloseConnection);
            this.Controls.Add(this.status);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.listViewChat);
            this.Controls.Add(this.listViewOnline);
            this.Controls.Add(this.btnConnectServer);
            this.Controls.Add(this.panelClientName);
            this.Controls.Add(this.panelPort);
            this.Controls.Add(this.panelIP);
            this.Name = "Client";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.Client_Load);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.panelClientName.ResumeLayout(false);
            this.panelClientName.PerformLayout();
            this.panelPort.ResumeLayout(false);
            this.panelPort.PerformLayout();
            this.panelIP.ResumeLayout(false);
            this.panelIP.PerformLayout();
            this.panelChatType.ResumeLayout(false);
            this.panelChatType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseConnection;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.ListView listViewChat;
        private System.Windows.Forms.ListView listViewOnline;
        private System.Windows.Forms.Button btnConnectServer;
        private System.Windows.Forms.Panel panelClientName;
        private System.Windows.Forms.TextBox textBoxClientName;
        private System.Windows.Forms.Label labelClientName;
        private System.Windows.Forms.Panel panelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Panel panelIP;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblOnlineFriends;
        private System.Windows.Forms.Panel panelChatType;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RadioButton radioButtonSingle;
        private System.Windows.Forms.RadioButton radioButtonGroup;
        private System.Windows.Forms.RadioButton radioButtonServer;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using task4Lib;
using System.Diagnostics;
using Message = task4Lib.Message;
namespace task4Client
{
    public delegate void SimpleDelegate();

    public delegate void UpdateControlsDelegate(bool OnConnect);
    public delegate void MessageDelegate(Message msg);
    public delegate void OnlineOfflineDelegate(Message msg);
    public delegate void FileInfoDelegate(Message msg);
    public delegate void StatusDelegate(string str);
    public delegate void ExceptionDelegate(Exception ex);
    public delegate void SocketErrorDelegate(int id);

    public partial class Client : Form
    {

        Socket client;
        bool IsConnected;

        ManualResetEvent mreMessage;

        UserInformation SelfInfo;
        UserInformation ServerInfo;
        IPAddress ServerIP;
        IPEndPoint hostEndPoint;
        Thread connection;
        Thread threadMessageProcess;
        ClientMessageProcess MsgProcess;
        Queue<Message> msgQueue;
        Dictionary<uint, string> friends;

        public Client()
        {
            InitializeComponent();
            this.FormClosed += Client_FormClosed;
            this.FormClosing+=Client_FormClosing;
            textBoxIP.Text = IPAddress.Loopback.ToString();
            msgQueue = new Queue<Message>();
            mreMessage = new ManualResetEvent(false);

            friends = new Dictionary<uint, string>();
            IsConnected = false;
            SelfInfo = new UserInformation();
            ServerInfo = new UserInformation(UserInformation.ServerID);
            MsgProcess = new ClientMessageProcess();

            MsgProcess.OnlineNotifyReceived += MsgProcess_OnlineNotifyReceived;
            MsgProcess.OfflineNotifyReceived += MsgProcess_OfflineNotifyReceived;
            MsgProcess.MessageReceived += MsgProcess_MessageReceived;
            MsgProcess.FileInfoMessageReceived += MsgProcess_FileInfoMessageReceived;
            MsgProcess.MessageProcessErrorOccurred += MsgProcess_MessageProcessErrorOccurred;

            listViewChat.Columns.Add("用户", listViewChat.Width / 3);
            listViewChat.Columns.Add("内容", listViewChat.Width);

             listViewOnline.Columns.Add("ID", listViewOnline.Width / 3);
            listViewOnline.Columns.Add("用户", listViewOnline.Width * 2 / 3);
           
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Clean();
        }

        void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }


        #region 消息处理产生错误
        void MsgProcess_MessageProcessErrorOccurred(object sender, MessageProcessErrorArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new ExceptionDelegate(MessageProcessErrorOccurred), e.InnerException);
            }
            else
            {
                this.MessageProcessErrorOccurred(e.InnerException);
            }
        }
        public void MessageProcessErrorOccurred(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        #endregion

        #region 文件处理
        void MsgProcess_FileInfoMessageReceived(object sender, MessageEventArgs e)
        {
            if (InvokeRequired)
                this.BeginInvoke(new FileInfoDelegate(this.OnFileInfoReceived), e.Msg);

            else
            {
                OnFileInfoReceived(e.Msg);
            }
        }
        private void OnFileInfoReceived(Message msg)
        {
            new Thread(delegate () { new FileReceiverForm(msg).ShowDialog(); }).Start();
        }
        #endregion

        #region 消息处理
        void MsgProcess_MessageReceived(object sender, MessageEventArgs e)
        {
            if (InvokeRequired)
                this.BeginInvoke(new MessageDelegate(this.OnMessageReceived), e.Msg);

            else
            {
                this.OnMessageReceived(e.Msg);
            }
        }
        void OnMessageReceived(Message msg)
        {
            ListViewItem item = new ListViewItem();
            if(msg.SenderID==UserInformation.ServerID)
            {
                item.Text = "服务器:" + ServerInfo.Name;
            }
            else if(msg.SenderID==UserInformation.GroupID)
            {
                item.Text = "群发";
            }
            else if(msg.SenderID==SelfInfo.ID)
            {
                item.Text = "我";
            }
            else
            {
                item.Text = friends[msg.SenderID];
            } 
            item.SubItems.Add(msg.Text);
            listViewChat.Items.Add(item);
        }
        #endregion

        #region 上线处理
        void MsgProcess_OnlineNotifyReceived(object sender, MessageEventArgs e)
        {
            if (InvokeRequired)
                this.BeginInvoke(new OnlineOfflineDelegate(this.OnOnlineNotifyReceived), e.Msg);
            else
            {
                this.OnOnlineNotifyReceived(e.Msg);
            }
        }

        private void OnOnlineNotifyReceived(Message msg)
        {
            if (msg.SenderID == this.SelfInfo.ID)
                return;
            if (!this.friends.ContainsKey(msg.SenderID))
            {
                lock (this.friends)
                {
                    this.friends.Add(msg.SenderID, msg.SenderName);
                }
                ListViewItem item = new ListViewItem();
                item.Text = msg.SenderID.ToString();
                item.SubItems.Add(msg.SenderName);
                listViewOnline.Items.Add(item);
            }
        }
        private void FirstOnlineNotifyReceived(Message msg)
        {
            foreach (var friend in msg.OnlineInfo)
            {
                if (friend.Key == this.SelfInfo.ID)
                    continue;
                if (!this.friends.ContainsKey(friend.Key))
                {
                    lock (this.friends)
                    {
                        this.friends.Add(friend.Key, friend.Value);
                    }
                    ListViewItem item = new ListViewItem();
                    item.Text = friend.Key.ToString();
                    item.SubItems.Add(friend.Value);
                    listViewOnline.Items.Add(item);
                }
            }
        }
        #endregion

        #region 下线处理
        void MsgProcess_OfflineNotifyReceived(object sender, MessageEventArgs e)
        {
            if (InvokeRequired)
                this.BeginInvoke(new OnlineOfflineDelegate(this.OnOfflineNotifyReceived), e.Msg);

            else
            {
                OnOfflineNotifyReceived(e.Msg);
            }
        }
        private void OnOfflineNotifyReceived(Message msg)
        {
            string strID = msg.OfflineID.ToString();
            if (friends.ContainsKey(msg.OfflineID))
            {
                friends.Remove(msg.OfflineID);
            }
            foreach (ListViewItem item in listViewOnline.Items)
            {
                if (item.SubItems[0].Text == strID)
                {
                    listViewOnline.Items.Remove(item);
                    return;
                }
            }

        }
        #endregion

        public void ExceptionOccurred(Exception ex)
        {
            if (InvokeRequired)
                this.BeginInvoke(new ExceptionDelegate(ExceptionOccurred), ex);
            else
            {
                MessageBox.Show(ex.ToString());
                this.Clean();
            }
        }

        private void UpdateStatus(string str)
        {
            status.Text = str;
        }
        private void btnConnectServer_Click(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(textBoxIP.Text, out ServerIP))
            {
                MessageBox.Show("IP地址错误", "错误");
                return;
            }
            if (textBoxClientName.Text == string.Empty)
            {
                MessageBox.Show("用户名不能为空", "错误");
                return;
            }
            SelfInfo.Name = textBoxClientName.Text;
            try
            {
                ServerInfo.Port = Convert.ToInt32(textBoxPort.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("端口号错误", "错误");
                return;
            }
            if (ServerInfo.Port <= 0)
            {
                MessageBox.Show("端口号错误", "错误");
                return;
            }
            try
            {
                hostEndPoint = new IPEndPoint(ServerIP, ServerInfo.Port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误");
                return;
            }
            MsgProcess.InitClientMessageProcess(SelfInfo, msgQueue, friends, mreMessage);
            connection = new Thread(ConnectServer);
            connection.IsBackground = true;
            connection.Start();
            threadMessageProcess = new Thread(MsgProcess.StartMessageProcess);
            threadMessageProcess.IsBackground = true;
            threadMessageProcess.Start();
            UpdateControls(true);
        }

        private void ConnectServer()
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.SendBufferSize = MessageConvert.MaxMessageSize;
                client.ReceiveBufferSize = MessageConvert.MaxMessageSize;
                client.BeginConnect(hostEndPoint, new AsyncCallback(OnConnected), client);
            }
            catch (Exception e)
            {

            }

        }

        public void ConnectFailed(Exception e)
        {
            MessageBox.Show(e.ToString(), "连接失败");
            UpdateControls(false);
        }

        public void OnConnected(IAsyncResult ar)
        {

            Socket client = (Socket)ar.AsyncState;


            byte[] data = null;
            byte[] onlines = null;
            Message msg;
            try
            {
                client.EndConnect(ar);
                msg = new Message(this.SelfInfo.Name);
                data = MessageConvert.GetBytes(msg);
                client.Send(data, 0, data.Length, SocketFlags.None);
                data = new byte[MessageConvert.MaxMessageSize];
                client.Receive(data);
                onlines = new byte[MessageConvert.MaxMessageSize];
                client.Receive(onlines);
            }
            catch (Exception e)
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new ExceptionDelegate(ConnectFailed), e);
                }
                else
                {
                    this.ConnectFailed(e);
                }
                this.BeginInvoke(new SimpleDelegate(this.Clean));
                return;
            }
            msg = new Message();
            try
            {
                msg = MessageConvert.RestoreBytes(data);
                this.BeginInvoke(new OnlineOfflineDelegate(FirstOnlineNotifyReceived), MessageConvert.RestoreBytes(onlines));
            }
            catch (Exception ex)
            {
                ExceptionOccurred(ex);
                return;
            }
            this.SelfInfo.ID = msg.ReceiverID;
            this.ServerInfo.Name = msg.SenderName;
            IsConnected = true;

            WaitForData();

        }

        private void WaitForData()
        {
            if (!IsConnected)
                return;
            SocketPacket sp = new SocketPacket(client,SelfInfo.ID);
                byte[] data = new byte[MessageConvert.MaxMessageSize];
                try
                {
                    client.BeginReceive(sp.dataBuffer,0, sp.dataBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveData),sp);
                }
                catch (Exception ex)
                {
                    ExceptionOccurred(ex);
                    return;
                }
        }
        private void ReceiveData(IAsyncResult ar)
        {
            if (!IsConnected)
                return;
            SocketPacket sp = (SocketPacket)ar.AsyncState;
            try
            {
                int size = sp.currentSocket.EndReceive(ar);
                if (size == 0)
                {
                    this.BeginInvoke(new SimpleDelegate(this.Clean));
                    return;
                }
                byte[] data = new byte[size];
                Array.Copy(sp.dataBuffer, data, data.Length);
                Message msg = MessageConvert.RestoreBytes(data);
                lock (msgQueue)
                {
                    msgQueue.Enqueue(msg);
                    mreMessage.Set();
                }
            }
            catch (Exception ex)
            {
                ExceptionOccurred(ex);
                return;
            }
            WaitForData();
        }
        public void UpdateControls(bool OnConnect)
        {
            textBoxIP.Enabled = !OnConnect;
            textBoxPort.Enabled = !OnConnect;
            textBoxClientName.Enabled = !OnConnect;
            textBoxMessage.Enabled = OnConnect;
            textBoxMessage.Text = "";
            btnConnectServer.Enabled = !OnConnect;
            btnCloseConnection.Enabled = OnConnect;

            btnSend.Enabled = OnConnect;
            btnClear.Enabled = OnConnect;
            btnSendFile.Enabled = OnConnect;
            
            if (OnConnect)
            {
                status.Text = "已连接至服务器";
            }
            else
            {
                status.Text = "未连接服务";
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!client.Connected)
            {
                MessageBox.Show("未连接", "错误");
                return;
            }
            if (radioButtonGroup.Checked)
            {
                byte[] data = MessageConvert.GetMessageBytes(SelfInfo.ID, UserInformation.GroupID, textBoxMessage.Text);
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendMessage), data);
            }
            else if (radioButtonSingle.Checked)
            {
                uint id;
                try
                {

                    UInt32.TryParse(listViewOnline.FocusedItem.SubItems[0].Text, out id);
                    byte[] data = MessageConvert.GetMessageBytes(SelfInfo.ID, id, textBoxMessage.Text);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SendMessage), data);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                byte[] data = MessageConvert.GetMessageBytes(SelfInfo.ID, UserInformation.ServerID, textBoxMessage.Text);
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendMessage), data);
            }

        }
        public void SendMessage(object state)
        {
            byte[] data = (byte[])state;
            client.Send(data, 0, data.Length, SocketFlags.None);
        }
        private void btnCloseConnection_Click(object sender, EventArgs e)
        {
            this.Clean();
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                MessageBox.Show("未连接", "错误");
                return;
            }
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            uint id = 0;
            if (radioButtonGroup.Checked)
            {
                id = UserInformation.GroupID;
            }
            else if (radioButtonSingle.Checked)
            {
                try
                {
                    UInt32.TryParse(listViewOnline.FocusedItem.SubItems[1].Text, out id);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                id = UserInformation.ServerID;
            }
            Thread thread = new Thread(this.SendFile);
            thread.Start(id);
        }
        public void SendFile(object id)
        {

            Socket fileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            fileSocket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            UserInformation sender = new UserInformation();
            sender.Name = SelfInfo.Name;
            sender.ID = SelfInfo.ID;
            sender.IP = ((IPEndPoint)fileSocket.LocalEndPoint).Address.ToString();
            sender.Port = ((IPEndPoint)fileSocket.LocalEndPoint).Port;
            byte[] data = MessageConvert.GetFileInfoBytes(sender, (uint)id, openFileDialog.FileName);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendMessage), data);

            new Thread(delegate () { new FileSenderForm(fileSocket, openFileDialog.FileName).ShowDialog(); }).Start();
        }
        private void Client_Load(object sender, EventArgs e)
        {
            UpdateControls(false);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            listViewChat.Items.Clear();
        }

        public void Clean()
        {
            IsConnected = false;
            UpdateControls(false);
            if (client != null)
            {
                if(client.Connected)
                    client.Shutdown(SocketShutdown.Both);
                //client.Blocking = false;
                client.Close();
            }
            if (threadMessageProcess != null)
                threadMessageProcess.Abort();
            this.listViewChat.Items.Clear();
        }
    }
}

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

namespace task4Server
{
    public delegate void MessageDelegate(Message msg);
    public delegate void OnlineOfflineDelegate(Message msg);
    public delegate void FileInfoDelegate(Message msg);
    public delegate void ProgressDelegate(FileTransmission task);
    public delegate void ExceptionDelegate(Exception ex);
    public delegate void SocketErrorDelegate(int id);
    public delegate void StatusDelegate(string str);
    public delegate void SimpleDelegate();


    public partial class Server : Form
    {


        private ManualResetEvent mreConnect;
        private ManualResetEvent mreMessage;
        private ManualResetEvent mreFile;

        UserInformation ServerInfo;
        Socket Listener;
        IPAddress ServerIP;
        IPEndPoint hostEndPoint;
        Thread threadServer;
        Thread threadMessageProcess;
        int clientNum = -1;

        Dictionary<int, UserInformation> ClientInfoDict;
        Dictionary<int, Socket> clientSocketsDict;

        ServerMessageProcess MsgProcess;
        Queue<Message> msgQueue;
        Queue<Message> msgFileQueue;
        public Server()
        {
            InitializeComponent();
            textBoxIP.Text = IPAddress.Loopback.ToString();
            this.FormClosed += Server_FormClosed;
            mreConnect = new ManualResetEvent(false);
            mreMessage = new ManualResetEvent(false);
            mreFile = new ManualResetEvent(false);

            ServerInfo = new UserInformation();
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientNum = -1;

            ClientInfoDict = new Dictionary<int, UserInformation>();
            clientSocketsDict = new Dictionary<int, Socket>();


            msgQueue = new Queue<Message>();
            msgFileQueue = new Queue<Message>();
            MsgProcess = new ServerMessageProcess(ServerInfo, msgQueue, clientSocketsDict, ClientInfoDict, mreMessage);
            MsgProcess.OnlineNotifyReceived += MsgProcess_OnlineNotifyReceived;
            MsgProcess.OfflineNotifyReceived += MsgProcess_OfflineNotifyReceived;
            MsgProcess.MessageReceived += MsgProcess_MessageReceived;
            MsgProcess.FileInfoMessageReceived += MsgProcess_FileInfoMessageReceived;
            MsgProcess.SocketErrorOccurred += MsgProcess_SocketErrorOccurred;
            MsgProcess.MessageProcessErrorOccurred += MsgProcess_MessageProcessErrorOccurred;
            listViewChat.Columns.Add("用户");
            listViewChat.Columns.Add("内容");
            listViewChat.Columns.Add("发送至");

            listViewClient.Columns.Add("用户");
            listViewClient.Columns.Add("ID");
            listViewClient.Columns.Add("IP");
        }

        void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

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

        void MsgProcess_SocketErrorOccurred(object sender, SocketErrorArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new SocketErrorDelegate(SocketErrorOccurred), e.ID);
            }
            else
            {
                this.SocketErrorOccurred(e.ID);
            }
        }
        void SocketErrorOccurred(int ID)
        {
            if (clientSocketsDict.ContainsKey(ID))
            {
                lock (clientSocketsDict)
                {
                    clientSocketsDict.Remove(ID);
                }
            }
            Message msg = null;
            if (ClientInfoDict.ContainsKey(ID))
            {
                msg = new Message(Message.MessageType.Offline, ClientInfoDict[ID]);
                lock (ClientInfoDict)
                {
                    ClientInfoDict.Remove(ID);
                }
            }
            else
                msg = new Message(Message.MessageType.Offline, new UserInformation(ID));
            lock (msgQueue)
            {
                msgQueue.Enqueue(msg);
                mreMessage.Set();
            }
        }

        public void ExceptionOccurred(Exception ex)
        {
            if (InvokeRequired)
                this.Invoke(new ExceptionDelegate(ExceptionOccurred), ex);
            else
            {
                MessageBox.Show(ex.ToString());
                //this.Clean();
            }
        }

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
            new Thread(delegate() { new FileReceiverForm(msg).ShowDialog(); }).Start();
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
            item.Text = msg.Sender.Name;
            item.SubItems.Add(msg.Content.ToString());
            item.SubItems.Add(msg.Receiver.Name);
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
            ListViewItem item = new ListViewItem();
            item.Text = msg.Friend.Name;
            item.SubItems.Add(msg.Friend.ID.ToString());
            item.SubItems.Add(((IPEndPoint)clientSocketsDict[msg.Friend.ID].RemoteEndPoint).ToString());
            listViewClient.Items.Add(item);
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
            string strID = msg.Friend.ID.ToString();
            foreach (ListViewItem item in listViewClient.Items)
            {
                if (item.SubItems[1].Text == strID)
                {
                    listViewClient.Items.Remove(item);
                    return;
                }
            }
        }
        #endregion


        private void textBoxServerName_TextChanged(object sender, EventArgs e)
        {
            ServerInfo.Name = textBoxServerName.Text;
        }

        private void Server_Load(object sender, EventArgs e)
        {
            UpdateControls(false);
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(textBoxIP.Text, out ServerIP))
            {
                MessageBox.Show("IP地址错误", "错误");
                return;
            }
            else
            {
                ServerInfo.IP = ServerIP.ToString();
            }

            if (textBoxServerName.Text == string.Empty)
            {
                MessageBox.Show("服务器名字不能为空", "错误");
                return;
            }
            else
            {
                ServerInfo.Name = textBoxServerName.Text;
            }

            if (!Int32.TryParse(textBoxPort.Text, out ServerInfo.Port))
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
            UpdateControls(true);



            threadServer = new Thread(StartServer);
            
            threadServer.IsBackground = true;
            threadMessageProcess = new Thread(MsgProcess.StartMessageProcess);
            threadMessageProcess.IsBackground = true;
            threadServer.Start();
            threadMessageProcess.Start();
        }


        public void UpdateControls(bool OnServe)
        {
            textBoxIP.Enabled = !OnServe;
            textBoxPort.Enabled = !OnServe;
            textBoxServerName.Enabled = !OnServe;
            textBoxMessage.Enabled = OnServe;

            btnCloseServer.Enabled = OnServe;
            btnStartServer.Enabled = !OnServe;

            btnSingle.Enabled = OnServe;
            btnGroup.Enabled = OnServe;
            btnSendFile.Enabled = OnServe;
            if (OnServe)
            {
                status.Text = "已启动服务";
            }
            else
            {
                status.Text = "未启动服务";
            }
        }
        private void StartServer()
        {
            try
            {
                if (!Listener.IsBound)
                {
                    Listener.Bind(hostEndPoint);
                }
                Listener.SendBufferSize = Message.MaxMessageSize;
                Listener.ReceiveBufferSize = Message.MaxMessageSize;
                Listener.Listen(100);
            }
            catch (Exception ex)
            {
                ExceptionOccurred(ex);
                return;
            }
            while (true)
            {
                mreConnect.Reset();
                try
                {
                    Listener.BeginAccept(Message.MaxMessageSize, new AsyncCallback(OnClientConnect), Listener);
                }
                catch (SocketException ex)
                {

                }
                catch (Exception ex)
                {

                }
                mreConnect.WaitOne();
            }
        }

        public void OnClientConnect(IAsyncResult ar)
        {
            mreConnect.Set();
            Socket listener = (Socket)ar.AsyncState;
            byte[] buffer = null;
            int bytesTransferred;
            Socket socketHandler = null;
            try
            {
                socketHandler = listener.EndAccept(out buffer, out bytesTransferred, ar);
            }
            catch (SocketException ex)
            {
                return;
            }
            catch (Exception ex)
            {
                ExceptionOccurred(ex);
                return;
            }
            Interlocked.Increment(ref clientNum);

            Message msg = new Message();
            msg.ParseXmlString(buffer);
            msg.Sender.ID = clientNum;
            Message clientInfo = new Message(this.ServerInfo, msg.Sender, "你的id是 " + clientNum.ToString());
            Message newClient = new Message(new UserInformation(UserInformation.GroupID), Message.MessageType.Online, msg.Sender);
            Message allClientInfo = new Message(msg.Sender, Message.MessageType.Online, ClientInfoDict);

            lock (clientSocketsDict)
            {
                clientSocketsDict.Add(clientNum, socketHandler);
            }
            lock (ClientInfoDict)
            {
                ClientInfoDict.Add(clientNum, msg.Sender);
            }
            lock (msgQueue)
            {
                msgQueue.Enqueue(msg);
                msgQueue.Enqueue(clientInfo);
                msgQueue.Enqueue(newClient);
                msgQueue.Enqueue(allClientInfo);
                mreMessage.Set();
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(WaitForData), new SocketPacket(socketHandler, msg.Sender));
        }
        private void WaitForData(object state)
        {
            AsyncCallback callback = new AsyncCallback(OnDataReceived);
            SocketPacket sp = (SocketPacket)state;
            try
            {
                sp.currentSocket.BeginReceive(sp.dataBuffer, 0, sp.dataBuffer.Length, SocketFlags.None, callback, sp);
            }
            catch (SocketException)
            {
                SocketErrorOccurred(sp.clientNumber);
                return;
            }
            catch (Exception ex)
            {
                ExceptionOccurred(ex);
                return;
            }
        }
        public void OnDataReceived(IAsyncResult ar)
        {
            SocketPacket sp = (SocketPacket)ar.AsyncState;

            try
            {
                int size = sp.currentSocket.EndReceive(ar);
                Message msg = new Message();
                byte[] data = new byte[size];
                Array.Copy(sp.dataBuffer, data, data.Length);
                msg.ParseXmlString(data);
                lock (msgQueue)
                {
                    msgQueue.Enqueue(msg);
                    mreMessage.Set();
                }
                WaitForData(sp);
            }
            catch (SocketException)
            {
                SocketErrorOccurred(sp.clientNumber);
                return;
            }
            catch (Exception ex)
            {
                ExceptionOccurred(ex);
                return;
            }
        }
        private void btnCloseServer_Click(object sender, EventArgs e)
        {
            this.Clean();
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            if (Listener.IsBound)
            {

                if (listViewClient.FocusedItem == null)
                {
                    MessageBox.Show("请选择一个用户", "提示");
                    return;
                }
                string content;
                int id;
                Int32.TryParse(listViewClient.FocusedItem.SubItems[1].Text, out id);
                content = textBoxMessage.Text;
                if (content == string.Empty)
                {
                    MessageBox.Show("请输入消息内容", "提示");
                    return;
                }
                Message msg = new Message(this.ServerInfo, ClientInfoDict[id], content);
                lock (msgQueue)
                {
                    msgQueue.Enqueue(msg);
                    mreMessage.Set();
                }
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            if (Listener.IsBound)
            {
                string content = textBoxMessage.Text;
                if (content == string.Empty)
                {
                    MessageBox.Show("请输入消息内容", "提示");
                    return;
                }
                Message msg;
                msg = new Message(this.ServerInfo, new UserInformation(UserInformation.GroupID), content);
                lock (msgQueue)
                {
                    msgQueue.Enqueue(msg);
                    mreMessage.Set();
                }

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listViewChat.Items.Clear();
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (Listener.IsBound)
            {

                if (listViewClient.FocusedItem == null)
                {
                    MessageBox.Show("请选择一个用户", "提示");
                    return;
                }

                int id;
                Int32.TryParse(listViewClient.FocusedItem.SubItems[1].Text, out id);
                Thread th = new Thread(SendFile);
                th.Start(id);
                this.status.Text = "等待发送文件";
            }
        }
        public void SendFile(object id)
        {
            Socket fileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            fileSocket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            UserInformation sender = new UserInformation();
            sender.Name = ServerInfo.Name;
            sender.ID = ServerInfo.ID;
            sender.IP = ((IPEndPoint)fileSocket.LocalEndPoint).Address.ToString();
            sender.Port = ((IPEndPoint)fileSocket.LocalEndPoint).Port;
            Message msg = new Message(sender, ClientInfoDict[(int)id], new FileInformation(openFileDialog.FileName));
            lock (msgQueue)
            {
                msgQueue.Enqueue(msg);
                mreMessage.Set();
            }
            new Thread(delegate() { new FileSenderForm(fileSocket, openFileDialog.FileName).ShowDialog(); }).Start();
        }
        public void Clean()
        {
            listViewClient.Items.Clear();
            if (threadMessageProcess != null)
                threadMessageProcess.Abort();
            if (threadServer != null)
                threadServer.Abort();
            if (Listener != null)
            {
                if (Listener.Connected)
                {
                    Listener.Shutdown(SocketShutdown.Both);

                }
                Listener.Close();
                Listener = Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            UpdateControls(false);
        }
    }

}

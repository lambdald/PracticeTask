using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace task4Lib
{
    #region 消息相关
    public enum MessageType
    {
        Message,
        Online,
        Offline,
        File,
    };
    /// <summary>
    /// 用于传输的对象（通过序列化与反序列化）
    /// </summary>
    [Serializable]
    public class Message
    {
        public MessageType MessageType;

        //发送方的信息
        public uint SenderID;
        public string SenderName;
        public string SenderIP;     //在消息类型为File的时候，IP和端口号接受方需要知道发送文件的Socket
        public int SenderPort;
        //接收方ID
        public uint ReceiverID;

        public Dictionary<uint,string> OnlineInfo;        //用于上线的通知，需要告知用户其他用户的用户名和id
        public uint OfflineID;                            //用于下线通知，只通过id标识即可

        public string Text;      //消息类型为Message时，为聊天内容，消息类型为File时，为文件名字


        public Message()
        {

        }
        /// <summary>
        /// 要生成Message类型的消息
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="content"></param>
        public Message(uint senderID,uint receiverID,string content)
        {
            this.MessageType = MessageType.Message;
            this.SenderID = senderID;
            this.ReceiverID = receiverID;
            this.Text = content;
        }
        /// <summary>
        /// 新上线的用户只需要发送给服务器自己的用户名
        /// </summary>
        /// <param name="senderName"></param>
        public Message(string senderName)
        {
            this.MessageType = MessageType.Online;
            this.SenderName = senderName;
        }
        public Message(uint receiverID,Dictionary<uint,string> onlineIDs)
        {
            this.MessageType = MessageType.Online;
            this.ReceiverID = receiverID;
            this.OnlineInfo = onlineIDs;
        }
        /// <summary>
        /// 用于生成离线消息
        /// </summary>
        /// <param name="offlineID"></param>
        public Message(uint offlineID)
        {
            this.MessageType = MessageType.Offline;
            this.OfflineID = offlineID;
        }
        public Message(UserInformation sender,uint receiverID,string fileName)
        {
            this.MessageType = MessageType.File;
            this.SenderID = sender.ID;
            this.SenderIP = sender.IP;
            this.SenderPort = sender.Port;
            this.ReceiverID = receiverID;
            this.Text = fileName;
        }
    }

    /// <summary>
    /// 提供对象和字节数组之间转换的静态方法
    /// </summary>
    public static class MessageConvert
    {
        public static int MaxMessageSize = 2048;
        /// <summary>
        /// 序列化对象存入MemoryStream,并返回字节数组
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] GetBytes(Message msg)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, msg);
            return stream.ToArray();
        }
        /// <summary>
        /// 获取信息类型的字节数组
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] GetMessageBytes(uint senderID, uint receiverID, string content)
        {
            Message msg = new Message();
            msg.MessageType = MessageType.Message;
            msg.SenderID = senderID;
            msg.ReceiverID = receiverID;
            msg.Text = content;
            return GetBytes(msg);
        }
        /// <summary>
        /// 获取上线通知的字节数组
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
        public static byte[] GetOnlineNotifyBytes(uint onlineID,string name)
        {
            Message msg = new Message();
            msg.MessageType = MessageType.Online;
            msg.OnlineInfo = new Dictionary<uint, string>();
            msg.OnlineInfo.Add(onlineID,name);
            return GetBytes(msg);
        }
        /// <summary>
        /// 获取下线通知的字节数组
        /// </summary>
        /// <param name="offlineID"></param>
        /// <returns></returns>
        public static byte[] GetOfflineNotifyBytes(uint offlineID)
        {
            Message msg = new Message();
            msg.MessageType = MessageType.Offline;
            msg.OfflineID = offlineID;
            return GetBytes(msg);
        }
        /// <summary>
        /// 新上线的用户需要获取已经在线的用户列表
        /// </summary>
        /// <param name="onlineIDs"></param>
        /// <returns></returns>
        public static byte[] GetAllOnlineNotifyBytes(Dictionary<uint,string> onlineIDs)
        {
            Message msg = new Message();
            msg.MessageType = MessageType.Online;
            msg.OnlineInfo = onlineIDs;
            return GetBytes(msg);
        }
        public static byte[] GetFileInfoBytes(UserInformation sender,uint receiverID,string fileName)
        {
            Message msg = new Message();
            msg.MessageType = MessageType.File;
            msg.SenderID = sender.ID;
            msg.SenderName = sender.Name;
            msg.SenderIP = sender.IP;
            msg.SenderPort = sender.Port;
            msg.ReceiverID = receiverID;
            msg.Text = fileName;
            return GetBytes(msg);
        }

        public static Message RestoreBytes(byte[] data)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);
            Message msg = (Message)formatter.Deserialize(stream);
            return msg;
        }
    }
    #endregion

    #region 事件参数类

    public class MessageEventArgs : EventArgs
    {
        public Message Msg;
        public MessageEventArgs(Message msg)
        {
            this.Msg = msg;
        }
    }
    public class SocketErrorArgs:EventArgs
    {
        public uint ID;
        public SocketErrorArgs(uint id)
        {
            this.ID = id;
        }
    }
    public class MessageProcessErrorArgs:EventArgs
    {
        public Exception InnerException;
        public MessageProcessErrorArgs(Exception ex)
        {
            this.InnerException=ex;
        }
    }

    #endregion

    #region 委托

    public delegate void MessageReceivedHandler(object sender, MessageEventArgs e);
    public delegate void OnlineNotifyReceivedHandler(object sender, MessageEventArgs e);
    public delegate void OfflineNotifyReceivedHandler(object sender, MessageEventArgs e);
    public delegate void FileInfoReceivedHandler(object sender, MessageEventArgs e);
    public delegate void SocketErrorHandler(object sender, SocketErrorArgs e);
    public delegate void MessageProcessErrorHandler(object sender,MessageProcessErrorArgs e);
    #endregion

    #region 消息基类
    public class MessageProcess
    {

        public event MessageReceivedHandler MessageReceived;
        public event OnlineNotifyReceivedHandler OnlineNotifyReceived;
        public event OfflineNotifyReceivedHandler OfflineNotifyReceived;
        public event FileInfoReceivedHandler FileInfoMessageReceived;
        public event SocketErrorHandler SocketErrorOccurred;
        public event MessageProcessErrorHandler MessageProcessErrorOccurred;

        public ManualResetEvent mreMessage;
        public Queue<Message> MessageQueue;
        public Dictionary<uint, string> Friends;
        public UserInformation HostInfo;
        protected void InitMessageProcess(UserInformation HostInfo, Dictionary<uint, string> friends, ManualResetEvent mreMsg, Queue<Message> msgQueue)
        {
            this.Friends = friends;
            this.mreMessage = mreMsg;
            this.MessageQueue = msgQueue;
            this.HostInfo = HostInfo;
        }

        public virtual void OnMessageReceived(Message msg)
        {
            if (MessageReceived != null)
            {
                this.MessageReceived(this, new MessageEventArgs(msg));
            }
        }
        public virtual void OnOnlineNotifyReceived(Message msg)
        {
            if (OnlineNotifyReceived != null)
            {
                this.OnlineNotifyReceived(this, new MessageEventArgs(msg));
            }
        }
        public virtual void OnOfflineNotifyReceived(Message msg)
        {
            if (OfflineNotifyReceived != null)
            {
                this.OfflineNotifyReceived(this, new MessageEventArgs(msg));
            }
        }
        public virtual void OnFileInfoMessageReceived(Message msg)
        {
            if (FileInfoMessageReceived != null)
            {
                this.FileInfoMessageReceived(this, new MessageEventArgs(msg));
            }
        }
        public virtual void OnSocketErrorOccurred(uint id)
        {
            if(SocketErrorOccurred!=null)
            {
                this.SocketErrorOccurred(this, new SocketErrorArgs(id));
            }
        }
        public virtual void OnMessageProcessErrorOccurred(Exception ex)
        {
            if (MessageProcessErrorOccurred != null)
            {
                this.MessageProcessErrorOccurred(this, new MessageProcessErrorArgs(ex));
            }
        }

        public void StartMessageProcess()
        {
            while (true)
            {
                if (MessageQueue.Count != 0)
                {
                    Message msg;
                    lock(MessageQueue)
                    {
                      msg = MessageQueue.Dequeue();
                    }
                    switch (msg.MessageType)
                    {
                        case MessageType.Online:
                            this.OnlineNotify(msg);
                            break;
                        case MessageType.Offline:
                            this.OfflineNotify(msg);
                            break;
                        case MessageType.Message:
                            this.MessageGet(msg);
                            break;
                        case MessageType.File:
                            this.FileInfoGet(msg);
                            break;
                    }
                }
                else
                {
                    mreMessage.Reset();
                    mreMessage.WaitOne();
                }
            }
        }

        protected virtual void FileInfoGet(Message msg)
        {
            OnFileInfoMessageReceived(msg);
        }

        protected virtual void MessageGet(Message msg)
        {
            OnMessageReceived(msg);
        }

        protected virtual void OfflineNotify(Message msg)
        {
            OnOfflineNotifyReceived(msg);
        }

        protected virtual void OnlineNotify(Message msg)
        {
            OnOnlineNotifyReceived(msg);
        }
    }
    #endregion

    #region 服务器的消息处理类
    public class ServerMessageProcess : MessageProcess
    {

        Dictionary<uint, Socket> ClientSocketDict;

        public void InitServerMessageProcess(UserInformation server,
            Queue<Message> msgQueue,
            Dictionary<uint, Socket> clients,
            Dictionary<uint, string> friends,
            ManualResetEvent mreMsg)
        {
            base.InitMessageProcess(server, friends, mreMsg, msgQueue);
            this.ClientSocketDict = clients;
        }
        protected override void FileInfoGet(Message msg)
        {
            switch (msg.ReceiverID)
            {
                case UserInformation.ServerID:
                    OnFileInfoMessageReceived(msg);
                    break;
                case UserInformation.GroupID:
                    OnFileInfoMessageReceived(msg);
                    SendFileInfoGroup(msg);
                    break;
                default:
                    SendFileInfoSingle(msg);
                    break;
            }
        }
        private void SendFileInfoGroup(Message msg)
        {
            byte[] data;
            lock (ClientSocketDict)
            {
                foreach (var client in ClientSocketDict)
                {

                    try
                    {
                        if (!client.Value.Connected)
                        {
                            continue;
                        }
                        data = MessageConvert.GetBytes(msg);
                        client.Value.Send(data, data.Length, SocketFlags.None);
                    }
                    catch (SocketException)
                    {
                        this.OnSocketErrorOccurred(client.Key);
                    }
                    catch (Exception ex)
                    {
                        this.OnMessageProcessErrorOccurred(ex);
                    }
                }
            }
        }
        private void SendFileInfoSingle(Message msg)
        {
            byte[] data;
            try
            {
                data = MessageConvert.GetBytes(msg);
                ClientSocketDict[msg.ReceiverID].Send(data, data.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                this.OnSocketErrorOccurred(msg.ReceiverID);
            }
            catch (Exception ex)
            {
                this.OnMessageProcessErrorOccurred(ex);
            }
        }

        protected override void MessageGet(Message msg)
        {
            this.OnMessageReceived(msg);
            switch (msg.ReceiverID)
            {
                case UserInformation.ServerID:
                    break;
                case UserInformation.GroupID:
                    SendMessageGroup(msg);
                    break;
                default:
                    SendMessageSingle(msg);
                    break;
            }
        }
        protected void SendMessageGroup(Message msg)
        {
            byte[] data;
            lock (ClientSocketDict)
            {
                foreach (var client in ClientSocketDict)
                {

                    try
                    {
                        if (!client.Value.Connected)
                        {
                            continue;
                        }
                        data = MessageConvert.GetBytes(msg);
                        client.Value.Send(data, data.Length, SocketFlags.None);
                    }
                    catch (SocketException)
                    {
                        this.OnSocketErrorOccurred(client.Key);
                    }
                    catch (Exception ex)
                    {
                        this.OnMessageProcessErrorOccurred(ex);
                    }
                }
            }


        }
        private void SendMessageSingle(Message msg)
        {
            byte[] data;
            try
            {
                data = MessageConvert.GetBytes(msg);
                ClientSocketDict[msg.ReceiverID].Send(data, data.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                this.OnSocketErrorOccurred(msg.ReceiverID);
            }
            catch (Exception ex)
            {
                this.OnMessageProcessErrorOccurred(ex);
            }
        }

        protected override void OfflineNotify(Message msg)
        {
            OnOfflineNotifyReceived(msg);
            byte[] data;
            lock (ClientSocketDict)
            {
                foreach (var client in ClientSocketDict)
                {
                    try
                    {
                        data = MessageConvert.GetBytes(msg);
                        ClientSocketDict[client.Key].Send(data, data.Length, SocketFlags.None);
                    }
                    catch (SocketException)
                    {
                        this.OnSocketErrorOccurred(client.Key);
                    }
                    catch (Exception ex)
                    {
                        this.OnMessageProcessErrorOccurred(ex);
                    }
                }
            }

        }
        protected override void OnlineNotify(Message msg)
        {
            if (msg.ReceiverID == UserInformation.GroupID)
            {
                OnOnlineNotifyReceived(msg);
                OnlineNotifyGroup(msg);
            }
            else
            {
                OnlineNotifySingle(msg);
            }

        }
        private void OnlineNotifySingle(Message msg)
        {
            byte[] data = MessageConvert.GetBytes(msg);
            try
            {
                ClientSocketDict[msg.ReceiverID].Send(data, data.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                this.OnSocketErrorOccurred(msg.ReceiverID);
            }
            catch (Exception ex)
            {
                this.OnMessageProcessErrorOccurred(ex);
            }
        }
        private void OnlineNotifyGroup(Message msg)
        {
            byte[] data;
            lock (ClientSocketDict)
            {
                foreach (var client in ClientSocketDict)
                {
                    if (client.Key == msg.SenderID)
                    {
                        continue;
                    }
                    try
                    {
                        data = MessageConvert.GetBytes(msg);
                        client.Value.Send(data, data.Length, SocketFlags.None);
                    }
                    catch (SocketException)
                    {
                        this.OnSocketErrorOccurred(client.Key);
                    }
                    catch (Exception ex)
                    {
                        this.OnMessageProcessErrorOccurred(ex);
                    }
                }
            }

        }
    }
    #endregion

    #region 客户端的消息处理类
    public class ClientMessageProcess : MessageProcess
    {
        public TcpClient client;
        public void InitClientMessageProcess(UserInformation selfInfo, Queue<Message> msgQueue, Dictionary<uint, string> friends, ManualResetEvent mreMsg)
        {
            base.InitMessageProcess(selfInfo, friends, mreMsg, msgQueue);
        }
    }
    #endregion

}


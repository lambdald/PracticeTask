using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
        Empty
    };
    public enum TargetType
    {
        Group,
        Single,
        Server
    };
    public struct MessageToTransmit
    {
        public MessageType MessageType;

        //发送方的信息
        public int SenderID;
        public string SenderIP;     //在消息类型为File的时候，IP和端口号接受方需要知道发送文件的Socket
        public int SenderPort;
        //接收方ID
        public int ReceiverID;

        public List<int> NotifyIDs;        //用于上下线的通知

        public string Content;      //消息类型为Message时，为聊天内容，消息类型为File时，为文件名字
    }

    public class Message
    {
        public static int MaxMessageSize = 2048;
        public enum MessageType
        {
            Message,
            Online,
            Offline,
            File,
            Empty
        };
        public enum TargetType
        {
            Group,
            Single,
            Server
        };
        private enum XMLElement
        {
            sender,
            target,
            receiver,
            content,
            filename,
        };
        private enum XMLAttribute
        {
            id,
            ip,
            port,
            filesize,
            packetcount,
        }
        public MessageType MsgType = MessageType.Empty;
        public UserInformation Sender;
        public UserInformation Receiver;
        public UserInformation Friend;
        public string Content;
        public Dictionary<int, UserInformation> Friends;

        public string XmlString;
        public byte[] data;
        public FileInformation FileInfo;

        public Message()
        {
            this.Sender = new UserInformation();
            this.Receiver = new UserInformation();
        }
        public Message(UserInformation sender, UserInformation receiver)
        {
            this.Sender = sender;
            this.Receiver = receiver;
        }

        /// <summary>
        /// Message类型的消息
        /// </summary>
        public Message(UserInformation sender, UserInformation receiver, string content)
        {
            this.MsgType = MessageType.Message;
            this.Sender = sender;
            this.Receiver = receiver;
            this.Content = content;
        }
        public Message(UserInformation sender, string content)
        {
            this.MsgType = MessageType.Message;
            this.Sender = sender;
            this.Content = content;
        }

        public Message(UserInformation receiver, MessageType msgtype, Dictionary<int, UserInformation> friends)
        {
            this.MsgType = msgtype;
            this.Receiver = receiver;
            this.Friends = friends;
        }
        public Message(UserInformation receiver, MessageType msgtype, UserInformation friend)
        {
            this.MsgType = msgtype;
            this.Receiver = receiver;
            this.Friend = friend;
        }
        public Message(MessageType msgType, UserInformation friend)
        {
            this.MsgType = msgType;
            this.Friend = friend;
        }
        public Message(UserInformation sender, UserInformation receiver, FileInformation fileinfo)
        {
            this.MsgType = MessageType.File;
            this.Sender = sender;
            this.FileInfo = fileinfo;
            this.Receiver = receiver;
        }

        public static byte[] GetMessageBytes(UserInformation sender, UserInformation receiver, string content)
        {
            string xmlmsg;
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter xtw = new XmlTextWriter(sw);

                xtw.WriteStartElement("message");

                xtw.WriteStartElement("sender");
                xtw.WriteAttributeString("id", sender.ID.ToString());
                xtw.WriteAttributeString("ip", sender.IP);
                xtw.WriteAttributeString("port", sender.Port.ToString());
                xtw.WriteString(sender.Name);
                xtw.WriteEndElement();

                xtw.WriteStartElement("receiver");
                xtw.WriteAttributeString("id", receiver.ID.ToString());
                xtw.WriteAttributeString("ip", receiver.IP);
                xtw.WriteAttributeString("port", receiver.Port.ToString());
                xtw.WriteString(receiver.Name);
                xtw.WriteEndElement();

                xtw.WriteStartElement("content");
                xtw.WriteString(content);
                xtw.WriteEndElement();

                xtw.WriteEndElement();//root

                xmlmsg = sw.ToString();
            }
            return GetBytes(xmlmsg);
        }
        public static byte[] GetOnlineNotifyBytes(UserInformation friend)
        {
            return GetNotifyBytes(friend, MessageType.Online);
        }
        public static byte[] GetOnlineNotifyBytes(UserInformation receiver, Dictionary<int, UserInformation> friends)
        {
            string xmlmsg;
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter xtw = new XmlTextWriter(sw);
                xtw.WriteStartElement("online");
                xtw.WriteAttributeString("num", friends.Count.ToString());
                foreach (var friend in friends)
                {
                    if (friend.Key == receiver.ID)
                    {
                        continue;
                    }
                    xtw.WriteStartElement("friend");
                    xtw.WriteAttributeString("id", friend.Value.ID.ToString());
                    xtw.WriteAttributeString("ip", friend.Value.IP);
                    xtw.WriteAttributeString("port", friend.Value.Port.ToString());
                    xtw.WriteString(friend.Value.Name);
                    xtw.WriteEndElement();
                }
                xtw.WriteEndElement();
                xmlmsg = sw.ToString();
            }
            return GetBytes(xmlmsg);

        }
        public static byte[] GetOfflineNotifyBytes(UserInformation friend)
        {
            return GetNotifyBytes(friend, MessageType.Offline);
        }
        private static byte[] GetNotifyBytes(UserInformation friend, MessageType type)
        {
            string xmlmsg;
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter xtw = new XmlTextWriter(sw);

                xtw.WriteStartElement(type.ToString().ToLower());

                xtw.WriteStartElement("friend");
                xtw.WriteAttributeString("id", friend.ID.ToString());
                xtw.WriteAttributeString("ip", friend.IP);
                xtw.WriteAttributeString("port", friend.Port.ToString());
                xtw.WriteString(friend.Name);
                xtw.WriteEndElement();

                xtw.WriteEndElement();

                xmlmsg = sw.ToString();
            }
            return GetBytes(xmlmsg);
        }
        public void ParseXmlString(byte[] data)
        {
            string xmlstring = XmlString = GetString(data);
            if (string.IsNullOrWhiteSpace(xmlstring))
                return;
            //Clear();
            using (StringReader sr = new StringReader(xmlstring))
            {
                XmlTextReader xtr = new XmlTextReader(sr);
                string element = string.Empty;
                xtr.Read();
                switch (xtr.Name)
                {
                    case "message":
                        MsgType = MessageType.Message;
                        ParseMessage(xtr);
                        break;
                    case "online":
                        MsgType = MessageType.Online;
                        ParseOnlineNotify(xtr);
                        break;
                    case "offline":
                        MsgType = MessageType.Offline;
                        ParseOfflineNotify(xtr);
                        break;
                    case "file":
                        MsgType = MessageType.File;
                        ParseFile(xtr);
                        break;
                    default:
                        break;
                }
            }
        }
        private void ParseMessage(XmlTextReader xtr)
        {
            this.Sender = new UserInformation();
            this.Receiver = new UserInformation();
            string element = string.Empty;
            while (xtr.Read())
            {
                switch (xtr.NodeType)
                {
                    case XmlNodeType.Element:
                        element = xtr.Name;
                        switch (element)
                        {
                            case "sender":
                                Int32.TryParse(xtr["id"], out this.Sender.ID);
                                Int32.TryParse(xtr["port"], out this.Sender.Port);
                                this.Sender.IP = xtr["ip"];
                                break;
                            case "receiver":
                                Int32.TryParse(xtr["id"], out this.Receiver.ID);
                                Int32.TryParse(xtr["port"], out this.Receiver.Port);
                                this.Receiver.IP = xtr["ip"];
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (element)
                        {
                            case "sender":
                                this.Sender.Name = xtr.Value;
                                break;
                            case "receiver":
                                this.Receiver.Name = xtr.Value;
                                break;
                            case "content":
                                this.Content = xtr.Value;
                                break;
                            default:
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        element = string.Empty;
                        break;
                }
            }
        }
        private void ParseOnlineNotify(XmlTextReader xtr)
        {
            this.Friends = new Dictionary<int, UserInformation>();
            UserInformation friend = null;
            string element = string.Empty;
            while (xtr.Read())
            {
                switch (xtr.NodeType)
                {
                    case XmlNodeType.Element:
                        element = xtr.Name;
                        if (element == "friend")
                        {
                            friend = new UserInformation();
                            Int32.TryParse(xtr["id"], out friend.ID);
                            Int32.TryParse(xtr["port"], out friend.Port);
                            friend.IP = xtr["ip"];
                        }
                        break;
                    case XmlNodeType.Text:
                        if (element == "friend")
                        {
                            friend.Name = xtr.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (!this.Friends.ContainsKey(friend.ID))
                        {
                            this.Friends.Add(friend.ID, friend);
                        }
                        element = string.Empty;
                        break;
                }
            }
        }
        private void ParseOfflineNotify(XmlTextReader xtr)
        {
            this.Friend = new UserInformation();
            string element = string.Empty;
            while (xtr.Read())
            {
                switch (xtr.NodeType)
                {
                    case XmlNodeType.Element:
                        element = xtr.Name;
                        if (element == "friend")
                        {
                            this.Friend = new UserInformation();
                            Int32.TryParse(xtr["id"], out this.Friend.ID);
                            Int32.TryParse(xtr["port"], out this.Friend.Port);
                            this.Friend.IP = xtr["ip"];
                        }
                        break;
                    case XmlNodeType.Text:
                        if (element == "friend")
                        {
                            this.Friend.Name = xtr.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        element = string.Empty;
                        break;
                }
            }
        }
        public static byte[] GetFileBytes(UserInformation sender, UserInformation receiver, FileInformation fileinfo)
        {
            string xmlmsg;
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter xtw = new XmlTextWriter(sw);

                xtw.WriteStartElement("file");

                xtw.WriteStartElement("sender");
                xtw.WriteAttributeString("id", sender.ID.ToString());
                xtw.WriteAttributeString("ip", sender.IP);
                xtw.WriteAttributeString("port", sender.Port.ToString());
                xtw.WriteString(sender.Name);
                xtw.WriteEndElement();

                xtw.WriteStartElement("receiver");
                xtw.WriteAttributeString("id", receiver.ID.ToString());
                xtw.WriteAttributeString("ip", receiver.IP);
                xtw.WriteAttributeString("port", receiver.Port.ToString());
                xtw.WriteString(receiver.Name);
                xtw.WriteEndElement();

                xtw.WriteStartElement("fileinfo");
                xtw.WriteAttributeString("size", fileinfo.FileSize.ToString());
                xtw.WriteAttributeString("blockcount", fileinfo.BlockCount.ToString());
                xtw.WriteString(fileinfo.FileName);
                xtw.WriteEndElement();

                xtw.WriteEndElement();//root

                xmlmsg = sw.ToString();
            }
            return GetBytes(xmlmsg);
        }
        private void ParseFile(XmlTextReader xtr)
        {
            FileInfo = new FileInformation();
            string element = string.Empty;
            while (xtr.Read())
            {
                switch (xtr.NodeType)
                {
                    case XmlNodeType.Element:
                        element = xtr.Name;
                        switch (element)
                        {
                            case "sender":
                                this.Sender.IP = xtr["ip"];
                                Int32.TryParse(xtr["id"], out this.Sender.ID);
                                Int32.TryParse(xtr["port"], out this.Sender.Port);
                                break;
                            case "receiver":
                                this.Receiver.IP = xtr["ip"];
                                Int32.TryParse(xtr["id"], out this.Receiver.ID);
                                Int32.TryParse(xtr["port"], out this.Receiver.Port);
                                break;
                            case "fileinfo":
                                Int64.TryParse(xtr["size"], out this.FileInfo.FileSize);
                                Int64.TryParse(xtr["blockcount"], out this.FileInfo.BlockCount);
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (element)
                        {
                            case "sender":
                                this.Sender.Name = xtr.Value;
                                break;
                            case "receiver":
                                this.Receiver.Name = xtr.Value;
                                break;
                            case "fileinfo":
                                this.FileInfo.FileName = xtr.Value;
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        element = string.Empty;
                        break;
                }
            }
        }
        private void Clear()
        {
            MsgType = MessageType.Empty;
            Sender = null;
            Receiver = null;
            Content = string.Empty;
            Friends = null;
            FileInfo = null;
        }
        public static byte[] GetBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
        public static string GetString(byte[] b)
        {
            return Encoding.UTF8.GetString(b);
        }
        public byte[] Relay()
        {
            switch (this.MsgType)
            {
                case MessageType.Message:
                    return GetMessageBytes(this.Sender, this.Receiver, this.Content);
                case MessageType.Online:
                    return GetOnlineNotifyBytes(this.Receiver, this.Friends);
                case MessageType.Offline:
                    return GetOfflineNotifyBytes(this.Friend);
                case MessageType.File:
                    return GetFileBytes(this.Sender, this.Receiver, this.FileInfo);
                default:
                    return new byte[0];
            }
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
        public int ID;
        public SocketErrorArgs(int id)
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
        public Dictionary<int, UserInformation> Friends;
        public UserInformation HostInfo;
        public MessageProcess(UserInformation HostInfo, Dictionary<int, UserInformation> friends, ManualResetEvent mreMsg, Queue<Message> msgQueue)
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
        public virtual void OnSocketErrorOccurred(int id)
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
                    switch (msg.MsgType)
                    {
                        case Message.MessageType.Online:
                            this.OnlineNotify(msg);
                            break;
                        case Message.MessageType.Offline:
                            this.OfflineNotify(msg);
                            break;
                        case Message.MessageType.Message:
                            this.MessageGet(msg);
                            break;
                        case Message.MessageType.File:
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

        Dictionary<int, Socket> ClientSocketDict;

        public ServerMessageProcess(UserInformation server,
            Queue<Message> msgQueue,
            Dictionary<int, Socket> clients,
            Dictionary<int, UserInformation> friends,
            ManualResetEvent mreMsg) :
            base(server, friends, mreMsg, msgQueue)
        {
            this.ClientSocketDict = clients;
        }
        protected override void FileInfoGet(Message msg)
        {
            switch (msg.Receiver.ID)
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
                        data = Message.GetFileBytes(msg.Sender, msg.Receiver, msg.FileInfo);
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
                data = Message.GetFileBytes(msg.Sender, msg.Receiver, msg.FileInfo);
                ClientSocketDict[msg.Receiver.ID].Send(data, data.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                this.OnSocketErrorOccurred(msg.Receiver.ID);
            }
            catch (Exception ex)
            {
                this.OnMessageProcessErrorOccurred(ex);
            }
        }

        protected override void MessageGet(Message msg)
        {
            this.OnMessageReceived(msg);
            switch (msg.Receiver.ID)
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
                        data = Message.GetMessageBytes(msg.Sender, msg.Receiver, msg.Content);
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
                data = Message.GetMessageBytes(msg.Sender, msg.Receiver, msg.Content);
                ClientSocketDict[msg.Receiver.ID].Send(data, data.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                this.OnSocketErrorOccurred(msg.Receiver.ID);
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
                        data = Message.GetOfflineNotifyBytes(msg.Friend);
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
            if (msg.Receiver.ID == UserInformation.GroupID)
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
            byte[] data = Message.GetOnlineNotifyBytes(msg.Receiver, msg.Friends);
            try
            {
                ClientSocketDict[msg.Receiver.ID].Send(data, data.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                this.OnSocketErrorOccurred(msg.Receiver.ID);
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
                    if (client.Key == msg.Friend.ID)
                    {
                        continue;
                    }
                    try
                    {
                        if (!client.Value.Connected)
                        {
                            continue;
                        }
                        data = Message.GetOnlineNotifyBytes(msg.Friend);
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
        public ClientMessageProcess(UserInformation selfInfo, Queue<Message> msgQueue, Dictionary<int, UserInformation> friends, ManualResetEvent mreMsg) :
            base(selfInfo, friends, mreMsg, msgQueue)
        {
        }
    }
    #endregion
}


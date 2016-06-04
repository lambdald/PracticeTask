using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task4Lib
{
    public class SocketPacket
    {
        public UserInformation ClientInfo;
        public System.Net.Sockets.Socket currentSocket; // 当前的Socket
        public int clientNumber; // 客户号
        public byte[] dataBuffer = new byte[Message.MaxMessageSize]; // 发给服务器的数据
        // 构造函数
        public SocketPacket(System.Net.Sockets.Socket socket, UserInformation clientinfo)
        {
            currentSocket = socket;
            this.ClientInfo=clientinfo;
        }
    }
}

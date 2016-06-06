using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task4Lib
{
    public class SocketPacket
    {
        public uint ClientID;
        public System.Net.Sockets.Socket currentSocket; // 当前的Socket
        public byte[] dataBuffer = new byte[MessageConvert.MaxMessageSize]; // 数据
        // 构造函数
        public SocketPacket(System.Net.Sockets.Socket socket, uint clientID)
        {
            currentSocket = socket;
            this.ClientID=clientID;
        }
    }
}

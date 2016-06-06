using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task4Lib
{
    public class UserInformation
    {
        public const uint GroupID = int.MaxValue-1;
        public const uint ServerID = int.MaxValue;
        public string Name; //发送者名字
        public uint ID;  //发送者ID
        public string IP; //发送者IP
        public int Port;
        public UserInformation(string name,uint id,string ip,int port)
        {
            this.Name = name;
            this.ID = id;
            this.IP = ip;
            this.Port = port;
        }
        /// <summary>
        /// 群发
        /// </summary>
        /// <param name="name"></param>
        public UserInformation(uint id)
        {
            this.ID = id;
            this.IP = "";
            this.Port = -1;
        }
        public UserInformation()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task4Lib
{
    public class UserInformation
    {
        public const int GroupID = -1;
        public const int ServerID = int.MaxValue;
        public string Name; //发送者名字
        public int ID;  //发送者ID
        public string IP; //发送者IP
        public int Port;
        public UserInformation(string name,int id,string ip,int port)
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
        public UserInformation(int id)
        {
            this.ID = id;
            this.IP = "";
            this.Port = -1;
        }
        public UserInformation()
        {

        }
    }
    public class FileInformation
    {
        public string FileName;
        public long FileSize;
        public long BlockCount;
        public FileInformation(string filename, long filesize, long blockcount)
        {
            this.FileName = filename;
            this.FileSize = filesize;
            this.BlockCount = blockcount;
        }
        public FileInformation(string filename)
        {
            this.FileName = filename;
        }

        public FileInformation()
        {
            // TODO: Complete member initialization
        }
    }
}

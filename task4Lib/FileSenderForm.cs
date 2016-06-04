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

namespace task4Lib
{
    public delegate void SimpleDelegate();
    public partial class FileSenderForm : Form
    {
        FileSender task;
        Thread fileSend;
        public FileSenderForm(Socket socket, string fileName)
        {
            InitializeComponent();

            task = new FileSender();
            task.FullFileName = fileName;
            task.EnabledIOBuffer = true;
            task.BlockFinished += new BlockFinishedEventHandler(file_BlockFinished);
            task.ConnectLost += new EventHandler(file_ConnectLost);
            task.ErrorOccurred += new FileTransmissionErrorOccurEventHandler(file_ErrorOccurred);
            task.AllFinished += new EventHandler(file_AllFinished);
            fileSend = new Thread(task.Listen);
            fileSend.IsBackground = true;
            fileSend.Start(socket);
        }
        private void file_AllFinished(object sender, EventArgs e)
        {
            if (InvokeRequired)
                this.BeginInvoke(new SimpleDelegate(this.AllFinished));
            else
                this.AllFinished();
        }
        private void AllFinished()
        {
            this.Text = "传输完成！";
            this.btnHide.Enabled = false;
           
            this.transmitProgress.Value =  this.transmitProgress.Maximum;
            if (!this.Visible)
            {
                this.Show();
            }
        }
        private void file_ErrorOccurred(object sender, FileTransmissionErrorOccurEventArgs e)
        {
            if (e.InnerException is IOException)
            {
                if (MessageBox.Show(e.InnerException.Message, "IO异常", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                {
                    e.Continue = false;
                }
                else
                    e.Continue = true;
            }
            else
                MessageBox.Show(e.InnerException.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (InvokeRequired)
                this.Invoke(new SimpleDelegate(this.ErrorOccurred));
            else
                this.ErrorOccurred();
        }

        private void ErrorOccurred()
        {

        }

        private void file_ConnectLost(object sender, EventArgs e)
        {
            MessageBox.Show("连接中断", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (InvokeRequired)
                this.Invoke(new SimpleDelegate(this.ConnectLost));
            else
                this.ConnectLost();
        }
        private void ConnectLost()
        {
        }

        delegate void Delegate_Progress(FileTransmission task);
        private void file_BlockFinished(object sender, BlockFinishedEventArgs e)
        {
            FileTransmission task = (FileTransmission)sender;
            if (InvokeRequired)
                this.Invoke(new Delegate_Progress(SetProgress), task);
            else
                SetProgress(task);
        }

        void SetProgressBar(FileTransmission task)
        {
            this.transmitProgress.Maximum = task.Blocks.Count;
            this.transmitProgress.Value = task.Blocks.CountValid;
        }
        void SetProgress(FileTransmission task)
        {
            this.Text = "发送端 上传中";
            SetProgressBar(task);
            this.lblProgress.Text = string.Format("进度:{0:N2}%   总长度:{1}   已完成:{2}", task.Progress, task.TotalSize, task.FinishedSize);
            this.lblSpeed.Text = string.Format("平均速度:{0:N2}KB/s", task.KByteAverSpeed);
            this.lblTime.Text = string.Format("已用时:{0}  估计剩余时间:{1}", task.TimePast, task.TimeRemaining);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (task != null)
                task.Stop(true);
            this.Close();
        }
    }
}

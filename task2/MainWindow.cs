using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task2
{
    public partial class MainWindow : Form
    {
        DateTime datetime = DateTime.Now;
        Font font;
        SolidBrush drawBrush;
        //System.Timers.Timer timer;

        Image img;
        Bitmap bitmap;
        public MainWindow()
        {
            InitializeComponent();

            //timer = new System.Timers.Timer(1000);
            font = new Font(new FontFamily("宋体"), 9);
            drawBrush = new SolidBrush(Color.Black);
            // this.lblTime.Text = datetime.ToString("yyyy年MM月dd日  dddd  HH时mm分ss秒");
            // this.thUpdateTime.Start();
        }

        public void UpdateTime(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        private void btnFontDialog_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                font = fontDialog.Font;
                drawBrush = new SolidBrush(fontDialog.Color);
                this.picBoxDatetime.Invalidate();
            }

        }

        private void picBoxDatetime_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (bitmap != null)
            {
                g.DrawImage(bitmap, 0, 0);
            }
            g.DrawString(datetime.ToString("yyyy年MM月dd日  dddd  HH时mm分ss秒"), font, drawBrush, 0, 0);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (datetimePicker.Checked)
            {
                datetime = datetimePicker.Value;
                this.picBoxDatetime.Invalidate();
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                try
                {
                    using (FileStream fsImg = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        img = Image.FromStream(fsImg);
                    }
                }
                catch (FileNotFoundException exception)
                {
                    MessageBox.Show(exception.ToString());
                    Application.Exit();
                }
                bitmap = new Bitmap(img, this.picBoxDatetime.Width, this.picBoxDatetime.Height);
                this.picBoxDatetime.Invalidate();
            }
        }

        private void UpdateTime_Tick(object sender, EventArgs e)
        {
            datetime = datetime.AddSeconds(1);
            this.picBoxDatetime.Invalidate();
        }
    }
}

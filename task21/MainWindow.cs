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

namespace task21
{
    public partial class MainWindow : Form
    {
        DateTime datetime = DateTime.Now;
        Font font;
        SolidBrush drawBrush;
        Pen pen;

        Point pntClock;
        Point pntClockTop;
        int iRadiusOfClock = 100;
        Point pntHourHand;
        Point pntMinuteHand;
        Point pntSecondHand;
        Image img;
        Bitmap bitmap;
        public MainWindow()
        {
            InitializeComponent();

            font = new Font(new FontFamily("宋体"), 15);
            drawBrush = new SolidBrush(Color.Black);
            pntClock = new Point(700, 200);
            pntClockTop = new Point(700, 200 - iRadiusOfClock);

            pen = new Pen(Color.Black);

        }

        private void btnFontDialog_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
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
            g.DrawString(datetime.ToString("yyyy年MM月dd日  dddd HH:mm:ss"), font, drawBrush, 0, 0);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen.Color = Color.Red;
            pen.Width = 8;
            g.DrawEllipse(pen, pntClock.X - iRadiusOfClock, pntClock.Y - iRadiusOfClock, iRadiusOfClock * 2, iRadiusOfClock * 2);
            
            //画时钟
            pen.Color = Color.Black;
            double sin_angle, cos_angle;
            //时针
            sin_angle = Math.Sin(datetime.Hour / 12.0 * Math.PI * 2.0);
            cos_angle = Math.Cos(datetime.Hour / 12.0 * Math.PI * 2.0);
            pntHourHand = new Point(pntClock.X + Convert.ToInt32(iRadiusOfClock*0.5 * sin_angle)
                , pntClock.Y - Convert.ToInt32(iRadiusOfClock*0.5 * cos_angle));
            pen.Width = 6;
            g.DrawLine(pen, pntClock, pntHourHand);
            //分针
            sin_angle = Math.Sin(datetime.Minute / 60.0 * Math.PI * 2.0);
            cos_angle = Math.Cos(datetime.Minute / 60.0 * Math.PI * 2.0);
            pntMinuteHand = new Point(pntClock.X + Convert.ToInt32(iRadiusOfClock*0.7 * sin_angle)
                , pntClock.Y - Convert.ToInt32(iRadiusOfClock*0.7 * cos_angle));
            pen.Width = 4;
            g.DrawLine(pen, pntClock, pntMinuteHand);
            //秒针
            sin_angle = Math.Sin(datetime.Second / 60.0 * Math.PI * 2.0);
            cos_angle = Math.Cos(datetime.Second / 60.0 * Math.PI * 2.0);
            pntSecondHand = new Point(pntClock.X + Convert.ToInt32(iRadiusOfClock*0.9 * sin_angle)
                , pntClock.Y - Convert.ToInt32(iRadiusOfClock*0.9 * cos_angle));
            pen.Width = 2;
            g.DrawLine(pen, pntClock, pntSecondHand);
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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
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

        private void btnTimeNow_Click(object sender, EventArgs e)
        {
            datetime = DateTime.Now;
            this.picBoxDatetime.Invalidate();
        }

        private void btnDefaultFont_Click(object sender, EventArgs e)
        {
            font = new Font(new FontFamily("宋体"), 15);
            drawBrush = new SolidBrush(Color.Black);
            this.picBoxDatetime.Invalidate();            
        }

        private void btnDefaultImg_Click(object sender, EventArgs e)
        {
            img = null;
            bitmap = null;
            this.picBoxDatetime.Invalidate();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace task1
{
    public partial class MainWindow : Form
    {

        private string sImgFileName = @"..\..\..\source\location.png";
        private string sTxtFileName = @"..\..\..\source\四个点.txt";

        //'+'标识符的大小
        private int iSizeOfSign = 30;

        //四个点坐标
        private Point[] pntArrOrignSign = new Point[4];     //读入的原始坐标
        private Point[] pntArrSign = new Point[4];          //拖动后的坐标
        private Point[] pntArrRelativeSign = new Point[4];  //拖动后的坐标按窗口和图片比例变换

        //读入的照片
        private Image img;
        //比例缩放照片
        private Bitmap bitmap;

        //存储当前拖动的点
        private int selectedPoint = 4;

        public MainWindow()
        {
            InitializeComponent();
            this.Resize += new EventHandler(this.Form_Resize);
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.ShowImgAndPlus_Paint);
            this.pictureBox.MouseMove += Form_MouseMove;
            this.pictureBox.MouseUp += Form_MouseUp;
            this.pictureBox.MouseDown += Form_MouseDown;

            LoadImgFile();
            ReadPointFromFile();
        }

        /// <summary>
        /// 计算两个点的距离
        /// </summary>
        /// <param name="a">第一个点</param>
        /// <param name="b">第二个点</param>
        /// <returns></returns>
        public double CalcDistance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }


        /// <summary>
        /// 当鼠标点击时，记录当前点击的点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                if (CalcDistance(new Point(e.X, e.Y), pntArrRelativeSign[i]) < iSizeOfSign)
                {
                    selectedPoint = i;
                }
            }
        }
        /// <summary>
        /// 鼠标释放后，需要将鼠标变成箭头，并更新坐标数组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Arrow;
            for (int i = 0; i < 4; i++)
            {
                pntArrSign[i].X = pntArrRelativeSign[i].X * img.Width / this.pictureBox.Width;
                pntArrSign[i].Y = pntArrRelativeSign[i].Y * img.Height / this.pictureBox.Height;
            }
            selectedPoint = 4;
        }

        /// <summary>
        /// 实现拖动‘+’
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPoint == 4)
                return;
            ((Control)sender).Cursor = Cursors.SizeAll;
            Point mousePos = ((Control)sender).Parent.PointToClient(Control.MousePosition);
            //防止将'+'拖出窗体
            if (mousePos.X <= 0)
            {
                mousePos.X = 0;
            }
            if (mousePos.X >= this.pictureBox.Width)
            {
                mousePos.X = this.pictureBox.Width;
            }
            if (mousePos.Y <= 0)
            {
                mousePos.Y = 0;
            }
            if (mousePos.Y >= this.pictureBox.Height)
            {
                mousePos.Y = this.pictureBox.Height;
            }
            pntArrRelativeSign[selectedPoint] = new Point(mousePos.X, mousePos.Y);

            ((Control)sender).Invalidate();
        }

        /// <summary>
        /// 载入图片文件
        /// </summary>
        public void LoadImgFile()
        {
            try
            {
                using (FileStream fsImg = new FileStream(sImgFileName,
                    FileMode.Open, FileAccess.Read))
                {
                    img = Image.FromStream(fsImg);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.ToString());
                Application.Exit();
            }
            bitmap = new Bitmap(img, this.pictureBox.Width, this.pictureBox.Height);
        }


        /// <summary>
        /// 从txt文件中读取四个点
        /// </summary>
        public void ReadPointFromFile()
        {
            byte[] bArrTxt = null;
            try
            {
                using (FileStream fsTxt = new FileStream(sTxtFileName,
                    FileMode.Open, FileAccess.Read))
                {
                    bArrTxt = new byte[fsTxt.Length + 10];
                    int numBytesToRead = (int)fsTxt.Length;
                    int numBytesRead = 0;
                    do
                    {
                        int n = fsTxt.Read(bArrTxt, numBytesRead, 10);
                        numBytesRead += n;
                        numBytesToRead -= n;
                    } while (numBytesToRead > 0);
                }

            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.ToString());
                Application.Exit();
            }
            string strPoint = Encoding.Default.GetString(bArrTxt);

            string regex = @"\d+";
            Regex reg = new Regex(regex, RegexOptions.IgnorePatternWhitespace);
            Match match = reg.Match(strPoint);

            for (int i = 0; i < 4; i++)
            {
                pntArrOrignSign[i] = new Point();
                pntArrRelativeSign[i] = new Point();

                if (match.Success)
                {
                    pntArrOrignSign[i].X = Convert.ToInt32(match.Value);
                    match = match.NextMatch();
                }
                if (match.Success)
                {
                    pntArrOrignSign[i].Y = Convert.ToInt32(match.Value);
                    match = match.NextMatch();
                }
                pntArrSign[i] = new Point(pntArrOrignSign[i].X, pntArrOrignSign[i].Y);
            }
            CovertSignPoint();
        }


        /// <summary>
        /// 窗口重绘，绘制图片和四个十字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowImgAndPlus_Paint(object sender, PaintEventArgs e)
        {
            Graphics gdi = e.Graphics;
            gdi.DrawImage(bitmap, 0, 0);

            Pen pen = new Pen(Color.White);
            gdi.DrawLine(pen, pntArrRelativeSign[0], pntArrRelativeSign[1]);
            gdi.DrawLine(pen, pntArrRelativeSign[2], pntArrRelativeSign[3]);
            gdi.DrawLine(pen, pntArrRelativeSign[0], pntArrRelativeSign[2]);
            gdi.DrawLine(pen, pntArrRelativeSign[1], pntArrRelativeSign[3]);

            //绘制十字
            pen.Color = Color.Red;
            for (int i = 0; i < 4; i++)
            {
                gdi.DrawLine(pen, pntArrRelativeSign[i].X - iSizeOfSign / 2, pntArrRelativeSign[i].Y
                          , pntArrRelativeSign[i].X + iSizeOfSign / 2, pntArrRelativeSign[i].Y);
                gdi.DrawLine(pen, pntArrRelativeSign[i].X, pntArrRelativeSign[i].Y - iSizeOfSign / 2
                           , pntArrRelativeSign[i].X, pntArrRelativeSign[i].Y + iSizeOfSign / 2);
            }
        }

        /// <summary>
        /// 坐标的比例变换
        /// </summary>
        public void CovertSignPoint()
        {
            for (int i = 0; i < 4; i++)
            {
                pntArrRelativeSign[i].X = pntArrSign[i].X * this.pictureBox.Width / img.Width;
                pntArrRelativeSign[i].Y = pntArrSign[i].Y * this.pictureBox.Height / img.Height;
            }
        }

        /// <summary>
        /// 当大小改变时，图片需要重新缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Resize(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            bitmap = new Bitmap(img, this.pictureBox.Width, this.pictureBox.Height);
            CovertSignPoint();
        }
    }
}

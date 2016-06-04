using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * 
 * 1. 将附件图片利用文件流的方式读取并显示在界面上；
 * 2. 将附件txt中的内容读出，里面是四个点的坐标，请将这四个点绘制到显示的图片上，对应的点为了观看方便用“+”显示；
 * 3. 将四个点中分别两两绘制一条直线，形成一个四边形；
 * 4. 用鼠标可以拖动四个点，拖动时对应点的“+”随之移动，直线也做相应的变化。
 * 
 * */

namespace task1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}

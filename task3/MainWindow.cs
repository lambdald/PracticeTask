using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task3
{
    public partial class MainWindow : Form
    {
        public delegate void FolderDelegate(DirectoryInfo dir);
        private string path;    //文件夹路径
        DirectoryInfo selectedFolder;   //文件夹
        DirectoryInfo[] subDirectories; //子文件夹
        List<FileInfo> fileList;    //所有文件
        public MainWindow()
        {
            InitializeComponent();
            fileList=new List<FileInfo>();
            listViewFolder.Columns.Add("文件列表", listViewFolder.Width);
        }
        //通过修改时间比较文件
        public static int CompareFileByTime(FileInfo file1,FileInfo file2)
        {
            return file1.LastWriteTime.CompareTo(file2.LastWriteTime);
        }
        //通过比较扩展名比较文件
        public static int CompareFileByExtension(FileInfo file1,FileInfo file2)
        {
            return file1.Extension.CompareTo(file2.Extension);
        }
        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            if(chooseFolder.ShowDialog()==DialogResult.OK)
            {
                path = chooseFolder.SelectedPath;
                folder.Text = path;
                selectedFolder = new DirectoryInfo(path);
                fileList.Clear();
                FolderTree(selectedFolder);
                UpdateData();
            }
        }
        //递归遍历文件夹
        public void FolderTree(DirectoryInfo dir)
        {
            fileList.AddRange(dir.EnumerateFiles());
            subDirectories = dir.GetDirectories();
            foreach(DirectoryInfo subdir in subDirectories)
            {
                FolderTree(subdir);
            }
        }
        //更新ListView
        public void UpdateData()
        {
            listViewFolder.Clear();
            listViewFolder.Columns.Add("文件列表", listViewFolder.Width / 5 * 4);
            listViewFolder.Columns.Add("修改时间", listViewFolder.Width / 5);
            if (radioBtnTime.Checked)
            {
                listViewFolder.Groups.Clear();
                fileList.Sort(CompareFileByTime);

                foreach (FileInfo file in fileList)
                {
                    ListViewItem listItem = new ListViewItem();
                    listItem.Text = file.Name;
                    listItem.SubItems.Add(file.LastWriteTime.ToShortDateString());
                    listViewFolder.Items.Add(listItem);
                }
            }
            else if (radioBtnType.Checked)
            {
                fileList.Sort(CompareFileByExtension);
                string ext = null;
                ListViewGroup group = new ListViewGroup();
                foreach (FileInfo file in fileList)
                {
                    if (ext != file.Extension)
                    {
                        ext = file.Extension;
                        group = new ListViewGroup(ext);
                    }
                    listViewFolder.Groups.Add(group);
                    ListViewItem listItem = new ListViewItem(group);
                    listItem.Text = file.Name;
                    listItem.SubItems.Add(file.LastWriteTime.ToShortDateString());
                    listViewFolder.Items.Add(listItem);
                }
            }
            else
            {
                foreach (FileInfo file in fileList)
                {
                    ListViewItem listItem = new ListViewItem();
                    listItem.Text = file.Name;
                    listItem.SubItems.Add(file.LastWriteTime.ToShortDateString());
                    listViewFolder.Items.Add(listItem);
                }
            }
        }
        private void radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if(selectedFolder==null)
            {
                return;
            }
            UpdateData();
        }
    }
}

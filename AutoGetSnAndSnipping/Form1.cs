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
using System.Runtime.InteropServices;

namespace AutoGetSnAndSnipping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //监控的文件夹路径

            string strPath = @"C:\classifiedDefects\data";
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            this.fileSystemWatcher1.Path = @"C:\classifiedDefects\data";
            ContextMenuStrip listboxMenu = new ContextMenuStrip();
            ToolStripMenuItem rightMenu = new ToolStripMenuItem("截图");
            ToolStripMenuItem rightMenu2 = new ToolStripMenuItem("复制Sn");
            rightMenu.Click += new EventHandler(Copy_Click);
            rightMenu2.Click += new EventHandler(Copy_Click2);
            listboxMenu.Items.AddRange(new ToolStripItem[] { rightMenu,rightMenu2 });
            listBox2.ContextMenuStrip = listboxMenu;
            this.textBoxLine.Text = "R12";
            this.textBoxSN.Text = "1PL0123445";
            this.textBoxMachine.Text = "V810-8086S2";
            this.textBox2.Text = "";
            this.Text = "截图自动V2.4.5 [20201212]";
        }













        private void Copy_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(listBox2.Items[listBox2.SelectedIndex].ToString().Trim());
            string sn = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[0];
            string lineStr = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[1];
            string machineStr = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[2];
            SaveImageAndDelety(sn,lineStr,machineStr);
        }
        private void Copy_Click2(object sender, EventArgs e)
        {
            string sn = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[0];
            Clipboard.SetDataObject(sn);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtBoxPath.Text = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良";
            AppHotKey.RegisterHotKey(Handle, 101, AppHotKey.KeyModifiers.Ctrl | AppHotKey.KeyModifiers.Alt, Keys.A);
            this.radioButton2.Checked = true;
        }



        /// <summary>
        /// 保存图片延迟带子文件夹
        /// </summary>
        /// <param name="path"></param>
        private void SaveImageAndPathDelayAndChild(string path, string snNumber2, string machineStr, string childfolder)
        {
            string txtboxDelay = this.txtboxDelay.Text.Trim();
            int txtboxDelayint = int.Parse(txtboxDelay);
            //获得当前屏幕的分辨率            
            Screen scr = Screen.PrimaryScreen;
            Rectangle rc = scr.Bounds;
            int iWidth = rc.Width;
            int iHeight = rc.Height;
            //创建一个和屏幕一样大的Bitmap            
            Image myImage = new Bitmap(iWidth - 100, iHeight - 105);
            //从一个继承自Image类的对象中创建Graphics对象            
            Graphics g = Graphics.FromImage(myImage);
            //抓屏并拷贝到myimage里            
            this.Hide();
            Thread.Sleep(txtboxDelayint);
            g.CopyFromScreen(new Point(100, 40), new Point(0, 0), new Size(iWidth - 100, iHeight - 105));
            this.Show();
            //水印内容
            // string waterText = "Designed By 5DX Team " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // Font font = new Font("宋体", 25);
            //Font font1 = new Font("宋体", 18);
            //  Font font2 = new Font("新宋体", 14);
            //用于确定水印的大小
            //  SizeF zisizeF = new SizeF();
            //  zisizeF = g.MeasureString(waterText, font);
            //亮度，红色，绿色，蓝色
            // SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 76, 255, 43));
            // SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(255, 255, 0, 0));
            //  g.TranslateTransform(0, iHeight);
            //以原点为中心 转 -45度
            //g.RotateTransform(-45);
            //水印
            // g.DrawString(waterText, font2, solidBrush, new PointF(iWidth / 2 + 180, iHeight / 2 + 300));
            //g.RotateTransform(45);
            //左下角
            g.DrawImage(GetChinaStyleNormal(), -8, 875);
            //    GetChinaStyleTopLeft()
            //                int angle = -270;
            //              g.TranslateTransform(110, 0);
            //        GetChinaStyleTopRight()
            //                int angle = -180;
            //              g.TranslateTransform(110, 110);
            //GetChinaStyleBottomRight()
            //              int angle = -90;
            //g.TranslateTransform(0, 110);
            // 左上角
            g.DrawImage(GetChinaStyleRandAngle(-270, 110, 0), -10, -8);
            //右上角
            g.DrawImage(GetChinaStyleRandAngle(-180, 110, 110), 1717, -10);
            //右下角
            g.DrawImage(GetChinaStyleRandAngle(-90, 0, 110), 1719, 873);
            //画四条粗红线
            Pen MyPen = new Pen(Color.FromArgb(203, 28, 43), 4);
            g.DrawLine(MyPen, 2, 84, 2, 885);   //左边竖线
            g.DrawLine(MyPen, 1817, 40, 1817, 890);   //右边竖线
            g.DrawLine(MyPen, 88, 2, 1735, 2);   //上边横线
            g.DrawLine(MyPen, 88, 973, 1735, 973);   //下边横线
            TestOnSeal _top = new TestOnSeal();
            _top.TextFont = new Font("黑体", 16, FontStyle.Bold);
            _top.FillColor = Color.Red;
            //_top.ColorTOP = Color.Black;
            _top.Text = "广达(上海)制造城";
            _top.BaseString = "5DX专用";
            _top.ShowPath = true;
            _top.LetterSpace = 1;
            _top.SealSize = 180;
            _top.CharDirection = Char_Direction.Center;
            _top.SetIndent(20);
            g.DrawImage(_top.TextOnPathBitmap(), 1600, 780);
            _top.SetIndent(20);
            //   g.DrawImage(_top.TextOnPathBitmap(), 180, 180);
            //  pictureBox1.Image = _top.TextOnPathBitmap();
            g.Dispose();
            string parentPath = this.txtBoxPath.Text.Trim();
            string timefolder = DateTime.Now.ToString("mmddss");
            string timefolder2 = DateTime.Now.ToString("MMdd");
            DateTime dt = DateTime.Now;
            string dateStr1 = "20:00:00";
            string dateStr2 = dt.ToString("HH:mm:ss");
            DateTime t1 = Convert.ToDateTime(dateStr1);
            DateTime t2 = Convert.ToDateTime(dateStr2);
            int compNum = DateTime.Compare(t1, t2);
            if (compNum > 0)
            {
                //继续创建第二天（当天的）的文件夹，并且往里面加上图片
                string saveRealPath = parentPath + "\\" + path + "\\" + timefolder2 + "\\" + childfolder + "\\" + snNumber2.ToUpper() + "__" + machineStr + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + path + "\\" + timefolder2 + "\\" + childfolder;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);


                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snNumber2.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snNumber2.Trim().ToUpper());
                    tw.Close();
                }
                

            }
            if (compNum == 0)
            {
            }
            if (compNum < 0)
            {
                //创建第二天的文件夹，并且往里面加上图片
                string tommorm = DateTime.Now.AddDays(+1).ToString("MMdd");
                string saveRealPath = parentPath + "\\" + path + "\\" + tommorm + "\\" + childfolder + "\\" + snNumber2.ToUpper() + "__" + machineStr + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + path + "\\" + tommorm + "\\" + childfolder;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);


                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snNumber2.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snNumber2.Trim().ToUpper());
                    tw.Close();
                }
                

            }
        }







        private Bitmap GetChinaStyleNormal()
        {
            Bitmap bt = new Bitmap(110, 110);
            Graphics g = Graphics.FromImage(bt);
            Pen MyPen = new Pen(Color.FromArgb(203, 28, 43), 4);
            Pen MyPen1 = new Pen(Color.Black, 4);
            g.DrawLine(MyPen, 10, 10, 10, 60);
            g.DrawLine(MyPen, 8, 62, 30, 62);
            g.DrawLine(MyPen, 28, 60, 28, 98);
            g.DrawLine(MyPen, 26, 98, 38, 98);
            g.DrawLine(MyPen, 38, 100, 38, 87);
            g.DrawLine(MyPen, 40, 87, 10, 87);
            g.DrawLine(MyPen, 10, 85, 10, 99);
            g.DrawLine(MyPen, 8, 98, 23, 98);
            g.DrawLine(MyPen, 21, 99, 21, 68);
            g.DrawLine(MyPen, 21, 70, 8, 70);
            g.DrawLine(MyPen, 10, 70, 10, 82);
            g.DrawLine(MyPen, 8, 80, 48, 80);
            g.DrawLine(MyPen, 46, 80, 46, 100);
            g.DrawLine(MyPen, 46, 98, 96, 98);
            Pen MyPen2 = new Pen(Color.FromArgb(203, 28, 43), 2);
            g.DrawLine(MyPen2, 16, 36, 16, 56);
            g.DrawLine(MyPen2, 15, 56, 34, 56);
            g.DrawLine(MyPen2, 34, 55, 34, 75);
            g.DrawLine(MyPen2, 33, 75, 53, 75);
            g.DrawLine(MyPen2, 52, 75, 52, 92);
            g.DrawLine(MyPen2, 51, 92, 70, 92);
            g.Dispose();
            return bt;
        }


        private Bitmap GetChinaStyleRandAngle(int angle, int X, int Y)
        {
            Bitmap bt = new Bitmap(110, 110);
            Graphics g = Graphics.FromImage(bt);
            g.TranslateTransform(X, Y);
            g.RotateTransform(angle);
            Pen MyPen = new Pen(Color.FromArgb(203, 28, 43), 4);
            Pen MyPen1 = new Pen(Color.Black, 4);
            g.DrawLine(MyPen, 10, 10, 10, 60);
            g.DrawLine(MyPen, 8, 62, 30, 62);
            g.DrawLine(MyPen, 28, 60, 28, 98);
            g.DrawLine(MyPen, 26, 98, 38, 98);
            g.DrawLine(MyPen, 38, 100, 38, 87);
            g.DrawLine(MyPen, 40, 87, 10, 87);
            g.DrawLine(MyPen, 10, 85, 10, 99);
            g.DrawLine(MyPen, 8, 98, 23, 98);
            g.DrawLine(MyPen, 21, 99, 21, 68);
            g.DrawLine(MyPen, 21, 70, 8, 70);
            g.DrawLine(MyPen, 10, 70, 10, 82);
            g.DrawLine(MyPen, 8, 80, 48, 80);
            g.DrawLine(MyPen, 46, 80, 46, 100);
            g.DrawLine(MyPen, 46, 98, 96, 98);
            Pen MyPen2 = new Pen(Color.FromArgb(203, 28, 43), 2);
            g.DrawLine(MyPen2, 16, 36, 16, 56);
            g.DrawLine(MyPen2, 15, 56, 34, 56);
            g.DrawLine(MyPen2, 34, 55, 34, 75);
            g.DrawLine(MyPen2, 33, 75, 53, 75);
            g.DrawLine(MyPen2, 52, 75, 52, 92);
            g.DrawLine(MyPen2, 51, 92, 70, 92);
            g.Dispose();
            return bt;
        }











        /// <summary>
        /// 通用截图API方法的的封装
        /// </summary>
        /// <param name="snnumber">sn</param>
        /// <param name="lineStr">线别</param>
        private void SaveImageAndDelety(string snnumber, string lineStr,string machineStr)
        {
            string txtboxDelay = this.txtboxDelay.Text.Trim();
            int txtboxDelayint = int.Parse(txtboxDelay);
            //获得当前屏幕的分辨率            
            Screen scr = Screen.PrimaryScreen;
            Rectangle rc = scr.Bounds;
            int iWidth = rc.Width;
            int iHeight = rc.Height;
            //创建一个和屏幕一样大的Bitmap            
            Image myImage = new Bitmap(iWidth-100, iHeight-105);
            //从一个继承自Image类的对象中创建Graphics对象            
            Graphics g = Graphics.FromImage(myImage);
            //抓屏并拷贝到myimage里            
            this.Hide();
            Thread.Sleep(txtboxDelayint);
            g.CopyFromScreen(new Point(100, 40), new Point(0, 0), new Size(iWidth-100, iHeight-105));
            this.Show();
            //水印内容
           // string waterText = "Designed By 5DX Team " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           // Font font = new Font("宋体", 25);
            //Font font1 = new Font("宋体", 18);
          //  Font font2 = new Font("新宋体", 14);
            //用于确定水印的大小
          //  SizeF zisizeF = new SizeF();
          //  zisizeF = g.MeasureString(waterText, font);
            //亮度，红色，绿色，蓝色
           // SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 76, 255, 43));
           // SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(255, 255, 0, 0));
          //  g.TranslateTransform(0, iHeight);
            //以原点为中心 转 -45度
            //g.RotateTransform(-45);
            //水印
           // g.DrawString(waterText, font2, solidBrush, new PointF(iWidth / 2 + 180, iHeight / 2 + 300));
            //g.RotateTransform(45);
            //左下角
            g.DrawImage(GetChinaStyleNormal(), -8, 875);
            //    GetChinaStyleTopLeft()
            //                int angle = -270;
            //              g.TranslateTransform(110, 0);
            //        GetChinaStyleTopRight()
            //                int angle = -180;
            //              g.TranslateTransform(110, 110);
            //GetChinaStyleBottomRight()
            //              int angle = -90;
            //g.TranslateTransform(0, 110);
            // 左上角
            g.DrawImage(GetChinaStyleRandAngle(-270, 110, 0), -10, -8);
            //右上角
            g.DrawImage(GetChinaStyleRandAngle(-180, 110, 110), 1717, -10);
            //右下角
            g.DrawImage(GetChinaStyleRandAngle(-90, 0, 110), 1719, 873);
            //画四条粗红线
            Pen MyPen = new Pen(Color.FromArgb(203, 28, 43), 4);
            g.DrawLine(MyPen, 2, 84, 2, 885);   //左边竖线
            g.DrawLine(MyPen, 1817, 40, 1817, 890);   //右边竖线
            g.DrawLine(MyPen, 88, 2, 1735, 2);   //上边横线
            g.DrawLine(MyPen, 88, 973, 1735, 973);   //下边横线
            TestOnSeal _top = new TestOnSeal();
            _top.TextFont = new Font("黑体", 16, FontStyle.Bold);
            _top.FillColor = Color.Red;
            //_top.ColorTOP = Color.Black;
            _top.Text = "广达(上海)制造城";
            _top.BaseString = "5DX专用";
            _top.ShowPath = true;
            _top.LetterSpace = 1;
            _top.SealSize = 180;
            _top.CharDirection = Char_Direction.Center;
            _top.SetIndent(20);
            g.DrawImage(_top.TextOnPathBitmap(), 1600, 780);
            _top.SetIndent(20);
            //   g.DrawImage(_top.TextOnPathBitmap(), 180, 180);
            //  pictureBox1.Image = _top.TextOnPathBitmap();
            g.Dispose();
            string parentPath = this.txtBoxPath.Text.Trim();
            string timefolder = DateTime.Now.ToString("mmddss");
            string timefolder2 = DateTime.Now.ToString("MMdd");
            DateTime dt = DateTime.Now;
            string dateStr1 = "20:00:00";
            string dateStr2 = dt.ToString("HH:mm:ss");
            DateTime t1 = Convert.ToDateTime(dateStr1);
            DateTime t2 = Convert.ToDateTime(dateStr2);
            int compNum = DateTime.Compare(t1, t2);
            if (compNum > 0)
            {
                //继续创建当天的文件夹，并且往里面加上图片
                string saveRealPath = parentPath + "\\" + lineStr + "\\" + timefolder2 + "\\" + snnumber.ToUpper() + "__" + machineStr + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + lineStr + "\\" + timefolder2;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                
            
            }
            if (compNum < 0)
            {
                //创建第二天的文件夹，并且往里面加上图片
                string tommorm = DateTime.Now.AddDays(+1).ToString("MMdd");
                string saveRealPath = parentPath + "\\" + lineStr + "\\" + tommorm + "\\" + snnumber.ToUpper() + "__" + machineStr + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + lineStr + "\\" + tommorm;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                
              
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //非空校验
            if (this.textBoxMachine.Text.Length != 0 && textBoxSN.Text.Length != 0 && textBoxLine.Text.Length != 0 && textBox2.Text.Length==0)
            {
                SaveImageAndDelety(textBoxSN.Text.Trim().ToUpper(), textBoxLine.Text.Trim(),textBoxMachine.Text.Trim());
            }
            else if (this.textBoxMachine.Text.Length != 0 && textBoxSN.Text.Length != 0 && textBoxLine.Text.Length != 0 && textBox2.Text.Length != 0)
            {
                SaveImageAndPathDelayAndChild(textBoxLine.Text.Trim(), textBoxSN.Text.Trim().ToUpper(), textBoxMachine.Text.Trim(), textBox2.Text.ToUpper());
            
            }
            else
            {
                MessageBox.Show("确保输入框有值");
            }
        }
        //成员变量放置处
        StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 文件创建的时候的执行的函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            string lineStr = "";
            if (e.FullPath.EndsWith("BoardStatus.txt"))
            {
                sb.Clear();
                string snTextPath = e.FullPath.ToString();
                Thread.Sleep(200);
                //Console.WriteLine(snTextPath); //C:\classifiedDefects\data\V810-3328S2EX[@$@]2020-11-03-20-53-12\BoardStatus.txt
                //主线程休眠时间,防止复制的到C盘 classicDefect 的速度很慢 ，导致 BoardStatus.txt 没有复制到
                if (File.Exists(snTextPath))
                {
                    try
                    {
                        //读取sn异常捕获一下
                        StreamReader sr = new StreamReader(snTextPath, Encoding.Default); //文件IO流的读取 创建
                        string firstStr = sr.ReadLine();
                        string[] arrStr = firstStr.Split(';');
                        string sn = arrStr[1];
                        this.textBoxSN.Text = sn;
                        sb.Append(sn);
                        sr.Close();
                        string[] strArr = snTextPath.Split('\\');
                        string lastStr = strArr[strArr.Length - 2];
                        lineStr = lastStr.Split('[')[0];   // 机器编号
                        this.textBoxMachine.Text = lineStr;
                        sb.Append("_");
                        if (lineStr.Equals("V810-8057S2"))
                        {
                            this.textBoxLine.Text = "L22";
                            sb.Append("L22");
                        }
                        if (lineStr.Equals("V810-8064S2"))
                        {
                            this.textBoxLine.Text = "L12";
                            sb.Append("L12");
                        }
                        if (lineStr.Equals("V810-8070S2"))
                        {
                            this.textBoxLine.Text = "K22";
                            sb.Append("K22");
                        }
                        if (lineStr.Equals("V810-8085S2"))
                        {
                            this.textBoxLine.Text = "K12";
                            sb.Append("K12");
                        }
                        if (lineStr.Equals("V810-8096S2"))
                        {
                            this.textBoxLine.Text = "J12";
                            sb.Append("J12");
                        }
                        if (lineStr.Equals("V810-3327S2EX"))
                        {
                            this.textBoxLine.Text = "I12";
                            sb.Append("I12");
                        }
                        if (lineStr.Equals("V810-3323S2EX"))
                        {
                            this.textBoxLine.Text = "I22";
                            sb.Append("I22");
                        }
                        if (lineStr.Equals("V810-3328S2EX"))
                        {
                            this.textBoxLine.Text = "H12";
                            sb.Append("H12");
                        }
                        if (lineStr.Equals("V810-8086S2"))
                        {
                            this.textBoxLine.Text = "P12";
                            sb.Append("P12");
                        }
                        if (lineStr.Equals("V810-3325S2EX"))
                        {
                            this.textBoxLine.Text = "Q12";
                            sb.Append("Q12");
                        }
                        sb.Append("_");
                        sb.Append(lineStr);
                        this.listBox1.Items.Add(sb.ToString().Trim());
                        if (!Directory.Exists(@"C:\sn\records\"))
                        {
                            Directory.CreateDirectory(@"C:\sn\records\");
                        }
                        string dt = DateTime.Now.ToString("yyyyMMdd");
                        string filepath = @"C:\sn\records\" + dt + ".txt";
                        if (!File.Exists(filepath))
                        {
                            TextWriter tw = new StreamWriter(filepath);
                            tw.WriteLine(sb.ToString().Trim());
                            tw.Close();
                        }
                        else
                        {
                            TextWriter tw = new StreamWriter(filepath, true);
                            tw.WriteLine(sb.ToString().Trim());
                            tw.Close();
                        }
                        string[] strArray = new string[this.listBox1.Items.Count];
                        this.listBox1.Items.CopyTo(strArray, 0);
                        Array.Reverse(strArray);
                        listBox2.Items.Clear();
                        foreach (string str in strArray)
                        {
                            listBox2.Items.Add(str);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("异常信息是" + ex.Message);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox2.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //模糊搜索按钮
         if(this.textBox1.Text.Length==0){
            MessageBox.Show("请填写模糊匹配的sn");
            return;
         }
         this.listBox2.Items.Clear();
         string sn=  this.textBox1.Text.Trim().ToUpper();
         string dt = DateTime.Now.ToString("yyyyMMdd");
         string filepath = @"C:\sn\records\" + dt + ".txt";
         string firstStr = "";
         //读取sn异常捕获一下
         StreamReader sr = null;
         try 
         {
            sr = new StreamReader(filepath, Encoding.Default); //文件IO流的读取 创建
            List<string> listSn = new List<string>();
            listSn.Clear();
            while((firstStr=sr.ReadLine())!=null){
                listSn.Add(firstStr.ToUpper());
            }
            foreach (string item in listSn)
            {
                if(item.Contains(sn)){
                    listBox2.Items.Add(item);
                }
            }
            this.textBox1.Text = "";
         }
            catch(Exception ex)
            {
               MessageBox.Show("异常信息是"+ex.Message);
            }
            finally
            {
             if(sr!=null){
              sr.Close();
              }
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter){
                button4_Click(null,null);
            }
        }
      

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           /* int index = this.listBox2.IndexFromPoint(e.Location);
            if(index!=System.Windows.Forms.ListBox.NoMatches){
                MessageBox.Show(index.ToString());
            }
            */
        }
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if(listBox2.SelectedItem!=null){
                string sn = listBox2.SelectedItem.ToString().Trim().Split('_')[0];
                string lineStr = listBox2.SelectedItem.ToString().Trim().Split('_')[1];
                string machineStr = listBox2.SelectedItem.ToString().Trim().Split('_')[2];
                SaveImageAndDelety(sn, lineStr, machineStr);
            }
        }
        public class AppHotKey
        {
            [DllImport("kernel32.dll")]
            public static extern uint GetLastError();
            //如果函数执行成功，返回值不为0。
            //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool RegisterHotKey(
                IntPtr hWnd,                //要定义热键的窗口的句柄
                int id,                     //定义热键ID（不能与其它ID重复）           
                KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
                Keys vk                     //定义热键的内容
                );
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool UnregisterHotKey(
                IntPtr hWnd,                //要取消热键的窗口的句柄
                int id                      //要取消热键的ID
                );
            //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
            [Flags()]
            public enum KeyModifiers
            {
                None = 0,
                Alt = 1,
                Ctrl = 2,
                Shift = 4,
                WindowsKey = 8
            }
            /// <summary>
            /// 注册热键
            /// </summary>
            /// <param name="hwnd">窗口句柄</param>
            /// <param name="hotKey_id">热键ID</param>
            /// <param name="keyModifiers">组合键</param>
            /// <param name="key">热键</param>
            public static void RegKey(IntPtr hwnd, int hotKey_id, KeyModifiers keyModifiers, Keys key)
            {
                try
                {
                    if (!RegisterHotKey(hwnd, hotKey_id, keyModifiers, key))
                    {
                        if (Marshal.GetLastWin32Error() == 1409) { MessageBox.Show("热键被占用 ！"); }
                        else
                        {
                            MessageBox.Show("注册热键失败！");
                        }
                    }
                }
                catch (Exception) { }
            }
            /// <summary>
            /// 注销热键
            /// </summary>
            /// <param name="hwnd">窗口句柄</param>
            /// <param name="hotKey_id">热键ID</param>
            public static void UnRegKey(IntPtr hwnd, int hotKey_id)
            {
                //注销Id号为hotKey_id的热键设定
                UnregisterHotKey(hwnd, hotKey_id);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗?", "退出询问",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;

            }
            else
            {
                AppHotKey.UnregisterHotKey(Handle, 101);
            }

        }
        private const int WM_HOTKEY = 0x312; //窗口消息-热键
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        private const int Space = 0x3572; //热键ID
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键ID
                    switch (m.WParam.ToInt32())
                    {
                        case 101: //热键ID
                            button1_Click(null,null);
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE: //窗口消息-创建
                    AppHotKey.RegKey(Handle, Space, AppHotKey.KeyModifiers.Ctrl | AppHotKey.KeyModifiers.Shift | AppHotKey.KeyModifiers.Alt, Keys.S);
                    break;
                case WM_DESTROY: //窗口消息-销毁
                    AppHotKey.UnRegKey(Handle, Space); //销毁热键
                    break;
                default:
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (this.textBox3.Text=="")
            {
                MessageBox.Show("请输入SN");
                return;
            }
            string lastname = this.textBox3.Text.Trim().ToUpper();
            string sn4 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<ns1:BoardTestXMLExport xmlns:ns1=\"http://tempuri.org/BoardTestXMLExport.xsd\" testTime=\"2020-04-20T00:02:01.000+08:00\" testerTestStartTime=\"2020-04-20T00:02:01.000+08:00\" testerTestEndTime=\"2020-04-20T00:02:14.000+08:00\" numberOfComponentsTested=\"1\" numberOfJointsTested=\"1667\" numberOfIndictedComponents=\"0\" numberOfIndictedPins=\"0\" numberOfDefects=\"0\" testStatus=\"Passed\" repairStatus=\"Repair None\" repairStationId=\"3328-VVTS\">\r\n" +
            "    <ns1:BoardXML serialNumber=\"" + lastname + "\" imageId=\"1\" boardType=\"F20-MB-00Y0-E3N-DD-02\" boardRevision=\"1587312134000\" assemblyRevision=\"F20-MB-00Y0-E3N-DD-02\"/>\r\n" +
            "    <ns1:StationXML testerName=\"V810-8086S2\" stage=\"v810\"/>\r\n" +
            "    <ns1:RepairEventXML repairStartTime=\"2020-04-20T00:02:16.000+08:00\" repairEndTime=\"2020-04-20T00:03:59.000+08:00\" repairOperator=\"c_admin\" numberOfActiveDefects=\"0\" numberOfActiveComponents=\"0\" numberOfActivePins=\"0\" numberOfFalseCalledDefects=\"0\" numberOfFalseCalledComponents=\"0\" numberOfFalseCalledPins=\"0\" numberOfRepairedDefects=\"0\" numberOfRepairedComponents=\"0\" numberOfRepairedPins=\"0\" numberOfRepairLaterDefects=\"0\" numberOfRepairLaterComponents=\"0\" numberOfRepairLaterPins=\"0\" numberOfVariationOkDefects=\"0\" numberOfVariationOkComponents=\"0\" numberOfVariationOkPins=\"0\"/>\r\n" +
            "</ns1:BoardTestXMLExport>\r\n" +
            "";
            string fileoutname = "C:\\ITF\\XMLCEXPORT\\" + lastname + "_#F08-MB-00B0-F3J-DD-01#AXI#system-764#1_012524.xml";
            if (!File.Exists(fileoutname))
            {
                TextWriter tw2 = new StreamWriter(fileoutname);
                tw2.WriteLine(sn4);
                tw2.Close();
            }
            else
            {
            }
            this.textBox3.Text = "";
            this.textBox3.Focus();


        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter){

                button5_Click(null,null);


            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBoxPath.Text = @"\\172.26.12.16\aoi\5DX\5DX不良";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBoxPath.Text = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBoxExePath.Text != "")
            {
                try
                {
                    if (!File.Exists(textBoxExePath.Text))
                        throw new Exception("文件不存在");
                    Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                    rk.SetValue(textBoxExePath.Text.Substring(textBoxExePath.Text.LastIndexOf("\\") + 1), textBoxExePath.Text);
                    if (MessageBox.Show("设置开机启动成功") == DialogResult.OK)
                        ;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
            else {

                MessageBox.Show("请选择路径");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
             if (textBoxExePath.Text != "")
            {
                try
                {
                    if (!File.Exists(textBoxExePath.Text))
                        throw new Exception("文件不存在");
                    Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                    rk.DeleteValue(textBoxExePath.Text.Substring(textBoxExePath.Text.LastIndexOf("\\") + 1), false);
                    if (MessageBox.Show("取消开机自启动成功") == DialogResult.OK)
                        ;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
            else
            {

                MessageBox.Show("请选择路径");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "应用程序(.exe)|*.exe|所有程序(*.*)|*.*";
            if (OFD.ShowDialog() == DialogResult.OK)
                textBoxExePath.Text = OFD.FileName;

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path1=@"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良";
            System.Diagnostics.Process.Start("explorer", path1);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path2 = @"\\172.26.12.16\aoi\5DX\5DX不良";
            System.Diagnostics.Process.Start("explorer",path2);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSN_TextChanged(object sender, EventArgs e)
        {

        }



       
      
    }
}

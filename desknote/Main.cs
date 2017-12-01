using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace desknote
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            
            Properties.Settings.Default.mydocument = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            mydocument = Properties.Settings.Default.mydocument + "\\Hebin\\" + mydocument;

            // 位置
            Rectangle rec = Screen.GetWorkingArea(this);
            int SH = rec.Height;
            int SW = rec.Width;
            //MessageBox.Show(SH + "," + SW);
            this.Location = new Point(SW - this.Width, SH - this.Height);

            // 颜色
            try
            {
                ChangeColor(Color.FromArgb(Properties.Settings.Default.header), Color.FromArgb(Properties.Settings.Default.content), Color.FromArgb(Properties.Settings.Default.foot));
            }
            catch { }

            // 置顶
            try
            {
                this.TopMost = Properties.Settings.Default.topmost;
                toolStripMenuItem4.Checked = this.TopMost;
            }
            catch { }

            load();
        }

        private string mydocument = "content";
        private string content_path = "1";
        private void load()
        {
            content_path = Properties.Settings.Default.content_path;

            if (!Directory.Exists(mydocument))
            {
                try
                {
                    Directory.CreateDirectory(mydocument);
                }
                catch { }
            }
            if (!File.Exists(mydocument + "\\" + "content_1"))
            {
                try
                {
                    StreamWriter sw = new StreamWriter(mydocument + "\\content_" + content_path);
                    sw.Write("");
                    sw.Close();
                }
                catch { }
            }

            // 内容
            richTextBox1.TextChanged -= richTextBox1_TextChanged;
            try
            {
                StreamReader sr = new StreamReader(mydocument + "\\content_" + content_path);
                richTextBox1.Rtf = sr.ReadToEnd();
                sr.Close();
            }
            catch { }

            if (richTextBox1.Text == "")
            {
                //richTextBox1.Text = "欢迎使用桌面小便签\r\n支持粘贴图片，支持拖拽文件\r\n右击可以设置颜色和退出";
                richTextBox1.Rtf = "{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0\\fnil\\fcharset134 \\'d0\\'c2\\'cb\\'ce\\'cc\\'e5;}}{\\colortbl ;\\red255\\green128\\blue192;}\\viewkind4\\uc1\\pard\\lang2052\\f0\\fs36\\'bb\\'b6\\'d3\\'ad\\fs24\\'ca\\'b9\\'d3\\'c3\\ul\\fs30\\'d7\\'c0\\'c3\\'e6\\ulnone\\b\\'d0\\'a1\\'b1\\'e3\\'c7\\'a9\\b0\\par 1.\\'d6\\'a7\\'b3\\'d6\\'d5\\'b3\\'cc\\'f9\\'cd\\'bc\\'c6\\'ac\\'a1\\'a2\\'cd\\'cf\\'d7\\'a7\\'ce\\'c4\\'bc\\'fe\\par 2.\\'d6\\'a7\\'b3\\'d6\\ul\\'d7\\'d4\\'b6\\'a8\\'d2\\'e5\\ulnone\\'d7\\'d6\\'cc\\'e5\\par 3.\\'d6\\'a7\\'b3\\'d6\\'b1\\'ea\\'c7\\'a9\\'d7\\'f3\\'d3\\'d2\\'c7\\'d0\\'bb\\'bb\\par 4.\\'d3\\'d2\\'bb\\'f7\\cf1\\'d3\\'d0\\'be\\'aa\\'cf\\'b2\\cf0\\par}";
            }

            // 绑定保存
            richTextBox1.TextChanged += richTextBox1_TextChanged;

            // 绑定右键
            bindClick();
            
            /*// 绘制便签标题
            Graphics graphics = pictureBox1.CreateGraphics();
            // this.show()的目的是使这个窗体加载完成, 以便于可以在上面绘图
            this.Show();
            graphics.Clear(pictureBox1.BackColor);
            SolidBrush mybrush = new SolidBrush(Color.Black);
            Font myfont = new Font("黑体", 18);
            graphics.DrawString("便签" + content_path, myfont, mybrush, 110, 5);*/
        }

        // 超文本连接
        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Focus();
            if (e.Button == MouseButtons.Left)
            {
                if (sender.GetType().ToString().IndexOf("Label") >= 0)
                {
                    Label label = (Label)sender;
                    mouseOff = new Point(-label.Location.X - e.X, -label.Location.Y - e.Y); //得到变量的值
                }
                else
                {
                    mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                }
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                // 要偏移左panel
                mouseSet.Offset(mouseOff.X - splitContainer2.Panel1.Width, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
            if (this.Location.Y < 0)
            {
                this.Location = new Point(this.Location.X, 0);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting(this);
            setting.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ChangeColor(Color color1, Color color2, Color color3)
        {
            pictureBox1.BackColor = color1;
            richTextBox1.BackColor = color2;
            button7.BackColor = color1;
            button1.BackColor = color3;
            button2.BackColor = color3;
            button3.BackColor = color3;
            button4.BackColor = color3;
            button5.BackColor = color3;
            button6.BackColor = color3;
        }

        // 内容变更时
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter(mydocument + "\\content_" + content_path);
                sw.Write(richTextBox1.Rtf);
                sw.Close();
            }
            catch { }
        }

        // 退出事件
        private void appExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // 更换标签事件
        private void changeText_Click(object sender, EventArgs e)
        {
            content_path = sender.ToString().Replace("便签", "");
            Properties.Settings.Default.content_path = content_path;
            Properties.Settings.Default.Save();
            // 重载界面
            load();
        }

        // 绑定托盘图标按钮
        private void bindClick()
        {
            // 清空所有按钮
            contextMenuStrip2.Items.Clear();

            int i = 1;
            ToolStripMenuItem tool;
            DirectoryInfo folder = new DirectoryInfo(mydocument);
            //遍历文件
            foreach (FileInfo NextFile in folder.GetFiles())
            {
                tool = new ToolStripMenuItem("便签" + i);
                if(content_path == i + "")
                {
                    tool.Checked = true;
                }
                i++;
                tool.Click += changeText_Click;
                contextMenuStrip2.Items.Add(tool);
            }

            // 注册退出按钮
            tool = new ToolStripMenuItem("退出");
            tool.Click += appExit_Click;
            contextMenuStrip2.Items.Add(tool);
        }

        // 新建便签
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DirectoryInfo folder = new DirectoryInfo(mydocument);
            int count = folder.GetFiles().Count();
            content_path = (count + 1) + "";
            Properties.Settings.Default.content_path = content_path;
            Properties.Settings.Default.Save();
            try
            {
                StreamWriter sw = new StreamWriter(mydocument + "\\content_" + content_path);
                sw.Write("");
                sw.Close();
            }
            catch { }
            load();
        }

        // 置顶按钮
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripMenuItem4.Checked = !toolStripMenuItem4.Checked;
            this.TopMost = !this.TopMost;
            Properties.Settings.Default.topmost = this.TopMost;
            Properties.Settings.Default.Save();
        }

        // 单击置顶
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            // 调用win32的API来实现置顶
            // 获取句柄
            IntPtr handle = FindWindow(null, "desknote");
            SwitchToThisWindow(handle, true);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 删除便签
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DirectoryInfo folder = new DirectoryInfo(mydocument);
            int count = folder.GetFiles().Count();
            try
            {
                // 删除文件
                File.Delete(mydocument + "\\content_" + content_path);
                int t = Convert.ToInt32(content_path);
                for(int i = t; i < count; i++)
                {
                    File.Move(mydocument + "\\content_" + (i + 1), mydocument + "/content_" + i);
                }
                content_path = "1";
                Properties.Settings.Default.content_path = "1";
                Properties.Settings.Default.Save();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            load();
        }

        // 前一个
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int pre = Convert.ToInt32(content_path) - 1;
            if(File.Exists(mydocument + "\\content_" + pre))
            {
                Properties.Settings.Default.content_path = pre + "";
                Properties.Settings.Default.Save();
                load();
            }
            else
            {
                DirectoryInfo folder = new DirectoryInfo(mydocument);
                int count = 1;
                if (Directory.Exists(mydocument)) count = folder.GetFiles().Count();
                Properties.Settings.Default.content_path = count + "";
                Properties.Settings.Default.Save();
                load();
            }
        }

        // 后一个
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            int next = Convert.ToInt32(content_path) + 1;
            if (File.Exists(mydocument + "\\content_" + next))
            {
                Properties.Settings.Default.content_path = next + "";
                Properties.Settings.Default.Save();
                load();
            }
            else
            {
                Properties.Settings.Default.content_path = "1";
                Properties.Settings.Default.Save();
                load();
            }
        }

        // 加大字体
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                FontStyle fontStyle = richTextBox1.SelectionFont.Style;
                richTextBox1.SelectionFont = new Font("新宋体", richTextBox1.SelectionFont.Size + 1, fontStyle);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("这是个Bug, 可能你设置字体的时候混杂了中英文, 请分开设置.");
            }
        }

        // 减小字体
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                FontStyle fontStyle = richTextBox1.SelectionFont.Style;
                richTextBox1.SelectionFont = new Font("新宋体", richTextBox1.SelectionFont.Size - 1, fontStyle);
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("这是个Bug, 可能你设置字体的时候混杂了中英文, 请分开设置.");
            }
        }

        // 加粗字体
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FontStyle fontStyle = richTextBox1.SelectionFont.Style;
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, fontStyle ^ FontStyle.Bold);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("这是个Bug, 可能你设置字体的时候混杂了中英文, 请分开设置.");
            }
}

        // 斜体
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                FontStyle fontStyle = richTextBox1.SelectionFont.Style;
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, fontStyle ^ FontStyle.Italic);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("这是个Bug, 可能你设置字体的时候混杂了中英文, 请分开设置.");
            }
        }

        // 下划线
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                FontStyle fontStyle = richTextBox1.SelectionFont.Style;
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, fontStyle ^ FontStyle.Underline);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("这是个Bug, 可能你设置字体的时候混杂了中英文, 请分开设置.");
            }
        }

        // 更改颜色
        private void button6_Click(object sender, EventArgs e)
        {
            ColorDialog ColorForm = new ColorDialog();
            if (ColorForm.ShowDialog() == DialogResult.OK)
            {
                Color GetColor = ColorForm.Color;
                //GetColor就是用户选择的颜色，接下来就可以使用该颜色了
                richTextBox1.SelectionColor = GetColor;
            }
        }

        // 本地备份数据
        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ZIP文件(*.zip)|*.zip";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if(saveFileDialog.FileName.IndexOf(mydocument) >= 0)
                {
                    MessageBox.Show("无法在这里创建备份文件!");
                }
                else
                {
                    try
                    {
                        ZipFile.CreateFromDirectory(mydocument, saveFileDialog.FileName);
                        MessageBox.Show("备份完成!");
                    }
                    catch
                    {
                        MessageBox.Show("创建备份文件失败, 请重试!");
                    }
                }
            }
        }
    }
}

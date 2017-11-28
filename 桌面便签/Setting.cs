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

namespace 桌面便签
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private Main Main;
        public Setting(Form form)
        {
            InitializeComponent();
            Main = (Main)form;
            button1.BackColor = Main.get_PicColor();
            button2.BackColor = Main.get_TxtColor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog ColorForm = new ColorDialog();
            if (ColorForm.ShowDialog() == DialogResult.OK)
            {
                Color GetColor = ColorForm.Color;
                //GetColor就是用户选择的颜色，接下来就可以使用该颜色了
                button1.BackColor = GetColor;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog ColorForm = new ColorDialog();
            if (ColorForm.ShowDialog() == DialogResult.OK)
            {
                Color GetColor = ColorForm.Color;
                //GetColor就是用户选择的颜色，接下来就可以使用该颜色了
                button2.BackColor = GetColor;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("config.ini");
                sw.Write(button1.BackColor.ToArgb() + "," + button2.BackColor.ToArgb());
                sw.Close();
                MessageBox.Show("设置完成, 重启后生效!");
            }
            catch
            {
                MessageBox.Show("设置颜色失败!");
            }
            this.Close();
        }
    }
}

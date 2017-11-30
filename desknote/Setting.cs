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

namespace desknote
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
            button1.BackColor = Color.FromArgb(Properties.Settings.Default.header);
            button2.BackColor = Color.FromArgb(Properties.Settings.Default.content);
            button4.BackColor = Color.FromArgb(Properties.Settings.Default.foot);
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

        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog ColorForm = new ColorDialog();
            if (ColorForm.ShowDialog() == DialogResult.OK)
            {
                Color GetColor = ColorForm.Color;
                //GetColor就是用户选择的颜色，接下来就可以使用该颜色了
                button4.BackColor = GetColor;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.header = button1.BackColor.ToArgb();
                Properties.Settings.Default.content = button2.BackColor.ToArgb();
                Properties.Settings.Default.foot = button4.BackColor.ToArgb();
                Properties.Settings.Default.Save();

                // 不写标题了, 把颜色设置加上
                Main.ChangeColor(button1.BackColor, button2.BackColor, button4.BackColor);
                //MessageBox.Show("设置完成, 重启后生效!");
            }
            catch
            {
                MessageBox.Show("设置颜色失败!");
            }
            this.Close();
        }
    }
}

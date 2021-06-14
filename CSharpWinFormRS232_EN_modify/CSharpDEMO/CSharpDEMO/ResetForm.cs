using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpDEMO
{
    public partial class ResetForm : Form
    {
        Form1 parent;
        int maxCount;
        public ResetForm(Form1 f1, int count)
        {
            InitializeComponent();
            parent = f1;
            maxCount = count;
        }

        private void ResetForm_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (parent.retry>=maxCount||parent.reset_flag!=-1)//timeout or resetting succesfully
            {
                this.Dispose();
            }
            else
            {
                //label1.Text = "後" +ss.Seconds.ToString()+ "秒";
                label1.Text = "後"+(maxCount- parent.retry).ToString()+"秒で閉じる";
            }
            
        }
    }
}

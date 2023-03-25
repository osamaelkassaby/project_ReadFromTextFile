using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_1
{
    public partial class strat : Form
    {
        public strat()
        {
            InitializeComponent();
        }
        public bool state = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            progress.Width += 2;
            label2.Text = progress.Width / 10 + 20 + "%";

            if (progress.Width >= 300)
            {
                label1.Text = "Created by osamaelkassaby";
                this.Name = "strat";
              
            }
            if (progress.Width >= 600)
            {
                state = false;

                label1.Text = "Prof. Hesham Arfat";
                Font font = new Font("Segoe UI", 24);

                label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                label1.Location = new System.Drawing.Point(250, 6);
            }
           
            if (progress.Width >= 800)
            {
                timer1.Stop();
                Form1  frm = new Form1();
                frm.ShowDialog();
                this.Hide();
            }
        }

        private void strat_Load(object sender, EventArgs e)
        {

        }
    }
}

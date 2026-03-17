using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class Splash : Form
    {
      
        public Splash()
        {
            InitializeComponent();
           
           
        }

        private void Splash_Load(object sender, EventArgs e)
        {
timer2.Start();
        }

        private void guna2ProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
 guna2ProgressBar1.Increment(1);

            if (guna2ProgressBar1.Value >= 100)
            {
                timer2.Stop();
                LoginForm loginForm = new LoginForm();
                this.Hide();
                loginForm.Show();
            }
        }
    }
}

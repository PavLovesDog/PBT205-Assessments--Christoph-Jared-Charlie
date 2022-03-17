using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBT205_Group_Project
{
    public partial class appSelectWindow : Form
    {
        public appSelectWindow()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TradingButton_Click(object sender, System.EventArgs e)
        {
            // Go to Trading App
        }

        private void MessagingButton_Click(object sender, System.EventArgs e)
        {
            // Go to Messaging App
        }

        private void ContactButton_Click(object sender, System.EventArgs e)
        {
            // Go to Contact Tracing App
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close(); // close current window
        }
    }
}

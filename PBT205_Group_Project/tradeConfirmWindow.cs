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
    public partial class tradeConfirmWindow : Form
    {
        public tradeConfirmWindow()
        {
            InitializeComponent();
            confirmMessage.Text = "You have made a " + tradingWindow.selectedBuyOrSell + " order of " + tradingWindow.price + ", thank you";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

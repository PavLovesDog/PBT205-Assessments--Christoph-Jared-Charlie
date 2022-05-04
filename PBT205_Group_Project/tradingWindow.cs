using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace PBT205_Group_Project
{
    public partial class tradingWindow : Form
    {
        List<string> orderChat = new List<string>();
        List<string> tradeChat = new List<string>();
        public static string selectedTopic = "";
        public static string price = "";
        public static string selectedBuyOrSell = "";
        ClientSocket client;
        public tradingWindow(ClientSocket cs)
        {
            InitializeComponent();
            lstTopics.SelectedIndex = 0;
            buySellCombo.SelectedIndex = 0;
            client = cs;
            //Message box currently shows which topic is active
            messageBox.Text = lstTopics.GetItemText(lstTopics.SelectedItem);
        }

        private void lstTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Changes the message box to display the current topic when changed
            messageBox.Text = lstTopics.GetItemText(lstTopics.SelectedItem);
            selectedTopic = lstTopics.GetItemText(lstTopics.SelectedItem);
        }

        private void confirmOrderBtn_Click(object sender, EventArgs e)
        {
            selectedTopic = lstTopics.GetItemText(lstTopics.SelectedItem);
            selectedBuyOrSell = buySellCombo.GetItemText(buySellCombo.SelectedItem);
            price = priceTextBox.Text;
            MessageBox.Show("You have made a " + selectedBuyOrSell + " order of " + price + ", thank you","Thank",MessageBoxButtons.OK);
            /*
            tradeConfirmWindow tConfWindow = new tradeConfirmWindow();
            tConfWindow.ShowDialog();
            */
        }

        private void quitBtn_Click(object sender, EventArgs e)
        {
            appSelectWindow selectWindow = new appSelectWindow(client);
            selectWindow.ShowDialog();
            this.Close();
        }

        private void logOutBtn_Click(object sender, EventArgs e)
        {
            logInWindow login = new logInWindow();

            this.Close();
            login.ShowDialog();
        }
    }
}

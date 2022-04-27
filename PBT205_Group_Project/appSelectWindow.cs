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
    public partial class appSelectWindow : Form
    {
        ClientSocket server;
        public appSelectWindow(ClientSocket s)
        {
            InitializeComponent();
            server = s;
        }

        private void TradingButton_Click(object sender, System.EventArgs e)
        {
            // Go to Trading App
            tradingWindow tWindow = new tradingWindow(server);
            tWindow.Show();
            this.Close();
        }

        private void MessagingButton_Click(object sender, System.EventArgs e)
        {
            // Go to Messaging App
<<<<<<< Updated upstream
            messagingWindow msgWndw = new messagingWindow();
=======
            messagingWindow msgWndw = new messagingWindow(server);
>>>>>>> Stashed changes
            msgWndw.Show();
            this.Close();
        }

        private void ContactButton_Click(object sender, System.EventArgs e)
        {
            // Go to Contact Tracing App
            contactTracingWindow contactTracingWindow = new contactTracingWindow(server);
            contactTracingWindow.Show();
            this.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close(); // close current window
        }
    }
}

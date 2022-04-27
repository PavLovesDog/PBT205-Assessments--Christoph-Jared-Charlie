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
    public partial class messagingWindow : Form
    {
        ClientSocket client;
        public messagingWindow(ClientSocket cs)
        {
            client = cs;
            InitializeComponent();
        }

        private void sendMessageBtn_Click(object sender, EventArgs e)
        {
            textBox.Text += logInWindow.username + ": " + chatBox.Text+"\n";
            chatBox.Clear();
        }
    }
}

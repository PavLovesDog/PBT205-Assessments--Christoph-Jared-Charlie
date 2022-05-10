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
using System.Net;

namespace PBT205_Group_Project
{
    public partial class tradingWindow : Form
    {
        List<string> orderChat = new List<string>(); //keep track of active trades
        List<string> tradeChat = new List<string>(); //keep track of done trades

        public static string selectedTopic = "";
        public static string price = "";
        public static string selectedBuyOrSell = "";
        byte[] buffer = new byte[2048];

        ClientSocket currentUser;
        Socket serverSocket;
        public tradingWindow(ClientSocket cs)
        {
            InitializeComponent();
            currentUser = cs;
            switch (selectedTopic)
            {
                case "Orders":
                    Send("<FetchActiveTrades>");
                    foreach (string s in tradeChat)
                    {
                        Invoke((Action)delegate
                        {
                            messageBox.Text += s;
                        });
                    }
                    break;
                case "Trades":
                    Send("<FetchDoneTrades>");
                    foreach (string s in orderChat)
                    {
                        messageBox.Text += s;
                    }
                    break;
            }
            ConnectToServer();
            if (serverSocket.Connected)
            {
                SendState();
                lstTopics.SelectedIndex = 0;
                buySellCombo.SelectedIndex = 0;
            }
            //Message box currently shows which topic is active

            //            messageBox.Text = lstTopics.GetItemText(lstTopics.SelectedItem);
        }
        void SendState()
        {
            byte[] msg = Encoding.ASCII.GetBytes("<State> " + currentUser.state);
            serverSocket.Send(msg);
        }
        private void ConnectToServer()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEp = new IPEndPoint(ipAddress, 11000);

                currentUser.socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                while (!currentUser.socket.Connected)
                {
                    try
                    {
                        currentUser.socket.Connect(remoteEp);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Error");
                    }
                }
                serverSocket = currentUser.socket;

                serverSocket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, currentUser);
                //connectDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            ClientSocket currentClientUser = (ClientSocket)ar.AsyncState;
            int received;//recevied bytes
            try
            {
                received = serverSocket.EndReceive(ar); //get the data from the stream
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK);
                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
                return;
            }

            byte[] recBuf = new byte[received]; //set a buffer for the received
            Array.Copy(buffer, recBuf, received); //copies the bytes into the buffer object
            string data = Encoding.ASCII.GetString(recBuf); //converty the bytes to human readable string
            //get all active trades and put them into the orderChat List
            if (data.StartsWith("<ActiveTrades>"))
            {
                data = data.Substring("<ActiveTrades>".Length);
                string[] trades = data.Split(new[] { "<NEXT>" }, StringSplitOptions.RemoveEmptyEntries);
                orderChat.Clear();
                for (int i = 0; i < trades.Length; i++)
                {
                    if (trades[i] != "")
                    {
                        string[] entry = trades[i].Split(new[] { "<Price>" }, StringSplitOptions.None);
                        AddToOrders(entry[0] + " - " + entry[1]);
                    }
                }
            }
            //get all done trades and put them into the tradesChat List
            else if (data.StartsWith("<DoneTrades>"))
            {
                data = data.Substring("<DoneTrades>".Length);
                string[] trades = data.Split(new[] { "<NEXT>" }, StringSplitOptions.RemoveEmptyEntries);

                tradeChat.Clear();
                for (int i = 0; i < trades.Length; i++)
                {
                    if (trades[i] != "")
                    {
                        string[] entry = trades[i].Split(new[] { "<Price>" }, StringSplitOptions.None);
                        AddToTrades(entry[0] + " - " + entry[1]);
                    }
                }
            }
            else if (data.StartsWith("<TradeFound>"))
            {
                string m = data.Substring("<TradeFound>".Length);
                string[] sArr = m.Split(new[] { "<Price>" }, StringSplitOptions.None);
                MessageBox.Show("The order to " + sArr[0] + " stock at " + sArr[1] + " was succesful");
                Send("<FetchDoneTrades>");
            }
            else if (data.StartsWith("<TradeNotFound>"))
            {
                string m = data.Substring("<TradeNotFound>".Length);
                string[] sArr = m.Split(new[] { "<Price>" }, StringSplitOptions.None);
                MessageBox.Show("The order to " + sArr[0] + " stock at " + sArr[1] + " was not found, added to orders list.");
                Send("<FetchActiveTrades>");
            }

            currentClientUser.socket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, currentClientUser);
        }

        private void AddToOrders(string s)
        {
            orderChat.Add(s + Environment.NewLine);
        }

        private void AddToTrades(string s)
        {
            tradeChat.Add(s + Environment.NewLine);

        }
        private void lstTopics_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Changes the message box to display the current topic when changed
            messageBox.Text = "";

            selectedTopic = lstTopics.GetItemText(lstTopics.SelectedItem);
            switch (selectedTopic)
            {
                case "Orders":
                    //Send("<FetchActiveTrades>");
                    foreach (string s in orderChat)
                    {
                        //Invoke((Action)delegate
                        // {
                        messageBox.Text += s;
                        //});
                    }
                    break;
                case "Trades":
                    // Send("<FetchDoneTrades>");
                    foreach (string s in tradeChat)
                    {
                        messageBox.Text += s;
                    }
                    break;
            }
        }

        private void confirmOrderBtn_Click(object sender, EventArgs e)
        {

            //selectedTopic = lstTopics.GetItemText(lstTopics.SelectedItem);
            selectedBuyOrSell = buySellCombo.GetItemText(buySellCombo.SelectedItem);
            price = priceTextBox.Text;

            //Send message to server letting it know what trades to do
            string msg = "<Trade>" + selectedBuyOrSell + "<Price>" + price;
            Send(msg);
        }

        private void Send(string msg)
        {
            byte[] m = Encoding.ASCII.GetBytes(msg);
            serverSocket.Send(m);
        }

        private void quitBtn_Click(object sender, EventArgs e)
        {
            appSelectWindow selectWindow = new appSelectWindow(currentUser);
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

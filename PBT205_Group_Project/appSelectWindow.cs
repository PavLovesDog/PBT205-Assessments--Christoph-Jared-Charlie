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
    public partial class appSelectWindow : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ClientSocket currentUser;

        byte[] msg;
        byte[] buffer = new byte[2048];

        public appSelectWindow(ClientSocket self)
        {
            currentUser = self;
            currentUser.socket = serverSocket;
            InitializeComponent();

            ConnectToServer();
            if (serverSocket.Connected)
            {
                SendState();
            }

        }

        private void ConnectToServer()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEp = new IPEndPoint(ipAddress, 11000);
                serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                while (!serverSocket.Connected)
                {
                    try
                    {
                        serverSocket.Connect(remoteEp);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Error");
                    }
                }


                serverSocket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, currentUser);
                //connectDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            ClientSocket currentClient = (ClientSocket)ar.AsyncState;
            int receivedBytes;
            try
            {
                receivedBytes = currentClient.socket.EndReceive(ar);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return;
            }

            byte[] receivedBuffer = new byte[receivedBytes];
            Array.Copy(currentClient.buffer, receivedBuffer, receivedBytes);
            string text = Encoding.ASCII.GetString(receivedBuffer);
            MessageBox.Show(text + " convered from " + receivedBuffer, "Converted", MessageBoxButtons.OK);
            currentClient.socket.BeginReceive(currentClient.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClient);
        }
        //Function to send state to server
        void SendState()
        {
            msg = Encoding.ASCII.GetBytes("<State> " + currentUser.state);
            serverSocket.Send(msg);
        }

        private void TradingButton_Click(object sender, System.EventArgs e)
        {
            // Go to Trading App
            currentUser.state = State.Trading;
            SendState();
            tradingWindow tWindow = new tradingWindow(currentUser);

            this.Close();
            tWindow.ShowDialog();
        }
        private void MessagingButton_Click(object sender, System.EventArgs e)
        {
            // Go to Messaging App
            currentUser.state = State.Messaging;
            SendState();
            messagingWindow msgWndw = new messagingWindow(currentUser);
            this.Close();
            msgWndw.ShowDialog();
        }

        private void ContactButton_Click(object sender, System.EventArgs e)
        {
            // Go to Contact Tracing App
            currentUser.state = State.ContactTracing;
            SendState();
            contactTracingWindow contactTracingWindow = new contactTracingWindow(currentUser);
            this.Close();
            contactTracingWindow.ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            msg = Encoding.ASCII.GetBytes("<EXIT>");
            serverSocket.Send(msg);
            logInWindow logWindow = new logInWindow();
            this.Close(); // close current window
            logWindow.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace PBT205_Group_Project
{
    public partial class messagingWindow : Form
    {
        Socket socket;
        EndPoint epLocal, epRemote;
        byte[] buffer = new byte[2048];
        ClientSocket currentUser;
        Socket serverSocket;
        public messagingWindow(ClientSocket cs)
        {
            currentUser = cs;
            InitializeComponent();
            ConnectToServer();
            if (serverSocket.Connected)
            {
                currentUser.socket = serverSocket;
                SendState();
            }
        }
        //connect to server
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
        //handle any messages that come in here
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
            AddToChat(data);
            //handle data here with
            //StartsWith() function checks if the message starts with a certain string
            // maybe look up String.Split() and String.Substring()
            //both useful to work with
            //SendMessage(textMessage.Text,serverSocket);

            currentClientUser.socket.BeginReceive(buffer,0,2048, SocketFlags.None, ReceiveCallback, currentClientUser);
        }
     
        void AddToChat(string msg)
        {
            listMessage.Invoke((Action)delegate
            {
                listMessage.Items.Add(msg);
            });
        }
        void SendMessage(string data, Socket target)
        {
            byte[] msg = Encoding.ASCII.GetBytes(data);
            target.Send(msg);
        }

        void SendState()
        {
            byte[] msg = Encoding.ASCII.GetBytes("<State> " + currentUser.state);
            serverSocket.Send(msg);
        }

        private void messagingWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            byte[] msg = Encoding.ASCII.GetBytes("<EXIT>");
            serverSocket.Send(msg);
            this.Close(); // close current window
            Application.Exit();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Converting the string message to a byte
            /*
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];
            sendingMessage = aEncoding.GetBytes(textMessage.Text);

            // Sending the encoded message
            socket.Send(sendingMessage);
            */

            SendMessage(currentUser.username+": "+ textMessage.Text,serverSocket);

            // Adding to the listbox
            //listMessage.Items.Add("Me: " + textMessage.Text);
            textMessage.Text = "";
        }
    }
}

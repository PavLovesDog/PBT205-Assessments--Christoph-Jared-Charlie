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
        byte[] buffer;
        ClientSocket currentUser;
        Socket serverSocket;
        public messagingWindow(ClientSocket cs)
        {
            currentUser = cs;
            InitializeComponent();
            ConnectToServer();
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
            serverSocket = (Socket)ar.AsyncState;
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

            //handle data here with
            //StartsWith() function checks if the message starts with a certain string
            // maybe look up String.Split() and String.Substring()
            //both useful to work with
            SendMessage(textMessage.Text,serverSocket);
        }
        /*
        private void messagingWindow_Load(object sender, EventArgs e)
        {
            // Setting up the socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            // Getting user IP
            textLocalIp.Text = GetLocalIP();
            textRemoteIP.Text = GetLocalIP();
        }

        */
        /*
        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }
        */
        /*
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // Binding the socket
            epLocal = new IPEndPoint(IPAddress.Parse(textLocalIp.Text), Convert.ToInt32(textLocalPort.Text));
            socket.Bind(epLocal);

            // Connecting to the Remote IP
            epRemote = new IPEndPoint(IPAddress.Parse(textRemoteIP.Text), Convert.ToInt32(textRemoteIP.Text));
            socket.Connect(epRemote);

            // Listening to the specific port
            buffer = new byte[1500];
            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
        }
        */

        /*
        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                byte[] receivedData = new byte[1500];
                receivedData = (byte[])aResult.AsyncState;

                // Converting the byte into a string
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                string receivedMessage = aEncoding.GetString(receivedData);

                // Adding the message into listbox
                listMessage.Items.Add("Friend: " + receivedMessage);

                buffer = new byte[1500];
                socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        */
        void SendMessage(string data, Socket target)
        {
            byte[] msg = Encoding.ASCII.GetBytes(data);
            target.Send(msg);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Converting the string message to a byte
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];
            sendingMessage = aEncoding.GetBytes(textMessage.Text);

            
            // Sending the encoded message
            socket.Send(sendingMessage);

            // Adding to the listbox
            listMessage.Items.Add("Me: " + textMessage.Text);
            textMessage.Text = "";
        }
    }
}

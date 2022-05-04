using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace PBT205_Group_Project
{
    public partial class logInWindow : Form
    {

        ClientSocket currentUser = new ClientSocket();
        //ClientSocket server = new ClientSocket();
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        byte[] buffer = new byte[2048];
        bool loggedin = false;
        public static string username = "";
        string password;

        ManualResetEvent connectDone = new ManualResetEvent(false);
        ManualResetEvent sendDone = new ManualResetEvent(false);
        ManualResetEvent receiveDone = new ManualResetEvent(false);

        public logInWindow()
        {
            InitializeComponent();
            currentUser.socket = serverSocket;
            //ConnectToServer();
            //currentClient.state = State.LoginWindow;
        }
        //when app starts connect to server.
        // when login button is pressed then send a message to server


        // LOG-IN BUTTON
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (usernameText.Text != "" && passwordText.Text != "")
            {
                username = usernameText.Text;
                password = passwordText.Text;
                ConnectToServer();

            }
            else
            {
                MessageBox.Show("Username/Password cannot be blank.", "No Blank Pls", MessageBoxButtons.OK);
            }
            //  if (!server.socket.Connected)
            //{
            //connect to server to check for loging details
            //}
            if (serverSocket.Connected)
            {
                byte[] msg = Encoding.ASCII.GetBytes("<username>" + username + "<EOF>" + password + "<EOF>");
                serverSocket.Send(msg);
            }



        }
        //Connects to the server
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
                        return;
                    }
                }
                serverSocket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, serverSocket);
                connectDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
        }


        // EXIT BUTTON
        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            if (serverSocket != null)
            {

                if (serverSocket.Connected)
                {
                    SendMessage("<EXIT>", serverSocket);
                    Thread.Sleep(500);
                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket.Close();
                }
            }
            Application.Exit();
        }

        void SendMessage(string data, Socket target)
        {
            byte[] msg = Encoding.ASCII.GetBytes(data);
            target.Send(msg);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            serverSocket = (Socket)ar.AsyncState;
            int received;
            try
            {
                received = serverSocket.EndReceive(ar);
            }
            catch (SocketException se)
            {

                serverSocket.Close();
                return;
            }
            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string data = Encoding.ASCII.GetString(recBuf);
            switch (data)
            {
                //connected
                case "Logged in":
                    loggedin = true;
                    break;
                case "<0>": // user exists, pass wrong
                    MessageBox.Show("Password is wrong, try again");
                    break;
                case "<1>": //user not exist, ask to create new user
                    DialogResult r = MessageBox.Show("User does not exist, would you like to create a new user with the chosen password?", "Create User?", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        byte[] msg = Encoding.ASCII.GetBytes("<Create> <User>" + username + "<Pass>" + password);
                        serverSocket.Send(msg);
                    }
                    else if (r == DialogResult.No)
                    {
                        serverSocket.Shutdown(SocketShutdown.Both);
                        serverSocket.Close();

                        //return;
                        break;
                    }
                    break;
                case "User Created":

                    loggedin = true;
                    break;
            }
            if (loggedin)
            {

                currentUser.username = username;
                currentUser.password = password;
                currentUser.socket = serverSocket;
                currentUser.state = State.AppSelect;
                SendMessage("<EXIT>", serverSocket);
                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
                appSelectWindow f2 = new appSelectWindow(currentUser);
                this.Invoke((MethodInvoker)delegate
                {
                    this.Hide();
                });
                f2.ShowDialog();


                return;
            }
            receiveDone.Set();
            if (serverSocket.Connected)
            {
                serverSocket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, serverSocket);
                //SendMessage(currentUser.state.ToString(), serverSocket);
            }
        }

        // these functions are for when user click on the user 0r password text.... probs not needed. 
        private void UserLabel_Click(object sender, System.EventArgs e)
        {
            //lblUserReply.Text = "Enter Your Username";
        }
        private void passwordLabel_Click(object sender, EventArgs e)
        {

        }

        // TITLE TEXT
        private void LblTitleText_Click(object sender, EventArgs e)
        {
            // MORE TEST CRAP, can remove
            lblRetort.Text = "You click good.";
        }

        // controls table setup.. but what to put here? colour?
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class logInWindow : Form
    {
        ClientSocket currentClient = new ClientSocket();
        public static string username = "";
        string password;
        public logInWindow()
        {
            InitializeComponent();
        }

        // LOG-IN BUTTON
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //connect to server to check for loging details
            username = usernameText.Text;
            password = passwordText.Text;
            if (ConnectToServer())
            {
                currentClient.username = username;
                currentClient.password = password;
                appSelectWindow f2 = new appSelectWindow(currentClient);
                f2.Show();
            }
            //check if username already exists
            //if not then ask to create user
            //if it does check if the password matches
            //if password doesn't match then let the user know
            //if password matches then login and connect with socket
            //carry over the socket information to use in the any of the apps

            // Open second window

        }

        private bool ConnectToServer()
        {
            bool connected = false;
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEp = new IPEndPoint(ipAddress, 11000);
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                
                try
                {
                    sender.Connect(remoteEp);

                    byte[] msg = Encoding.ASCII.GetBytes("<username>"+username + "<EOF>");
                    int bytesSent = sender.Send(msg);

                    msg = Encoding.ASCII.GetBytes(password + "<EOF>");
                    bytesSent = sender.Send(msg);

                    int bytesRec = sender.Receive(bytes);

                    string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    switch (data)
                    {
                        //connected
                        case "<True > Connected":
                            connected = true;
                            currentClient.socket = sender;
                            break;
                        case "<0>": // user exists, pass wrong
                            MessageBox.Show("Password is wrong, try again");
                            connected = false;
                            break;
                        case "<1>": //user not exist, ask to create new user
                            DialogResult r = MessageBox.Show("User does not exist, would you like to create a new user with the chosen password?","Create User?",MessageBoxButtons.YesNo);
                            if(r == DialogResult.Yes)
                            {
                                msg = Encoding.ASCII.GetBytes("<Create> <User>" + username + "<Pass>" + password);
                                bytesSent = sender.Send(msg);
                                bytesRec = sender.Receive(bytes);
                                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                                if(data == "User Created")
                                {
                                    connected = true;
                                }
                            }else if(r == DialogResult.No)
                            {
                                break;
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            return connected;
        }

        // EXIT BUTTON
        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
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

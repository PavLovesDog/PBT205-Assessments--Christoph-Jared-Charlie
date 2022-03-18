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
    public partial class logInWindow : Form
    {
        public logInWindow()
        {
            InitializeComponent();
        }

        // LOG-IN BUTTON
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            // TEST CRAP, can remove
            lblRetort.Text = "NO!";

            // Open second window
            appSelectWindow f2 = new appSelectWindow();
            f2.Show();

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

    }
}

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

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            // TEST CRAP, can remove
            lblRetort.Text = "NO!";

            // Open second window
            appSelectWindow f2 = new appSelectWindow();
            f2.Show();

        }

        // this function is for when user click on the text next to username text..... why??
        private void UserLabel_Click(object sender, System.EventArgs e)
        {
            lblUserReply.Text = "Enter Your Username";
        }

        // TITLE TEXT
        private void LblTitleText_Click(object sender, EventArgs e)
        {
            // MORE TEST CRAP, can remove
            lblRetort.Text = "You click good.";
        }

    }
}

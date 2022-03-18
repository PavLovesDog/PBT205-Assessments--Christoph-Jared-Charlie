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
    public partial class contactTracingWindow : Form
    {
        public contactTracingWindow()
        {
            InitializeComponent();
        }

        // Quit and return to App Select screen
        private void quitButton_Click(object sender, EventArgs e)
        {
            appSelectWindow selectWindow = new appSelectWindow();
            selectWindow.Show();
            this.Close();
        }

        // Log Out and return to log In page
        private void logOutButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Search text given
        private void searchButton_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

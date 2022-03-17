using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    
    public partial class Form2 : Form
    {
       
        public Form2()
        {
            InitializeComponent();
            label1.Text = "You have made a " +Form1.selectedBuyOrSell+" order of "+Form1.price+", thank you";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

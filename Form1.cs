using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public static string selectedTopic = "";
        public static string price = "";
        public static string selectedBuyOrSell = "";
        public Form1()
        {
            InitializeComponent();
            lstTopics.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            label2.Text = lstTopics.GetItemText(lstTopics.SelectedItem);
        }

        private void lstTopics_SelectedIndexChanged(object sender,EventArgs e)
        {
            label2.Text = lstTopics.GetItemText(lstTopics.SelectedItem);
            
            selectedTopic = lstTopics.GetItemText(lstTopics.SelectedItem);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedTopic = lstTopics.GetItemText(lstTopics.SelectedItem);
            selectedBuyOrSell = comboBox1.GetItemText(comboBox1.SelectedItem);
            price = textBox1.Text;

            Form2 f2 = new Form2();
            f2.Show();
        }
    }

}

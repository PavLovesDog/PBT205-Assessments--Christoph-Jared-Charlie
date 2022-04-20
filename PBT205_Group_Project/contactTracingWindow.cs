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
        // A struct to record positions
        struct Position
        {
            public int row, column;
        }

        Random random = new Random();

        List<int> move = new List<int>()
        {
            -1, 1, -9, 9, -10, 10, -11, 11
            
            // -1 moves to the left
            // 1 moves to the right
            // -9 moves diag up/right
            // 9 moves diag down/left
            // -10 moves up
            // 10 moves down
            // -11 moves diag up/left
            // 11 moves diag down/right
        };

        List<int> currentPersons = new List<int>();
        List<int> newPersons = new List<int>();

        // labels to hold data of where player moves, through clicks
        Label firstClick = null; 
        Label secondClick = null;

        //TODO UN-NEEDED
        private void CreatePositions()
        {
            // runs through every 'label' in grid window
            for (int y = 0; y <= tableLayoutPanel2.ColumnCount; y++)
            {
                for(int x = 0; x <= tableLayoutPanel2.RowCount; x++)
                {
                    Control c = tableLayoutPanel2.GetControlFromPosition(y, x);

                    if(c != null)
                    {
                        if (tableLayoutPanel2.GetColumn(c) == 0)
                        {

                        }
                         
                        tableLayoutPanel2.GetRow(c);
                        
                    }
                }
            }
      

                    // create a position
     
                    // set the position
                    
                    //add to list

        }

        // Function to reset the board
        private void ResetBoard()
        {
            // this runs through everylabel on the board
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    iconLabel.ForeColor = iconLabel.BackColor;
                }
            }
        }

        //Function to assign perosns to the board at beginning of game
        private void AssignPersons()
        {
            currentPersons.Clear(); // clear list for new players

            // run through every box(label) in table
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label iconLabel = control as Label; // converts the control variable to a label named iconLabel.

                if (iconLabel != null)
                {
                    int randomNumber = random.Next(99); // create a random int within range of list size
                    if (iconLabel.TabIndex < randomNumber + 3 && iconLabel.TabIndex > randomNumber - 3)
                    {
                        iconLabel.ForeColor = Color.Red; // set player
                        currentPersons.Add(iconLabel.TabIndex); // add label to list of players
                    }
                    else
                    {
                        iconLabel.ForeColor = iconLabel.BackColor; // Hides Icon from player
                    }
                }
            }

            updateTimer.Start();
        }

        // Function which updates positions of current players
        void UpdatePersons()
        {
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label iconLabel = control as Label; // converts the control variable to a label named iconLabel.

                // check if index is in our current players list
                bool containsIndex = currentPersons.Contains(iconLabel.TabIndex);

                if (containsIndex) // if tabIndex is matches any numbers in persons
                {
                    iconLabel.ForeColor = Color.Red; // set player
                }
                else
                {
                    iconLabel.ForeColor = iconLabel.BackColor; // Hides Icon from player
                }
                
            }
        }

        //TODO Add feature for player to Click and place his position, click to move around

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

        // If Start button is pressed!
        private void startButton_Click(object sender, EventArgs e)
        {
            ResetBoard();

            AssignPersons();

            //UpdatePersons();
        }

        //================================================================== UPDATE TIMER
        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            updateTimer.Stop();

            //Update new list
            // for every item in the position list
            foreach (int index in currentPersons)
            {

                // create random number based on move list
                int randomNumber = random.Next(0, 7);
                int movement = move[randomNumber];
                int tempIndex = index; // store old index
                tempIndex += movement; // create new index

                // if its within bounds
                if (tempIndex < 100 && tempIndex > -1)
                {
                    newPersons.Add(tempIndex); // add to new list
                }
                else // create new perp?
                {

                }
            }

            updateTimer.Start(); // reset timer for next run

            //Make persons list equal to newPersons list 
            currentPersons.Clear();
            currentPersons = new List<int>(newPersons);
            newPersons.Clear(); // reset for next take

            // UPDate new positions
            UpdatePersons();


            //TODO here, update postions of TabIndexs by random selection
            // add number to tabIndex to access new box
            // forecolor = backcolor of previous label
            // new label = color.red

            ////Hide both Icons?
            //firstClick.ForeColor = firstClick.BackColor;
            //secondClick.ForeColor = secondClick.BackColor;
            //
            ////reset first/secondClicked for next round
            //firstClick = null;
            //secondClick = null;
        }

        //TODO this doesn't work
        private void addPersonTimer_Tick(object sender, EventArgs e)
        {
            addPersonTimer.Stop();
            int randomAddition = random.Next(0, 99);
            currentPersons.Add(randomAddition);

            addPersonTimer.Start();
        }


        //TODO Handle or delete crap below safely
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}

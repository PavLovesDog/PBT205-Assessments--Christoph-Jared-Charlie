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

        List<int> currentPersons = new List<int>(); // lists to track persons positions
        List<int> newPersons = new List<int>();  // list to update persons positions
        List<int> playerPosition = new List<int>(); // list to track player position
        List<int> playerContacts = new List<int>(); // list to track all player contacts
        List<int> personsContacts = new List<int>(); // list to track all persons contacts

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

            //Assign Player 
            Control Player = tableLayoutPanel2;//??
            Label label = Player as Label;     //??
            label = label60; // middle location
            playerPosition.Add(label.TabIndex);
            label.ForeColor = Color.Blue;

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
                    iconLabel.ForeColor = Color.Red; // set person
                }
                else if(playerPosition.Contains(iconLabel.TabIndex)) // ensure player doesn't get painted over
                {
                    iconLabel.ForeColor = Color.Blue;
                }
                else
                {
                    iconLabel.ForeColor = iconLabel.BackColor; // Hides Icon from player
                }

                // ============================================================= Handle Contact with Player
                bool playerOnBoard = currentPersons.Contains(playerPosition[0]);
                bool playerOccupiesSpace = iconLabel.TabIndex == playerPosition[0];
                bool personOnBoard = currentPersons.Contains(iconLabel.TabIndex);

                if (playerOnBoard && playerOccupiesSpace)
                {
                    iconLabel.Text = "mm"; // Display 2 people in same square
                    //TODO LOG Contact Here!
                    playerContacts.Add(iconLabel.TabIndex); // log contact
                    iconLabel.ForeColor = Color.Green; // display contact through colour change
                }
                else
                {
                    iconLabel.Text = "m"; // revert to single person, if no one else occupies space

                    if(personOnBoard)
                    iconLabel.ForeColor = Color.Red;
                }


            }
        }

        public contactTracingWindow()
        {
            InitializeComponent();

        }

        // This event handler, handles every label in the grids Label Click event
        private void gridSpace_Click(object sender, EventArgs e)
        {
            Label clickedSpace = sender as Label;

            // ===============================================================Player Movement
            if (clickedSpace != null)
            {
                //TODO Change this so player can occupy same space as persons
                // if space already occupied
                if (clickedSpace.ForeColor == Color.Red)
                    return;

                //First click, Plays once!
                if(firstClick == null)
                {
                    playerPosition.RemoveAt(0); // remove Initial placement
                    firstClick = clickedSpace;
                    firstClick.ForeColor = Color.Blue;
                    playerPosition.Add(firstClick.TabIndex);
                    return;
                }
                //Second click
                else if(secondClick == null)
                {
                    playerPosition.RemoveAt(0); // 0 index always removes first item added
                    secondClick = clickedSpace;
                    secondClick.ForeColor = Color.Blue;
                    playerPosition.Add(secondClick.TabIndex);
                }
                else
                {
                    // Plays for EVERY subsequent click
                    playerPosition.RemoveAt(0); // 0 index always removes first item added
                    secondClick = clickedSpace;
                    secondClick.ForeColor = Color.Blue;
                    playerPosition.Add(secondClick.TabIndex);
                }

                // reset previous tile color
                if (firstClick != null)
                {
                    firstClick.ForeColor = firstClick.BackColor; // revert old color
                    firstClick = secondClick; // update previous position to current

                }
            }
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
            infoBox_TextChanged(sender, e);
        }

        // Handles Text Shown in search box!
        private void infoBox_TextChanged(object sender, EventArgs e)
        {
            string infoMessage = "";
            string addedMessage = "";

            if(playerContacts.Count > 0)
            {
                for (int i = 0; i < playerContacts.Count; i++)
                {
                    addedMessage = "Player contacted at location " + Convert.ToString(playerContacts[i]) + "  \n";
                    infoMessage += addedMessage + System.Environment.NewLine;
                }

                infoBox.Text = infoMessage;
            }
            else
            {
                infoBox.Text = "You Searched, but found only emptiness... ";
            }

        }

        // If Start button is pressed!
        private void startButton_Click(object sender, EventArgs e)
        {
            ResetBoard();

            AssignPersons();

            UpdatePersons();
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

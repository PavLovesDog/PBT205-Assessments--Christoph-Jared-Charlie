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
        ClientSocket server;
        public contactTracingWindow(ClientSocket s)
        {
            server = s;
            InitializeComponent();
        }

        // crete random seed
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

        //TODO set up a "player" script to hold positional data, infected status, movement data

        List<int> currentPersons = new List<int>(); // lists to track persons positions
        List<int> newPersons = new List<int>();  // list to update persons positions

        List<int> playerPosition = new List<int>(); // list to track player position
        List<int> playerMoves = new List<int>(); // list to store all player previous locations

        List<int> playerContacts = new List<int>(); // list to track all player contacts
        List<int> personsContacts = new List<int>(); // list to track all persons contacts
        List<int> duplicates = new List<int>(); // list to check if Persons are occupying same space
        List<int> Infected = new List<int>(); // List to track infected persons

        //Strings for message box
        string queryResponseMessage = "";
        string positionMessage = "";
        string infectedMessage = "";
        bool hitSearchButton = false;

        // labels to hold data of where player moves, through clicks
        Label firstClick = null; 
        Label secondClick = null;

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
            Control Player = tableLayoutPanel2;
            Label label = Player as Label;     
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

            //Start Timers to control movement & person additions
            updateTimer.Start();
            addPersonTimer.Start();
        }

        // Function which updates positions of current players
        void UpdatePersons()
        {
            //tableLayoutPanel2.GetCellPosition;


            //POSITIONAL STUFF
            //// get the position of the icon in that tableLayout
            //tableLayoutPanel2.GetCellPosition(iconLabel);
            //
            ////Creating a cell position
            //TableLayoutPanelCellPosition b = new TableLayoutPanelCellPosition();
            //b.Column = 5;
            //b.Row = 2;
            //b.Column--;

            // KEY INPUTS
            // in the Events, double click KeyDown, that will create method I can use to manipulate position
            /*
             private void contactTracingWindow_KeyPress(object sender, KeyPressEventArgs e)
             {
                if (e.KeyChar == (char)Keys.W)
                {
                    //Move player up on board
                }
             }
            */


            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label iconLabel = control as Label; // converts the control variable to a label named iconLabel.

                // check if index is in our current players list
                int cell = iconLabel.TabIndex;

                // positional bools
                bool playerOnBoard = currentPersons.Contains(playerPosition[0]);
                bool playerOccupiesSpace = iconLabel.TabIndex == playerPosition[0];
                bool personOnBoard = currentPersons.Contains(cell);

                bool isDuplicate = duplicates.Contains(cell);
                bool isInfected = Infected.Contains(cell);

                // ensure player doesn't get painted over
                if (playerPosition.Contains(cell)) 
                {
                    iconLabel.ForeColor = Color.Blue;
                }
                else
                {
                    iconLabel.ForeColor = iconLabel.BackColor;
                }

                // ============================================================= Handle Contact with Player & other Persons

                if (playerOnBoard && playerOccupiesSpace)
                {
                    iconLabel.Text = "mm"; // Display 2 people in same square
                    playerContacts.Add(cell); // log contact
                    iconLabel.ForeColor = Color.Green; // display contact through colour change
                }
                else if (isDuplicate) // check if position is already occupied
                {
                    iconLabel.Text = "mm";
                    iconLabel.ForeColor = Color.Green;
                    duplicates.Remove(cell);
                }
                else
                {
                    iconLabel.Text = "m"; // revert to single person, if no one else occupies space

                    if (personOnBoard && isInfected)
                    {
                        iconLabel.ForeColor = Color.Yellow;
                    } 
                    else if(personOnBoard)
                    {
                        iconLabel.ForeColor = Color.Red;
                    }
                }
            }
        }

        void UpdateTexts()
        {
            // Middle Text
            label3.Text = "        CLICK to MOVE \n   Total Persons on board: "
                            + (currentPersons.Count + playerPosition.Count).ToString() +
                            "\n       Total Infected:  " + Infected.Count.ToString();
            
        }


        public contactTracingWindow()
        {
            InitializeComponent();

        }

        // This event handler, handles every label in the grids Label Click event
        private void gridSpace_Click(object sender, EventArgs e)
        {
            Label clickedSpace = sender as Label;

            //========================================Limit player movement
            int move1 = playerPosition[0] - 1; // -1 moves to the left
            int move2 = playerPosition[0] + 1; // 1 moves to the right
            int move3 = playerPosition[0] - 9; // -9 moves diag up/right
            int move4 = playerPosition[0] + 9; // 9 moves diag down/left
            int move5 = playerPosition[0] - 10; // -10 moves up
            int move6 = playerPosition[0] + 10; // 10 moves down
            int move7 = playerPosition[0] - 11; // -11 moves diag up/left
            int move8 = playerPosition[0] + 11; // 11 moves diag down/right

            bool isWithinMoveSpace = clickedSpace.TabIndex == move1 || clickedSpace.TabIndex == move2 || 
                                     clickedSpace.TabIndex == move3 || clickedSpace.TabIndex == move4 || 
                                     clickedSpace.TabIndex == move5 || clickedSpace.TabIndex == move6 ||
                                     clickedSpace.TabIndex == move7 || clickedSpace.TabIndex == move8;

            // ===============================================================Player Movement
            if (clickedSpace != null)
            {

                // Limit movement to only spaces around player
                if(!isWithinMoveSpace)
                {
                    return;
                }

                // if space already occupied
                if (clickedSpace.ForeColor == Color.Red)
                {
                    //log contact
                    playerContacts.Add(clickedSpace.TabIndex);

                    playerPosition.RemoveAt(0);
                    clickedSpace.Text = "mm";
                    clickedSpace.ForeColor = Color.Green;
                    playerPosition.Add(clickedSpace.TabIndex);
                    playerMoves.Add(clickedSpace.TabIndex);
                }

                //First click, Plays once!
                if(firstClick == null)
                {
                    playerPosition.RemoveAt(0); // remove Initial placement
                    firstClick = clickedSpace;
                    firstClick.ForeColor = Color.Blue;
                    playerPosition.Add(firstClick.TabIndex);
                    playerMoves.Add(firstClick.TabIndex);
                }
                //Second click
                else if(secondClick == null)
                {

                    playerPosition.RemoveAt(0); // 0 index always removes first item added
                    secondClick = clickedSpace;
                    secondClick.ForeColor = Color.Blue;
                    playerPosition.Add(secondClick.TabIndex);
                    playerMoves.Add(secondClick.TabIndex);
                }
                else
                {

                    // Plays for EVERY subsequent click
                    playerPosition.RemoveAt(0); // 0 index always removes first item added
                    secondClick = clickedSpace;
                    if(!playerContacts.Contains(clickedSpace.TabIndex)) // don't paint over if player is currently contacting
                            secondClick.ForeColor = Color.Blue;
                    playerPosition.Add(secondClick.TabIndex);
                    playerMoves.Add(secondClick.TabIndex);
                }


                // reset previous tile color
                if (firstClick != null && secondClick != null)
                {
                    firstClick.ForeColor = firstClick.BackColor; // revert old color
                    firstClick = secondClick; // update previous position to current

                }

                //TODO THIS CALLS THE MESSAGE UPDATE
                infoBox_TextChanged(sender, e); // send a call to update messgae
            }
        }

        // Quit and return to App Select screen
        private void quitButton_Click(object sender, EventArgs e)
        {
            //TODO Server connection for return to window?
            appSelectWindow selectWindow = new appSelectWindow(server);
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
            hitSearchButton = true;
            playerMoves.Clear(); // clear list for fresh movements after search

            // trigger change in info box
            infoBox_TextChanged(sender, e);
        }

        // Handles Text Shown in search box!
        private void infoBox_TextChanged(object sender, EventArgs e)
        {
            queryResponseMessage = ""; // reset message ?
            positionMessage = "";
            infectedMessage = "";
            string addedMessage = ""; // clear addition

            //============================================================== SEARCH PLAYER CONTACTS
            if (comboBox1.Text == "Player Contacts" && hitSearchButton)
            {
                if (playerContacts.Count > 0)
                {
                    for (int i = 0; i < playerContacts.Count; i++)
                    {
                        addedMessage = "QUERY RESPONSE - Player contacted at location " + Convert.ToString(playerContacts[i]) + "  \n";
                        queryResponseMessage += addedMessage + System.Environment.NewLine;
                    }

                    infoBox.Text = queryResponseMessage;
                }
                else
                {
                    infoBox.Text = "QUERY RESPONSE - You Searched, but found only emptiness... ";
                }

                hitSearchButton = false; // reset for next run
            }
            //============================================================== PERSON CONTACTS
            else if (comboBox1.Text == "Person Contacts" && hitSearchButton)
            {
                if (personsContacts.Count > 0)
                {
                    //infoBox.Text = "QUERY RESPONSE - ";
                    for (int i = 0; i < personsContacts.Count; i++)
                    {
                        addedMessage = "QUERY RESPONSE - Persons contacted at location " + Convert.ToString(personsContacts[i]) + "  \n";
                        queryResponseMessage += addedMessage + System.Environment.NewLine;
                    }

                    infoBox.Text = queryResponseMessage;
                }
                else
                {
                    infoBox.Text = "QUERY RESPONSE - No one has contacted, yet...";
                }

                hitSearchButton = false; // reset for next run
            }
            //============================================================== INFECTED CONTACTS
            else if (comboBox1.Text == "Infected Contacts" && hitSearchButton)
            {
                if (Infected.Count > 0)
                {
                    for (int i = 0; i < Infected.Count; i++)
                    {
                        addedMessage = "QUERY RESPONSE - Infected person at cell " + Convert.ToString(Infected[i]) + "  \n";
                        infectedMessage += addedMessage + System.Environment.NewLine;
                    }

                    infoBox.Text = infectedMessage;
                }
                else
                {
                    infoBox.Text = "QUERY RESPONSE - You Searched, but found only emptiness... ";
                }

                hitSearchButton = false; // reset for next run
            }
            //============================================================== NO SELECTION
            else if (hitSearchButton)
            {
                infoBox.Text = "QUERY RESPONSE - No Contacts found, please specify Search Target";
            }
            //============================================================== DEFAULT MOVEMENT
            else
            {
                for (int i = 0; i < playerMoves.Count; i++)
                {
                    addedMessage = "POSITION - You Moved to cell " + Convert.ToString(playerMoves[i]) + "  \n";
                    positionMessage += addedMessage + System.Environment.NewLine;
                }

                infoBox.Text = positionMessage;
            }


        }

        // If Start button is pressed!
        private void startButton_Click(object sender, EventArgs e)
        {
            ResetBoard();

            AssignPersons();

            UpdatePersons();

            UpdateTexts();
        }


        //================================================================== UPDATE TIMER
        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            updateTimer.Stop();

            // for every item in the position list
            foreach (int index in currentPersons)
            {

                // create random number based on move list
                int randomNumber = random.Next(0, 7);
                int movement = move[randomNumber]; // chose a movement direction, based on number

                #region Handle Boundry

                //Check if against LEFT wall
                if (index == 0  || index == 10 || index == 20 || index == 30 || index == 40 || index == 50 || 
                    index == 60 || index == 70 || index == 80 || index == 90)
                {
                    // check if index is top or bottom row (0 or 90/ 9 or 99)
                    switch (index)
                    {
                        case 0: // TOP LEFT
                            movement = 11; // move diag down right
                            break;

                        case 90: // BOTTOM LEFT
                            movement = -9; // move diag up right
                            break;

                        default:
                            movement = 1; // moves them right one space
                            break;
                    }
                }

                //Check if against RIGHT wall
                if (index == 9 || index == 19 || index == 29 || index == 39 || index == 49 || index == 59 ||
                    index == 69 || index == 79 || index == 89 || index == 99)
                {
                    // check if index is top or bottom row (0 or 90/ 9 or 99)
                    switch (index)
                    {
                        case 9: // TOP RIGHT
                            movement = 9; // move diag down left
                            break;

                        case 99: // BOTTOM RIGHT
                            movement = -11; // move diag up left  
                            break;

                        default:
                            movement = -1; // moves them left one space
                            break;
                    }
                }

                //Check if against TOP wall
                if (index == 1 || index == 2 || index == 3 || index == 4 || index == 5 || index == 6 ||
                    index == 7 || index == 8)
                {
                    switch (index)
                    {
                        case 1: 
                            movement = 11; // move diag down right
                            break;
                        case 2: 
                            movement = 9; // move diag down left
                            break;
                        case 3: 
                            movement = -1; // move left
                            break;
                        case 4: 
                            movement = 10; // move down
                            break;
                        case 5: 
                            movement = 10; // move down
                            break;
                        case 6:
                            movement = 1; // move right  
                            break;
                        case 7: 
                            movement = 11; // move diag down right  
                            break;
                        case 8:
                            movement = 9; // move diag down left  
                            break;
                        //catch 
                        default:
                            movement = 10; // moves them down one space
                            break;
                    }
                }

                //Check if against BOTTOM wall
                if (index == 91 || index == 92 || index == 93 || index == 94 || index == 95 || index == 96 ||
                    index == 97 || index == 98)
                {
                    switch (index)
                    {
                        case 91:
                            movement = -9; // move diag up right
                            break;
                        case 92:
                            movement = -11; // move diag up left
                            break;
                        case 93:
                            movement = -1; // move left
                            break;
                        case 94:
                            movement = -10; // move up
                            break;
                        case 95:
                            movement = -10; // move up
                            break;
                        case 96:
                            movement = 1; // move right  
                            break;
                        case 97:
                            movement = -9; // move diag up right  
                            break;
                        case 98:
                            movement = -11; // move diag up left  
                            break;
                        //catch 
                        default:
                            movement = -10; // moves them up one space
                            break;
                    }
                }

                #endregion

                int tempIndex = index; // store old index

                // If the index is any of the following lists, Mark them as INFECTED
                if (duplicates.Contains(tempIndex) || playerContacts.Contains(tempIndex) || personsContacts.Contains(tempIndex)
                     || Infected.Contains(tempIndex))
                {
                    if(Infected.Contains(tempIndex)) Infected.Remove(tempIndex); // remove old position of infected
                    tempIndex += movement; // update index
                    if (tempIndex < 100 && tempIndex > -1) // if its within bounds
                        Infected.Add(tempIndex); // add next position to infected list
                }
                else
                {
                    tempIndex += movement; // otherwise, just create new index
                }

                // if index is within bounds 
                if (tempIndex < 100 && tempIndex > -1)
                {
                    if (newPersons.Contains(tempIndex)) // if updated positions ALREADY contain this position
                    {
                        personsContacts.Add(tempIndex); // add to contact tracing list for Text
                        duplicates.Add(tempIndex); // add to duplicate list for display
                    }

                    newPersons.Add(tempIndex); // add to new list for position update
                }
            }

            updateTimer.Start(); // reset timer for next run

            //Make persons list equal to newPersons list 
            currentPersons.Clear();
            currentPersons = new List<int>(newPersons);
            newPersons.Clear(); // reset for next take

            // Update new positions
            UpdatePersons();
            UpdateTexts();
        }

         // Timer to add people to the board
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

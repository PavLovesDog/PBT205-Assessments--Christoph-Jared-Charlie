using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace PBT205_Group_Project
{

    public partial class contactTracingWindow : Form
    {
        // random seed for move selection
        Random random = new Random();

        // List of possible moves for persons
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
        List<int> playerMoves = new List<int>(); // list to store all player previous locations

        List<int> playerContacts = new List<int>(); // list to track all player contacts
        List<int> personsContacts = new List<int>(); // list to track all persons contacts
        List<int> duplicates = new List<int>(); // list to check if Persons are occupying same space
        List<int> Exposed = new List<int>(); // List to track persons who have Contacted and could be infected

        //Strings for message box
        string queryResponseMessage = "";
        string positionMessage = "";
        string infectedMessage = "";
        string addedMessage = "";
        string receivedMessage = "";

        // bools for one-time events
        bool hitSearchButton = false;
        bool playerHasContacted = false;
        bool waitingForMessage = true;

        // labels to hold data of where player moves, through clicks
        Label firstClick = null;
        Label secondClick = null;

        // Server Variables
        byte[] buffer = new byte[2048];
        Socket serverSocket;
        ClientSocket currentUser; //keeps track of the currentuser's details

        // CONSTRUCTOR
        public contactTracingWindow(ClientSocket s)
        {
            currentUser = s;            
            InitializeComponent();

            ConnectToServer();
            currentUser.socket = serverSocket;

            if(serverSocket.Connected)
            currentUser.state = State.ContactTracing;

            SendMessage("<UpdateClient> <State>" + currentUser.state + 
                        "<User>" + currentUser.username + 
                        "<Pass>" + currentUser.password, 
                        serverSocket);
        }
        
        //======================================================SERVER FUNCTIONS
        // Connect to server
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
                    }
                }

                //this line is always listening for messages
                serverSocket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, serverSocket);
                //connectDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
        }
       
        //handles receiving a message from the server
        void ReceiveCallback(IAsyncResult ar)
        {
            serverSocket = (Socket)ar.AsyncState;
            int received;//recevied bytes
            try
            {
                received = serverSocket.EndReceive(ar); //get the data from the stream
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK);
                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
                return;
            }

            byte[] recBuf = new byte[received]; //set a buffer for the received
            Array.Copy(buffer, recBuf, received); //copies the bytes into the buffer object
            string data = Encoding.ASCII.GetString(recBuf); //converty the bytes to human readable string
            //handle data string somehow here
            //can use a switch or if statements

            // Check data received from server side and add it to string for infobox text update
            switch(data)
            {
                case "QUERY RESPONSE <User Contacts>":
                    receivedMessage += data;
                    waitingForMessage = false;
                    break;

                case "QUERY RESPONSE <Person Contacts>":
                    receivedMessage += data;
                    waitingForMessage = false;
                    break;

                case "QUERY RESPONSE <Exposed Contacts>":
                    receivedMessage += data;
                    waitingForMessage = false;
                    break;

                case "TOPIC <No Message>":
                    receivedMessage += data;
                    waitingForMessage = false;
                    break;

                case "POSITION <User Location>":
                    receivedMessage += data;
                    waitingForMessage = false;
                    break;

                default:
                    waitingForMessage = false; // catch to stop infinite while loops in click/search space
                    break;
            }

            //start BeginReceive again so that this can get messages from the server
            serverSocket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, serverSocket);
        }

        // Function to send a message to the server
        void SendMessage(string data, Socket target)
        {
            byte[] msg = Encoding.ASCII.GetBytes(data);
            target.Send(msg);
        }
        
        //====================================================INITIALIZE WINDOW
        // Initialize the window for display and operations
        public contactTracingWindow()
        {
            InitializeComponent();
        }

        //==================================================================SET UP FUNCTIONS
        // Function to reset the board for next play
        private void ResetBoard()
        {
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    iconLabel.ForeColor = iconLabel.BackColor; // set all tiles the same colour
                }
            }

            //reset player comtact bool
            playerHasContacted = false;
        }

        //Function to assign persons randomly to the board at beginning of game
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
                        iconLabel.ForeColor = Color.OrangeRed; // set player
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

        //===================================================================UPDATE POSITIONS & TEXT
        // Function which updates visual positions of current players/persons 
        // called every 2 seconds when the update timer runs
        void UpdatePersons()
        {
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label iconLabel = control as Label;

                // assign the index for readable code
                int cell = iconLabel.TabIndex;

                // positional bools
                bool playerOnBoard = currentPersons.Contains(playerPosition[0]);
                bool playerOccupiesSpace = cell == playerPosition[0];
                bool personOnBoard = currentPersons.Contains(cell);

                // contacted/infected bools
                bool isDuplicate = duplicates.Contains(cell);
                bool isInfected = Exposed.Contains(cell);

                // ensure player doesn't get painted over
                if (playerPosition.Contains(cell))
                {
                    if (playerHasContacted)
                    {
                        iconLabel.ForeColor = Color.MediumOrchid;
                    }
                    else
                    {
                        iconLabel.ForeColor = Color.DodgerBlue;
                    }
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
                    iconLabel.ForeColor = Color.ForestGreen; // display contact through colour change
                    playerHasContacted = true;
                }
                else if (isDuplicate) // check if position is already occupied
                {
                    iconLabel.Text = "mm";
                    if (isInfected)
                    {
                        iconLabel.ForeColor = Color.Goldenrod;
                    }
                    else
                    {
                        iconLabel.ForeColor = Color.ForestGreen;
                    }

                    duplicates.Remove(cell); // remove for next visual update
                }
                else
                {
                    iconLabel.Text = "m"; // revert to single person, if no one else occupies space

                    if (personOnBoard && isInfected)
                    {
                        iconLabel.ForeColor = Color.Goldenrod;
                    }
                    else if (personOnBoard)
                    {
                        iconLabel.ForeColor = Color.OrangeRed;
                    }
                }
            }
        }

        // This function updates the text to display controls, Persons on board & total contacts so far
        void UpdateTexts()
        {
            int playerInfected = 0;
            if (playerHasContacted)
            {
                playerInfected = 1;
            }
            // Info Text on side of screen
            label3.Text = "    'CLICK' to MOVE \nTotal Persons On Board: "
                            + (currentPersons.Count + playerPosition.Count).ToString() +
                            "\n       Total Exposed:  " + (Exposed.Count + playerInfected).ToString();

        }

        //==========================================================================PLAYER CONTROL
        // This event handler, handles every label in the grids Label-Click event
        // This is where the user controls the simulation/grid space
        private void gridSpace_Click(object sender, EventArgs e)
        {
            Label clickedSpace = sender as Label; // get information of the cell clicked on by userS

            //==========================================================Limit player movement
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
                if (!isWithinMoveSpace)
                {
                    return;
                }

                // if space already occupied
                if (clickedSpace.ForeColor == Color.OrangeRed || clickedSpace.ForeColor == Color.Goldenrod)
                {
                    //log contact
                    playerContacts.Add(clickedSpace.TabIndex);
                    playerHasContacted = true;

                    playerPosition.RemoveAt(0);
                    clickedSpace.Text = "mm";

                    // account for if the space occupied is from a contacted/infected persons
                    if (clickedSpace.ForeColor == Color.Goldenrod)
                    {
                        clickedSpace.ForeColor = Color.Goldenrod;
                    }
                    else
                    {
                        clickedSpace.ForeColor = Color.ForestGreen;
                    }
                    playerPosition.Add(clickedSpace.TabIndex);
                    playerMoves.Add(clickedSpace.TabIndex);
                }

                //First click, Plays once!
                if (firstClick == null)
                {
                    playerPosition.RemoveAt(0); // remove Initial placement
                    firstClick = clickedSpace;

                    if (playerHasContacted)
                    {
                        firstClick.ForeColor = Color.MediumOrchid;
                    }
                    else
                    {
                        firstClick.ForeColor = Color.DodgerBlue;
                    }

                    playerPosition.Add(firstClick.TabIndex);
                    playerMoves.Add(firstClick.TabIndex);
                    UpdatePersons(); // call update to remove first instance of player
                }
                //Second click
                else if (secondClick == null)
                {

                    playerPosition.RemoveAt(0); // 0 index always removes first item added
                    secondClick = clickedSpace;

                    if (playerHasContacted)
                    {
                        firstClick.ForeColor = Color.MediumOrchid;
                    }
                    else
                    {
                        secondClick.ForeColor = Color.DodgerBlue;
                    }

                    playerPosition.Add(secondClick.TabIndex);
                    playerMoves.Add(secondClick.TabIndex);
                }
                else // Plays for EVERY subsequent click
                {
                    playerPosition.RemoveAt(0);
                    secondClick = clickedSpace;

                    // don't paint over if player is currently contacting
                    if (!playerContacts.Contains(clickedSpace.TabIndex))
                    {
                        if (playerHasContacted)
                        {
                            secondClick.ForeColor = Color.MediumOrchid;
                        }
                        else
                        {
                            secondClick.ForeColor = Color.DodgerBlue;
                        }
                    }

                    playerPosition.Add(secondClick.TabIndex);
                    playerMoves.Add(secondClick.TabIndex);

                    currentUser.location = playerPosition.First().ToString();

                }


                // reset previous tile color
                if (firstClick != null && secondClick != null)
                {
                    firstClick.ForeColor = firstClick.BackColor; // revert old color
                    firstClick = secondClick; // update previous position to current
                }

                // send move position to Server
                SendMessage("POSITION <User Update>", serverSocket);
                waitingForMessage = true; // begin while loop whilst messages are updated server side
                while(waitingForMessage)
                {
                    /* Elevator music plays */
                }
                infoBox_TextChanged(sender, e); // send a call to update positional messgae
            }
        }

        // =========================================================================== INFO BOX
        // Handles Text Shown in search box!
        private void infoBox_TextChanged(object sender, EventArgs e)
        {
            queryResponseMessage = ""; // reset message ?
            positionMessage = "";
            infectedMessage = "";
            addedMessage = ""; // clear addition

            // Check what message was sent back from server
            //============================================================== USER CONTACTS
            if (receivedMessage == "QUERY RESPONSE <User Contacts>" && hitSearchButton)
            {
                if (playerContacts.Count > 0)
                {
                    for (int i = 0; i < playerContacts.Count; i++)
                    {
                        // account for persons in 0-9 tabIndex labels (as index does not log 01, just 1 for position)
                        string original = Convert.ToString(playerContacts[i]);
                        if (original == "0" || original == "1" || original == "2" || original == "3" || original == "4" ||
                            original == "5" || original == "6" || original == "7" || original == "8" || original == "9")
                        {
                            //create "0" string
                            string zero = "0";
                            original = zero + original;
                        }
                        string modified = original.Insert(1, ",");
                        addedMessage = "QUERY RESPONSE - User contacted another person at cell (x,y): " + modified + "  \n";
                        queryResponseMessage += addedMessage + System.Environment.NewLine;
                    }
                
                    infoBox.Text = queryResponseMessage;
                }
                else
                {
                    infoBox.Text = "QUERY RESPONSE - User has not contacted anyone yet... ";
                }

                waitingForMessage = true;
                hitSearchButton = false; // reset for next run
            }
            //============================================================== PERSON CONTACTS
            else if (receivedMessage == "QUERY RESPONSE <Person Contacts>" && hitSearchButton)
            {
                if (personsContacts.Count > 0)
                {
                    //infoBox.Text = "QUERY RESPONSE - ";
                    for (int i = 0; i < personsContacts.Count; i++)
                    {
                        string original = Convert.ToString(personsContacts[i]);
                        if (original == "0" || original == "1" || original == "2" || original == "3" || original == "4" ||
                            original == "5" || original == "6" || original == "7" || original == "8" || original == "9")
                        {
                            //create "0" string
                            string zero = "0";
                            original = zero + original;
                        }
                        string modified = original.Insert(1, ",");
                        addedMessage = "QUERY RESPONSE - Persons contacted one another at cell (x,y): " + modified + "  \n";
                        queryResponseMessage += addedMessage + System.Environment.NewLine;
                    }
            
                    infoBox.Text = queryResponseMessage;
                }
                else
                {
                    infoBox.Text = "QUERY RESPONSE - No one has contacted, yet...";
                }

                waitingForMessage = true;
                hitSearchButton = false;
            }
            //============================================================== INFECTED CONTACTS
            else if (receivedMessage == "QUERY RESPONSE <Exposed Contacts>" && hitSearchButton)
            {
                if (Exposed.Count > 0)
                {
                    for (int i = 0; i < Exposed.Count; i++)
                    {
                        string original = Convert.ToString(Exposed[i]);
                        if (original == "0" || original == "1" || original == "2" || original == "3" || original == "4" ||
                            original == "5" || original == "6" || original == "7" || original == "8" || original == "9")
                        {
                            //create "0" string
                            string zero = "0";
                            original = zero + original;
                        }
                        string modified = original.Insert(1, ",");
                        addedMessage = "QUERY RESPONSE - Exposed person in cell (x,y): " + modified + "  \n";
                        infectedMessage += addedMessage + System.Environment.NewLine;
                    }
            
                    infoBox.Text = infectedMessage;
                }
                else
                {
                    infoBox.Text = "QUERY RESPONSE - No potentially infected persons, yet... ";
                }

                waitingForMessage = true;
                hitSearchButton = false;
            }
            //============================================================== NO SELECTION
            else if (receivedMessage == "TOPIC <No Message>" && hitSearchButton)
            {
                infoBox.Text = "QUERY RESPONSE - No Contacts found, please specify Search Target";

                waitingForMessage = true;
                hitSearchButton = false;
            }
            //============================================================== USER MOVEMENT
            else if(receivedMessage == "POSITION <User Location>")
            {
                for (int i = 0; i < playerMoves.Count; i++)
                {
                    string originalIndex = Convert.ToString(playerMoves[i]);
                    if (originalIndex == "0" || originalIndex == "1" || originalIndex == "2" || originalIndex == "3" || originalIndex == "4" ||
                        originalIndex == "5" || originalIndex == "6" || originalIndex == "7" || originalIndex == "8" || originalIndex == "9")
                    {
                        //create "0" string
                        string zero = "0";
                        originalIndex = zero + originalIndex;
                    }
                    string modifiedIndex = originalIndex.Insert(1, ",");
                    addedMessage = "POSITION - You Moved to cell (x,y): " + modifiedIndex + "  \n";
                    positionMessage += addedMessage + System.Environment.NewLine;
                }
            
                infoBox.Text = positionMessage;
            }
            
            waitingForMessage = true; // reset waiting for messages from server next run
            receivedMessage = ""; // reset message
        }

        //================================================================== SEARH BUTTON
        // Search Button click event. this sends messages to the server, to update infobox message
        private void searchButton_Click(object sender, EventArgs e)
        {
            receivedMessage = ""; // reset message for update from server
            hitSearchButton = true;
            playerMoves.Clear(); // clear list for fresh movements after search

            // Send message to server based on drop-down selection
            if (comboBox1.Text == "User Contacts")
            {
                SendMessage("QUERY <User Contacts>", serverSocket);
            }
            else if (comboBox1.Text == "Person Contacts")
            {
                SendMessage("QUERY <Person Contacts>", serverSocket);
            }
            else if (comboBox1.Text == "Exposed Contacts")
            {
                SendMessage("QUERY <Exposed Contacts>", serverSocket);
            }
            else // Combobox is empty
            {
                SendMessage("Topic <No Search Topic>", serverSocket);
            }

            waitingForMessage = true; // initiate loop
            while (waitingForMessage) // Loop and wait for message from server
            {
                /* Elevator music plays */
            }

            // once message has been processed server-side, trigger change in info box
            infoBox_TextChanged(sender, e);
        }

        // ================================================================= START BUTTON
        // If Start button is pressed, call all necessary functions to begin application
        private void startButton_Click(object sender, EventArgs e)
        {
            ResetBoard();

            AssignPersons();

            UpdatePersons();

            UpdateTexts();
        }

        //====================================================================RESET BUTTON
        //Reset button, this resets the playing field so there's no need to close and re-open window
        private void button1_Click(object sender, EventArgs e)
        {
            //Clear info box
            infoBox.Text = "";

            // Reset play data
            ResetBoard();
            updateTimer.Stop();
            addPersonTimer.Stop();

            // reset strings and bools
            queryResponseMessage = "";
            positionMessage = "";
            infectedMessage = "";
            hitSearchButton = false;
            playerHasContacted = false;

            //Clear all lists
            currentPersons.Clear();
            newPersons.Clear();
            playerPosition.Clear();
            playerMoves.Clear();
            playerContacts.Clear();
            personsContacts.Clear();
            duplicates.Clear();
            Exposed.Clear();

        }

        //========================================================= LOG OUT & QUIT BUTTONS
        // Quit and return to App Select screen
        private void quitButton_Click(object sender, EventArgs e)
        {
            appSelectWindow selectWindow = new appSelectWindow(currentUser);
            selectWindow.ShowDialog();
            this.Close();
        }

        // Log Out and return to log In page
        private void logOutButton_Click(object sender, EventArgs e)
        {
            this.Close();
            logInWindow login = new logInWindow();
            login.ShowDialog();
        }

        //================================================================== UPDATE TIMER
        // This timer handles all positional data of persons on board & updates their new positions
        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            // stop timer to perform its funcitons
            updateTimer.Stop();

            // for every item in the position list
            foreach (int index in currentPersons)
            {

                // create random number based on move list
                int randomNumber = random.Next(0, 7);
                int movement = move[randomNumber]; // chose a movement direction, based on number

                // Region is to handle persons trying to leave to visible area
                #region Handle Boundry

                //Check if against LEFT wall
                if (index == 0 || index == 10 || index == 20 || index == 30 || index == 40 || index == 50 ||
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

                // If the index is any of the following lists, Mark them as EXPOSED
                if (duplicates.Contains(tempIndex) || playerContacts.Contains(tempIndex) ||
                    personsContacts.Contains(tempIndex) || Exposed.Contains(tempIndex))
                {
                    if (Exposed.Contains(tempIndex)) Exposed.Remove(tempIndex); // remove old position of exposed
                    tempIndex += movement; // update index
                    if (tempIndex < 100 && tempIndex > -1) // if its within bounds
                        Exposed.Add(tempIndex); // add next position to exposed list, were it will be next update
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

        // ============================================================== ADD PERSON TIMER
        // Timer to add people to the board after a given amount of time
        private void addPersonTimer_Tick(object sender, EventArgs e)
        {
            addPersonTimer.Stop();
            int randomAddition = random.Next(0, 99);
            currentPersons.Add(randomAddition);

            addPersonTimer.Start();
        }
    }
}

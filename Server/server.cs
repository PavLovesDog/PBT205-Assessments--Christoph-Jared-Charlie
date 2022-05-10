﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Data.SQLite;
using System.Threading;

class Server
{
    //list of currently connected clients
    List<ClientSocket> connectedClients = new List<ClientSocket>();

    public Socket serverSocket;

    ManualResetEvent allDone = new ManualResetEvent(false);
    //database connection
    static SQLiteConnection connection;
    static void Main()
    {
        StartDBConnection();
        //setup the server
        Server s = new Server();
        s.SetupServer();
    }
    //Database
    public static void StartDBConnection()
    {

        //SQLiteConnection.CreateFile("users.db");
        connection = new SQLiteConnection("Data Source = users.db");
        connection.Open();

        string statement = "CREATE TABLE IF NOT EXISTS users (username TEXT PRIMARY KEY, password TEXT, location TEXT, UNIQUE(username, password));";
        var command = new SQLiteCommand(statement, connection);
        command.CommandText = statement;
        command.ExecuteNonQuery();

        statement = "INSERT OR IGNORE INTO users (username, password) VALUES ('Admin','Admin');";
        command.CommandText = statement;
        command.ExecuteNonQuery();
        //connection.Close();

    }

    //setup the server
    public void SetupServer()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            serverSocket.Bind(localEndPoint);

            serverSocket.Listen(0);
            while (true)
            {
                allDone.Reset();
                Console.WriteLine("Waiting for connection...");
                serverSocket.BeginAccept(AcceptCallback, serverSocket);
                allDone.WaitOne();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine("Closing server...");
        Console.ReadKey();
    }

    void AcceptCallback(IAsyncResult ar)
    {
        Socket joiningSocket;
        //joiningSocket = (Socket)ar.AsyncState;
        try
        {
            joiningSocket = (Socket)serverSocket.EndAccept(ar);
        }
        catch (ObjectDisposedException ode)
        {
            Console.WriteLine(ode.ToString());
            return;
        }
        ClientSocket newClientSocket = new ClientSocket();
        newClientSocket.socket = joiningSocket;
        //newClientSocket.state = State.LoginWindow; //BAD, defaulting state
        connectedClients.Add(newClientSocket);
        Console.WriteLine(joiningSocket + " joined");
        joiningSocket.BeginReceive(newClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, newClientSocket);
        Console.WriteLine("Client connected...");

        serverSocket.BeginAccept(AcceptCallback, null);
    }

    private ClientSocket UpdateClient(string message, ClientSocket currentClient)
    {
        //ClientSocket cs = new ClientSocket();
        string m = message.Substring("<UpdateClient> <State>".Length);
        string[] str = m.Split("<User>", StringSplitOptions.None);
        switch (str[0])
        {
            case "LoginWindow":
                currentClient.state = State.LoginWindow;
                break;
            case "AppSelect":
                currentClient.state = State.AppSelect;
                break;
            case "Trading":
                currentClient.state = State.Trading;
                break;
            case "Messaging":
                currentClient.state = State.Messaging;
                break;
            case "ContactTracing":
                currentClient.state = State.ContactTracing;
                break;
        }
        str = str[1].Split("<Pass>", StringSplitOptions.None);
        currentClient.username = str[0];
        currentClient.password = str[1];

        return currentClient;
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        ClientSocket currentClientSocket = new ClientSocket();
        currentClientSocket = (ClientSocket)ar.AsyncState;
        int received;


        //check which window the user is on
        //while (true)
        // {
        try
        {

            received = currentClientSocket.socket.EndReceive(ar);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            currentClientSocket.socket.Close();
            connectedClients.Remove(currentClientSocket);
            return;
        }
        //get data and convert to string
        byte[] bytes = new byte[received];
        Array.Copy(currentClientSocket.buffer, bytes, received);
        string data = Encoding.ASCII.GetString(bytes);
        //if teh string received is EXIT then disconnect the client
        if (data == "<EXIT>")
        {
            currentClientSocket.socket.Shutdown(SocketShutdown.Both);
            currentClientSocket.socket.Close();
            connectedClients.Remove(currentClientSocket);
            Console.WriteLine(currentClientSocket.username + " has disconnected");
            return;
        }
        else if (data.StartsWith("<UpdateClient>"))
        {
            currentClientSocket = UpdateClient(data,currentClientSocket);
        }

        GetState(currentClientSocket);
        if (data.StartsWith("<State> "))
        {
            data = data.Substring("<State> ".Length);
            switch (data)
            {
                case "AppSelect":
                    currentClientSocket.state = State.AppSelect;
                    break;
                case "ContactTracing":
                    currentClientSocket.state = State.ContactTracing;
                    break;
                case "Messaging":
                    currentClientSocket.state = State.Messaging;
                    break;
                case "Trading":
                    currentClientSocket.state = State.Trading;
                    break;
            }
        }

        switch (currentClientSocket.state)
        {

            //Handle login window stuff
            case State.LoginWindow:
                /*
                Console.WriteLine("user is on login Screen");

                //when in the LoginWindow check what message was received from the client and handle that.
                if (data.ToLower().StartsWith("<username>"))
                {
                    string[] loginDetails = data.Split("<EOF>", StringSplitOptions.None);
                    string user = loginDetails[0];
                    user = user.Substring("<username>".Length);
                    string pass = loginDetails[1];
                    if (FindUser(user))
                    {
                        if (FindPass(pass))
                        {
                            //byte[] msg = Encoding.ASCII.GetBytes("<True> Connected");
                            //int bytesSent = handler.Send(msg);
                            currentClientSocket.username = user;
                            currentClientSocket.password = pass;
                            SendData("Logged in", currentClientSocket);
                            Console.WriteLine("{0} connected", loginDetails[0]);
                            Console.WriteLine("with password {0}", loginDetails[1]);
                            currentClientSocket.state = State.AppSelect;
                            Console.WriteLine(currentClientSocket.username + " has logged in and has changed state to " + currentClientSocket.state);
                        }
                        else
                        {
                            byte[] msg = Encoding.ASCII.GetBytes("<0>"); //user exists but password is wrong
                            int bytesSent = currentClientSocket.socket.Send(msg);
                        }
                    }
                    else
                    {
                        byte[] msg = Encoding.ASCII.GetBytes("<1>");//user doesn't exist
                        int bytesSent = currentClientSocket.socket.Send(msg);
                    }
                }
                else if (data.StartsWith("<Create> <User>"))
                {
                    data = data.Substring("<Create> <User>".Length);
                    string[] s = data.Split("<Pass>", StringSplitOptions.None);
                    string user = s[0];
                    string pass = s[1];

                    AddUserToDB(user, pass);
                    byte[] msg = Encoding.ASCII.GetBytes("User Created");
                    currentClientSocket.username = user;
                    currentClientSocket.password = pass;
                    currentClientSocket.socket.Send(msg);

                }
                */
                currentClientSocket = HandleLogin(data, currentClientSocket);
                currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);

                break;
            case State.AppSelect:
                /*
                Console.WriteLine("User is at app select");
                if (data == "<Messaging>")
                {
                    currentClientSocket.state = State.Messaging;
                }
                if (data == "<Trading>")
                {
                    currentClientSocket.state = State.Trading;
                }
                if (data == "<ContactTracing>")
                {
                    currentClientSocket.state = State.ContactTracing;
                }*/
                currentClientSocket = HandleAppSelect(data, currentClientSocket);
                break;
            case State.Messaging:
                //Handle any messaging stuff here
                currentClientSocket = HandleMessage(data, currentClientSocket);

                /*
                Console.WriteLine("Opening messageing app for " + currentClientSocket.username);
                */
                currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
                break;
            case State.Trading:
                currentClientSocket = HandleTrading(data, currentClientSocket);
                //handle any trading stuff here

                //Console.WriteLine("Opening Trading app for " + currentClientSocket.username);
                currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
                break;
            case State.ContactTracing:
                //handle any contactTracing stuff here
                currentClientSocket = HandleTracing(data, currentClientSocket);

                //Console.WriteLine("Opening Contact Tracingapp for " + currentClientSocket.username);
                currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
                break;
        }

        // serverSocket.socket.BeginAccept(AcceptCallback, null);
        //}
    }

    public static void SendData(string data, ClientSocket target)
    {
        byte[] msg = Encoding.ASCII.GetBytes(data);
        target.socket.Send(msg);
    }
    //Look up client's state
    State GetState(ClientSocket cs)
    {
        return cs.state;
    }
    //Function for looking up user in database
    public static bool FindUser(string user)
    {
        //connection.Open();
        bool foundUser = false;
        var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM users WHERE username = '" + user + "';";
        SQLiteDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            foundUser = true;
            Console.WriteLine(user + " was found matching " + reader.GetString(0));
        }
        //connection.Close();
        return foundUser;
    }

    //add user to the database with the given username and password
    private static void AddUserToDB(string user, string pass)
    {
        //connection.Open();
        var command = new SQLiteCommand(connection);
        command.CommandText = "INSERT OR IGNORE INTO users (username, password) VALUES ('" + user + "','" + pass + "');";
        command.ExecuteNonQuery();
        Console.WriteLine(user + " was added to database wit the password: " + pass);
        //   connection.Close();
    }
    //Looking up a password in the database
    private static bool FindPass(string pass)
    {
        //  connection.Open();
        bool found = false;
        var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM users WHERE password = '" + pass + "';";
        SQLiteDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            found = true;
            reader.Read();
            Console.WriteLine(pass + " was found matching " + reader.GetString(1));
        }
        //  connection.Close();
        return found;
    }


    #region Login Window Handling
    private ClientSocket HandleLogin(string message, ClientSocket cs)
    {
        Console.WriteLine("user is on login Screen");

        //when in the LoginWindow check what message was received from the client and handle that.
        if (message.ToLower().StartsWith("<username>"))
        {
            string[] loginDetails = message.Split("<EOF>", StringSplitOptions.None);
            string user = loginDetails[0];
            user = user.Substring("<username>".Length);
            string pass = loginDetails[1];
            if (FindUser(user))
            {
                if (FindPass(pass))
                {
                    //byte[] msg = Encoding.ASCII.GetBytes("<True> Connected");
                    //int bytesSent = handler.Send(msg);
                    cs.username = user;
                    cs.password = pass;
                    SendData("Logged in", cs);
                    Console.WriteLine("{0} connected", loginDetails[0]);
                    Console.WriteLine("with password {0}", loginDetails[1]);
                    cs.state = State.AppSelect;
                    Console.WriteLine(cs.username + " has logged in and has changed state to " + cs.state);
                }
                else
                {
                    byte[] msg = Encoding.ASCII.GetBytes("<0>"); //user exists but password is wrong
                    int bytesSent = cs.socket.Send(msg);
                }
            }
            else
            {
                byte[] msg = Encoding.ASCII.GetBytes("<1>");//user doesn't exist
                int bytesSent = cs.socket.Send(msg);
            }
        }
        else if (message.StartsWith("<Create> <User>"))
        {
            message = message.Substring("<Create> <User>".Length);
            string[] s = message.Split("<Pass>", StringSplitOptions.None);
            string user = s[0];
            string pass = s[1];

            AddUserToDB(user, pass);
            byte[] msg = Encoding.ASCII.GetBytes("User Created");
            cs.username = user;
            cs.password = pass;
            cs.socket.Send(msg);

        }
        return cs;
    }
    #endregion


    #region App Select Handling
    private ClientSocket HandleAppSelect(string message, ClientSocket cs)
    {
        Console.WriteLine("User is at app select");
        if (message == "<Messaging>")
        {
            cs.state = State.Messaging;
        }
        if (message == "<Trading>")
        {
            cs.state = State.Trading;
        }
        if (message == "<ContactTracing>")
        {
            cs.state = State.ContactTracing;
        }
        return cs;
    }

    #endregion
    #region Contact Tracing Handling
    private ClientSocket HandleTracing(string message, ClientSocket cs)
    {
        // Check what was sent through and send the back the appropriate response to client side
        if (message == "POSITION <User Update>")
        {
            SendData("POSITION <User Location>", cs);
        }
        else if (message == "QUERY <User Contacts>")
        {
            SendData("QUERY RESPONSE <User Contacts>", cs);
        }
        else if (message == "QUERY <Person Contacts>")
        {
            SendData("QUERY RESPONSE <Person Contacts>", cs);
        }
        else if (message == "QUERY <Exposed Contacts>")
        {
            SendData("QUERY RESPONSE <Exposed Contacts>", cs);
        }
        else if (message == "Topic <No Search Topic>")
        {
            SendData("TOPIC <No Message>", cs);
        }

        return cs;
    }
    #endregion
    #region Messaging Handling
    private ClientSocket HandleMessage(string message, ClientSocket cs)
    {
        //do server side message stuff here
        return cs;
    }

    void SendMessageToAll(string message, ClientSocket sender)
    {
       
    }


    #endregion

    #region Trading Handling
    private ClientSocket HandleTrading(string message, ClientSocket cs)
    {
        //do servse side trading stuff here
        return cs;
    }
    #endregion
}

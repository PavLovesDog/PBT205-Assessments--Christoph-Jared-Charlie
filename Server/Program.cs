using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Data.SQLite;

using System.Threading;
//THE SERVER DON'T DELETE
class SocketListener
{

    static SQLiteConnection connection;
    static void Main(string[] args)
    {
        StartDBConnection();
        StartServer();
    }
    public static void StartDBConnection()
    {

        SQLiteConnection.CreateFile("users.db");
        connection = new SQLiteConnection("Data Source = users.db");
        connection.Open();

        string statement = "CREATE TABLE IF NOT EXISTS users (username TEXT PRIMARY KEY, password TEXT, location TEXT);";
        var command = new SQLiteCommand(statement, connection);
        command.CommandText = statement;
        command.ExecuteNonQuery();

        statement = "INSERT INTO users (username, password) VALUES ('Admin','Admin');";
        command.CommandText = statement;
        command.ExecuteNonQuery();

        statement = "SELECT * FROM users;";
        command.CommandText = statement;
        SQLiteDataReader r = command.ExecuteReader();
        while (r.Read())
        {
            Console.WriteLine("Username: "+r.GetString(0) + "\nPassword: " + r.GetString(1));
        }
        connection.Close();
    }
    public static void StartServer()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        try
        {

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);

            listener.Listen(10);

            Console.WriteLine("Waiting for a connection...");
            Socket handler = listener.Accept();

            string data = null;
            byte[] bytes = null;

            while (handler.Connected)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                //if the sent message starts with <username>
                if (data.StartsWith("<username>"))
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
                           SendData("<True> Connected", handler);

                        }
                        else
                        {
                            byte[] msg = Encoding.ASCII.GetBytes("<0>"); //user exists but password is wrong
                            int bytesSent = handler.Send(msg);
                            
                            
                        }
                    }
                    else
                    {
                        byte[] msg = Encoding.ASCII.GetBytes("<1>");//user doesn't exist
                        int bytesSent = handler.Send(msg);
                    }
                    Console.WriteLine("{0} connected", loginDetails[0]);
                    Console.WriteLine("with password {0}", loginDetails[1]);

                }
                //if the message is to create a user
                //TODO:
                // - have the user be created in the database
                if (data.StartsWith("<Create> <User>"))
                {
                    data = data.Substring("<Create> <User>".Length);
                    string[] s = data.Split("<Pass>", StringSplitOptions.None);
                    string user = s[0];
                    string pass = s[1];

                    AddUserToDB(user, pass);
                    byte[] msg = Encoding.ASCII.GetBytes("User Created");
                    handler.Send(msg);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        Console.WriteLine("\n Press any key to continue...");
        Console.ReadKey();
    }

    public static string GetUserData(string user, string dataType)
    {
        string data = "";
        connection.Open();
        var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT "+dataType+" FROM users WHERE username = '" + user + "';";
        SQLiteDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            data = reader.GetString(0);
            Console.WriteLine(user + " was found matching " + reader.GetString(0));
        }
        connection.Close();
        return data;
    }

    #region Server
    public static void SendData(string data,Socket target)
    {
        byte[] msg = Encoding.ASCII.GetBytes(data);
        target.Send(msg);
    }
    #endregion


    #region Login Window
    //Function for adding user to database
    private static void AddUserToDB(string user, string pass)
    {
            connection.Open();

            var command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO users (username, password) VALUES ('" + user + "','" + pass + "');";
            command.ExecuteNonQuery();
            connection.Close();
        
    }

    //Function for removing user to database
    private static void RemoveUserFromDB(string user)
    {
            var command = new SQLiteCommand(connection);
            command.CommandText = "DELETE FROM users WHERE username = '" + user + "';";
            command.ExecuteNonQuery();
        
    }
    //Function for looking up user
    public static bool FindUser(string user)
    {
        connection.Open();
        bool foundUser = false;
        var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM users WHERE username = '" + user + "';";
        SQLiteDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            foundUser = true;
            Console.WriteLine(user + " was found matching " + reader.GetString(0));
        }
        connection.Close();
        return foundUser;
    }
    //Looking up a password in the database
    private static bool FindPass(string pass)
    {
        connection.Open();
        bool found = false;
        var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM users WHERE password = " + pass + ";";
        SQLiteDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            found = true;
            Console.WriteLine(pass + " was found matching " + reader.GetString(1));
        }
        connection.Close();
        return found;
    }
    //
    private static bool FindUserPass(string user, string pass)
    {
        connection.Open();
        bool found = false;
        var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM users WHERE username = " + user + " AND password = " + pass + ";";
        SQLiteDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            found = true;
            Console.WriteLine(user + " was found matching " + reader.GetString(0) + " with the matching pass: " + pass + " to " + reader.GetString(1));
        }
        connection.Close();
        return found;
    }
    #endregion

    #region appSelectWindow
    //server side handling of app select window

    #endregion

    #region tradingWindow
    //server side for handling the trading app

    #endregion


    #region messagingWindow
    //server side for handling of messaging app

    #endregion



    #region contactTracingWindow
    //Logic for server side handling of the contact tracing

    #endregion
}
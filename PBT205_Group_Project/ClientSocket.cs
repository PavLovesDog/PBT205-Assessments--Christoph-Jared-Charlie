using System;
using System.Net.Sockets;
using System.Text;
namespace PBT205_Group_Project
{

    public enum State
    {
        LoginWindow,
        AppSelect,
        Trading,
        Messaging,
        ContactTracing
    }
    public class ClientSocket
    {
        public string username;
        public string password;
        public Socket socket;
        public string location;
        public State state;
    }
}
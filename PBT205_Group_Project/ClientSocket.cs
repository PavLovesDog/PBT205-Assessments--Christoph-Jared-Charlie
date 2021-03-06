using System.Net.Sockets;


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
    public Socket socket;
    public string username;
    public const int BUFFER_SIZE = 2048;
    public byte[] buffer = new byte[BUFFER_SIZE];
    public string password;
    //for contact tracing app
    public string location;
    public State state;
}
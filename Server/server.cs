using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;

using System.Threading;

namespace ConsoleApp1
{
    class Server
    {
        List<Socket> connectedClients = new List<Socket>();

        static void main()
        {
            Server main = new Server();
            main.server_start();

            Console.ReadLine();
        }
        TcpListener server = new TcpListener(IPAddress.Any, 8888);

        private void server_start()
        {
            server.Start();
            accept_connection();
        }

        private void accept_connection()
        {
            server.BeginAcceptTcpClient(handle_connection, server);
        }

        private void handle_connection(IAsyncResult result)
        {
            accept_connection();
            TcpClient client = server.EndAcceptTcpClient(result);
            NetworkStream ns = client.GetStream();
        }

    }
}


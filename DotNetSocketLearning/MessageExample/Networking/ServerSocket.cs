using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MessageExample.Networking
{

    class ServerSocket
    {
        private Socket _socket;
        byte[] _buffer = new byte[1024];

        public ServerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Bind(int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen(int backlog)
        {
            _socket.Listen(500);
        }

        public void Accept()
        {
            _socket.BeginAccept(AcceptedCallback, null);
            Accept();
        }

        private void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSocket = _socket.EndAccept(result);
            _buffer = new byte[1024];
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecievedCallback, clientSocket);
            Accept();
        }

        private void RecievedCallback(IAsyncResult result)
        {
            Socket clientSocket = result.AsyncState as Socket;
            int bufferSize = clientSocket.EndReceive(result);
            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, packet.Length);

            // Handle packet.
            _buffer = new byte[1024];
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecievedCallback, clientSocket);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.IO;

namespace ComputerController
{
    class ServerHandle
    {
        public static void InitialConnection(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"[{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint}]: Assumed The Wrong Client ID: '{_clientIdCheck}'!");
                Server.clients[_fromClient].Disconnect(_fromClient);
            }
        }
    }
}

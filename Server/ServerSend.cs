using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerController
{
    class ServerSend
    {
        // Sends a packet to a client via TCP.
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        // Sends a packet to all clients via TCP.
        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();

            for (int i = 1; i <= Server.ConnectedClientsIndex; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        // Sends a packet to all clients except one via TCP.
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.ConnectedClientsIndex; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        #region Packets

        public static void InitialConnection(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.InitialConnection))
            {
                _packet.Write(_toClient);
                SendTCPData(_toClient, _packet);
            }
        }

        // Sends a message to the given client.
        public static void CmdCommand(int _toClient, string _command)
        {
            using (Packet _packet = new Packet((int)ServerPackets.CmdCommand))
            {
                _packet.Write(_command);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void CmdCommandToAll(string _command)
        {
            using (Packet _packet = new Packet((int)ServerPackets.CmdCommand))
            {
                _packet.Write(_command);
                SendTCPDataToAll(_packet);
            }
        }

        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;

public class ClientSend
{
    #region Send Methods

    /// Sends a packet to the server via TCP.
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.tcp.SendData(_packet);
    }

    #endregion

    public static void InitialConnection()
    {
        using (Packet _packet = new Packet((int)ClientPackets.InitialConnection))
        {
            _packet.Write(Client.myId);

            SendTCPData(_packet);
        }
    }
}

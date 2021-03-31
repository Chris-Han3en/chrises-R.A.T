using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;

public class ClientHandle
{
    public static void InitialConnection(Packet _packet)
    {
        int _myId = _packet.ReadInt();

        Client.myId = _myId;
        ClientSend.InitialConnection();
        Client.isConnected = true;
    }

    public static void CmdCommand(Packet _packet)
    {
        string command = _packet.ReadString();

        Commands.RunCommand(command);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

public class Client
{
    #region Variables

    // Connection Misc Variables
    public static int dataBufferSize = 4096;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    // The current ip and port values
    public static string ip = "Put your ip here";
    public static int port = ; //put your port fowarded port here

    // Int to store Client ID
    public static int myId = 0;

    // Boolean used to determine whether the Client is connected to a server or not
    public static bool isConnected = false;

    // Boolean used to determine whether the Client has made an attempt to connect to a server or not
    public static bool socketOpen = false;

    public static Timer reconnectTimer;

    // Connection Protocols
    public static TCP tcp;

    #endregion

    #region TCP

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        /// Attempts to connect to the server via TCP.
        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }

        /// Initializes the newly connected client's TCP-related info.
        private void ConnectCallback(IAsyncResult _result)
        {
            try 
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                stream = socket.GetStream();

                receivedData = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            catch
            {
                int errorpreventer = 0; // random stuff just to stop error
            }
        }

        /// Sends data to the client via TCP.
        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null); /// Send data to server
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex);
            }
        }

        /// Reads incoming data from the stream.
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data)); /// Reset receivedData if all data was handled
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }

        /// Prepares received data to be used by the appropriate packet handler methods.
        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                /// If client's received data contains a packet
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    /// If packet contains no data
                    return true; /// Reset receivedData instance to allow it to be reused
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                /// While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet); /// Call appropriate method to handle the packet
                    }
                });

                _packetLength = 0; /// Reset packet length
                if (receivedData.UnreadLength() >= 4)
                {
                    /// If client's received data contains another packet
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        /// If packet contains no data
                        return true; /// Reset receivedData instance to allow it to be reused
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true; /// Reset receivedData instance to allow it to be reused
            }

            return false;
        }

        /// Disconnects from the server and cleans up the TCP connection.
        private void Disconnect()
        {
            Client.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    #endregion

    #region Functions


    // Attempts to connect to a server with the specified ip and port
    public static void ConnectToServer()
    {
        if (!socketOpen && !isConnected)
        {
            tcp = new TCP();

            InitializeClientData();

            tcp.Connect(); // Connect tcp

            socketOpen = true;

            CheckConnection();
        }

        else
        {
            Disconnect();
            ConnectToServer();
        }
    }

    // Initializes all necessary client data.
    public static void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.InitialConnection, ClientHandle.InitialConnection },
            { (int)ServerPackets.CmdCommand, ClientHandle.CmdCommand },
        };
    }

    // Disconnects from the server and stops all network traffic
    public static void Disconnect()
    {
        if (socketOpen)
        {
            tcp.socket.Close();

            isConnected = false;
            socketOpen = false;
        }

        ConnectToServer();
    }

    public static void CheckConnection()
    {
        reconnectTimer = new Timer(2000);
        reconnectTimer.Start();
        reconnectTimer.Elapsed += TimerCallback;
        reconnectTimer.Enabled = true;
        
    }

    public static void TimerCallback(Object source, ElapsedEventArgs args)
    {
        if (!isConnected && socketOpen)
        {
            Disconnect();
            reconnectTimer.Dispose();
        }

        reconnectTimer.Stop();
        reconnectTimer.Dispose();
    }

    #endregion

}



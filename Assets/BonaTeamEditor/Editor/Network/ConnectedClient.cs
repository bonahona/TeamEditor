using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ConnectedClient
{
    public const int RecieveBuffer = 1024;

    public event MessageRecieved OnMessageRecieved;

    public Socket Socket { get; set; }
    public UserData UserData { get; set; }
    public byte[] SocketBuffer { get; set; }

    public ConnectedClient()
    {
        Socket = null;
        UserData = new UserData { Username = "", Color = Color.black };
        SocketBuffer = new byte[RecieveBuffer];
    }

    public void SetupRecieve()
    {
        Socket.BeginReceive(SocketBuffer, 0, RecieveBuffer, SocketFlags.None, new AsyncCallback(RecieveCallback), this);
    }

    public void RecieveCallback(IAsyncResult ar)
    {
        Socket.EndReceive(ar);
        InterpretData();
        SetupRecieve();
    }

    private void InterpretData()
    {
        var message = new NetworkMessage(SocketBuffer);

        if(OnMessageRecieved != null) {
            OnMessageRecieved(message, this);
        }
    }
}

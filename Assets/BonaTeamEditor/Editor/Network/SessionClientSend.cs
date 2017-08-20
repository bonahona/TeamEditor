using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TeamEditorExtensions;
using UnityEngine;

namespace BonaTeamEditor.Network
{
    public partial class SessionClient
    {
        public void SendDummy()
        {
            SendMessage(TcpClient.Client, NetworkMessageHelper.CreateDummyMessage());
        }
        public void SendUserData()
        {
            SendMessage(TcpClient.Client, NetworkMessageHelper.CreateUserMessage(UserData));
        }

        public void SendMessage(Socket socket, NetworkMessage message)
        {
            TeamEditorConsole.LogFormat("Client Sent {0} ", TeamEditorLogEntry.LogEntryType.Client, message.GetHeader(NetworkHeaders.Path));
            var byteBuffer = message.ToByteArray();
            socket.BeginSend(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(GenericSendCallback), socket);
        }

        public void GenericSendCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState.To<Socket>();
            client.EndConnect(ar);
        }
    }
}
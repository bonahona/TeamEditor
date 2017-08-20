using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BonaTeamEditor.Network.Messages;
using System.Net.Sockets;
using System;
using TeamEditorExtensions;

namespace BonaTeamEditor.Network
{
    public partial class SessionHost
    {
        public void SendSessionDescription(ConnectedClient client)
        {
            SendMessage(client, NetworkMessageHelper.CreateSessionDescription(new SessionServerDescription(this)));
        }

        public void SendMessage(ConnectedClient client, NetworkMessage message)
        {
            TeamEditorConsole.LogFormat("Host Sent {0} ", TeamEditorLogEntry.LogEntryType.Host, message.GetHeader(NetworkHeaders.Path));
            var byteBuffer = message.ToByteArray();
            client.Socket.BeginSend(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(GenericSendCallback), client.Socket);
        }

        public void GenericSendCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState.To<Socket>();
            client.EndConnect(ar);
        }
    }
}

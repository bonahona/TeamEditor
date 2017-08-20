using BonaTeamEditor.Network.Messages;
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
        public event MessageRecieved OnMessageRecieved;

        public byte[] SocketBuffer { get; set; }
        public NetworkRouter Router { get; set; }

        public void SetupRouter()
        {
            Router = new NetworkRouter();
            Router["/sessiondescription"] = HandleSessionDescription;
        }

        public void SetupRecieve()
        {
            SocketBuffer = new byte[1024];
            TcpClient.Client.BeginReceive(SocketBuffer, 0, SocketBuffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), TcpClient.Client);
        }

        public void RecieveCallback(IAsyncResult ar)
        {
            TcpClient.Client.EndReceive(ar);
            InterpretData();
            SetupRecieve();
        }

        private void InterpretData()
        {
            var message = new NetworkMessage(SocketBuffer);

            if (OnMessageRecieved != null) {
                OnMessageRecieved(message, null);
            }
        }

        private void OnMessageRecievedCallback(NetworkMessage message, ConnectedClient client)
        {
            TeamEditorConsole.LogFormat("Client Recieved {0} ", TeamEditorLogEntry.LogEntryType.Client, message.GetHeader(NetworkHeaders.Path));
            Router.Dispatch(message, client);
        }

        public void HandleSessionDescription(NetworkMessage message, ConnectedClient client)
        {
            SessionDescription = message.GetPayload<SessionServerDescription>();
        }
    }
}
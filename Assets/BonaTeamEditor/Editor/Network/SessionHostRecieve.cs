using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BonaTeamEditor.Network
{
    public partial class SessionHost
    {
        public NetworkRouter Router { get; set; }

        private void SetupRouter()
        {
            Router = new NetworkRouter();
            Router["/dummy"] = (n, c) => { };           // Do nothing
            Router["/userdata"] = HandleUserData;
        }

        private void OnMessageRecievedCallback(NetworkMessage message, ConnectedClient client)
        {
            TeamEditorConsole.LogFormat("Host Recieved {0} ", TeamEditorLogEntry.LogEntryType.Host, message.GetHeader(NetworkHeaders.Path));
            Router.Dispatch(message, client);
        }

        private void HandleUserData(NetworkMessage message, ConnectedClient client)
        {
            var userData = message.GetPayload<UserData>();
            client.UserData = userData;
            ConnectedUsers = GetUpdatedConnectedUsers();

            SendSessionDescription(client);
        }
    }
}
using BonaTeamEditor.Network;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BonaTeamEditor.Network.Messages
{
    [System.Serializable]
    public class SessionServerDescription
    {
        public string SessionName;
        public List<UserData> UserData;

        public SessionServerDescription()
        {
            UserData = new List<UserData>();
        }

        public SessionServerDescription(SessionHost host)
        {
            SessionName = host.SessionName;
            UserData = host.ConnectedUsers.ToList();
        }
    }
}

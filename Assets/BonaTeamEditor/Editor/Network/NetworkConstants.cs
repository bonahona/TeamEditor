using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace BonaTeamEditor.Network
{
    public static class NetworkConstants
    {
        public const int DefaultPort = 14511;
        public static readonly IPAddress DefaultListeningEndpoint = IPAddress.Any;
        public const string DefaultSessionName = "Noname session";
    }
}
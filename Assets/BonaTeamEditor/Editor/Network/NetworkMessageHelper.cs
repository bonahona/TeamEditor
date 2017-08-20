using BonaTeamEditor.Network;
using BonaTeamEditor.Network.Messages;
using System.Collections;
using System.Collections.Generic;
using TeamEditorExtensions;
using UnityEngine;

public static class NetworkMessageHelper
{
    public static NetworkMessage CreateDummyMessage()
    {
        return new NetworkMessage().Do(n => n.CreateHeaders("/dummy", "1"));
    }

    public static NetworkMessage CreateUserMessage(UserData userData)
    {
        return new NetworkMessage().Do(n => n.CreateHeaders("/userdata", "1")).Do(n => n.SetPayload(userData));
    }

    public static NetworkMessage CreateSessionDescription(SessionServerDescription description)
    {
        return new NetworkMessage().Do(n => n.CreateHeaders("/sessiondescription", "1")).Do(n => n.SetPayload(description));
    }
}

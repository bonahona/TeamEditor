using BonaTeamEditor.Network;
using BonaTeamEditor.Network.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkMessageHelper
{
    public static NetworkMessage CreateUserMessage(UserData userData)
    {
        var result = new NetworkMessage();
        result.CreateHeaders("/userdata", "1");
        result.SetPayload(userData);
        return result;
    }

    public static NetworkMessage CreateSessionDescription(SessionServerDescription description)
    {
        var result = new NetworkMessage();
        result.CreateHeaders("/sessiondescription", "1");
        result.SetPayload(description);
        return result;
    }
}

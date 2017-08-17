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
}

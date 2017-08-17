using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRouter
{
    // Exception thrown when the router can not find a handler for the Path set in the network messages header
    public class NoSuchNetworkPathException : Exception {
        public NoSuchNetworkPathException(String message) : base(message) { }
    }

    private Dictionary<string, Action<NetworkMessage, ConnectedClient>> Paths;

    public NetworkRouter()
    {
        Paths = new Dictionary<string, Action<NetworkMessage, ConnectedClient>>();
    }

    public Action<NetworkMessage, ConnectedClient> this[string path] {
        get {
            if (Paths.ContainsKey(path)) {
                return Paths[path];
            } else {
                throw new NoSuchNetworkPathException(string.Format("Network path {0} have no handler", path));
            }
        }

        set {
            if (Paths.ContainsKey(path)) {
                Paths[path] = value;
            } else {
                Paths.Add(path, value);
            }
        }
    } 

    public void Dispatch(NetworkMessage message, ConnectedClient client)
    {
        var path = message.GetHeader(NetworkHeaders.Path);
        this[path].Invoke(message, client);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public delegate void MessageRecieved(NetworkMessage message, ConnectedClient client);

public class NetworkMessage
{ 
    // Exception thrown when looking for a required header that is not present in the network message
    public class MissingHeaderException : Exception
    {
        public MissingHeaderException(String message) : base(message) { }
    }

    public const string NewLine = "\n";
    public const string Separator = "\n\n";

    public Dictionary<string, string> Headers { get; set; }
    public string PayLoad { get; set; }

    public NetworkMessage()
    {
        Headers = new Dictionary<string, string>();
        PayLoad = "";
    }

    public NetworkMessage(byte[] buffer)
    {
        Headers = new Dictionary<string, string>();
        PayLoad = "";

        var lines = Encoding.UTF8.GetString(buffer).Split('\n');

        var bodyFound = false;

        foreach(var line in lines) {
            if (bodyFound) {
                PayLoad = line;
            } else {
                if (line == string.Empty) {
                    bodyFound = true;
                } else {
                    var segments = line.Split(' ');
                    Headers.Add(segments[0], segments[1]);
                }
            }

        }
    }

    public void CreateHeaders(string path, string version)
    {
        CheckOrCreateHeader(NetworkHeaders.Path, path);
        CheckOrCreateHeader(NetworkHeaders.Version, version);
    }

    public string GetHeader(string headerName)
    {
        if (Headers.ContainsKey(headerName)) {
            return Headers[headerName];
        } else {
            throw new MissingHeaderException(string.Format("Header {0} not present in the network message", headerName));
        }
    }

    public void SetPayload(object o)
    {
        PayLoad = JsonUtility.ToJson(o);
    }

    public string GetPayload()
    {
        return PayLoad;
    }

    public T GetPayload<T>()
    {
        return JsonUtility.FromJson<T>(PayLoad);
    }

    public byte[] ToByteArray()
    {
        return Encoding.UTF8.GetBytes(ToString());
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach(var header in Headers) {
            builder.AppendFormat("{0} {1}\n", header.Key, header.Value);
        }
        builder.Append(NewLine);
        builder.Append(PayLoad);

        return builder.ToString();
    }

    private void CheckOrCreateHeader(string headerName, string value)
    {
        if (!Headers.ContainsKey(headerName)) {
            Headers.Add(headerName, value);
        } else {
            Headers[headerName] = value;
        }
    }
}

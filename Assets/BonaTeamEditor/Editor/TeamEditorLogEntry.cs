using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamEditorLogEntry
{
    public enum LogEntryType : int
    {
        None,
        Host,
        Client,
        Alert
    }

    public string Message { get; set; }
    public LogEntryType Type { get; set; }
}

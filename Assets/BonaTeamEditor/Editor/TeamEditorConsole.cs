using BonaTeamEditor.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LogMessage(TeamEditorLogEntry logEntry);

public class TeamEditorConsole
{
    public static TeamEditorConsole Instance { get; private set; }

    static TeamEditorConsole()
    {
        Instance = new TeamEditorConsole();
    }

    public static void Log(string message, TeamEditorLogEntry.LogEntryType type = TeamEditorLogEntry.LogEntryType.None)
    {
        Instance.Log(new TeamEditorLogEntry { Message = message, Type = type });
    }

    public static void LogFormat(string format, TeamEditorLogEntry.LogEntryType type, params string[] data)
    {
        Log(string.Format(format, data), type);
    }

    public static void LogAlert(string message)
    {
        Instance.Log(new TeamEditorLogEntry { Message = message, Type = TeamEditorLogEntry.LogEntryType.Alert });
    }

    public static void Clear()
    {
        Instance.ClearLog();
    }

    public LogMessage OnLogMessage { get; set; }
    public List<TeamEditorLogEntry> LogEntries { get; set; }

    public TeamEditorConsole()
    {
        LogEntries = new List<TeamEditorLogEntry>();
    }

    public void Log(TeamEditorLogEntry logEntry)
    {
        LogEntries.Add(logEntry);
        RaiseOnLogMessage(logEntry);
    }

    public void ClearLog()
    {
        LogEntries.Clear();
    }

    public void RaiseOnLogMessage(TeamEditorLogEntry logEntry)
    {
        if(OnLogMessage != null) {
            OnLogMessage(logEntry);
        }
    }
}

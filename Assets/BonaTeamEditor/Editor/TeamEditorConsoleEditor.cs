using System.Collections;
using System.Collections.Generic;
using TeamEditorExtensions;
using UnityEditor;
using UnityEngine;

public class TeamEditorConsoleEditor : EditorWindow
{
    public static readonly string[] WindowTabs = { "Client", "Host" };
    public const int ClientTabIndex = 0;
    public const int HostTabIndex = 1;

    [MenuItem(TeamEditorConstants.WindowConsoleBasePath)]
    public static void ShowSettingsWindow()
    {
        GetWindow<TeamEditorConsoleEditor>().Do(w => w.Show()).Do(w => TeamEditorConsole.Instance.OnLogMessage += w.OnLogEntry);
    }

    public void OnGUI()
    {
        titleContent = new GUIContent("Team Editor");

        foreach(var logEntry in TeamEditorConsole.Instance.LogEntries) {
            EditorGUILayout.LabelField(logEntry.Message);
        }
    }

    public void OnLogEntry(TeamEditorLogEntry logEntry)
    {
        Debug.Log("OnLogEntry");
        this.Repaint();
    }
}

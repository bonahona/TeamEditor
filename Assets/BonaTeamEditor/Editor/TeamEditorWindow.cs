using BonaTeamEditor.Network;
using BonaTeamEditor.Network.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamEditorWindow : EditorWindow
{
    public static readonly string[] WindowTabs = { "Client", "Host" };
    public const int ClientTabIndex = 0;
    public const int HostTabIndex = 1;

    [MenuItem(TeamEditorConstants.WindowSettingsBasePath)]
    public static void ShowSettingsWindow()
    {
        GetWindow<TeamEditorWindow>().Show();
    }

    private int SelectedTabIndex { get; set; }

    // Client settings
    private string ClientHostname;
    private string ClientPassword;
    private bool IsClientUserOpen;
    private UserData ClientUserData = new UserData { Username = "New User", Color = Color.red };

    // Host settings
    private string HostSessionName;
    private string HostPassword;
    private bool IsHostUserOpen;
    private UserData HostUserData = new UserData { Username = "New User", Color = Color.blue };

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnHierarchiChanged;
        Selection.selectionChanged += OnSelectionChanged;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnHierarchiChanged;
        Selection.selectionChanged -= OnSelectionChanged;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnGUI()
    {
        EditorGUILayout.Separator();
        SelectedTabIndex = GUILayout.Toolbar(SelectedTabIndex, WindowTabs);
        if(SelectedTabIndex == ClientTabIndex) {
            RenderClientGui();
        }else if(SelectedTabIndex == HostTabIndex) {
            RenderHostGui();
        }
    }

    private void RenderClientGui()
    {
        ClientHostname = EditorGUILayout.TextField("Hostname", ClientHostname);
        ClientPassword = EditorGUILayout.PasswordField("Password", ClientPassword);
        IsClientUserOpen = EditorGUILayout.Foldout(IsClientUserOpen, string.Format("User: {0}", ClientUserData.Username));
        if (IsClientUserOpen) {
            RenderClientData(ClientUserData);
        }

        if (!SessionClient.IsConnected) {
            if (GUILayout.Button("Join session")) {
                SessionClient.Connect(ClientHostname, ClientPassword, ClientUserData);
            }
        } else {
            if (GUILayout.Button("Disconnect")) {
                SessionClient.Disconnect();
            } else {
                EditorGUILayout.HelpBox(string.Format("Connected to: {0}\n at {1}",  GetSessionName(SessionClient.Instance), ClientHostname), MessageType.Info);
                RenderSessionDescriptionUsers(SessionClient.Instance.SessionDescription);
            }

            if(GUILayout.Button("Send Dummy")) {
                SessionClient.Instance.SendDummy();
            }
        }
    }

    private string GetSessionName(SessionClient client)
    {
        if(client == null) {
            return string.Empty;
        }

        if(client.SessionDescription == null) {
            return string.Empty;
        }

        return client.SessionDescription.SessionName;
    }

    private void RenderClientData(UserData clientData)
    {
        clientData.Username = EditorGUILayout.TextField("Name", clientData.Username);
        clientData.Color = EditorGUILayout.ColorField("Color", clientData.Color);
    }

    private void RenderHostGui()
    {
        EditorGUILayout.Separator();

        HostSessionName = EditorGUILayout.TextField("Session name", HostSessionName);
        HostPassword = EditorGUILayout.PasswordField("Session password", HostPassword);
        IsHostUserOpen = EditorGUILayout.Foldout(IsHostUserOpen, string.Format("User: {0}", HostUserData.Username));
        if (IsHostUserOpen) {
            RenderClientData(HostUserData);
        }

        if (!SessionHost.IsHosting) {
            if (GUILayout.Button("Start session")) {
                SessionHost.Start(HostSessionName, HostPassword, HostUserData);
            }
        } else {
            if (GUILayout.Button("Stop session")) {
                SessionHost.Stop();
            }

            EditorGUILayout.HelpBox("Session is running", MessageType.Info);
            RenderConnectedUsers();
        }
    }

    private void RenderSessionDescriptionUsers(SessionServerDescription sessionDescription)
    {
        if(sessionDescription == null) {
            return;
        }

        foreach(var user in sessionDescription.UserData) {
            RenderConnectedUser(user);
        }
    }

    private void RenderConnectedUsers()
    {
        if (SessionHost.IsHosting) {
            foreach (var user in SessionHost.Instance.ConnectedUsers) {
                RenderConnectedUser(user);
            }
        }
    }

    private void RenderConnectedUser(UserData userData)
    {
        EditorGUILayout.LabelField(userData.Username);
    }

    public void OnHierarchiChanged(SceneView sceneView)
    {

    }

    public void OnSelectionChanged()
    {
        Debug.Log("Selection change");

    }

    public void OnSceneChanged(Scene old, Scene open)
    {
        Debug.Log("Scene change");

    }

    public void OnAssetChange()
    {

    }
}

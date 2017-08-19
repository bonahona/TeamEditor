using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using TeamEditorExtensions;

namespace BonaTeamEditor.Network
{
    public partial class SessionHost
    {
        public static SessionHost Instance { get; private set; }

        public static void Start(string sessionName, string sessionPassword, UserData localUserData)
        {
            Instance = new SessionHost(sessionName, sessionPassword, localUserData);
        }

        public static void Stop()
        {
            if(Instance == null) {
                return;
            }

            Instance.StopListener();
            Instance = null;
        }

        public static bool IsHosting {
            get {
                return Instance != null;
            }
        }

        public TcpListener TcpListener { get; set; }
        public string SessionName { get; set; }
        public string SessionPassword { get; set; }
        public List<ConnectedClient> ConnectedClients { get; set; }

        public UserData LocalUserData { get; set; }
        public List<UserData> ConnectedUsers { get; set; }

        public SessionHost(string sessionName, string sessionPassword = "", UserData localUserData = null, IPAddress listeningAddress = null, int listeningPort = NetworkConstants.DefaultPort)
        {
            SessionName = sessionName;
            SessionPassword = sessionPassword;
            ConnectedClients = new List<ConnectedClient>();

            LocalUserData = localUserData;
            ConnectedUsers = GetUpdatedConnectedUsers();

            SetupRouter();

            TcpListener = new TcpListener(IPAddress.Any, listeningPort);
            TcpListener.Start();
            TcpListener.BeginAcceptTcpClient(new AsyncCallback(ConnectClientCallback), TcpListener);
        }

        public void StopListener()
        {
            if(TcpListener == null) {
                return;
            }

            TcpListener.Stop();

            foreach(var connectedClient in ConnectedClients) {
                connectedClient.Socket.Close();
            }
        }

        public void ConnectClientCallback(IAsyncResult ar)
        {
            var listener = ar.AsyncState.To<TcpListener>();
            var connectedClient = new ConnectedClient { Socket = listener.EndAcceptSocket(ar) };
            connectedClient.SetupRecieve();
            connectedClient.OnMessageRecieved += OnMessageRecievedCallback;
            ConnectedClients.Add(connectedClient);
        }

        private List<UserData> GetUpdatedConnectedUsers()
        {
            var result = new List<UserData>();
            result.Add(LocalUserData);

            foreach(var user in ConnectedClients.Map(c => c.UserData)) {
                result.Add(user);
            }

            return result;
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TeamEditorExtensions;
using UnityEngine;

namespace BonaTeamEditor.Network
{
    public class SessionClient
    {
        public static SessionClient Instance { get; private set; }

        public static void Connect(string hostName, string sessionPassword, UserData userData, int port = NetworkConstants.DefaultPort)
        {
            Instance = new SessionClient(hostName, port, sessionPassword, userData);
        }

        public static void Disconnect()
        {
            Instance = null;
        }

        public static bool IsConnected {
            get {
                if(Instance == null) {
                    return false;
                }

                return Instance.TcpClient.Client.Connected;
            }
        }

        public TcpClient TcpClient { get; set; }
        public string SessionPassword { get; set; }
        public UserData UserData { get; set; }

        public SessionClient(string hostName, int port, string sessionPassword, UserData userData)
        {
            SessionPassword = sessionPassword;
            UserData = userData;

            TcpClient = new TcpClient();
            TcpClient.BeginConnect(hostName, port, new AsyncCallback(ConnectCallback), TcpClient);
        }

        public void ConnectCallback(IAsyncResult ar)
        {
            var client = ar.AsyncState.To<TcpClient>();
            client.EndConnect(ar);

            if (!client.Client.Connected) {
                Debug.Log("Failed to connect");
                Disconnect();
                return;
            }

            Debug.Log("Client connected to server");
        }

        public void DisconnectClient()
        {
            if (TcpClient == null) {
                return;
            }

            TcpClient.EndConnect(null);
        }

        public void SendUserData()
        {
            TcpClient.Client.Send(NetworkMessageHelper.CreateUserMessage(UserData).ToByteArray());
        }

        public void DisconnectCallback(IAsyncResult ar)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Bolt;
using Bolt.Matchmaking;
using Bolt.Utils;
using UdpKit;
using UnityEngine;
using UnityEngine.Events;

public class BoltManager : GlobalEventListener
{
    public override void BoltStartBegin()
    {
        BoltNetwork.RegisterTokenClass<RoomInfoToken>();
        BoltNetwork.RegisterTokenClass<ClientConnectToken>();
        BoltNetwork.RegisterTokenClass<ServerRefuseToken>();
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            OnBoltStartDone_Server?.Invoke();
        }

        if (BoltNetwork.IsClient)
        {
            OnBoltStartDone_Client?.Invoke();
        }
    }

    #region Server

    public static void ConnectRequest_Server(UdpEndPoint endpoint, IProtocolToken token)
    {
        if (BoltNetwork.IsServer)
        {
            if (GameManager.Instance.Cur_BallBattleManager != null)
            {
                if (GameManager.Instance.Cur_BallBattleManager.IsStart)
                {
                    ServerRefuseToken srt = new ServerRefuseToken();
                    srt.Message = "The game has begun.";
                    BoltNetwork.Refuse(endpoint, srt);
                    return;
                }
            }

            if (cur_ServerRoomInfo != null)
            {
                if (cur_ServerRoomInfo.Max_PlayerNumber <= BoltNetwork.Connections.ToList().Count + 1)
                {
                    ServerRefuseToken srt = new ServerRefuseToken();
                    srt.Message = "The room is full";
                    BoltNetwork.Refuse(endpoint, srt);
                    return;
                }
                else
                {
                    if (token is ClientConnectToken cct)
                    {
                        BoltNetwork.Accept(endpoint);
                        return;
                    }
                    else
                    {
                        ServerRefuseToken srt = new ServerRefuseToken();
                        srt.Message = "Wrong protocol";
                        BoltNetwork.Refuse(endpoint, srt);
                        return;
                    }
                }
            }
            else
            {
                ServerRefuseToken srt = new ServerRefuseToken();
                srt.Message = "Errors in this room";
                BoltNetwork.Refuse(endpoint, srt);
                return;
            }
        }
    }

    public static void SwitchScene_Server(string sceneName)
    {
        BoltNetwork.LoadScene(sceneName);
    }

    public static UnityAction OnBoltStartDone_Server;
    private static RoomInfoToken cur_ServerRoomInfo;

    public static void StartServerSession(BattleTypes battleType, string roomName, bool hasPassword, string password, bool visible)
    {
        if (BoltNetwork.IsServer)
        {
            cur_ServerRoomInfo = new RoomInfoToken();
            cur_ServerRoomInfo.UdpEndPoint = BoltNetwork.UdpSocket.WanEndPoint;
            cur_ServerRoomInfo.RoomName = roomName;
            cur_ServerRoomInfo.IsVisible = visible;
            cur_ServerRoomInfo.BattleType = battleType;
            cur_ServerRoomInfo.Cur_PlayerNumber = 1;
            cur_ServerRoomInfo.M_Status = RoomInfoToken.Status.Waiting;
            cur_ServerRoomInfo.Max_PlayerNumber = ConfigManager.BattleMaxPlayerNumberDict[battleType];
            cur_ServerRoomInfo.HasPassword = hasPassword;
            cur_ServerRoomInfo.Password = password;

            // TODO game name existed bug
            BoltMatchmaking.CreateSession(
                sessionID: roomName,
                sceneToLoad: "Battle_" + battleType,
                token: cur_ServerRoomInfo
            );
        }
    }

    public static void UpdateCurrentSession()
    {
        if (BoltNetwork.IsRunning && BoltNetwork.IsServer && cur_ServerRoomInfo != null)
        {
            cur_ServerRoomInfo.Cur_PlayerNumber = BoltNetwork.Clients.ToList().Count + 1;
            if (GameManager.Instance.Cur_BattleManager)
            {
                if (GameManager.Instance.Cur_BattleManager.IsClosing)
                {
                    cur_ServerRoomInfo.M_Status = RoomInfoToken.Status.Closing;
                }
                else
                {
                    if (GameManager.Instance.Cur_BattleManager.IsStart)
                    {
                        cur_ServerRoomInfo.M_Status = RoomInfoToken.Status.Playing;
                    }
                    else
                    {
                        if (cur_ServerRoomInfo.Cur_PlayerNumber == cur_ServerRoomInfo.Max_PlayerNumber)
                        {
                            cur_ServerRoomInfo.M_Status = RoomInfoToken.Status.Full;
                        }
                        else
                        {
                            cur_ServerRoomInfo.M_Status = RoomInfoToken.Status.Waiting;
                        }
                    }
                }
            }

            BoltMatchmaking.UpdateSession(cur_ServerRoomInfo);
        }
    }

    #endregion

    void Update()
    {
        if (BoltNetwork.IsRunning && BoltNetwork.IsClient)
        {
        }
    }

    #region Client

    public static UnityAction OnBoltStartDone_Client;

    private UnityAction OnConnectedAction;

    private static void TryConnect(UdpSession session, string userName)
    {
        if (BoltNetwork.IsRunning && BoltNetwork.IsClient)
        {
            UIManager.Instance.ShowUIForms<WaitingPanel>();
            ClientConnectToken cct = new ClientConnectToken();
            cct.UserName = userName;
            BoltMatchmaking.JoinSession(session, cct);
        }
        else
        {
            BoltLog.Warn("Only a started client can join sessions");
        }
    }

    public override void ConnectAttempt(UdpEndPoint endpoint, IProtocolToken token)
    {
        ClientConnectToken cct = new ClientConnectToken();
        cct.UserName = "NewUser";
        base.ConnectAttempt(endpoint, cct);
    }

    public override void ConnectRefused(UdpEndPoint endpoint, IProtocolToken token)
    {
        base.ConnectRefused(endpoint, token);
        if (BoltNetwork.IsClient)
        {
            NoticeManager.Instance.ShowInfoPanelCenter(((ServerRefuseToken) token).Message, 0f, 0.5f);
        }
    }

    public override void Connected(BoltConnection connection)
    {
        if (BoltNetwork.IsClient)
        {
            base.Connected(connection);
            OnConnectedAction?.Invoke();
        }
    }

    public override void ConnectFailed(UdpEndPoint endpoint, IProtocolToken token)
    {
        if (BoltNetwork.IsClient)
        {
            base.ConnectFailed(endpoint, token);
            NoticeManager.Instance.ShowInfoPanelCenter("Failed to connect to bolt", 0f, 0.5f);
        }
    }

    public static UnityAction<List<RoomInfoToken>> RefreshRoomListInUI;

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        UpdateRoomList(sessionList, GameManager.Instance.LobbyPanel.CurrentFilter);
    }

    public static void UpdateRoomList(Map<Guid, UdpSession> sessionList, string filter)
    {
        Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);
        List<RoomInfoToken> roomInfos = new List<RoomInfoToken>();
        foreach (KeyValuePair<Guid, UdpSession> kv in sessionList)
        {
            if (kv.Value.Source == UdpSessionSource.Photon)
            {
                RoomInfoToken ri = (RoomInfoToken) kv.Value.GetProtocolToken();

                ri.OnRoomButtonClick = delegate
                {
                    switch (ri.M_Status)
                    {
                        case RoomInfoToken.Status.Playing:
                        {
                            NoticeManager.Instance.ShowInfoPanelCenter("The game has begun", 0f, 0.5f);
                            break;
                        }
                        case RoomInfoToken.Status.Full:
                        {
                            NoticeManager.Instance.ShowInfoPanelCenter("The room is full", 0f, 0.5f);
                            break;
                        }
                        case RoomInfoToken.Status.Closing:
                        {
                            NoticeManager.Instance.ShowInfoPanelCenter("The game has closed", 0f, 0.5f);
                            break;
                        }
                        case RoomInfoToken.Status.Waiting:
                        {
                            if (ri.HasPassword)
                            {
                                PasswordPanel pp = UIManager.Instance.ShowUIForms<PasswordPanel>();
                                pp.ConfirmButton.onClick.RemoveAllListeners();
                                pp.ConfirmButton.onClick.AddListener(delegate
                                {
                                    if (ri.Password == pp.PasswordInputField.text.EncodeSHA512())
                                    {
                                        TryConnect(kv.Value, "NewUser");
                                    }
                                    else
                                    {
                                        NoticeManager.Instance.ShowInfoPanelCenter("Wrong password", 0f, 0.5f);
                                    }
                                });
                            }
                            else
                            {
                                TryConnect(kv.Value, "NewUser");
                            }

                            break;
                        }
                    }
                };

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    if (ri.RoomName.ToUpper().Contains(filter.ToUpper()))
                    {
                        roomInfos.Add(ri);
                    }
                }
                else
                {
                    if (ri.IsVisible)
                    {
                        roomInfos.Add(ri);
                    }
                }
            }
        }

        RefreshRoomListInUI?.Invoke(roomInfos);
    }

    #endregion
}
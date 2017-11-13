using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Google.Protobuf;
using Com.Violet.Rpc;

public delegate void MockResponse(IMessage request, RpcResponse callback);

public class MockManager : Manager
{
    public const string Name = "MockManager";
    static Dictionary<KeyCode, Action> m_mockActionMap = new Dictionary<KeyCode, Action>();
    static Dictionary<string, MockResponse> m_mockRequestMap = new Dictionary<string, MockResponse>();

    public override void OnManagerReady()
    {
        //注册Mock的Response
        //RegisteMockResponse("Hello",HelloResponse);
        //RegisteMockResponse("Login",LoginResponse);

        //注册Mock的Push
        RegisteMockAction(KeyCode.Keypad1,EnterBattleFieldPush);

        RegisteMockAction(KeyCode.Q,PlayerStateChagePush_Ready);
    }
    public override void OnManagerDestroy()
    {
        m_mockActionMap.Clear();
        m_mockRequestMap.Clear();
    }
    public void RegisteMockAction(KeyCode keyCode, Action action)
    {
        if (!m_mockActionMap.ContainsKey(keyCode))
            m_mockActionMap.Add(keyCode, action);
        else
            m_mockActionMap[keyCode] = action;
    }

    public void RegisteMockResponse(string rpc, MockResponse response)
    {
        
        if (!m_mockRequestMap.ContainsKey(rpc))
            m_mockRequestMap.Add(rpc, response);
        else
            m_mockRequestMap[rpc] = response;
    }

    public void MockPush(string rpcName, IMessage data)
    {
        App.Manager.Network.DispatchPush(rpcName, data);
    }

    public bool HasMock(string rpc)
    {
        return m_mockRequestMap.ContainsKey(rpc);
    }

    public void MockResponse(string rpc, IMessage request, RpcResponse callback)
    {
        if (m_mockRequestMap.ContainsKey(rpc)) m_mockRequestMap[rpc](request, callback);
    }

    private void OnGUI()
    {
        Event ev = Event.current;
        if (ev.isKey && Input.anyKeyDown && ev.keyCode != KeyCode.None)
        {
            if (m_mockActionMap.ContainsKey(ev.keyCode)) m_mockActionMap[ev.keyCode]();
        }
    }

    /// <summary>
    /// 按键进行PushMock
    /// </summary>
    #region
    void RoomStateChangePush()
    {
    }

    void EnterBattleFieldPush()
    {
    }

    void PlayerStateChagePush_Ready()
    {
        PlayerStateChagePush push = new PlayerStateChagePush();
        push.PlayerInfo = new Com.Violet.Rpc.PlayerInfo();
        push.PlayerInfo.State = (int)PlayerState.READY;
        //push.PlayerInfo.UserId = App.Package.ChessGame.EnemyPlayerData.playerInfo.userId;
        //push.PlayerInfo.UserName = App.Package.ChessGame.EnemyPlayerData.playerInfo.userName;
        MockPush("PlayerStateChage",push);
    }

    void GameStateChagePush()
    {
    }

    void ChessMovePush()
    {
    }

    void ChatMessagePush()
    {
    }
    #endregion


    /// <summary>
    /// mock的Response操作
    /// </summary>
    #region
    void HelloResponse(IMessage request, RpcResponse callback)
    {
        HelloRequest requestRpc = (HelloRequest) request;
        HelloResponse response = new HelloResponse();
        response.Greet = "Mock:"+requestRpc.Content;
        callback(response);
    }
    void LoginResponse(IMessage request, RpcResponse callback)
    {
        LoginRequest requestRpc = (LoginRequest)request;
        LoginResponse response = new LoginResponse();
        response.PlayerInfo = new PlayerInfo();
        response.PlayerInfo.UserId = 1;
        response.PlayerInfo.UserName = "KyArvis";
        response.PlayerInfo.ZoneId = 1;
        response.PlayerInfo.State = (int)PlayerState.UNKNOW;
        response.ServerTime = DateTime.Now.Ticks / 10000;
        Debuger.Log("userLogin:" + requestRpc.UserName);
        callback(response);
    }
    void FindEnemyResponse(IMessage request, RpcResponse callback)
    {

    }
    void CancelFindEnemyResponse(IMessage request, RpcResponse callback)
    {

    }
    void GetRoomListResponse(IMessage request, RpcResponse callback)
    {

    }
    void EnterRoomResponse(IMessage request, RpcResponse callback)
    {

    }
    void ReadyInRoomResponse(IMessage request, RpcResponse callback)
    {

    }
    void LeaveRoomResponse(IMessage request, RpcResponse callback)
    {

    }
    void MoveChessResponse(IMessage request, RpcResponse callback)
    {

    }
    void BattleMapResponse(IMessage request, RpcResponse callback)
    {

    }
    void CheckGameStateResponse(IMessage request, RpcResponse callback)
    {

    }
    void EnterBattleFieldResponse(IMessage request, RpcResponse callback)
    {

    }
    void SurrenderResponse(IMessage request, RpcResponse callback)
    {

    }
    void SendChatMessageResponse(IMessage request, RpcResponse callback)
    {

    }
    #endregion
}

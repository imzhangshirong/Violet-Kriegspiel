﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Google.Protobuf;

public delegate void NetworkPush(IMessage rpcData);
public class NetworkManager : Manager
{
    public const string Name = "NetworkManager";

    static Dictionary<string, string> m_NetPushMap = new Dictionary<string, string>();
    RpcNetwork m_rpc;
    
    public override void OnManagerReady()
    {
        RpcNetwork.Instance.Init();
        RpcNetwork.Instance.BindRpcReceive(DispatchPush);
    }
    public override void OnManagerDestroy()
    {
        m_NetPushMap.Clear();
        RpcNetwork.Instance.Destroy();
    }

    /// <summary>
    /// 注册socket推送
    /// </summary>
    /// <param name="rpcName"></param>
    /// <param name="eventMsg"></param>
    public void RegisterPush(string rpcName,string eventMsg)
    {
        if (!m_NetPushMap.ContainsKey(rpcName))
        {
            m_NetPushMap.Add(rpcName, eventMsg);
        }
        else
        {
            m_NetPushMap[rpcName] = eventMsg;
        }

    }

    void DispatchPush(string msg, IMessage data)
    {
        if (m_NetPushMap.ContainsKey(msg) && m_NetPushMap[msg] != "")
        {
            AppInterface.EventManager.Broadcast(m_NetPushMap[msg], data);
        }
    }
}
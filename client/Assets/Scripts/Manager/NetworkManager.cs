using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;

public delegate void NetworkPush(IMessage rpcData);
public class NetworkManager : Manager
{
    public const string Name = "NetworkManager";
    static Dictionary<int, Action> m_ErrorCodeAlert = new Dictionary<int, Action>();
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

    public void DispatchPush(string msg, IMessage data)
    {
        if (m_NetPushMap.ContainsKey(msg) && m_NetPushMap[msg] != "")
        {
            App.Manager.Event.Broadcast(m_NetPushMap[msg], data);
        }
    }

    public void Request<T>(string msg, T rpc, RpcResponse callback, bool autoRetry = true, bool needUIWaiting = true, bool needUIRetry = true) where T : IMessage
    {
        if (App.Manager.Mock.HasMock(msg))//启用Mock
            App.Manager.Mock.MockResponse(msg, rpc, callback);
        else
            RpcNetwork.Request<T>(msg, rpc, callback,autoRetry,needUIWaiting,needUIRetry);
    }

    public void RegisteErrorCode(int code,Action action)
    {
        if (!m_ErrorCodeAlert.ContainsKey(code))
            m_ErrorCodeAlert.Add(code, action);
        else
            m_ErrorCodeAlert[code] = action;
    }

    public bool HasRegistedErrorCode(int code)
    {
        return m_ErrorCodeAlert.ContainsKey(code);
    }
    public void DoErrorCode(int code)
    {
        if (m_ErrorCodeAlert.ContainsKey(code)) m_ErrorCodeAlert[code]();
    }
}
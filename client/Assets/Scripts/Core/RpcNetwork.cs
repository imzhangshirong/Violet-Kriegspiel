using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Google.Protobuf;
using Com.Violet.Rpc;
using System.Reflection;
using System.Threading;
using UnityEngine;

public delegate void RpcResponse(IMessage response);
public delegate void RpcReceiveListener(string rpcName, IMessage data);
public class RpcNetwork
{
    private static RpcNetwork m_Instance;

    bool m_isReady;
    bool m_isQueue;
    Thread m_Receive;
    Thread m_process;

    Thread m_heartBeat;
    Thread m_timeOut;
    public static bool IsReady{
        get
        {
            return m_Instance.m_isReady;
        }
    }
    public static bool IsQueue
    {
        get
        {
            return m_Instance.m_isQueue;
        }
    }
    public static RpcNetwork Instance {
        get {
            if (m_Instance != null)
            {
                return m_Instance;
            }
            return new RpcNetwork();
        }
    }
    static Socket m_socket;
    static List<byte[]> m_messageQueue = new List<byte[]>();//注意线程安全
    static Dictionary<string, RPC> m_RpcMap = new Dictionary<string, RPC>();//注意线程安全
    static Dictionary<string, RpcRequestUIData> m_RpcUIMap = new Dictionary<string, RpcRequestUIData>();
    static Dictionary<string, int> m_RpcUniqueIdMap = new Dictionary<string, int>();//维护uniqueId的最大值
    static RpcReceiveListener m_Listener = null;
    static int m_waitUICount = 0;
    string m_Token = "";
    public static string Token
    {
        get
        {
            return Instance.m_Token;
        }
    } 
    
    private static void Send(RPC rpc)
    {
        if (m_socket.Connected)
        {
            Debuger.Log(rpc.msg + " start send");
            m_socket.BeginSend(rpc.sendData, 0, rpc.sendData.Length, SocketFlags.None, Sent, m_socket);
        }
    }

    public static void SendUnEncode(string data)
    {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
        m_socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, Sent, m_socket);
    }

    ///只重发断开后的
    private static void ResendAll()
    {
        lock (m_RpcMap)
        {
            foreach (var item in m_RpcMap)
            {
                RPC rpc = item.Value;
                if(rpc.state == RpcState.Disconnect)Resend(rpc);
            }
        }
    }

    public static void Request<T>(string msg,T rpc,RpcResponse callback, bool autoRetry = true, bool needUIWaiting = true, bool needUIErrorCodeAlert = true, bool needUIRetry = true) where T : IMessage
    {
        
        ByteString rpcata = rpc.ToByteString();
        _Request request = new _Request();
        request.Token = Token;//Token用于识别
        request.Rpc = msg;
        request.Data = rpcata;
        UpdateUniqueIdMax(msg);
        request.Unique = GetUniqueIdMax(msg) + 1;//同名的唯一标识
        byte[] binaryData = request.ToByteArray();
        byte[] sendData = BuildSocketBinaryData(new SocketData {
            version = 1,
            dataLength = binaryData.Length,
            data = binaryData
        });
        RPC rpcData = new RPC
        {
            msg = msg,
            rpcType = rpc.GetType(),
            callback = callback,
            sendData = sendData,
            timestamp = DateTime.Now.Ticks,
            unique = request.Unique,
            state = RpcState.Waiting,
            autoRetry = autoRetry,
        };
        if(!m_RpcUniqueIdMap.ContainsKey(rpcData.uniqueName)){
            m_RpcUIMap.Add(rpcData.uniqueName,new RpcRequestUIData(){
                needErrorCodeAlert = needUIErrorCodeAlert,
                needRetry = needUIRetry,
                needWaitingUI = needUIWaiting,
            });
        }
        if(needUIWaiting){
            App.Manager.Thread.RunOnMainThread(()=>{
                Common.UI.OpenWaiting();
            });
            m_waitUICount ++;
        }
        AddNewRequest(rpcData);
        Send(rpcData);
    }
    
    static void Resend(RPC rpc){
        m_RpcMap.Remove(rpc.uniqueName);
        UpdateUniqueIdMax(rpc.msg);
        rpc.unique = GetUniqueIdMax(rpc.msg) + 1;
        rpc.timestamp = DateTime.Now.Ticks;
        rpc.state = RpcState.Waiting;
        AddNewRequest(rpc);
        Send(rpc);
    }
    static int GetUniqueIdMax(string rpcName)
    {
        if (!m_RpcUniqueIdMap.ContainsKey(rpcName))
        {
            m_RpcUniqueIdMap.Add(rpcName, 0);
        }
        return m_RpcUniqueIdMap[rpcName];
    }

    static void UpdateUniqueIdMax(string rpcName){
        if (!m_RpcUniqueIdMap.ContainsKey(rpcName))
        {
            m_RpcUniqueIdMap.Add(rpcName, 0);
        }
        int maxId = 0;
        foreach(var item in m_RpcMap){
            if(item.Value.msg == rpcName && item.Value.unique>maxId)maxId = item.Value.unique;
        }
        m_RpcUniqueIdMap[rpcName] = maxId;
    }

    static void AddNewRequest(RPC rpcData){
        lock (m_RpcMap)
        {
            Debuger.Log(rpcData.msg+" add in Queen");
            //加入列队
            if(!m_RpcMap.ContainsKey(rpcData.uniqueName)){
                m_RpcMap.Add(rpcData.uniqueName,rpcData);
            }
        }
    }

    private static byte[] BuildSocketBinaryData(SocketData socketData)
    {
        int headerLength = 10;
        byte[] bufferArray = new byte[headerLength + socketData.dataLength + 2];
        //头标识
        bufferArray[0] = 255;
        //socket结束标识
        bufferArray[bufferArray.Length - 2] = 0;
        bufferArray[bufferArray.Length - 1] = 254;
        bufferArray[1] = (byte)socketData.version;//协议版本号
        switch (socketData.version)
        {
            case 1:
                WriteLengthData(bufferArray, socketData.dataLength, 2, 3);//写出数据长度信息
                break;
        }
        //写出数据
        for (int i = 0; i < socketData.data.Length; i++)
        {
            bufferArray[headerLength + i] = socketData.data[i];
        }
        //
        return bufferArray;
    }
    private static SocketData ParseSocketBinaryData(byte[] bufferArray)
    {
        int headerLength = 10;
        SocketData sd = new SocketData();
        if (bufferArray[0] == 255)
        {
            sd.version = bufferArray[1];
            switch (sd.version)
            {
                case 1:
                    sd.dataLength = GetDataLength(bufferArray, 2, 3);
                    break;
            }
            byte[] data = new byte[sd.dataLength];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = bufferArray[headerLength + i];
            }
            sd.data = data;
            return sd;
        }
        return null;

    }

    private static void WriteLengthData(byte[] bufferArray,int dataLength,int offset,int length)
    {
        int l = dataLength;
        int d = 0;
        for (int i = length - 1; i > 0; i--)
        {
            d = l % 256;
            l = (int)Math.Floor((double)(l / 256));
            bufferArray[offset + i] = (byte)d;
        }
    }
    private static int GetDataLength(byte[] socketArray,int offset,int length)
    {
        int dataLength = 0;
        int baseSize = 1;
        for (int i = length - 1; i >= 0; i--)
        {
            dataLength += socketArray[offset + i] * baseSize;
            baseSize *= 256;
        }
        return dataLength;
    }
    private static int GetTransDataEnd(byte[] data)//-2为空，-1为没找到结尾
    {
        int empty = 0;
        for (int i = 0; i < data.Length - 1; i++)
        {
            if (data[i] == 0)
            {
                if (data[i + 1] == 254) return i + 1;
                empty++;
            }
        }
        if (empty == data.Length - 1 && data[empty + 1] == 0)
        {
            return -2;
        }
        return -1;
    }

    public void Init(bool sync = false)
    {
        m_isReady = false;
        m_isQueue = false;
        if (m_process != null) m_process.Abort();
        if (m_Receive != null) m_Receive.Abort();
        if (m_heartBeat != null) m_heartBeat.Abort();
        if (m_timeOut != null) m_timeOut.Abort();
        m_messageQueue.Clear();
        if (m_socket != null)
        {
            m_socket.Close();
            //m_socket.Shutdown(SocketShutdown.Both);
            m_socket = null;
        }
        if (m_socket == null)
        {
            
            App.Manager.Thread.RunOnMainThread(() => {
                Debuger.Log("Network Connecting");
            });
            IPAddress[] addr = Dns.GetHostAddresses(Config.ServerHost);
            var clientEndPoint = new IPEndPoint(addr[0], Config.ServerHostPort);
            m_socket = new Socket(addr[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                if (sync)
                {
                    m_socket.Connect(clientEndPoint);
                }
                else
                {
                    App.Manager.Thread.RunAsync(() =>
                    {
                        try
                        {
                            m_socket.Connect(clientEndPoint);
                            if (m_socket.Connected)
                            {
                                Connected(null);
                            }
                        }
                        catch (Exception e)
                        {
                            App.Manager.Thread.RunOnMainThread(() => {
                                Debuger.Error(e.ToString());
                            });
                            Disconnect();
                        }
                    });
                }
            }
            catch (Exception e)
            {
                App.Manager.Thread.RunOnMainThread(() => {
                    Debuger.Error(e.ToString());
                });
            }
        }
        else
        {
            Connected(null);
        }
        
    }
    public void Destroy()
    {
        try
        {
            m_isReady = false;
            m_isQueue = false;
            m_messageQueue.Clear();
            m_RpcMap.Clear();
            m_RpcUIMap.Clear();
            m_RpcUniqueIdMap.Clear();
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
            m_socket.Disconnect(false);
            m_socket = null;
        }
        catch(Exception e)
        {
            Debuger.Warn(e.ToString());
        }

    }
    /// <summary>
    /// 处理socket包接收
    /// </summary>
    private void QueueReceive()
    {
        int blockSize = 256;
        while (m_isQueue)
        {
            if (!m_socket.Connected) break;
            if (m_isReady)
            {
                byte[] buffer = new byte[blockSize];
                if (m_socket.Receive(buffer) > 0)
                {
                    lock (m_messageQueue)
                    {
                        m_messageQueue.Add(buffer);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 处理rpc组合、分发
    /// </summary>
    private void QueueProcess()
    {

        try
        {
            while (m_isQueue)
            {
                if (!m_socket.Connected) break;
                if (m_isReady)
                {
                    lock (m_messageQueue)
                    {
                        if (m_messageQueue.Count > 0)
                        {
                            byte[] dataBuffer = m_messageQueue[0];//获取第一个包
                            int endPoint = -1;
                            if (dataBuffer[0] == 255)//检测是否为包头
                            {
                                endPoint = GetTransDataEnd(dataBuffer);
                                int blockRange = 0;
                                for (int i = 1; i < m_messageQueue.Count; i++)
                                {
                                    if (endPoint > -1)//存在包结束标识结束
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        //合并下个包
                                        byte[] tempBuffer = new byte[dataBuffer.Length + m_messageQueue[i].Length];
                                        for (int j = 0; j < dataBuffer.Length; j++)
                                        {
                                            tempBuffer[j] = dataBuffer[j];
                                        }
                                        for (int j = 0; j < m_messageQueue[i].Length; j++)
                                        {
                                            tempBuffer[dataBuffer.Length + j] = m_messageQueue[i][j];
                                        }
                                        dataBuffer = tempBuffer;
                                        blockRange++;
                                        endPoint = GetTransDataEnd(dataBuffer);
                                    }
                                }
                                if (endPoint > -1)
                                {
                                    byte[] messageBuffer = new byte[endPoint + 1];
                                    for (int j = 0; j < messageBuffer.Length; j++)
                                    {
                                        messageBuffer[j] = dataBuffer[j];
                                    }
                                    byte[] remainBuffer = new byte[dataBuffer.Length - (endPoint + 1)];
                                    int empty = 0;
                                    for (int j = 0; j < remainBuffer.Length; j++)
                                    {
                                        remainBuffer[j] = dataBuffer[messageBuffer.Length + j];
                                        if (remainBuffer[j] == 0)
                                        {
                                            empty++;
                                        }

                                    }
                                    if (empty != remainBuffer.Length)
                                    {
                                        {
                                            m_messageQueue[0] = remainBuffer;
                                            m_messageQueue.RemoveRange(1, blockRange);
                                        }
                                    }
                                    else
                                    {
                                        {
                                            m_messageQueue.RemoveRange(0, blockRange + 1);
                                        }
                                    }
                                    //开始处理完整的包
                                    DealSocketData(messageBuffer);
                                }
                                else //还没有收到包含结束标识的包
                                {
                                    {
                                        m_messageQueue[0] = dataBuffer;
                                        m_messageQueue.RemoveRange(1, blockRange);
                                    }
                                }
                            }
                            else
                            {
                                {
                                    m_messageQueue.RemoveAt(0);//丢弃当前包
                                }
                            }
                        }
                    }
                    
                }
            }
        }
        catch(Exception e)
        {
            App.Manager.Thread.RunOnMainThread(() => {
                Debuger.Error(e.ToString());
            });
        }
        if(!m_socket.Connected)Disconnect();
    }


    private void HeartBeatProcess(){

    }

    
    private void TimeTick(){
        while(true){
            {
                foreach(var item in m_RpcMap){
                    RPC rpc = item.Value;
                    if(rpc.isTimeout()){
                        App.Manager.Thread.RunOnMainThread(()=>{
                            Debuger.Error(rpc.msg+":Timeout");
                        });
                        rpc.state = RpcState.Timeout;
                        if(m_RpcUIMap.ContainsKey(rpc.uniqueName)){
                            RpcRequestUIData uiData = m_RpcUIMap[rpc.uniqueName];
                            if(rpc.autoRetry){
                                Resend(rpc);
                            }
                            if(uiData.needRetry){
                                App.Manager.Thread.RunOnMainThread(()=>{
                                    Common.UI.OpenRetry();
                                });
                                
                            }
                        }
                        else{
                            lock(m_RpcMap){
                                m_RpcMap.Remove(rpc.uniqueName);
                            }
                            
                        }
                    }
                }
            }
            Thread.Sleep(1000);
        }
    }

    RPC GetRpcData(string rpc,int uniqueId){
        string uniqueName = rpc + "." + uniqueId;
        if(m_RpcMap.ContainsKey(uniqueName)){
            return m_RpcMap[uniqueName];
        }
        return null;
    }

    private void DealSocketData(byte[] receiveData)
    {
        SocketData socketData = ParseSocketBinaryData(receiveData);
        _Response response = _Response.Parser.ParseFrom(socketData.data);
        Assembly assem = response.GetType().Assembly;
        RPC currentRpc = GetRpcData(response.Rpc, response.Unique);
        if (currentRpc != null){
            if (currentRpc.isTimeout()){//已经超时的不处理
                lock (m_RpcMap)
                {
                    m_RpcMap.Remove(currentRpc.uniqueName);
                }
                return;
            }
            if (response.Token != "") m_Token = response.Token;//统一身份标识
            if (response.Code != 0)//处理ErrorCode
            {
                if (m_RpcUIMap.ContainsKey(currentRpc.uniqueName))
                {
                    RpcRequestUIData uiData = m_RpcUIMap[currentRpc.uniqueName];
                    if (uiData.needErrorCodeAlert)
                    {
                        App.Manager.Thread.RunOnMainThread(()=>{
                            Debuger.Log("Receive Rpc:"+response.Code);
                        });
                        if (App.Manager.Network.HasRegistedErrorCode(response.Code))
                            App.Manager.Thread.RunOnMainThread(() =>
                            {
                                Common.UI.CloseWaiting();
                                App.Manager.Network.DoErrorCode(response.Code);
                            });
                        else
                            App.Manager.Thread.RunOnMainThread(() =>
                            {
                                Common.UI.CloseWaiting();
                                Common.UI.OpenAlert("错误", "ErrorCode:" + response.Code, "确认", null);
                            });
                    }
                    if(uiData.needWaitingUI)m_waitUICount--;
                    if(m_waitUICount<=0){
                        App.Manager.Thread.RunOnMainThread(() =>
                        {
                            Common.UI.CloseWaiting();
                        });
                    }
                    lock (m_RpcUIMap)
                    {
                        m_RpcUIMap.Remove(currentRpc.uniqueName);
                    }
                }
                lock (m_RpcMap)
                {
                    m_RpcMap.Remove(currentRpc.uniqueName);
                }
            }
            else
            {
                
                Type type = assem.GetType(Config.RpcNamespace + "." + response.Rpc + "Response");//这里要按规则来！！！！
                if (type != null)
                {
                    IMessage resRpc = Activator.CreateInstance(type) as IMessage;
                    resRpc.MergeFrom(response.Data);
                    App.Manager.Thread.RunOnMainThread(() =>
                    {
                        currentRpc.callback(resRpc);
                    });
                    if (m_RpcUIMap.ContainsKey(currentRpc.uniqueName))
                    {
                        RpcRequestUIData uiData = m_RpcUIMap[currentRpc.uniqueName];
                        if(uiData.needWaitingUI)m_waitUICount--;
                        if(m_waitUICount<=0){
                            App.Manager.Thread.RunOnMainThread(() =>
                            {
                                Common.UI.CloseWaiting();
                            });
                        }
                        lock (m_RpcUIMap)
                        {
                            m_RpcUIMap.Remove(currentRpc.uniqueName);
                        }
                    }
                    lock (m_RpcMap)
                    {
                        m_RpcMap.Remove(currentRpc.uniqueName);
                    }
                }
                
            }
        }
        else{
            Type type = assem.GetType(Config.RpcNamespace + "." + response.Rpc + "Push");//这里要按Push规则来！！！！
            if (type != null)
            {
                IMessage resRpc = Activator.CreateInstance(type) as IMessage;
                resRpc.MergeFrom(response.Data);
                App.Manager.Thread.RunOnMainThread(()=>{
                    m_Listener.Invoke(response.Rpc, resRpc);//分发给注册的Listener
                });
            }
        }
    }

    private void Connected(IAsyncResult ar)
    {
        if (m_socket.Connected)
        {
            m_isReady = true;
            App.Manager.Thread.RunOnMainThread(() =>
            {
                Debuger.Log("Connected");
            });
            //开始监听
            m_isQueue = true;
            m_Receive = App.Manager.Thread.RunAsync(QueueReceive);
            m_process = App.Manager.Thread.RunAsync(QueueProcess);
            m_timeOut = App.Manager.Thread.RunAsync(TimeTick);
            m_heartBeat = App.Manager.Thread.RunAsync(HeartBeatProcess);
        }
    }
    private void Disconnect()
    {
        App.Manager.Thread.RunOnMainThread(() => {
            Debuger.Log("Disconnected");
        });
        while (!m_socket.Connected)
        {
            App.Manager.Thread.RunOnMainThread(() => {
                Debuger.Log("Reconnecting:"+ m_socket.Connected);
            });
            Thread.Sleep(1000);
            RpcNetwork.Instance.Init(true);
        }
        Reconnected();
        
    }
    private void Reconnected()
    {
        Connected(null);
        ResendAll();
        App.Manager.Thread.RunOnMainThread(() => {
            Debuger.Log("Reconnected");
        });
    }
    private static void Sent(IAsyncResult ar)
    {
        //
        App.Manager.Thread.RunOnMainThread(() => {
            Debuger.Log("finished");
        });
    }

    public void BindRpcReceive(RpcReceiveListener listener)
    {
        m_Listener = listener;
    }
}

public class RpcRequestUIData
{
    public bool needWaitingUI;
    public bool needErrorCodeAlert;
    public bool needRetry;
}

public class RPC
{
    public string msg;
    public Type rpcType;
    public RpcResponse callback;
    public byte[] sendData;
    public long timestamp;
    public int unique;
    public RpcState state = RpcState.Waiting;
    public bool autoRetry;
    public string uniqueName
    {
        get
        {
            return msg + "." + unique;
        }
    }
    public bool isTimeout(){
        return (DateTime.Now.Ticks - timestamp)/(long)10000 > Config.RpcTimeout;
    }
}
public class SocketData
{
    public uint version;
    public int dataLength;
    public byte[] data;
}
public enum RpcState{
    Waiting = 0,
    Timeout = 1,
    Disconnect = 2,
}

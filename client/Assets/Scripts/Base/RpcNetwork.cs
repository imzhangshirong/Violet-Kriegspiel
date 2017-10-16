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

public class RpcNetwork
{
    private static RpcNetwork m_Instance;
    bool m_isReady;
    bool m_isQueue;
    Thread m_recieve;
    Thread m_process;
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
    static List<RPC> m_RpcQueue = new List<RPC>();//注意线程安全

    class RPC {
        public string msg;
        public Type rpcType;
        public RpcResponse callback;
        public byte[] sendData;
    }
    class SocketData
    {
        public uint version;
        public int dataLength;
        public byte[] data;
    }

    private static void Send(RPC rpc)
    {
        if (m_socket.Connected) m_socket.BeginSend(rpc.sendData, 0, rpc.sendData.Length, SocketFlags.None, Sent, m_socket);
    }

    private static void ResendAll()
    {
        lock (m_RpcQueue)
        {
            for (int i = 0; i < m_RpcQueue.Count; i++)
            {
                RPC rpc = m_RpcQueue[i];
                Send(rpc);
            }
        }
    }

    public static void Request<T>(string msg,T rpc,RpcResponse callback) where T : IMessage
    {
        
        ByteString rpcata = rpc.ToByteString();
        _Request request = new _Request();
        request.Token = "";//Token用于识别
        request.Rpc = msg;
        request.Data = rpcata;
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
        };
        lock (m_RpcQueue)
        {
            //加入列队
            m_RpcQueue.Add(rpcData);
        }
        Send(rpcData);
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
        if (m_recieve != null) m_recieve.Abort();
        m_messageQueue.Clear();
        if (m_socket == null || !m_socket.Connected)
        {
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
                    m_socket.BeginConnect(clientEndPoint, Connected, m_socket);
                }
            }
            catch (Exception e)
            {
                Debuger.Error(e.ToString());
            }
        }
        else
        {
            Connected(null);
        }
        
    }
    public void Destroy()
    {
        m_isReady = false;
        m_isQueue = false;
        m_messageQueue.Clear();
        m_RpcQueue.Clear();
        m_socket.Close();
        m_socket = null;
        //退出线程
        if (m_process != null) m_process.Abort();
        if (m_recieve != null) m_recieve.Abort();

    }
    private void QueueRecieve()
    {
        int blockSize = 256;
        while (m_isQueue)
        {
            if (!m_socket.Connected) break;
            if (m_isReady)
            {
                byte[] buffer = new byte[blockSize];
                m_socket.Receive(buffer);
                lock(m_messageQueue)
                {
                    m_messageQueue.Add(buffer);
                }
            }
        }
    }
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
            AppInterface.ThreadManager.RunOnMainThread(() => {
                Debuger.Error(e.ToString());
            });
        }
        if(!m_socket.Connected)Disconnect();
    }
    private void DealSocketData(byte[] receiveData)
    {
        SocketData socketData = ParseSocketBinaryData(receiveData);
        _Response response = _Response.Parser.ParseFrom(socketData.data);
        RPC currentRpc = m_RpcQueue[0];
        lock (m_RpcQueue)
        {
            m_RpcQueue.RemoveAt(0);
        }
        Assembly assem = currentRpc.rpcType.Assembly;
        Type type = assem.GetType(currentRpc.rpcType.Namespace + "." + currentRpc.msg + "Response");
        if (type != null)
        {
            IMessage resRpc = Activator.CreateInstance(type) as IMessage;
            resRpc.MergeFrom(response.Data);
            AppInterface.ThreadManager.RunOnMainThread(() =>
            {
                currentRpc.callback(resRpc);
            });
        }
    }

    private void Connected(IAsyncResult ar)
    {
        if (m_socket.Connected)
        {
            m_isReady = true;
            AppInterface.ThreadManager.RunOnMainThread(() =>
            {
                Debuger.Log("connected");
            });
            //开始监听
            m_isQueue = true;
            m_recieve = AppInterface.ThreadManager.RunAsync(QueueRecieve);
            m_process = AppInterface.ThreadManager.RunAsync(QueueProcess);
        }
    }
    private void Disconnect()
    {
        AppInterface.ThreadManager.RunOnMainThread(() => {
            Debuger.Log("disconnected");
        });
        while (!m_socket.Connected)
        {
            AppInterface.ThreadManager.RunOnMainThread(() => {
                Debuger.Log("reconnecting");
            });
            Thread.Sleep(1000);
            Init(true);
        }
        ResendAll();
        AppInterface.ThreadManager.RunOnMainThread(() => {
            Debuger.Log("reconnected");
        });
    }
    private static void Sent(IAsyncResult ar)
    {
        //
    }

}
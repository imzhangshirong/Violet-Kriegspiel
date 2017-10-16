using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Google.Protobuf;

public class NetworkManager : Manager
{
    public const string Name = "NetworkManager";
    RpcNetwork m_rpc;
    
    public override void OnManagerReady()
    {

    }
    public override void OnManagerDestroy()
    {

    }
}
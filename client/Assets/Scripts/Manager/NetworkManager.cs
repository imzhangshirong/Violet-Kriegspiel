using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Google.Protobuf;

public class NetworkManager : ServiceModule<NetworkManager>
{
    RpcNetwork m_rpc;
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Config
{
    //网络层配置
    public static string ServerHost = "127.0.0.1";
    public static int ServerHostPort = 8000;
    public static readonly int RpcTimeout = 10000;//rpc请求的超时时间ms
    public static readonly string RpcNamespace = "Com.Violet.Rpc";

    //资源配置
    public static readonly string ResourceFullPath = "/Resources";
	public static readonly string StreamingResourceFullPath = "/StreamingAssets";
    public static readonly string TextureResourcePath = "Texture";
    public static readonly string UIResourcePath = "UI";
    public static readonly string GameResourcePath = "Game";

    public static readonly string UIRootPath = "/UI Root/Camera/";
    public static readonly int PageBaseDepth = 100;//最开始的Depth
	public static readonly int ViewLevelDepth = 100;//每一层page之间的最大depth
    

    public static readonly int OverViewLevelDepth = 50;//每一层overview之间的最大depth
    public static readonly int OverViewLevelBaseDepth = 100;

    public class Game{
        public static int WaitingFindEnemy = 45;//匹配等待秒数
        public static int WaitingReady = 60;//准备等待秒数
        public static int WaitingRound = 30;//回合等待秒数
    }

}
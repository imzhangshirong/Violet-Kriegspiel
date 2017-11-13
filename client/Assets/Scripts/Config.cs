using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class Config
{
    //网络层配置
    public static readonly string ServerHost = "127.0.0.1";
    public static readonly int ServerHostPort = 8000;
    public static readonly int RpcTimeout = 10000;//rpc请求的超时时间ms

    //资源配置
    public static readonly string ResourceFullPath = "/Resources";
	public static readonly string StreamingResourceFullPath = "/StreamingAssets";
    public static readonly string TextureResourcePath = "Texture";
    public static readonly string UIResourcePath = "UI";

    public static readonly string UIRootPath = "/UI Root/Camera/";
    public static readonly int PageBaseDepth = 100;//最开始的Depth
	public static readonly int ViewLevelDepth = 100;//每一层page之间的最大depth
    

    public static readonly int OverViewLevelDepth = 20;//每一层overview之间的最大depth
    public static readonly int OverViewLevelBaseDepth = 100;


}
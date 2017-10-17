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

    //资源配置
    public static readonly string ResourceFullPath = Application.dataPath + "/Resources";
	public static readonly string StreamingResourceFullPath = Application.dataPath + "/StreamingAssets";
    public static readonly string TextureResourcePath = "Texture";
    public static readonly string UIResourcePath = "UI";

    public static readonly string UIRootPath = "/UI Root/Camera/";
    public static readonly int PageBaseDepth = 20;
	public static readonly int OverViewBaseDepth = 1000;
	public static readonly int ViewLevelDepth = 100;
	
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class Config
{

	static public string ResourceFullPath
	{
		get
		{
			return Application.dataPath + "/Resource";
		}
	}
	static public string StreamingResourceFullPath
	{
		get
		{
			return Application.dataPath + "/StreamingAssets";
		}
	}
	static public readonly string TextureResourcePath = "Texture";
	static public readonly string UIResourcePath = "UI";
	static public string TextureFullPath
	{
		get
		{
			return Config.ResourceFullPath + "/"+Config.TextureResourcePath;
		}
	}
	static public string UIPath
	{
		get
		{
			return Config.ResourceFullPath + "/" + Config.UIResourcePath;
		}
	}
	static public readonly int PageBaseDepth = 20;
	static public readonly int OverViewBaseDepth = 1000;
	static public readonly int ViewLevelDepth = 100;
	/// <summary>
	/// 注册所有的UI界面
	/// </summary>
	static public void RegisteUI()
	{
		Global.MUI.RegisteUI("UIPlayerInfo", "UIPlayerInfo", UILayoutStyle.Top, UIWindowStyle.OverView);
		Global.MUI.RegisteUI("UIPlayerInfo2", "UIPlayerInfo2", UILayoutStyle.Top, UIWindowStyle.OverView);
		Global.MUI.RegisteUI("leftTest", "leftTest", UILayoutStyle.Left, UIWindowStyle.OverView);
		Global.MUI.RegisteUI("UIMainPanel", "UIMainPanel", UILayoutStyle.Center, UIWindowStyle.Page);
		Global.MUI.RegisteUI("UIPagePanel", "UIPagePanel", UILayoutStyle.Center, UIWindowStyle.Page);
	}
    static public readonly string ServerHost = "127.0.0.1";
    static public readonly int ServerHostPort = 8000;
}
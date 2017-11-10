using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Global : MonoBehaviour{
	//某些赋值需要在UnityEngine主线程里使用
	//初始化基础模块
	void Awake()
	{
        Debuger.Init();

        //加载Manager
        DontDestroyOnLoad(gameObject);
        App.Instance.AddManager<ThreadManager>(ThreadManager.Name);
        App.Instance.AddManager<LocalDataManager>(LocalDataManager.Name);
        App.Instance.AddManager<NetworkManager>(NetworkManager.Name);
        App.Instance.AddManager<ResourceManager>(ResourceManager.Name);
        App.Instance.AddManager<SceneManager>(SceneManager.Name);
        App.Instance.AddManager<ObjectPoolManager>(ObjectPoolManager.Name);
        App.Instance.AddManager<EventManager>(EventManager.Name);
        App.Instance.AddManager<UIManager>(UIManager.Name);
        App.Instance.AddManager<GameManager>(GameManager.Name);

        //注册对象池
        AppInterface.ObjectPoolManager.RegisteObject("UIAlertWindow", Config.UIResourcePath + "/UIAlertWindow", 0, 128, 8f);

        //注册UI
        AppInterface.UIManager.RegisteUI("UITopTest", "Widget/UITopTest", UILayoutStyle.Top, UIViewStyle.OverView);
        AppInterface.UIManager.RegisteUI("UITips", "UITips", UILayoutStyle.Center, UIViewStyle.Tips);
        AppInterface.UIManager.RegisteUI("UIAlertWindow", "UIAlertWindow", UILayoutStyle.Center, UIViewStyle.Window);

        AppInterface.UIManager.RegisteUI("UIMainPanel", "UIMainPanel", UILayoutStyle.Center, UIViewStyle.Page);
        AppInterface.UIManager.RegisteUI("UIGamePanel", "Game/UIGamePanel", UILayoutStyle.Center, UIViewStyle.Page);
        AppInterface.UIManager.RegisteUI("UIPagePanel", "UIPagePanel", UILayoutStyle.Center, UIViewStyle.Page);

        //注册RPC Push事件
        AppInterface.NetworkManager.RegisterPush("GameStart", "#NET_GameStart");
        AppInterface.NetworkManager.RegisterPush("GameEnd", "#NET_GameEnd");
        AppInterface.NetworkManager.RegisterPush("GameEnemyReady", "#NET_GameEnemyReady");
        AppInterface.NetworkManager.RegisterPush("GameChessMove", "#NET_GameChessMove");

        Debuger.Log("Inited");
    }
    void OnApplicationQuit()
    {
        Debuger.Log("Destroied");
    }
}

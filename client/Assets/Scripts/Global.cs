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
        App.Instance.AddManager<MockManager>(MockManager.Name);
        App.Instance.AddManager<ResourceManager>(ResourceManager.Name);
        App.Instance.AddManager<SceneManager>(SceneManager.Name);
        App.Instance.AddManager<ObjectPoolManager>(ObjectPoolManager.Name);
        App.Instance.AddManager<EventManager>(EventManager.Name);
        App.Instance.AddManager<UIManager>(UIManager.Name);
        App.Instance.AddManager<GameManager>(GameManager.Name);

        //注册对象池
        App.ObjectPoolManager.RegisteObject("UIAlertWindow", Config.UIResourcePath + "/UIAlertWindow", 0, 128, 8f);

        //注册UI
        App.UIManager.RegisteUI("UITopTest", "Widget/UITopTest", UILayoutStyle.Top, UIViewStyle.OverView);
        App.UIManager.RegisteUI("UITips", "UITips", UILayoutStyle.Center, UIViewStyle.Tips);
        App.UIManager.RegisteUI("UIAlertWindow", "UIAlertWindow", UILayoutStyle.Center, UIViewStyle.Window);

        App.UIManager.RegisteUI("UIMainPanel", "UIMainPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.UIManager.RegisteUI("UIGamePanel", "Game/UIGamePanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.UIManager.RegisteUI("UIPagePanel", "UIPagePanel", UILayoutStyle.Center, UIViewStyle.Page);

        //注册RPC Push事件
        App.NetworkManager.RegisterPush("GameStart", "#NET_GameStart");
        App.NetworkManager.RegisterPush("GameEnd", "#NET_GameEnd");
        App.NetworkManager.RegisterPush("GameEnemyReady", "#NET_GameEnemyReady");
        App.NetworkManager.RegisterPush("GameChessMove", "#NET_GameChessMove");

        Debuger.Log("Inited");
    }
    void OnApplicationQuit()
    {
        Debuger.Log("Destroied");
    }
}

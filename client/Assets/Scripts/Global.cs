using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Global : MonoBehaviour
{
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
#if DEBUG
        App.Instance.AddManager<MockManager>(MockManager.Name);
#endif
        App.Instance.AddManager<ResourceManager>(ResourceManager.Name);
        App.Instance.AddManager<SceneManager>(SceneManager.Name);
        App.Instance.AddManager<ObjectPoolManager>(ObjectPoolManager.Name);
        App.Instance.AddManager<EventManager>(EventManager.Name);
        App.Instance.AddManager<UIManager>(UIManager.Name);
        App.Instance.AddManager<GameManager>(GameManager.Name);

        //注册对象池
        App.Manager.ObjectPool.RegisteObject("UIAlertWindow", Config.UIResourcePath + "/UIAlertWindow", 0, 128, 8f);

        //注册UI
        App.Manager.UI.RegisteUI("UITopTest", "Widget/UITopTest", UILayoutStyle.Top, UIViewStyle.OverView);
        App.Manager.UI.RegisteUI("UITips", "UITips", UILayoutStyle.Center, UIViewStyle.Tips);
        App.Manager.UI.RegisteUI("UIAlertWindow", "UIAlertWindow", UILayoutStyle.Center, UIViewStyle.Window);
        App.Manager.UI.RegisteUI("UIMainPanel", "UIMainPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UILoginPanel", "UILoginPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UILobbyPanel", "UILobbyPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UIGamePanel", "Game/UIGamePanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UIPagePanel", "UIPagePanel", UILayoutStyle.Center, UIViewStyle.Page);

        //注册RPC Push事件
        App.Manager.Network.RegisterPush("RoomStateChange", "#NET_RoomStateChange");
        App.Manager.Network.RegisterPush("EnterBattleField", "#NET_EnterBattleField");
        App.Manager.Network.RegisterPush("PlayerStateChage", "#NET_PlayerStateChage");
        App.Manager.Network.RegisterPush("GameStateChage", "#NET_GameStateChage");
        App.Manager.Network.RegisterPush("ChessMove", "#NET_ChessMove");
        App.Manager.Network.RegisterPush("ChatMessagePush", "#NET_ChatMessagePush");


        Debuger.Log("Inited");
    }
    void OnApplicationQuit()
    {
        Debuger.Log("Destroied");
    }
}

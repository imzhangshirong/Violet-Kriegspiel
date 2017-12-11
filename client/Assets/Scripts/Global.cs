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
        DontDestroyOnLoad(gameObject);
        //加载Manager////

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

        //注册对象池////
        App.Manager.ObjectPool.RegisteObject("Arrow", Config.GameResourcePath + "/Arrow", 0, 64, 8f);

        //注册UI////
        //特殊UI注册
        App.Manager.UI.RegisteUI("UITips", "UITips", UILayoutStyle.Center, UIViewStyle.Tips);
        App.Manager.UI.RegisteUI("UIWaitingPanel", "UIWaitingPanel", UILayoutStyle.Center, UIViewStyle.OverView);
        App.Manager.UI.RegisteUI("UIAlertWindow", "UIAlertWindow", UILayoutStyle.Center, UIViewStyle.Window);
        //挂件注册
        App.Manager.UI.RegisteUI("UIPlayerInfo", "Widget/UIPlayerInfo", UILayoutStyle.Top, UIViewStyle.OverView);
        //页面注册
        App.Manager.UI.RegisteUI("UIMainPanel", "UIMainPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UILoginPanel", "UILoginPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UILobbyPanel", "UILobbyPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UIFindEnemyPanel", "UIFindEnemyPanel", UILayoutStyle.Center, UIViewStyle.Page);
        App.Manager.UI.RegisteUI("UIGamePanel", "Game/UIGamePanel", UILayoutStyle.Center, UIViewStyle.Page);

        //注册RPC Push事件////
        App.Manager.Network.RegisterPush("RoomStateChange", "#NET_RoomStateChange");
        App.Manager.Network.RegisterPush("EnterBattleField", "#NET_EnterBattleField");
        App.Manager.Network.RegisterPush("PlayerStateChange", "#NET_PlayerStateChange");
        App.Manager.Network.RegisterPush("GameStateChange", "#NET_GameStateChange");
        App.Manager.Network.RegisterPush("ChessMove", "#NET_ChessMove");
        App.Manager.Network.RegisterPush("ChatMessage", "#NET_ChatMessage");

        //注册RPC ErrorCode处理////
        App.Manager.Network.RegisteErrorCode(1, () => {
            Common.UI.OpenAlert("错误", "用户不存在", "确认", null);
        });
        App.Manager.Network.RegisteErrorCode(11, () => {
            Common.UI.OpenAlert("错误", "已经在匹配名单里", "确认", null);
        });
        App.Manager.Network.RegisteErrorCode(21, () => {
            Common.UI.OpenAlert("错误", "用户不在房间内", "确认", null);
        });
        App.Manager.Network.RegisteErrorCode(31, () => {
            Common.UI.OpenAlert("错误", "数据错误", "确认", null);
        });
        Debuger.Log("Inited");
    }
    void OnApplicationQuit()
    {
        Debuger.Log("Destroied");
    }
}

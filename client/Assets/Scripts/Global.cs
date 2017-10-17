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
        
        
		Config.RegisteUI();
        Debuger.Log("Inited");
    }
    void OnApplicationQuit()
    {
        RpcNetwork.Instance.Destroy();
        Debuger.Log("Destroied");
    }
}

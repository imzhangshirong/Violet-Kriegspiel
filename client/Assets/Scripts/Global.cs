using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Global : MonoBehaviour{
	public Debuger MDebuger;
	private static Global m_Global;
	public static Global Instance {
		get {
			return m_Global;
		}
	}
	public static ModuleManager MMoudule;
	public static GameManager MGame;
	public static SceneManager MScene;
	public static UIManager MUI;
	public static ResourceManager MResource;

	//某些赋值需要在UnityEngine主线程里使用
	//初始化基础模块
	void Awake()
	{
		m_Global = this;
        ThreadSwitcher.Init();
        Debuger.Init();
        RpcNetwork.Instance.Init();

        MMoudule = ModuleManager.Instance.Init();
		MGame = GameManager.Instance;
		MScene = SceneManager.Instance;
		MUI = UIManager.Instance.Init();
		MResource = ResourceManager.Instance;
        
		Config.RegisteUI();
        Debuger.Log("Inited");
    }
    void OnApplicationQuit()
    {
        RpcNetwork.Instance.Destroy();
        Debuger.Log("Destroied");
    }
}

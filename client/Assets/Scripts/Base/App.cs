using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class App : AppBase
{
    public static GameManager GameManager
    {
        get
        {
            return App.Instance.GetManager<GameManager>(GameManager.Name);
        }
    }
    public static EventManager EventManager
    {
        get
        {
            return App.Instance.GetManager<EventManager>(EventManager.Name);
        }
    }
    public static LocalDataManager LocalDataManager
    {
        get
        {
            return App.Instance.GetManager<LocalDataManager>(LocalDataManager.Name);
        }
    }
    public static NetworkManager NetworkManager
    {
        get
        {
            return App.Instance.GetManager<NetworkManager>(NetworkManager.Name);
        }
    }
    public static ObjectPoolManager ObjectPoolManager
    {
        get
        {
            return App.Instance.GetManager<ObjectPoolManager>(ObjectPoolManager.Name);
        }
    }
    public static ResourceManager ResourceManager
    {
        get
        {
            return App.Instance.GetManager<ResourceManager>(ResourceManager.Name);
        }
    }
    public static SceneManager SceneManager
    {
        get
        {
            return App.Instance.GetManager<SceneManager>(SceneManager.Name);
        }
    }
    public static ThreadManager ThreadManager
    {
        get
        {
            return App.Instance.GetManager<ThreadManager>(ThreadManager.Name);
        }
    }
    public static UIManager UIManager
    {
        get
        {
            return App.Instance.GetManager<UIManager>(UIManager.Name);
        }
    }
}

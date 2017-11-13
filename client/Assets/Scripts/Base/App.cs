using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class App : AppBase
{
    
    public class Manager{
        public static GameManager Game
        {
            get
            {
                return App.Instance.GetManager<GameManager>(GameManager.Name);
            }
        }
        public static EventManager Event
        {
            get
            {
                return App.Instance.GetManager<EventManager>(EventManager.Name);
            }
        }
        public static LocalDataManager LocalData
        {
            get
            {
                return App.Instance.GetManager<LocalDataManager>(LocalDataManager.Name);
            }
        }
        public static NetworkManager Network
        {
            get
            {
                return App.Instance.GetManager<NetworkManager>(NetworkManager.Name);
            }
        }
        public static ObjectPoolManager ObjectPool
        {
            get
            {
                return App.Instance.GetManager<ObjectPoolManager>(ObjectPoolManager.Name);
            }
        }
        public static ResourceManager Resource
        {
            get
            {
                return App.Instance.GetManager<ResourceManager>(ResourceManager.Name);
            }
        }
        public static SceneManager Scene
        {
            get
            {
                return App.Instance.GetManager<SceneManager>(SceneManager.Name);
            }
        }
        public static ThreadManager Thread
        {
            get
            {
                return App.Instance.GetManager<ThreadManager>(ThreadManager.Name);
            }
        }
        public static UIManager UI
        {
            get
            {
                return App.Instance.GetManager<UIManager>(UIManager.Name);
            }
        }
        public static MockManager Mock
        {
            get
            {
                return App.Instance.GetManager<MockManager>(MockManager.Name);
            }
        }
    }

    public class Package{
        public static PlayerPackage Player
        {
            get
            {
                return App.Package.Player;
            }
        }
        public static ChessGamePackage ChessGame
        {
            get
            {
                return ChessGamePackage.Instance;
            }
        }
    }
}

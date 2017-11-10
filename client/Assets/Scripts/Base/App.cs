using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class App : Singleton<App>
{
    static GameObject obj_Manager;
    Dictionary<string, object> m_Managers = new Dictionary<string, object>();
    static GameObject ManagerInstance
    {
        get
        {
            if (obj_Manager == null)
            {
                obj_Manager = GameObject.Find("Global");
            }
            return obj_Manager;
        }
    }

    public App()
    {
        Init();
    }
    public void Init()
    {
        
    }
    public void AddManager<T>(string managerName) where T : Component
    {
        Component c = ManagerInstance.AddComponent<T>();
        if (!m_Managers.ContainsKey(managerName))
        {
            m_Managers.Add(managerName, c);
        }
        else
        {
            m_Managers[managerName] = c;
        }
        //IManager manager = (IManager)c;
        //manager.OnManagerReady();//通知已经准备就绪
    }

    public void RemoveManager(string managerName) {
        if (m_Managers.ContainsKey(managerName))
        {
            object manager = null;
            m_Managers.TryGetValue(managerName, out manager);
            Type type = manager.GetType();
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                IManager imanager = (IManager)manager;
                imanager.OnManagerDestroy();//通知开始销毁
                GameObject.Destroy((Component)manager);
            }
            m_Managers.Remove(managerName);
        }
    }

    public T GetManager<T>(string managerName)
    {
        object manager = null;
        if (m_Managers.ContainsKey(managerName))
        {
            m_Managers.TryGetValue(managerName, out manager);
            return (T)manager;
        }
        throw new Exception(managerName+" not Added");
    }
}

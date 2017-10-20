using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//对象池
public class ObjectPoolManager : Manager
{
    public const string Name = "ObjectPoolManager";
    private Dictionary<string, GameObject> m_ObjectMap = new Dictionary<string, GameObject>();
    private Dictionary<string, List<ObjectPoolState>> m_ObjectInstanceMap = new Dictionary<string, List<ObjectPoolState>>();
    GameObject m_ObjectPool;
    private void Awake()
    {
        //创建Pool
        m_ObjectPool = new GameObject();
        m_ObjectPool.name = "ObjectPool";
        m_ObjectPool.transform.parent = gameObject.transform;
        m_ObjectPool.transform.localScale = Vector3.one;
        m_ObjectPool.transform.localPosition = Vector3.zero;

    }
    public void RegisteObject(string name,int min,int max,float cleanDuration,string resourcePath)
    {
        GameObject go = AppInterface.ResourceManager.Load<GameObject>(resourcePath);

    }
    public override void OnManagerReady()
    {

    }
    public override void OnManagerDestroy()
    {

    }
}

public class PoolObjectState
{
    public string name;
    public int min;
    public int max;
    GameObject gameObject;

}

public class PoolInstanceState
{
    public string name;
    public int stateId;
    public GameObject gameObject;
}

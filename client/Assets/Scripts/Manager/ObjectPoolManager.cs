using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//对象池
public class ObjectPoolManager : Manager
{
    public const string Name = "ObjectPoolManager";
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
    public override void OnManagerReady()
    {

    }
    public override void OnManagerDestroy()
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SceneManager : Manager//场景切换和缓存
{
    public const string Name = "SceneManager";
    GameObject m_ScenePool;


    public override void OnManagerReady()
    {
        //创建Pool
        if (m_ScenePool == null)
        {
            m_ScenePool = new GameObject();
            m_ScenePool.name = "ScenePool";
            m_ScenePool.transform.parent = gameObject.transform;
            m_ScenePool.transform.localScale = Vector3.one;
            m_ScenePool.transform.localPosition = Vector3.zero;
        }
    }
    public override void OnManagerDestroy()
    {

    }
}

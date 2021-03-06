﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TreeRoot : MonoBehaviour
{
	private Dictionary<string, List<TreeLeaf>> m_LeafMap = new Dictionary<string, List<TreeLeaf>>();
	private List<TreeLeaf> m_LeafList = new List<TreeLeaf>();
    public string[] m_EventKeys;
	public void Bind(TreeLeaf leaf)
	{
        if (m_LeafList.IndexOf(leaf) == -1)
        {

            m_LeafList.Add(leaf);
        }
	}
	public void Broadcast(string msg, object content) {
		Broadcast(null, msg, content);
	}


    public void Remove(TreeLeaf leaf)
    {
        m_LeafList.Remove(leaf);
    }

	public void Broadcast(TreeLeaf fromLeaf,string msg,object content)
    {
        for(int i=0;i< m_LeafList.Count; i++)
        {
            TreeLeaf leaf = m_LeafList[i];
            if (leaf.IsActive())
            {
                leaf.OnMessage(msg, content);
            }
        }

    }
	private List<TreeLeaf> GetLeafList(string type)
	{
        if (m_LeafMap.ContainsKey(type))
		{
			return m_LeafMap[type];
		}
		else
		{
			List<TreeLeaf> list = new List<TreeLeaf>();
			m_LeafMap.Add(type, list);
			return list;
		}
	}

    private void Awake()
    {
        //注册全局EventKey到EventManager
        for(int i = 0; i < m_EventKeys.Length; i++)
        {
            App.Manager.Event.RegisteTreeRoot(m_EventKeys[i], this);
        }
        
    }
    private void OnDestroy()
    {
        App.Manager.Event.RemoveTreeRoot(this);
    }
}



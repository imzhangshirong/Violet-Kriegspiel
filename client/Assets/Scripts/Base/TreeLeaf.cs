using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public delegate void MessageListener(object content);
public class TreeLeaf : MonoBehaviour
{
	//private Dictionary<string,>
	public TreeRoot treeRoot;
    private Dictionary<string, MessageListener> m_ListenerMap = new Dictionary<string, MessageListener>();
	void Start()
	{
		if (treeRoot == null)
		{
			Debuger.Warn(this.gameObject.name + ":leaf hasn't TreeRoot.");
		}
        treeRoot.Bind(this);
	}
	public void Push(string msg, object content)
	{
		if(treeRoot == null){
			Debuger.Warn(this.gameObject.name + ":leaf hasn't TreeRoot.");
		}
        treeRoot.Broadcast(this, msg, content);
	}
	public virtual void OnMessage(string msg, object content)
	{
        if (m_ListenerMap.ContainsKey(msg))
        {
            m_ListenerMap[msg](content);
        }
    }
    public void BindEvent(string msg, MessageListener listener) //绑定事件监听
    {
        if (!m_ListenerMap.ContainsKey(msg))
        {
            m_ListenerMap.Add(msg, listener);
        }
        else
        {
            m_ListenerMap[msg] = listener;
        }
    }
	public bool IsActive()
	{
		return this.gameObject.activeSelf;
	}
}
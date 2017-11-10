using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : Manager
{
    public const string Name = "EventManager";
    private static Dictionary<string, List<TreeRoot>> m_TreeMap = new Dictionary<string, List<TreeRoot>>();
    
    public override void OnManagerReady()
    {
        
    }
    public override void OnManagerDestroy()
    {
        m_TreeMap.Clear();
    }
    public void Broadcast(string msg,object content)
    {
        if (m_TreeMap.ContainsKey(msg))
        {
            for(int i = 0;i< m_TreeMap[msg].Count; i++) {
                m_TreeMap[msg][i].Broadcast(msg,content);
            }
        }
    }
    public void RegisteTreeRoot(string msg,TreeRoot treeRoot)
    {
        if (!m_TreeMap.ContainsKey(msg))
        {
            m_TreeMap.Add(msg, new List<TreeRoot>());
        }
        if (m_TreeMap[msg].IndexOf(treeRoot) == -1)
        {
            m_TreeMap[msg].Add(treeRoot);
        }
    }
}
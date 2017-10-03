using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TreeRoot : MonoBehaviour
{
	private Dictionary<string, List<TreeLeafState>> m_leafMap = new Dictionary<string, List<TreeLeafState>>();
	private List<TreeLeafState> m_leafList = new List<TreeLeafState>();
	public void Bind(TreeLeaf leaf,bool aActive = false)
	{
		m_leafList.Add(new TreeLeafState(leaf, aActive));
	}
	public void Broadcast(string msg, params object[] args) {
		Broadcast(null, msg, args);
	}
	public void Broadcast(TreeLeaf fromLeaf,string msg,params object[] args){
		foreach (var leafState in m_leafList)
		{
			if (fromLeaf == null || (fromLeaf != null && fromLeaf != leafState.leaf))
			{
				if (leafState.alwaysActive)
				{
					leafState.leaf.TreeData(msg, args);
				}
				else if (leafState.leaf.IsActive())
				{
					leafState.leaf.TreeData(msg, args);
				}
			}
		}
	}
	private List<TreeLeafState> GetLeafList(string type)
	{
		if (m_leafMap.ContainsKey(type))
		{
			return m_leafMap[type];
		}
		else
		{
			List<TreeLeafState> list = new List<TreeLeafState>();
			m_leafMap.Add(type, list);
			return list;
		}
	}
}


class TreeLeafState {
	public TreeLeaf leaf;
	public bool alwaysActive;
	public TreeLeafState(TreeLeaf leaf,bool aActive)
	{
		this.leaf = leaf;
		this.alwaysActive = aActive;
	}
}
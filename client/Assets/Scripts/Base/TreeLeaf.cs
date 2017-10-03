using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TreeLeaf : MonoBehaviour
{
	//private Dictionary<string,>
	public TreeRoot treeRoot;
	public string messageKey;
	void Awake()
	{
		if (treeRoot == null)
		{
			Debuger.Warn(this.gameObject.name + ":leaf hasn't TreeRoot.");
		}
		treeRoot.Bind(this);
	}
	public void Push(string msg, params object[] args)
	{
		if(treeRoot == null){
			Debuger.Warn(this.gameObject.name + ":leaf hasn't TreeRoot.");
		}
		treeRoot.Broadcast(this, msg, args);
	}
	public virtual void TreeData(string msg, params object[] args)
	{

	}
	public bool IsActive()
	{
		return this.gameObject.active;
	}
}
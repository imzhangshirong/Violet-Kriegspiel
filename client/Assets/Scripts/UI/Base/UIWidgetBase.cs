using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//
public class UIWidgetBase : TreeLeaf
{

	public virtual void Awake()
	{
		base.Awake();
	}
	public virtual void OnRefresh()
	{

	}
	public virtual void OnInit()
	{

	}
	public virtual void OnOpen(Intent intent)
	{

	}
	public virtual void OnClose()
	{
	}
}
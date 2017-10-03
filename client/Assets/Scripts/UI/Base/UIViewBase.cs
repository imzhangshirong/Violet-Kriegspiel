using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//
public class UIViewBase : TreeLeaf
{

	private UIPanel m_panel = null;
	public UIPanel Panel
	{
		get {
			return m_panel;
		}
	}
	void Awake()
	{
		m_panel = GetComponent<UIPanel>();
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
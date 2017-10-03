using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

public class BussinessModule : Module
{
	private EventMap m_eventMap = new EventMap();
	/// <summary>
	/// 业务模块显示标题
	/// </summary>
	public string title;

	public BussinessModule()
	{

	}
	public BussinessModule(string name)
	{
		this.m_name = name;
	}
	public MEvent Event(string key)
	{
		return m_eventMap.Event(key);
	}
	public override void Release()
	{
		m_eventMap.Clear();
		m_eventMap = null;
		base.Release();
	}
	internal void SetEventMap(EventMap eventMap)
	{
		m_eventMap = eventMap;
	}
	internal EventMap GetEventMap()
	{
		return m_eventMap;
	}
	public virtual void Create(object arg = null)
	{
		Debuger.Log("module \"" + name + "\" create, arg = {0} .",arg);
	}
	public virtual void Start()
	{
		Debuger.Log("module \"" + name + "\" start .");
	}
	/// <summary>
	/// 用于处理消息，执行对应的函数方法，内部
	/// </summary>
	/// <param name="mObj"></param>
	internal void HandleMessage(MessageObject mObj)
	{
		string msg = mObj.Message;
		object[] args = mObj.Params;
		Debuger.Log("module \"" + name + "\" HandleMessage \"" + msg + "\" args = {0}", args);
		MethodInfo methodInfo = this.GetType().GetMethod(msg, BindingFlags.NonPublic | BindingFlags.Instance);
		methodInfo.Invoke(this, args);
	}
}

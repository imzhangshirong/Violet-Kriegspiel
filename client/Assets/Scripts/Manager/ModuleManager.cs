using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ModuleManager : ServiceModule<ModuleManager>
{
	private Dictionary<string, List<MessageObject>> m_preMessageListMap = new Dictionary<string, List<MessageObject>>();
	private Dictionary<string, EventMap> m_preEventMap = new Dictionary<string, EventMap>();
	private Dictionary<string, BussinessModule> m_moduleMap = new Dictionary<string, BussinessModule>();

	private string m_domain;
	public ModuleManager Init(string domain = "Scripts.Module")
	{
		CheckSingleton();
		m_domain = domain;
		return Instance;
	}
	
	public void SendMessage(string name,MessageObject mObj)
	{
		if (m_moduleMap.ContainsKey(name))
		{
			m_moduleMap[name].HandleMessage(mObj);
		}
		else
		{
			if (m_preMessageListMap.ContainsKey(name))
			{
				m_preMessageListMap[name].Add(mObj);
			}
			else
			{
				List<MessageObject> list = new List<MessageObject>();
				list.Add(mObj);
				m_preMessageListMap.Add(name, list);
			}
		}
	}
	public void SendMessage(string name, string message, params object[] args)
	{
		SendMessage(name, new MessageObject(message, args));
	}
	//加载module
	public T Load<T>(object[] args) where T : BussinessModule
	{
		return (T)Load(typeof(T).Name, args);
	}
	public BussinessModule Load(string name, object[] args)
	{
		if (m_moduleMap.ContainsKey(name))
		{
			return m_moduleMap[name];
		}
		BussinessModule module;
		Type type = Type.GetType(m_domain + "." + name);
		if(type!=null)
		{
			module = Activator.CreateInstance(type) as BussinessModule;
		}
		else //Lua模块
		{
			module = new LuaModule(name);
		}
		m_moduleMap.Add(name, module);
		//处理加载模块之前需要处理的消息
		if (m_preMessageListMap.ContainsKey(name))
		{
			List<MessageObject> list = m_preMessageListMap[name];
			m_preMessageListMap.Remove(name);
			//处理未处理的消息
			foreach (var msg in list)
			{
				SendMessage(name,msg);
			}
		}
		//处理加载前的Event
		if (m_preEventMap.ContainsKey(name))
		{
			module.SetEventMap(m_preEventMap[name]);
			m_preEventMap.Remove(name);
		}
		module.Create(args);//创建模块
		return module;
	}
	//获取module
	public T GetModule<T>() where T : BussinessModule
	{
		return (T)GetModule(typeof(T).Name);
	}
	public BussinessModule GetModule(string name)
	{
		return m_moduleMap[name];
	}
	//释放module
	public void Release(string name)
	{
		if (m_moduleMap.ContainsKey(name))
		{
			m_moduleMap[name].Release();
			m_moduleMap.Remove(name);
		}
		m_preEventMap.Remove(name);
		m_preMessageListMap.Remove(name);
	}
	//释放所有模块，同时也释放未处理的message
	public void ReleaseAll()
	{
		foreach ( var item in m_moduleMap)
		{
			item.Value.Release();
		}
		m_moduleMap.Clear();
		m_preEventMap.Clear();
		m_preMessageListMap.Clear();
	}
	//获取对应的事件
	public MEvent Event(string module, string type)
	{
		if (m_moduleMap.ContainsKey(module))
		{
			return m_moduleMap[module].Event(type);
		}
		else
		{
			if (!m_preEventMap.ContainsKey(module))
			{
				EventMap em = new EventMap();
				m_preEventMap.Add(module, em);
			}
			return m_preEventMap[module].Event(type);
		}
	}
}

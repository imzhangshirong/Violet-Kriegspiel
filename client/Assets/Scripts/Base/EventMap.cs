using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

public class EventMap
{
	private Dictionary<string, MEvent> m_eventMap = new Dictionary<string, MEvent>();
	/// <summary>
	/// 获取key对应的Event事件
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public MEvent Event(string key)
	{
		if (!m_eventMap.ContainsKey(key))
		{
			m_eventMap.Add(key, new MEvent());
			return m_eventMap[key];
		}
		return m_eventMap[key];
	}
	/// <summary>
	/// 清除释放EventMap
	/// </summary>
	public void Clear()
	{
		if (m_eventMap != null)
		{
			foreach(var mevent in m_eventMap){
				mevent.Value.RemoveAllListeners();
			}
			m_eventMap.Clear();
		}
		m_eventMap = null;
	}
}



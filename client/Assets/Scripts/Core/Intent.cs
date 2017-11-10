using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Intent
{
	private Dictionary<string, object> m_map;
	public Intent()
	{
		m_map = new Dictionary<string, object>();
		
	}
	public void Push(string key, object value)
	{
		m_map.Add(key, value);
	}
	public T Value<T>(string key)
	{
		return (T)Value(key);
	}
	public object Value(string key)
	{
        if (m_map.ContainsKey(key)) return m_map[key];
        return null;

    }
}
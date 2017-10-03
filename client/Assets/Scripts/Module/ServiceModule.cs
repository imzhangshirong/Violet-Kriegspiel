using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ServiceModule<T> : Module where T : ServiceModule<T>, new()
{
	private static T m_instance;
	public static T Instance
	{ 
		get
		{
			if (m_instance == null)
			{
				m_instance = new T();
			}
			return m_instance;
		}
	}
	protected void CheckSingleton()
	{
		if (m_instance == null)
		{
			throw new Exception("ServiceModule \""+name+"\" not singleton");
		}
	}
}
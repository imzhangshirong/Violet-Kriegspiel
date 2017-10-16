using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Module
{
	protected string m_name;
	/// <summary>
	/// 模块名字
	/// </summary>
	public string name
	{
		get
		{
			if (string.IsNullOrEmpty(m_name))
			{
				return this.GetType().Name;
			}
			else
			{
				return m_name;
			}
		}
	}
	public virtual void Release()
	{
		Debuger.Log("module \"" + name + "\" release .");
	}

}
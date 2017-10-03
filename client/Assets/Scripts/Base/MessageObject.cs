using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MessageObject
{
	private object[] m_args;
	private string m_message;
	public MessageObject(string msg,params object[] args)
	{
		m_message = msg;
		m_args = args;
	}
	public object[] Params
	{
		get
		{
			return m_args;
		}
	}
	public string Message
	{
		get
		{
			return m_message;
		}
	}
}
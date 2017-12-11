using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.IO;

public class Debugger{
	private FileInfo logFile;
    private static Debugger m_Instance;

    public Debugger() {
#if LOGFILE
		//注意目录
		string dir = null;
#if UNITY_ANDROID
			dir = "jar:file://Assets/Logs/";    
#elif UNITY_IPHONE
			dir = "Assets/Logs/";    
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
			dir = "Assets/Logs/";
#endif
		string logName = dir + "debug.log";
		logFile = new FileInfo(logName);
		if (logFile.Exists)
		{
			logFile.Delete();
			logFile.Create();
		}
		else
		{
			logFile.Create();
		}
		Application.RegisterLogCallback(OnLog); 
#endif
    }
    public static void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = new Debugger();
        }
    }
	private void OnLog(string condition, string stackTrace, LogType type)
	{
#if LOGFILE
		string tag = string.Empty;
        string color = "";
		switch (type)
		{
			case LogType.Assert:
				tag = "A";
				break;
			case LogType.Error:
				tag = "E";
                color = "[FF0000]";
                break;
			case LogType.Log:
				tag = "I";
				break;
			case LogType.Warning:
                color = "[FFCC00]";
                tag = "W";
				break;
			case LogType.Exception:
				tag = "EX";
				break;
		}
		LogToFile(tag, condition, stackTrace);
#endif
    }
    public static void Log(string format, params object[] msgs)
	{
		string msg = System.DateTime.Now.ToString("MM-dd HH:mm:ss") + "(" + Time.time + "):" + string.Format(format, msgs);
#if DEBUG
		Debug.Log(msg);
#endif
	}
	public static void Warning(string format, params object[] msgs)
	{
		string msg = System.DateTime.Now.ToString("MM-dd HH:mm:ss") + "(" + Time.time + "):" + string.Format(format, msgs);
#if DEBUG
		Debug.LogWarning(msg);
#endif
	}
	public static void Error(string format, params object[] msgs)
	{
		string msg = System.DateTime.Now.ToString("MM-dd HH:mm:ss") + "(" + Time.time + "):" + string.Format(format, msgs);
#if DEBUG
		Debug.LogError(msg);
#endif
	}
	public static void Log(object msg)
	{
		Debugger.Log("{0}", msg);
	}
	public static void Warn(object msg)
	{
		Debugger.Warning("{0}",msg);
	}
	public static void Error(object msg)
	{
		Debugger.Error("{0}",msg);
	}
	private void LogToFile(string tag, string msg,string stack)
	{
		StreamWriter logWriter;
		logWriter = logFile.AppendText();
		logWriter.WriteLine("[" + tag + "] " + msg);
		logWriter.WriteLine(stack.Replace("\n", "\r\n"));
		logWriter.Close();
	}
}

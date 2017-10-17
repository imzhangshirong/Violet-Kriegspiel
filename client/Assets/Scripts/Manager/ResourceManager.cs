using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class ResourceManager : Manager
{
    public const string Name = "ResourceManager";
    private Dictionary<string, UnityEngine.Object> m_resourseMap = new Dictionary<string, UnityEngine.Object>();
	
	public string TextByKey(string key)
	{
		return "";
	}
	public T Load<T>(string path) where T : UnityEngine.Object
	{
		string fullPath = path;
        Debuger.Log(fullPath);
		T asset = null;
		if (m_resourseMap.ContainsKey(fullPath))
		{
			return m_resourseMap[fullPath] as T;
		}
        asset = Resources.Load<T>(path);
        if (asset == null)
        {
            Debuger.Error(fullPath + " not exist!");
        }
        m_resourseMap.Add(fullPath, asset);
		return asset;
	}
	public GameObject LoadUI(string name)
	{
        
        return Load<GameObject>(Config.UIResourcePath + "/" + name);
	}
    
	public Texture LoadTexture(string path)
	{
		return Load<Texture>(Config.TextureResourcePath + "/" + path);
	}
	/// <summary>
	/// 返回Android或iOS设备可访问的路径
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public string GetAccessablePath(string path)
	{
		string re = path;
		return re;
	}
}

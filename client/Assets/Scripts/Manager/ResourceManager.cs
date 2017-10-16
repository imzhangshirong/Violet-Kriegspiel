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
	public T Load<T>(string path,ResourceType type) where T : UnityEngine.Object
	{
		string fullPath = path;
        Debuger.Log(fullPath);
		T asset = null;
		if (m_resourseMap.ContainsKey(fullPath))
		{
			return m_resourseMap[fullPath] as T;
		}
		switch (type)
		{
			case ResourceType.Resource:
				asset = Resources.Load<T>(path);
				if (asset == null)
				{
					Debuger.Error(fullPath + " not exist!");
				}
				break;
			case ResourceType.BundleResource:
				break;
		}
		m_resourseMap.Add(fullPath, asset);
		return asset;
	}
	public GameObject LoadUI(string name)
	{
        
        return Load<GameObject>(Config.UIResourcePath + "/" + name, ResourceType.Resource);
	}
    
	public Texture LoadTexture(string path, TextureType type)
	{
		return Load<Texture>(Config.TextureResourcePath + "/" + path, ResourceType.Resource);
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
public enum ResourceType{
	Resource,
	BundleResource,
}
public enum TextureType{
	Skill,
	Stuff,
}
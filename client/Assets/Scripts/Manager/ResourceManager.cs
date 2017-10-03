using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class ResourceManager : ServiceModule<ResourceManager>
{
	private Dictionary<string, UnityEngine.Object> m_resourseMap = new Dictionary<string, UnityEngine.Object>();
	private string m_UIPath;
	private string m_TexturePath;
	
	public string TextByKey(string key)
	{
		return "";
	}
	public T Load<T>(string path,ResourceType type) where T : UnityEngine.Object
	{
		string fullPath = GetResourceFullPath(path, type);
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
	public GameObject LoadUIPrefab(string path)
	{
		return Load<GameObject>(Config.UIResourcePath + "/" + path, ResourceType.Resource);
	}
	public ResourceManager Init(string UIPath, string TexturePath)
	{
		m_UIPath = UIPath;
		m_TexturePath = TexturePath;
		return Instance;
	}
	public string GetResourceFullPath(string ppath,ResourceType type)
	{
		string path = null;
		switch (type)
		{
			case ResourceType.Resource:
				path = Config.ResourceFullPath + "/" + ppath;
			
				break;
			case ResourceType.BundleResource:
				break;
		}
		return path;
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
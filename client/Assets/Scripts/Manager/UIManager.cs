using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UIManager : Manager
{
    public const string Name = "UIManager";
    Transform ml_Left;
	Transform ml_Top;
	Transform ml_Right;
	Transform ml_Bottom;
	Transform ml_Center;
	List<UIData> m_UIDataRegisted = new List<UIData>();
	Dictionary<string, UIData> m_UIDataMap = new Dictionary<string, UIData>();
	List<UIData> m_StackPage = new List<UIData>();
	List<UIData> m_StackOverView = new List<UIData>();
	private int m_baseOverViewDepth = Config.OverViewBaseDepth;
	private int m_basePageDepth = Config.PageBaseDepth;
	public string TopPageName
	{
		get {
			return m_topPage;
		}
	}
	public string TopOverViewName
	{
		get
		{
			return m_topOverView;
		}
	}
	private string m_topPage = null;
	private string m_topOverView = null;
	//注册UI
	public void RegisteUI(string name, string resourceName, UILayoutStyle layoutStyle, UIWindowStyle windowStyle)
	{
		foreach (var item in m_UIDataRegisted)
		{
			if (item.name == name)
			{
				m_UIDataRegisted.Remove(item);
				break;
			}
		}
		m_UIDataRegisted.Add(new UIData(name, resourceName, layoutStyle, windowStyle));
	}
    private void Awake()
    {
        InitLayout();
    }
    public override void OnManagerReady()
    {

    }
    public override void OnManagerDestroy()
    {

    }

	private void InitLayout()
	{
		ml_Left = GameObject.Find(Config.UIRootPath + "_LeftAnchor").transform;
		ml_Right = GameObject.Find(Config.UIRootPath + "_RightAnchor").transform;
		ml_Top = GameObject.Find(Config.UIRootPath + "_TopAnchor").transform;
		ml_Bottom = GameObject.Find(Config.UIRootPath + "_BottomAnchor").transform;
		ml_Center = GameObject.Find(Config.UIRootPath + "_CenterAnchor").transform;
		if (ml_Center == null)
		{
			Debuger.Log("null");
		}
	}
	private UIData GetUIData(string name)
	{
		UIData m_UIData = null;
		foreach (var item in m_UIDataRegisted)
		{
			if (item.name == name)
			{
				m_UIData = item;
				break;
			}
		}
		return m_UIData;
	}
	private UIData LoadUI(string name)
	{
		UIData m_UIData = null;
		if (m_UIDataMap.ContainsKey(name))
		{
			m_UIData = m_UIDataMap[name];
			if (m_UIData.gameObject != null)
			{
				return m_UIData;
			}
		}
		if (m_UIData == null) m_UIData = GetUIData(name);
        if (m_UIData != null)
		{
			m_UIData.gameObject = MonoBehaviour.Instantiate(AppInterface.ResourceManager.LoadUI(m_UIData.resourceName)) as GameObject;
            m_UIData.gameObject.name = name;

            m_UIDataMap.Add(name, m_UIData);
		}
		return m_UIData;
	}
	public void CloseView(string name)
	{
		if (m_UIDataMap.ContainsKey(name))
		{
			UIData m_UIData = m_UIDataMap[name];
			m_UIData.gameObject.SetActive(false);
			UIViewBase baseView = m_UIDataMap[name].gameObject.GetComponent<UIViewBase>();
			if (baseView != null) baseView.OnClose();
			switch (m_UIData.windowStyle)
			{
				case UIWindowStyle.Page:
					m_StackPage.Remove(m_UIData);
					if (m_StackPage.Count > 0)
					{
						ActiveView(m_StackPage[m_StackPage.Count - 1]);
						m_topPage = m_StackPage[m_StackPage.Count - 1].name;
					}
					else
					{
						m_topPage = null;
					}
					break;
				case UIWindowStyle.OverView:
					m_StackOverView.Remove(m_UIData);
					if (m_StackOverView.Count > 0)
					{
						ActiveView(m_StackOverView[m_StackOverView.Count - 1]);
						m_topOverView = m_StackOverView[m_StackOverView.Count - 1].name;
					}
					else
					{
						m_topOverView = null;
					}
					break;
			}
			foreach(var item in m_UIData.hidenOverView)
			{
				ActiveView(item);
			}
			m_UIData.hidenOverView.Clear();
		}
	}
	public void HideOverViewByPage(string name)
	{
		if (m_UIDataMap.ContainsKey(name) && m_topPage !=null && m_UIDataMap.ContainsKey(m_topPage))
		{
			UIData m_UIData = m_UIDataMap[name];
			if (m_UIData.windowStyle == UIWindowStyle.OverView)
			{
				UIData m_UIDataPage = m_UIDataMap[m_topPage];
				if (m_UIData.gameObject.activeSelf)
				{
					m_UIDataPage.hidenOverView.Add(m_UIData);
					m_UIData.gameObject.SetActive(false);

				}
				
			}
			
		}
	}
    private void AutoDepthOverView(int baseDepth)
    {
        
        foreach (var item in m_UIDataMap)
        {
            UIData m_UIData = item.Value;
            if (m_UIData.windowStyle == UIWindowStyle.OverView)
            {
                UIPanel panel = m_UIData.gameObject.GetComponent<UIPanel>();
                panel.depth = baseDepth + 

            }
        }
        if (m_UIDataMap.ContainsKey(name) && m_topPage != null && m_UIDataMap.ContainsKey(m_topPage))
        {
            UIData m_UIData = m_UIDataMap[name];
            if (m_UIData.windowStyle == UIWindowStyle.OverView)
            {
                UIData m_UIDataPage = m_UIDataMap[m_topPage];
                if (m_UIData.gameObject.activeSelf)
                {
                    m_UIDataPage.hidenOverView.Add(m_UIData);
                    m_UIData.gameObject.SetActive(false);

                }

            }

        }
    }
    private void HideView(UIData view)
	{
		if (view == null) return;
		UIViewBase baseView = view.gameObject.GetComponent<UIViewBase>();
		view.gameObject.SetActive(false);
		if (baseView != null) baseView.OnClose();
	}
	private void ActiveView(UIData view)
	{
		if (view == null) return;
		UIViewBase baseView = view.gameObject.GetComponent<UIViewBase>();
		view.gameObject.SetActive(true);
		if (baseView != null) baseView.OnRefresh();
	}
	private void InitView(UIData view,Intent intent = null)
	{
		if (view == null) return;
		UIViewBase baseView = view.gameObject.GetComponent<UIViewBase>();
		UIPanel panel = view.gameObject.GetComponent<UIPanel>();
		if (baseView != null) baseView.OnInit();
		if (baseView == null && panel == null)
		{
			Debuger.Error(view.name + " need UIViewBase or UIPanel!");
			return;
		}
		view.gameObject.SetActive(true);
		switch (view.windowStyle)
		{
			case UIWindowStyle.Page:
				if (m_baseOverViewDepth <= m_basePageDepth + Config.ViewLevelDepth)
				{
					m_baseOverViewDepth *= 2;
					for (int i = m_StackOverView.Count - 1; i >= 0; i--)
					{
						m_StackOverView[i].gameObject.GetComponent<UIViewBase>().Panel.depth = m_baseOverViewDepth + Config.ViewLevelDepth * i;
					}
				}
				if (panel != null)
				{
					panel.depth = m_basePageDepth;
				}
				m_basePageDepth += Config.ViewLevelDepth;
				break;
			case UIWindowStyle.OverView:
				if (panel != null)
				{
					panel.depth = m_baseOverViewDepth + Config.ViewLevelDepth * (m_StackOverView.Count - 1);
				}
				break;
            case UIWindowStyle.Tips:
                if (panel != null)
                {
                    panel.depth = (m_baseOverViewDepth + Config.ViewLevelDepth * (m_StackOverView.Count - 1)) * 10;
                }
                break;
        }
		if (baseView != null) baseView.OnOpen(intent);
	}
	public void PageBack()
	{
		CloseView(m_topPage);
		m_basePageDepth -= Config.ViewLevelDepth;
		//恢复之前隐藏状态
		UIData m_UIData = m_UIDataMap[m_topPage];
		foreach (var item in m_UIData.hidenOverView)
		{
			HideView(item);
		}
	}
	public void ReplaceView(string name, Intent intent = null)
	{
		if (m_topPage != name && m_UIDataMap.ContainsKey(m_topPage))
		{
			UIData m_UIData = m_UIDataMap[m_topPage];
			m_UIData.gameObject.SetActive(false);
			UIViewBase baseView = m_UIData.gameObject.GetComponent<UIViewBase>();
			if (baseView == null)
			{
				Debuger.Error(name + " need UIViewBase!");
				return;
			}
			baseView.OnClose();
			foreach (var item in m_UIData.hidenOverView)
			{
				ActiveView(item);
			}
			OpenView(name, intent);
		}
	}
    public void OpenView(string name)
    {
        OpenView(name, null);
    }
    public void OpenView(string name,Intent intent)
	{
		UIData m_UIData = LoadUI(name);
		if (m_UIData != null && m_topPage != name && m_topOverView != name)
		{
			Transform parent = ml_Center;
			switch (m_UIData.layoutStyle)
			{
				case UILayoutStyle.Top:
					parent = ml_Top;
					break;
				case UILayoutStyle.Left:
					parent = ml_Left;
					break;
				case UILayoutStyle.Bottom:
					parent = ml_Bottom;
					break;
				case UILayoutStyle.Right:
					parent = ml_Right;
					break;
				case UILayoutStyle.Center:
					parent = ml_Center;
					break;
			}
			Vector3 oldPosition = m_UIData.gameObject.transform.localPosition;
			Vector3 oldScale = m_UIData.gameObject.transform.localScale;
			Quaternion oldRotation = m_UIData.gameObject.transform.localRotation;
			m_UIData.gameObject.transform.SetParent(parent);
			m_UIData.gameObject.transform.localPosition = oldPosition;
			m_UIData.gameObject.transform.localScale = oldScale;
			m_UIData.gameObject.transform.localRotation = oldRotation;
			switch (m_UIData.windowStyle)
			{
				case UIWindowStyle.Page:
					m_StackPage.Add(m_UIData);
					m_topPage = name;
					break;
				case UIWindowStyle.OverView:
					m_StackOverView.Add(m_UIData);
					m_topOverView = name;
					break;
                case UIWindowStyle.Tips:
                    break;
            }
			InitView(m_UIData, intent);
			
		}
	}
    
}

public enum UILayoutStyle{
	Top,
	Left,
	Bottom,
	Center,
	Right,
}

public enum UIWindowStyle{
	Page,
	OverView,
    Tips,
}

public class UIData{
	public string name;
	public string resourceName;
	public UILayoutStyle layoutStyle;
	public UIWindowStyle windowStyle;
	public GameObject gameObject;
	public List<UIData> hidenOverView = new List<UIData>();
	public UIData(string name, string resourceName, UILayoutStyle layoutStyle, UIWindowStyle windowStyle)
	{
		this.name = name;
		this.resourceName = resourceName;
		this.layoutStyle = layoutStyle;
		this.windowStyle = windowStyle;
	}

}
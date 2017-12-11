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
    UIData m_topViewData;
    public UIData TopView
    {
        get
        {
            return m_topViewData;
        }
    }
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
	public void RegisteUI(string name, string resourceName, UILayoutStyle layoutStyle, UIViewStyle viewStyle)
	{
		foreach (var item in m_UIDataRegisted)
		{
			if (item.name == name)
			{
				m_UIDataRegisted.Remove(item);
				break;
			}
		}
        if(viewStyle == UIViewStyle.Window)//window允许多个
        {
            App.Manager.ObjectPool.RegisteObject(name, Config.UIResourcePath + "/" + resourceName, 0, 64, 8f);
        }
		m_UIDataRegisted.Add(new UIData(name, resourceName, layoutStyle, viewStyle));
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
        if(m_UIData.viewStyle == UIViewStyle.Window)
        {
            return m_UIData.Clone();
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
            
            if (m_UIData.viewStyle != UIViewStyle.Window)
            {
                m_UIData.gameObject = MonoBehaviour.Instantiate(App.Manager.Resource.LoadUI(m_UIData.resourceName)) as GameObject;
                m_UIData.gameObject.name = name;
                m_UIDataMap.Add(name, m_UIData);
            }
            else
            {
                m_UIData.gameObject = App.Manager.ObjectPool.Instantiate(name);
                m_UIData.gameObject.name = name;

            }
        }
		return m_UIData;
	}
    

    /// <summary>
    /// 销毁View，彻底关闭
    /// </summary>
    /// <param name="name"></param>
    public void CloseViewAndDestroy(string name)
    {
        CloseView(name);
        for(int i = 0; i < m_StackOverView.Count; i++)
        {
            if(m_StackOverView[i].name == name)
            {
                m_StackOverView.RemoveAt(i);
            }
        }
        for (int i = 0; i < m_StackPage.Count; i++)
        {
            if (m_StackPage[i].name == name)
            {
                m_StackPage.RemoveAt(i);
            }
        }
        if (m_UIDataMap.ContainsKey(name))
        {
            if (App.Manager.ObjectPool.HasRegisted(name))
                App.Manager.ObjectPool.Release(name, m_UIDataMap[name].gameObject);//对象池注册的交给对象池管理
            else
                Destroy(m_UIDataMap[name].gameObject);//销毁
        }
    }
    /// <summary>
    /// 关闭View
    /// </summary>
    /// <param name="name"></param>
    public void CloseView(string name)
	{
		if (m_UIDataMap.ContainsKey(name))
		{
			UIData m_UIData = m_UIDataMap[name];
			m_UIData.gameObject.SetActive(false);
			UIViewBase baseView = m_UIDataMap[name].gameObject.GetComponent<UIViewBase>();
			if (baseView != null) baseView.OnClose();
			switch (m_UIData.viewStyle)
			{
				case UIViewStyle.Page:
                case UIViewStyle.Window:
                    m_StackPage.Remove(m_UIData);
					if (m_StackPage.Count > 0)
					{
						ActiveView(m_StackPage[m_StackPage.Count - 1],m_UIData.viewStyle == UIViewStyle.Page);
						m_topPage = m_StackPage[m_StackPage.Count - 1].name;
                        m_topViewData = m_StackPage[m_StackPage.Count - 1];

                    }
					else
					{
						m_topPage = null;
                        m_topViewData = null;

                    }
					break;
				case UIViewStyle.OverView:
					m_StackOverView.Remove(m_UIData);
					if (m_StackOverView.Count > 0)
					{
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
			if (m_UIData.viewStyle == UIViewStyle.OverView)
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

    public void HideAllOverViewByPage()
    {
        if(m_topPage != null && m_UIDataMap.ContainsKey(m_topPage))
        {
            foreach(var item in m_UIDataMap)
            {
                UIData m_UIData = item.Value;
                Debuger.Error(m_UIData.name + "=>" + m_UIData.viewStyle);
                if (m_UIData.viewStyle == UIViewStyle.OverView)
                {
                    UIData m_UIDataPage = m_UIDataMap[m_topPage];
                    if (m_UIData.gameObject.activeSelf)
                    {
                        Debuger.Error(m_UIData.name + "=>" + m_UIData.viewStyle);
                        m_UIDataPage.hidenOverView.Add(m_UIData);
                        m_UIData.gameObject.SetActive(false);

                    }
                }
            }
        }
    }

    private void AutoDepthOverView(int baseDepth)
    {
        for(int i = 0; i < m_StackOverView.Count; i++)
        {
            UIData m_UIData = m_StackOverView[i];
            if (m_UIData.viewStyle == UIViewStyle.OverView)
            {
                UIPanel panel = m_UIData.gameObject.GetComponent<UIPanel>();
                if (panel == null) continue;
                panel.depth = baseDepth + Config.OverViewLevelBaseDepth + (i) * Config.OverViewLevelDepth;

            }
        }
    }
    private void AutoDepth()
    {
        m_basePageDepth = Config.PageBaseDepth;
        int overViewAllDepth = Config.OverViewLevelBaseDepth + m_StackOverView.Count * Config.OverViewLevelDepth;
        bool hasWindow = false;
        for(int i = 0; i < m_StackPage.Count; i++)
        {
            UIPanel panel = m_StackPage[i].gameObject.GetComponent<UIPanel>();
            if (panel != null)
            {
                if (m_StackPage[i].viewStyle == UIViewStyle.Page)
                {
                    panel.depth = m_basePageDepth;
                }
                else if (m_StackPage[i].viewStyle == UIViewStyle.Window && !hasWindow)
                {
                    m_basePageDepth -= Config.ViewLevelDepth;
                    AutoDepthOverView(m_basePageDepth);
                    m_basePageDepth += overViewAllDepth;
                    hasWindow = true;
                    panel.depth = m_basePageDepth;
                }
                m_basePageDepth += Config.ViewLevelDepth;
            }
        }
        if (!hasWindow)
        {
            AutoDepthOverView(m_basePageDepth);
        }
    }
    private void HideView(UIData view)
	{
		if (view == null) return;
		UIViewBase baseView = view.gameObject.GetComponent<UIViewBase>();
		view.gameObject.SetActive(false);
		if (baseView != null) baseView.OnClose();
	}
	private void ActiveView(UIData view,bool needRefresh = true)
	{
		if (view == null) return;
		UIViewBase baseView = view.gameObject.GetComponent<UIViewBase>();
		view.gameObject.SetActive(true);
		if (baseView != null && needRefresh) baseView.OnRefresh();
	}
	private void InitView(UIData view,Intent intent = null)
	{
		if (view == null) return;
		UIViewBase baseView = view.gameObject.GetComponent<UIViewBase>();
		UIPanel panel = view.gameObject.GetComponent<UIPanel>();
		if (panel == null)
		{
			Debuger.Error(view.name + " need UIViewBase or UIPanel!");
			return;
		}
        AutoDepth();
        if (baseView != null) baseView.OnInit();
        if (view.viewStyle == UIViewStyle.Tips)
        {
            panel.depth = m_basePageDepth + Config.ViewLevelDepth;
        }
        view.gameObject.SetActive(true);
        if (baseView != null) baseView.OnOpen(intent);
	}
	public void PageBack(string toPage = "")
	{
        if(toPage == "")
        {
            if (TopView.viewStyle == UIViewStyle.Window)
            {
                TopView.gameObject.SetActive(false);
                UIViewBase baseView = TopView.gameObject.GetComponent<UIViewBase>();
                if (baseView != null) baseView.OnClose();
                m_StackPage.Remove(TopView);
                App.Manager.ObjectPool.Release(TopView.name, TopView.gameObject);
                if (m_StackPage.Count > 0)
                {
                    ActiveView(m_StackPage[m_StackPage.Count - 1],false);
                    m_topPage = m_StackPage[m_StackPage.Count - 1].name;
                    m_topViewData = m_StackPage[m_StackPage.Count - 1];

                }
                else
                {
                    m_topPage = null;
                    m_topViewData = null;

                }
                m_basePageDepth -= Config.ViewLevelDepth;
            }
            else
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
        }
        else
        {
            for(int i = m_StackPage.Count - 1; i >= 0; i--)
            {
                if (m_StackPage[i].name == toPage)
                {
                    for(int j = 0;j< m_StackPage.Count - i; j++)
                    {
                        PageBack();
                    }
                    break;
                }
            }
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
		if (m_UIData != null && m_topOverView != name)
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
			switch (m_UIData.viewStyle)
			{
				case UIViewStyle.Page:
                case UIViewStyle.Window:
                    m_StackPage.Add(m_UIData);
					m_topPage = name;
                    m_topViewData = m_UIData;
					break;
				case UIViewStyle.OverView:
					m_StackOverView.Add(m_UIData);
					m_topOverView = name;
					break;
                case UIViewStyle.Tips:
                    break;
            }
			InitView(m_UIData, intent);
			
		}
	}
    
}

public enum UILayoutStyle{
	Center = 0,
	Top = 1,
    Right = 2,
	Bottom = 3,
    Left = 4,
}

public enum UIViewStyle{
	Page = 0,//只有一个
    OverView = 1,//只有一个
    Tips = 2,//taost
    Window = 3,//可以重复创建
}

public class UIData{
	public string name;
	public string resourceName;
	public UILayoutStyle layoutStyle;
	public UIViewStyle viewStyle;
	public GameObject gameObject;
	public List<UIData> hidenOverView = new List<UIData>();
	public UIData(string name, string resourceName, UILayoutStyle layoutStyle, UIViewStyle viewStyle)
	{
		this.name = name;
		this.resourceName = resourceName;
		this.layoutStyle = layoutStyle;
		this.viewStyle = viewStyle;
	}
    public UIData Clone()
    {
        return new UIData(name,resourceName,layoutStyle,viewStyle);
    }

}
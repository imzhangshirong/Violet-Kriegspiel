using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Package<T> : IPackage where T : new()
{

    static T m_instance;
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
    bool m_inited = false;
    private Dictionary<string, object> m_DicData = new Dictionary<string, object>();



    public void Push(string key, object value)
    {
        m_DicData.Add(key, value);
    }
    public object Value(string key)
    {
        if (m_DicData.ContainsKey(key)) return m_DicData[key];
        return null;

    }
    public bool inited{
        get{
            return m_inited;
        }
    }
    public virtual void Init(object data)
    {
        m_inited = true;
    }

    public virtual void Release()
    {
        m_inited = false;
    }
}
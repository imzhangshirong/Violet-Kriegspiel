using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//对象池
public class ObjectPoolManager : Manager
{
    public const string Name = "ObjectPoolManager";
    private Dictionary<string, PoolObjectState> m_ObjectMap = new Dictionary<string, PoolObjectState>();
    private Dictionary<string, List<PoolInstanceState>> m_ObjectInstanceMap = new Dictionary<string, List<PoolInstanceState>>();
    GameObject m_ObjectPool;
    public void RegisteObject(string name, string resourcePath, int min,int max,float cleanDuration)
    {
        if (min > max || max <= 0) throw new Exception("ObjectPool Max must > 0!");
        GameObject go = AppInterface.ResourceManager.Load<GameObject>(resourcePath);
        PoolObjectState pos = new PoolObjectState();
        pos.min = min;
        pos.max = max;
        pos.cleanDuration = cleanDuration;
        pos.resource = resourcePath;
        pos.gameObject = go;
        m_ObjectMap.Add(name, pos);
        m_ObjectInstanceMap.Add(name, new List<PoolInstanceState>());
        for(int i = 0;i< pos.min; i++)
        {
            GameObject goI = Instantiate<GameObject>(go);
            goI.SetActive(false);
            goI.transform.parent = m_ObjectPool.transform;
            goI.transform.localScale = Vector3.one;
            goI.transform.localPosition = Vector3.one;
            PoolInstanceState pis = new PoolInstanceState();
            pis.gameObject = goI;
            pis.name = pos.name;
            pis.stateId = -1;
            pis.state = PoolInstanceStateType.Sleep;
            m_ObjectInstanceMap[name].Add(pis);
        }
        StartCoroutine(AutoClean(name));
    }
    public GameObject Instantiate(string name)
    {
        if (m_ObjectMap.ContainsKey(name))
        {
            PoolObjectState pos = m_ObjectMap[name];
            List<PoolInstanceState> poiss = m_ObjectInstanceMap[name];
            int activeCount = 0;
            int minId = -1;
            int maxId = 0;
            PoolInstanceState oldest = null;
            PoolInstanceState sleepOne = null;
            for (int i = 0; i < poiss.Count; i++)
            {
                if (i == 0 && minId == -1) minId = poiss[i].stateId;
                if(poiss[i].state == PoolInstanceStateType.Sleep)
                {
                    sleepOne = poiss[i];
                }
                else
                {
                    activeCount++;
                    if (poiss[i].stateId <= minId)
                    {
                        oldest = poiss[i];
                        minId = oldest.stateId;
                    }
                    if(maxId < poiss[i].stateId)
                    {
                        maxId = poiss[i].stateId;
                    }
                }
            }
            if (sleepOne != null)
            {
                sleepOne.stateId = maxId + 1;
                sleepOne.state = PoolInstanceStateType.Active;
                ObjectPoolInstance opi = sleepOne.gameObject.GetComponent<ObjectPoolInstance>();
                if (opi != null)
                {
                    opi.OnPoolInstantiated();
                }
                return sleepOne.gameObject;
            }
            else
            {
                if (activeCount >= pos.max)
                {
                    sleepOne = oldest;
                    sleepOne.stateId = maxId + 1;
                    sleepOne.state = PoolInstanceStateType.Active;
                    ObjectPoolInstance opi = sleepOne.gameObject.GetComponent<ObjectPoolInstance>();
                    if (opi != null)
                    {
                        opi.OnPoolRelease();
                        opi.OnPoolInstantiated();
                    }
                    return sleepOne.gameObject;
                }
                else
                {
                    GameObject goI = Instantiate<GameObject>(pos.gameObject);
                    goI.SetActive(false);
                    goI.transform.parent = m_ObjectPool.transform;
                    goI.transform.localScale = Vector3.one;
                    goI.transform.localPosition = Vector3.one;
                    PoolInstanceState pis = new PoolInstanceState();
                    pis.gameObject = goI;
                    pis.name = pos.name;
                    pis.stateId = maxId + 1;
                    pis.state = PoolInstanceStateType.Active;
                    m_ObjectInstanceMap[name].Add(pis);
                    ObjectPoolInstance opi = goI.GetComponent<ObjectPoolInstance>();
                    if (opi != null)
                    {
                        opi.OnPoolInstantiated();
                    }
                    return goI;
                }
            }
        }
        return null;
    }

    public void Releasse(string name, GameObject instanceObject)
    {
        if (m_ObjectMap.ContainsKey(name))
        {
            PoolObjectState pos = m_ObjectMap[name];
            List<PoolInstanceState> poiss = m_ObjectInstanceMap[name];
            for (int i = 0; i < poiss.Count; i++)
            {
                if (poiss[i].gameObject == instanceObject)
                {
                    poiss[i].state = PoolInstanceStateType.Sleep;
                    ObjectPoolInstance opi = instanceObject.GetComponent<ObjectPoolInstance>();
                    if (opi != null)
                    {
                        opi.OnPoolRelease();
                    }
                    instanceObject.SetActive(false);
                    instanceObject.transform.parent = m_ObjectPool.transform;
                    instanceObject.transform.localScale = Vector3.one;
                    instanceObject.transform.localPosition = Vector3.one;
                }
            }
            
        }
    }

    IEnumerator AutoClean(string name)
    {
        List<PoolInstanceState> needDestroy = new List<PoolInstanceState>();
        while (m_ObjectMap.ContainsKey(name))
        {
            PoolObjectState pos = m_ObjectMap[name];
            List<PoolInstanceState> poiss = m_ObjectInstanceMap[name];
            if (poiss.Count > pos.min)
            {
                needDestroy.Clear();
                for (int i=0;i< poiss.Count; i++)
                {
                    if(poiss[i].state == PoolInstanceStateType.Sleep)
                    {
                        needDestroy.Add(poiss[i]);
                    }
                    if (poiss.Count <= pos.min + needDestroy.Count) break;
                }
                for(int i = needDestroy.Count - 1; i >= 0; i--)
                {
                    Destroy(needDestroy[i].gameObject);
                    poiss.Remove(needDestroy[i]);

                }
                needDestroy.Clear();
            }
            yield return new WaitForSeconds(pos.cleanDuration);
        }
    }

    public override void OnManagerReady()
    {
        //创建Pool
        if (m_ObjectPool == null)
        {
            m_ObjectPool = new GameObject();
            m_ObjectPool.name = "ObjectPool";
            m_ObjectPool.transform.parent = gameObject.transform;
            m_ObjectPool.transform.localScale = Vector3.one;
            m_ObjectPool.transform.localPosition = Vector3.zero;
        }
    }
    public override void OnManagerDestroy()
    {
        m_ObjectInstanceMap.Clear();
        m_ObjectMap.Clear();
    }
}

public class PoolObjectState
{
    public string name;
    public int min;
    public int max;
    public float cleanDuration;
    public GameObject gameObject;
    public string resource;
}

public class PoolInstanceState
{
    public string name;
    public int stateId;
    public GameObject gameObject;
    public PoolInstanceStateType state = PoolInstanceStateType.Sleep;
}

public enum PoolInstanceStateType
{
    Active,
    Sleep,
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(UIEventListener))]
public class NClickEvent : TreeLeaf
{
    public string[] m_EventKeys;
    public void OnClick()
    {
        for(int i = 0; i < m_EventKeys.Length; i++)
        {
            Push(m_EventKeys[i],gameObject);
        }
    }
}

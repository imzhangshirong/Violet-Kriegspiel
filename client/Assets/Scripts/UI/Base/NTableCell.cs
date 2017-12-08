using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NTableCell : MonoBehaviour
{
    public NTableView m_TableView;
    public void Push(string msg, object content = null)
    {
        m_TableView.treeRoot.Broadcast(msg, content);
    }
    public void BindEvent(string msg, MessageListener listener) //绑定事件监听
    {
        m_TableView.BindEvent(msg, listener);
    }
}

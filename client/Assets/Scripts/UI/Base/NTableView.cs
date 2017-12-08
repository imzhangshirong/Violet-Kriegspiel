using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
public class NTableView : TreeLeaf
{
    public UIScrollView m_ScrollView;
    public UIPanel m_ScrollPanel;
    public GameObject m_start;
    public GameObject m_end;
    public TableViewOrienatation m_Orienatation;
    public float size;//高或宽，看Orienatation

    
    List<NTableCell> availableCells = new List<NTableCell>();
    List<NTableCell> visibleCells = new List<NTableCell>();
    IList<object> listData;
    float defualtMinSize;
    void Start(){
        
        for(int i = 0; i < transform.childCount; i++)
        {
            NTableCell cell = transform.GetChild(i).GetComponent<NTableCell>();
            if (cell != null)
            {
                availableCells.Add(cell);
                cell.gameObject.SetActive(false);
            }
        }
        m_ScrollView.onMomentumMove += ScrollMoving;
        m_ScrollView.onDragStarted += ScrollMoving;
        m_ScrollView.onDragFinished += ScrollMoving;
        defualtMinSize = Mathf.Abs(m_end.transform.localPosition.y - m_start.transform.localPosition.y);
    }

    void ScrollMoving()
    {
        
    }

    void SetCell()
    {
        
    }
    /// <summary>
    /// 设置data
    /// </summary>
    /// <param name="list"></param>
    void SetData(IList<object> list)
    {
        listData = list;

    }
    /// <summary>
    /// 更新view
    /// </summary>
    void NotifyUpdate()
    {

    }
}
public enum TableViewOrienatation
{
    Horizontal = 0,
    Vertical = 1,
}
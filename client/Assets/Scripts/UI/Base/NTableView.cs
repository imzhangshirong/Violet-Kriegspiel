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
    public float cellSpace;
    
    List<NTableCell> availableCells = new List<NTableCell>();
    List<NTableCell> visibleCells = new List<NTableCell>();
    IList<object> listData;
    float defualtMinSize;
    int topItemIndex = -1;
    int visibleItemCount = 0;
    float offset = 0;
    bool isDraging = false;
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
        if (availableCells.Count > 0)
        {
            //offset = (m_Orienatation == TableViewOrienatation.Vertical) ? availableCells[0].height: availableCells[0].width;
        }
        
        

        VisibleProcess();
        m_start.SetActive(true);
        m_start.transform.localPosition = ((m_Orienatation == TableViewOrienatation.Vertical) ? Vector3.down : Vector3.right);
        m_end.SetActive(true);
        m_end.transform.localPosition = ((m_Orienatation==TableViewOrienatation.Vertical)?Vector3.down:Vector3.right)*(defualtMinSize-1);
        NotifyUpdate();
        m_ScrollView.onMomentumMove += ScrollMoving;
        m_ScrollView.onDragStarted += DragStarted;
        m_ScrollView.onDragFinished += DragFinished;
    }

    void GetMiniSize()
    {
        defualtMinSize = Mathf.Abs((m_Orienatation == TableViewOrienatation.Vertical) ? (m_end.transform.localPosition.y - m_start.transform.localPosition.y) : (m_end.transform.localPosition.x - m_start.transform.localPosition.x));
    }

    public void ResetPosition()
    {
        m_ScrollView.ResetPosition();
        ScrollMoving();
    }

    void DragStarted()
    {
        isDraging = true;
    }
    void DragFinished()
    {
        isDraging = false;
    }

    private void FixedUpdate()
    {
        if (isDraging) ScrollMoving();
    }

    void ScrollMoving()
    {
        VisibleProcess();
        SetCell();
    }

    void SetCell(bool update = false)
    {
        if (listData == null) return;
        List<NTableCell> needHide = new List<NTableCell>();
        Dictionary<int, bool> hasItem = new Dictionary<int, bool>();
        int n = topItemIndex + visibleItemCount;
        if (n > listData.Count - 1) n = listData.Count - 1;
        for (int i = 0; i < visibleCells.Count; i++)
        {
            int index = (int)(((m_Orienatation == TableViewOrienatation.Vertical) ? -visibleCells[i].transform.localPosition.y : visibleCells[i].transform.localPosition.x)/ (size + cellSpace));
            if (index < topItemIndex || index > n)
            {
                visibleCells[i].gameObject.SetActive(false);
                needHide.Add(visibleCells[i]);
            }
            else
            {
                if (!hasItem.ContainsKey(index))
                {
                    if(update) visibleCells[i].OnDraw(index, listData[index]);
                    hasItem.Add(index, true);
                }
                else
                {
                    visibleCells[i].gameObject.SetActive(false);
                    needHide.Add(visibleCells[i]);
                }
            }
        }
        for(int i = 0; i < needHide.Count; i++)
        {
            availableCells.Add(needHide[i]);
            visibleCells.Remove(needHide[i]);
        }
        for (int i = topItemIndex; i <= n; i++)
        {
            if (!hasItem.ContainsKey(i) && availableCells.Count > 0)
            {
                NTableCell newCell = availableCells[0];
                availableCells.RemoveAt(0);
                newCell.transform.localPosition = ((m_Orienatation == TableViewOrienatation.Vertical) ? Vector3.down : Vector3.right) * (i * (size + cellSpace));
                newCell.gameObject.SetActive(true);
                newCell.OnDraw(i, listData[i]);
                visibleCells.Add(newCell);
            }
        }
    }

    void VisibleProcess()
    {
        if (defualtMinSize <= 0) GetMiniSize();
        float start = (m_Orienatation == TableViewOrienatation.Vertical) ? -m_ScrollPanel.clipOffset.y : m_ScrollPanel.clipOffset.x;
        float panelSize = (m_Orienatation == TableViewOrienatation.Vertical) ? m_ScrollPanel.GetViewSize().y : m_ScrollPanel.GetViewSize().x;
        offset = (panelSize - size) / 2 - ((m_Orienatation == TableViewOrienatation.Vertical) ? m_ScrollPanel.clipSoftness.y : m_ScrollPanel.clipSoftness.x);
        transform.localPosition = ((m_Orienatation == TableViewOrienatation.Vertical) ? Vector3.up : Vector3.left) * offset;
        visibleItemCount = (int)(panelSize / (size + cellSpace)) + 2;
        topItemIndex = (int)(start / (size + cellSpace)) - 1;
        if (listData == null)
        {
            topItemIndex = -1;
            visibleItemCount = 0;
        }
        else
        {
            if (topItemIndex + visibleItemCount >= listData.Count)
            {
                topItemIndex = listData.Count - visibleItemCount;
            }
            if (topItemIndex < 0) topItemIndex = 0;
        }
    }

    /// <summary>
    /// 设置data
    /// </summary>
    /// <param name="list"></param>
    public void SetData<T>(IList<T> list,bool updateImmediately = false)
    {
        if (defualtMinSize <= 0) GetMiniSize();
        listData = new List<object>();
        for(int i = 0; i < list.Count; i++)
        {
            listData.Add((object)list[i]);
        }
        float area = list.Count * (size + cellSpace) - 1 - ((list.Count > 0) ? cellSpace : 0);
        if (area < defualtMinSize) area = defualtMinSize;
        m_end.transform.localPosition = ((m_Orienatation == TableViewOrienatation.Vertical) ? Vector3.down : Vector3.right) * area;
        VisibleProcess();
        SetCell(updateImmediately);
    }
    /// <summary>
    /// 更新view
    /// </summary>
    public void NotifyUpdate()
    {
        VisibleProcess();
        SetCell(true);
    }
}
public enum TableViewOrienatation
{
    Horizontal = 0,
    Vertical = 1,
}
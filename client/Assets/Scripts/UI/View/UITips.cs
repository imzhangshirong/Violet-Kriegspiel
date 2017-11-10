using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UITips : UIViewBase
{
    public UILabel label;
    public float stayTime;
    private bool nextDisable;
    private bool isShowing;
    TweenAlpha tween;
    private List<string> m_MsgList = new List<string>();

    public override void OnInit()
    {
        base.OnInit();
        tween = GetComponent<TweenAlpha>();
    }
    public override void OnOpen(Intent intent)
    {
        base.OnOpen(intent);
        string msg = (string)intent.Value("tipContent");
        StartShow(msg);

    }
    void StartShow(string msg)
    {
        if (!isShowing)
        {
            isShowing = true;
            label.text = msg;
            tween.enabled = true;
            tween.PlayForward();
        }
        else
        {
            m_MsgList.Add(msg);
        }
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
    }
    public void ReadyClose()
    {
        if (nextDisable)
        {
            gameObject.SetActive(false);
            nextDisable = false;
            tween.enabled = false;
            isShowing = false;
            if (m_MsgList.Count > 0)
            {
                gameObject.SetActive(true);
                StartShow(m_MsgList[0]);
                m_MsgList.RemoveAt(0);
            }
            return;
        }
        StartCoroutine(holdAndTweenBack());
    }
    IEnumerator holdAndTweenBack()
    {
        yield return new WaitForSeconds(stayTime);
        tween.PlayReverse();
        nextDisable = true;
    }
}

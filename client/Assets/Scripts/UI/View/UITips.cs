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
        StopAllCoroutines();
        label.text = msg;
        tween.enabled = true;
        tween.ResetToBeginning();
        tween.PlayForward();
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
    }
    public void ReadyClose()
    {
        StartCoroutine(holdAndTweenBack());
    }
    IEnumerator holdAndTweenBack()
    {
        yield return new WaitForSeconds(stayTime);
        tween.PlayReverse();
        nextDisable = true;
    }
}

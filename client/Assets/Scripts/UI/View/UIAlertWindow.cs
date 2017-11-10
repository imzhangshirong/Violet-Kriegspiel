using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public delegate void AlertWindowClick();

public class UIAlertWindow : UIViewBase
{
    public UILabel title;
    public UILabel content;
    public UILabel positiveLabel0;
    public UILabel positiveLabel;
    public UILabel negativeLabel;
    public GameObject OneButtonMode;
    public GameObject TwoButtonMode;

    AlertWindowClick positive;
    AlertWindowClick negative;
    TweenScale tween;

    public override void OnInit()
    {
        base.OnInit();
        tween = GetComponentInChildren<TweenScale>();
    }
    public override void OnOpen(Intent intent)
    {
        base.OnOpen(intent);
        string titleStr = intent.Value<string>("title");
        string contentStr = intent.Value<string>("content");
        string positiveStr = intent.Value<string>("positiveStr");
        string negativeStr = intent.Value<string>("negativeStr");
        positive = intent.Value<AlertWindowClick>("positive");
        negative = intent.Value<AlertWindowClick>("negative");
        int mode = intent.Value<int>("mode");
        title.text = titleStr;
        //content.text = "";
        content.text = contentStr;
        positiveLabel0.text = positiveStr;
        positiveLabel.text = positiveStr;
        negativeLabel.text = negativeStr;
        OneButtonMode.SetActive(false);
        TwoButtonMode.SetActive(false);
        switch ((AlertWindowMode)mode)
        {
            case AlertWindowMode.OneButton:
                OneButtonMode.SetActive(true);
                break;
            case AlertWindowMode.TwoButton:
                TwoButtonMode.SetActive(true);
                break;
        }
        tween.ResetToBeginning();
        tween.PlayForward();
        
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
    }

    public void OnPositiveClick()
    {
        if (positive != null)
        {
            positive();
        }
        else
        {
            Common.UI.BackPage();
        }
    }
    public void OnNegativeClick()
    {
        if (negative != null)
        {
            negative();
        }
        else
        {
            Common.UI.BackPage();
        }
    }
}

public enum AlertWindowMode
{
    OneButton = 0,
    TwoButton = 1,
}
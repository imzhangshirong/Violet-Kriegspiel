using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIAlertWindow : UIViewBase
{
    public override void OnInit()
    {
        base.OnInit();
    }
    public override void OnOpen(Intent intent)
    {
        base.OnOpen(intent);
    }
    public void BackPage()
    {
        AppInterface.UIManager.PageBack();
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
    }

    public void OnPositiveClick()
    {

    }
    public void OnNagetiveClick()
    {

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIPagePanel : UIViewBase
{
	public override void OnInit()
	{
		base.OnInit();
		Global.MUI.HideOverViewByPage("UIPlayerInfo");
	}
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		Debuger.Log("PagePanel Open");
	}
	public void BackPage()
	{
		Global.MUI.PageBack();
	}
	public override void OnRefresh()
	{
		base.OnRefresh();
		Debuger.Log("PagePanel Refresh");
	}
}

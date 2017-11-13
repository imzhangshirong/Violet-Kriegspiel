using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UILobbyPanel : UIViewBase
{
	public override void OnInit()
	{
		base.OnInit();
        App.Manager.UI.OpenView("UIPlayerInfo");
	}
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
	}
}

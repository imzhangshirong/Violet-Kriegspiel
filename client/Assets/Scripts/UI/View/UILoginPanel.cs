using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UILoginPanel : UIViewBase
{
	public override void OnInit()
	{
		base.OnInit();
        App.Manager.UI.HideOverViewByPage("UITopTest");
	}
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
	}

	public void Login()
	{
		App.Manager.UI.ReplaceView("UILobbyPanel");
	}
}

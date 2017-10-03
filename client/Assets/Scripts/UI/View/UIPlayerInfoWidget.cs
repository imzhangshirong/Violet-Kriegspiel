using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIPlayerInfoWidget : UIViewBase
{
	public override void OnInit()
	{
		base.OnInit();
		Debuger.Log("init");
	}
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		Debuger.Log("Open");
	}
}

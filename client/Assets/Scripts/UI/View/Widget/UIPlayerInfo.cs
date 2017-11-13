using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Violet.Rpc;
class UIPlayerInfo : UIViewBase
{
    public UILabel userName;
    public UILabel userLevel;
    public override void OnInit()
	{
		base.OnInit();
		Debuger.Log("UIPlayerInfoPanel Init");
        
    }
    public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		UpdateData();
		Debuger.Log("UIPlayerInfoPanel Open");
	}

	public void UpdateData(){
		PlayerInfo playerInfo = App.Package.Player.playerInfo;
		userName.text = playerInfo.UserName;
		userLevel.text = "Level." + playerInfo.Level;
	}
}

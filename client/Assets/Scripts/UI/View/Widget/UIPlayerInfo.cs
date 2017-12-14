using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Violet.Rpc;
public class UIPlayerInfo : UIViewBase
{
    public UILabel userName;
    public UILabel userLevel;
    public override void OnInit()
	{
		base.OnInit();
		Debugger.Log("UIPlayerInfoPanel Init");
        
    }
    public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		UpdateData();
		Debugger.Log("UIPlayerInfoPanel Open");
	}

	public void UpdateData(){
		PlayerInfo playerInfo = App.Package.Player.playerInfo;
		userName.text = playerInfo.UserName;
		userLevel.text = "Level." + playerInfo.Level;
	}

    public void DestroyGame()
    {
        Application.Quit();
    }
}

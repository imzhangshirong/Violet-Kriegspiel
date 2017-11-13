﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
		Debuger.Log("UIPlayerInfoPanel Open");
	}
}
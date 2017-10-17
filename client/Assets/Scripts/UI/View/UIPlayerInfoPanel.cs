using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIPlayerInfoPanel : UIViewBase
{
    public UILabel globalLabel;
    public UILabel widgetLabel;
    private int currentNumber = 0;
    public override void OnInit()
	{
		base.OnInit();
		Debuger.Log("UIPlayerInfoPanel Init");

        //绑定事件
        BindEvent("_addNumber", AddNumber);
        BindEvent("_greeting", Greeting);
    }
    private void AddNumber(object content)
    {
        int num = (int)content;
        currentNumber += num;
        widgetLabel.text = "FromWidget:" + currentNumber;
    }
    private void Greeting(object content)
    {
        string greet = (string)content;
        globalLabel.text = "FromGlobal:" + greet;
    }
    public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		Debuger.Log("UIPlayerInfoPanel Open");
	}
}

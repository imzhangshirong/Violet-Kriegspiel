using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using Com.Violet.Rpc;
using Google.Protobuf;

public class UIMainPanel : UIViewBase
{
    public UILabel rpcResponse;
    public UIInput rpcInput;
    public UIInput ipInput;
    public UIInput portInput;
    public GameObject debug;
    public UILabel debugText;
    public UIScrollView debugTextScroll;
    bool debugOpened = false;
    public override void OnInit()
	{
        Debuger.outLabel = debugText;
        Debuger.outLabelScroll = debugTextScroll;
	}

    public void OpenDebug()
    {
        debugOpened = !debugOpened;
        debug.SetActive(debugOpened);
    }

    public void OnSetIp()
    {
        try
        {
            Config.ServerHost = ipInput.value;
            Config.ServerHostPort = int.Parse(portInput.value);
            RpcNetwork.Instance.Init();
            Common.UI.OpenTips("Set IP Config");
        }
        catch(Exception e)
        {
            Debuger.Error(e.Message);
        }
        
    }

	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		Debuger.Log("MainPanel Open");
	}
	public void NextPage()
	{
        //进入游戏测试
        App.Manager.UI.ReplaceView("UILoginPanel");
        App.Manager.UI.CloseView("UITopTest");
        App.Manager.UI.CloseView("UIMainPanel");
    }
	public override void OnRefresh()
	{
		base.OnRefresh();
		Debuger.Log("MainPanel Refresh");
	}
	public override void OnClose()
	{
		base.OnClose();
		Debuger.Log("MainPanel close");
	}
    public void RPC_TEST()
    {
        HelloRequest request = new HelloRequest();
        request.Content = rpcInput.value;
        App.Manager.Network.Request("Hello",request,delegate(IMessage response) {
            HelloResponse res = response as HelloResponse;
            rpcResponse.text = res.Greet;
            App.Manager.Event.Broadcast("_greeting", res.Greet);
        });
    }
    public void SIMPLE_TEST()
    {
        Common.UI.OpenAlert("Confirm", "Will Send String \""+ rpcInput.value + "\" To Server", "OK", Test, "Cancel", Nagetive, false);
        //Debuger.Log("send string:" + rpcInput.value);
        //RpcNetwork.SendUnEncode(rpcInput.value);
    }
    void Test()
    {
        Common.UI.OpenAlert("Confirm弹窗测试", "RRRWill Send String \"" + rpcInput.value + "\" To Server", "OK", Test, "Cancel", Nagetive, false);

    }
    void Positive()
    {
        try
        {
            Debuger.Log("send string:" + rpcInput.value);
            RpcNetwork.SendUnEncode(rpcInput.value);
        }
        catch(Exception e)
        {
            Debuger.Warn(e.StackTrace);
        }
        Common.UI.BackPage();
    }
    void Nagetive()
    {
        Debuger.Log("send cancel");
        Common.UI.BackPage();
    }
}

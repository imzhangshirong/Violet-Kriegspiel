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
    bool debugOpened = false;
    public override void OnInit()
	{

	}

    public void OnSetIp()
    {
        try
        {
            Config.ServerHost = ipInput.value;
            PlayerPrefs.SetString("hostIp",ipInput.value);
            Config.ServerHostPort = int.Parse(portInput.value);
            RpcNetwork.Instance.Init();
            Common.UI.OpenTips("Set IP Config");
            NextPage();
        }
        catch(Exception e)
        {
            Debugger.Error(e.Message);
        }
        
    }

	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		Debugger.Log("MainPanel Open");
        string ip = PlayerPrefs.GetString("hostIp");
        if (ip != "")ipInput.value = ip;
        //OnSetIp();
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
		Debugger.Log("MainPanel Refresh");
	}
	public override void OnClose()
	{
		base.OnClose();
		Debugger.Log("MainPanel close");
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
        //Debugger.Log("send string:" + rpcInput.value);
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
            Debugger.Log("send string:" + rpcInput.value);
            RpcNetwork.SendUnEncode(rpcInput.value);
        }
        catch(Exception e)
        {
            Debugger.Warn(e.StackTrace);
        }
        Common.UI.BackPage();
    }
    void Nagetive()
    {
        Debugger.Log("send cancel");
        Common.UI.BackPage();
    }
    void OnClick()
    {
        if(Input.touchCount>0){
            for(int i=0;i<Input.touchCount;i++){
                Debugger.Warn("Touch:"+Input.touches[i].position.ToString());
            }
        }
        Debugger.Warn("Mouse:"+Input.mousePosition.ToString());
    }
    public void SetLocal()
    {
        Config.ServerHost = "192.168.80.15";
        Config.ServerHostPort = 8000;
        RpcNetwork.Instance.Init();
        NextPage();
    }
    public void SetRemote()
    {
        Config.ServerHost = "123.207.122.248";
        Config.ServerHostPort = 8000;
        RpcNetwork.Instance.Init();
        NextPage();
    }
}

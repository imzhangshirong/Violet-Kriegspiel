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
        string ip = PlayerPrefs.GetString("hostIp");
        if (ip != "")ipInput.value = ip;
        FieldRoadStation s1= new FieldRoadStation(){
            type = FieldRoadStationType.Rail,
            point = new ChessPoint(1,5),
        };
        FieldRoadStation s2= new FieldRoadStation(){
            type = FieldRoadStationType.Barrack,
            point = new ChessPoint(1,6),
        };
        Debuger.Warn(ChessAgainst.IsBarrack(new ChessPoint(1,7)));
        Debuger.Warn(ChessAgainst.InRailArea(new ChessPoint(0,7)));
        Debuger.Warn(ChessAgainst.InRailArea(new ChessPoint(1,6)));
        Debuger.Warn(ChessAgainst.InRailArea(new ChessPoint(1,10)));
        Debuger.Warn(ChessAgainst.InRailArea(new ChessPoint(4,6)));
        Debuger.Warn(ChessAgainst.IsConnected(s1,s2));

        //App.Manager.UI.ReplaceView("UIGamePanel");
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

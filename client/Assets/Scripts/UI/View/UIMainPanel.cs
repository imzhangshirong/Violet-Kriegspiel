using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Com.Violet.Rpc;
using Google.Protobuf;

public class UIMainPanel : UIViewBase
{
    public UILabel rpcResponse;
    public UIInput rpcInput;
	public override void OnInit()
	{
		
	}
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		Debuger.Log("MainPanel Open");
	}
	public void NextPage()
	{
        AppInterface.UIManager.ReplaceView("UIGamePanel");
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
        RpcNetwork.Request("Hello",request,delegate(IMessage response) {
            HelloResponse res = response as HelloResponse;
            rpcResponse.text = res.Greet;
            AppInterface.EventManager.Broadcast("_greeting", res.Greet);
        });
    }
    public void SIMPLE_TEST()
    {
        Common.UI.OpenAlert("Confirm", "Will Send String \""+ rpcInput.value + "\" To Server", "OK", Test, "Cancel", Nagetive, AlertWindowMode.TwoButton);
        //Debuger.Log("send string:" + rpcInput.value);
        //RpcNetwork.SendUnEncode(rpcInput.value);
    }
    void Test()
    {
        Common.UI.OpenAlert("Confirm", "RRRWill Send String \"" + rpcInput.value + "\" To Server", "OK", Test, "Cancel", Nagetive, AlertWindowMode.TwoButton);

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

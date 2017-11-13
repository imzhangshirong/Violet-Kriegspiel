using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;
using Com.Violet.Rpc;
public class UILoginPanel : UIViewBase
{
    public UIInput m_username;
    public UIInput m_password;
    public override void OnInit()
	{
		base.OnInit();
    }
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
	}

	public void LoginClick()
	{
        LoginRequest request = new LoginRequest();
        request.UserName = m_username.value;
        request.Password = m_password.value;
        App.Manager.UI.OpenView("UIWaitingPanel");
        App.Manager.Network.Request<LoginRequest>("Login", request, delegate (IMessage response)
        {
            App.Manager.UI.CloseView("UIWaitingPanel");
            App.Package.Player.Init(response);
            App.Manager.UI.ReplaceView("UILobbyPanel");
        });
	}
}

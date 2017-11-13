using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;
using Com.Violet.Rpc;
public class UILobbyPanel : UIViewBase
{
	public override void OnInit()
	{
		base.OnInit();
        App.Manager.UI.OpenView("UIPlayerInfo");
	}
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
	}

	public void FindEnemyClick(){
		FindEnemyRequest request = new FindEnemyRequest();
		App.Manager.Network.Request("FindEnemy",request,delegate(IMessage responseData){
			FindEnemyResponse response = (FindEnemyResponse)responseData;
			if(response.JoinGameField){
				App.Manager.UI.ReplaceView("UIFindEnemyPanel");
			}
		});
	}
	public void HistoryModeClick(){
		Common.UI.OpenAlert("提示", "还没有开放此功能，敬请期待！", "好的", null);
	}
	public void ChooseRoomClick(){
		Common.UI.OpenAlert("提示", "还没有开放此功能，敬请期待！", "好的", null);
	}
}

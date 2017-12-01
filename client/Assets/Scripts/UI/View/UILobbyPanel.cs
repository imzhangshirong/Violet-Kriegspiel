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
        //CheckGameState();

    }

	public override void OnRefresh(){
		//CheckGameState();
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

    public void CheckGameState()
    {
        CheckGameStateRequest request = new CheckGameStateRequest();
        App.Manager.Network.Request("CheckGameState", request, delegate (IMessage responseData) {
            CheckGameStateResponse response = (CheckGameStateResponse)responseData;
            if (response.RoomId != "")
            {
                Common.UI.OpenTips("正在恢复战场...");
                EnterBattleFieldRequest nRequest = new EnterBattleFieldRequest();
                nRequest.RoomId = response.RoomId;
				App.Package.ChessGame.RoomId = response.RoomId;
                App.Manager.Network.Request("EnterBattleField", nRequest, delegate (IMessage nResponseData) {
                    Common.UI.OpenTips("战斗还在继续，刻不容缓!");
                    EnterBattleFieldResponse nResponse = (EnterBattleFieldResponse)nResponseData;
                    App.Package.ChessGame.Recover(nResponse);
                    App.Manager.UI.ReplaceView("UIGamePanel");
                });
            }
        });
    }
}

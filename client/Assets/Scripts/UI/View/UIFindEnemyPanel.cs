using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;
using Com.Violet.Rpc;

public class UIFindEnemyPanel : UIViewBase
{
	public UIWTimer timer;
	public override void OnInit()
	{
		base.OnInit();
		BindEvent("_findEnemyTimeUp",OnFindEnemyTimeUp);
		BindEvent("#NET_EnterBattleField",OnEnterBattleField);
	}
	public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
		timer.enabled = true;
		timer.SetRemainTime(45);
	}
	public override void OnRefresh()
	{
		base.OnRefresh();
	}
	public void CancelFindClick(){
		CancelFindEnemyRequest request = new CancelFindEnemyRequest();
		App.Manager.Network.Request("CancelFindEnemy",request,delegate(IMessage responseData){
			CancelFindEnemyResponse response = (CancelFindEnemyResponse)responseData;
			if(response.IsCancel){
				Common.UI.OpenTips("已经取消匹配");
				Common.UI.BackPage();
			}
		});
	}

	public void OnFindEnemyTimeUp(object content){
		Common.UI.OpenTips("匹配超时，重新匹配");
		timer.SetRemainTime(45);
	}

	public void OnEnterBattleField(object content){
		App.Package.ChessGame.Init(content);
		Common.UI.OpenTips("成功匹配到对手！准备吧");
		App.Manager.UI.ReplaceView("UIGamePanel");
	}
}

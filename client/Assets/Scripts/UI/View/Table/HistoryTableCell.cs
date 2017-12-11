using UnityEngine;
using Com.Violet.Rpc;
public class HistoryTableCell : NTableCell
{
    public UILabel itemLable;
    HistoryStep stepData;
    public override void OnDraw(int index, object data)
    {
        stepData = (HistoryStep)data;
        string belongMe = App.Package.Player.playerInfo.ZoneId + "/" + App.Package.Player.playerInfo.UserId;
        bool me = true;
        PlayerInfo enemy = null;
        for(int i = 0; i < App.Package.ChessGame.EnemyPlayerList.Count; i++)
        {
            PlayerInfo player = App.Package.ChessGame.EnemyPlayerList[i];
            string belong = player.ZoneId + "/" + player.UserId;
            if (belong == stepData.Source.Belong)
            {
                enemy = player;
                me = false;
                break;
            }
            if (belong == stepData.Target.Belong)
            {
                enemy = player;
                break;
            }
        }
        string item = "第" + (stepData.Counter + 1) + "回合:";
        if (me)
        {
            item += "我方 [11B0FF]" + ChessAgainst.ChessHeroNameDefine[stepData.Source.ChessType] + "[-] ";
            if(stepData.Target.ChessRemoteId == 0)
            {
                item += "走子";
            }
            else
            {
                if(stepData.Result == (int)ChessMoveResult.WIN)
                {
                    item += "[FFC300]胜利";
                }
                else
                {
                    item += "[999999]阵亡";
                }
            }
        }
        else
        {
            if (stepData.Target.ChessRemoteId == 0)
            {
                item += "敌方走子";
            }
            else
            {
                if (stepData.Result == (int)ChessMoveResult.WIN)
                {
                    item += "敌方走子,我方 [11B0FF]" + ChessAgainst.ChessHeroNameDefine[stepData.Target.ChessType] + "[-] [FFC300]胜利";
                }
                else
                {
                    item += "敌方走子,我方 [11B0FF]" + ChessAgainst.ChessHeroNameDefine[stepData.Target.ChessType] + "[-] [999999]阵亡";
                }
            }
        }
        itemLable.text = item;

    }
    void OnClick()
    {

    }
}
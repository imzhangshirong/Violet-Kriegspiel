using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIWChessItem : UIWidgetBase
{
    public int chessId;
    private void Start()
    {
        
    }
    public void OnClick()
    {
        if (ChessGamePackage.Instance.IsGameStart)//游戏开始了之后才能走
        {
            Intent intent = new Intent();
            intent.Push("id", chessId);
            intent.Push("gameObject", gameObject);
            Push("_chessClick", intent);
        }
        else
        {
            Common.UI.OpenTips("比赛还没开始哦，不要心急");
        }
        
    }
    
}

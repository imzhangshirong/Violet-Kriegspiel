using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Violet.Rpc;
public class UIWChessHistory : UIWidgetBase
{
    public NTableView table;
    public UILabel MyChessNum;
    public UILabel EnemyChessNum;
    private void Start()
    {

    }

    private void OnEnable()
    {
        table.SetData(App.Package.ChessGame.ChessHistorySteps,true);
        table.ResetPosition();
        List<int> ids = App.Package.ChessGame.ChessDataIds;
        int my = 0;
        int enemy = 0;
        for (int i=0;i< App.Package.ChessGame.ChessDataIds.Count; i++)
        {
            ChessHeroData chess = App.Package.ChessGame.GetChessHeroDataById(App.Package.ChessGame.ChessDataIds[i]);
            if(chess.state == ChessHeroState.Alive)
            {
                if (chess.group == ChessHeroGroup.Myself)
                {
                    my++;
                }
                else
                {
                    enemy++;
                }
            }
        }
        MyChessNum.text = my.ToString();
        EnemyChessNum.text = enemy.ToString();
    }
}

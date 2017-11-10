using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIWChessPlayerState : UIWidgetBase
{
    public UILabel userName;
    public UILabel userLevel;
    public GameObject readyState;
    public GameObject unReadyState;
    public GameObject roundState;
    public UIWTimer roundTimer;

    public ChessHeroGroup group;

    private void Awake()
    {
        BindEvent("_" + group.ToString() + "_updateState", OnUpdateState);
    }

    void OnUpdateState(object content)
    {
        ChessPlayerData playerData = (ChessPlayerData)content;
        readyState.SetActive(false);
        unReadyState.SetActive(false);
        roundState.SetActive(false);
        roundTimer.remainTime = playerData.remainTime;
        switch (playerData.state)
        {
            case ChessPlayerState.UnReady:
                unReadyState.SetActive(true);
                break;
            case ChessPlayerState.Ready:
                readyState.SetActive(true);
                break;
            case ChessPlayerState.Gaming:
                roundState.SetActive(true);
                break;
        }
        userName.text = playerData.playerInfo.userName;
        userLevel.text = "lv." + playerData.playerInfo.level;
    }
}
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
    public TweenAlpha roundEffect;
    
    

    public void UpdateState(ChessPlayerData playerData)
    {
        if (readyState) readyState.SetActive(false);
        if (unReadyState) unReadyState.SetActive(false);
        if (roundState) roundState.SetActive(false);
        if (roundEffect) roundEffect.enabled = false;
        switch (playerData.state)
        {
            case ChessPlayerState.UnReady:
                if (unReadyState) unReadyState.SetActive(true);
                break;
            case ChessPlayerState.Ready:
                if (readyState) readyState.SetActive(true);
                break;
            case ChessPlayerState.Gaming:
                if (roundState)
                {
                    roundState.SetActive(true);
                    roundTimer.SetRemainTime(playerData.remainTime);
                }
                if (roundEffect)
                {
                    roundEffect.enabled = true;
                    roundEffect.ResetToBeginning();
                    roundEffect.PlayForward();
                }
                break;
        }
        
        userName.text = playerData.playerInfo.userName;
        userLevel.text = "Lv." + playerData.playerInfo.level;
    }
}
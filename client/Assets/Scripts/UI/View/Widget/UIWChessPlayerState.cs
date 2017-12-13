using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Violet.Rpc;
public class UIWChessPlayerState : UIWidgetBase
{
    public UILabel userName;
    public UILabel userLevel;
    public GameObject readyState;
    public GameObject unReadyState;
    public GameObject roundState;
    public UIWTimer readyTimer;
    public UIWTimer roundTimer;
    public GameObject roundEffect;
    
    

    public void UpdateState(PlayerInfo playerData)
    {
        if (readyState) readyState.SetActive(false);
        if (unReadyState) unReadyState.SetActive(false);
        if (roundState) roundState.SetActive(false);
        if (roundEffect) roundEffect.SetActive(false);
        switch ((PlayerState)playerData.State)
        {
            case PlayerState.UNREADY:
                if (unReadyState)
                {
                    unReadyState.SetActive(true);
                    readyTimer.SetRemainTime(playerData.GameRemainTime);
                }
                break;
            case PlayerState.READY:
                if (readyState) readyState.SetActive(true);
                break;
            case PlayerState.GAMING:
                if (playerData.GameRemainTime > 0)
                {
                    if (roundState)
                    {
                        roundState.SetActive(true);
                        roundTimer.SetRemainTime(playerData.GameRemainTime);
                        Debugger.Log("roundTime:" + playerData.GameRemainTime);
                    }
                    if (roundEffect) roundEffect.SetActive(true);
                }
                break;
        }
        
        userName.text = playerData.UserName;
        userLevel.text = "Lv." + playerData.Level;
    }
}
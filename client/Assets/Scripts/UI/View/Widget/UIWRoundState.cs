using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Violet.Rpc;
public class UIWRoundState : UIWidgetBase
{
    public UILabel chessName;
    public UILabel chessState;
	public GameObject win;
	public GameObject lose;
    public override void OnInit()
	{
		base.OnInit();
        
    }
    public override void OnOpen(Intent intent)
	{
		base.OnOpen(intent);
	}

	public void SetState(int chessType,bool result){
        StopAllCoroutines();
		win.SetActive(result);
		lose.SetActive(!result);
        gameObject.SetActive(true);
		chessName.text = ChessAgainst.ChessHeroNameDefine[chessType];
		chessState.text = (result)?"我方棋子胜利！":"我方棋子阵亡";
        StartCoroutine(DelayClose());

	}
    IEnumerator DelayClose()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    void OnClick()
    {
        gameObject.SetActive(false);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIWChessHeroItem : UIWidgetBase
{
    public UILabel name;
    public UISprite light;
    public UISprite normal;
    public GameObject dragState;
    public GameObject chooseState;
    public GameObject willWin;
    public GameObject willLose;
    public GameObject willTie;

    [HideInInspector]
    public int chessHeroId;//棋子类型id
    [HideInInspector]
    public int chessId;
    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;
    [HideInInspector]
    public ChessHeroState state = ChessHeroState.Died;
    [HideInInspector]
    public ChessHeroLabelState labelState = ChessHeroLabelState.Hide;
    [HideInInspector]
    public bool isChoosed = false;
    

    private void Start()
    {
        BindEvent("_chessHeroChoosed", ChessHeroSetToNormal);
    }
    public void OnClick()
    {
        
        Intent intent = new Intent();
        intent.Push("id", chessId);
        intent.Push("gameObject", gameObject);
        Push("_chessHeroClick", intent);
    }

    public void OnDragStart()
    {
        dragState.SetActive(true);
        name.depth += 10;
        light.depth += 10;
        normal.depth += 10;
        Push("_chessHeroChoosed", -1);
    }

    public void OnDrag(Vector2 delta)
    {

    }

    public void OnDragEnd()
    {
        dragState.SetActive(false);
        name.depth -= 10;
        light.depth -= 10;
        normal.depth -= 10;
    }

    public void OnDragOver()
    {
        chooseState.SetActive(true);
    }

    public void OnDragOut()
    {
        chooseState.SetActive(false);
    }

    public void OnDrop(GameObject go)
    {
        if (ChessGamePackage.Instance.CanDragChess)
        {
            UIWChessHeroItem moveUI = go.GetComponent<UIWChessHeroItem>();
            if(ChessGamePackage.Instance.GetChessGroupById(moveUI.chessId) == ChessHeroGroup.Myself && ChessGamePackage.Instance.GetChessGroupById(chessId) == ChessHeroGroup.Myself) //只有自己的才可以拖拽
            {
                Intent intent = new Intent();
                intent.Push("move", moveUI);
                intent.Push("place", this);
                Push("_chessExchange", intent);
            }
            else
            {
                Common.UI.OpenTips("不能互换敌方棋子");
            }
            
        }
        else
        {
            if(ChessGamePackage.Instance.IsReadyGame && !ChessGamePackage.Instance.IsGameStart) Common.UI.OpenTips("已经准备就绪，无法调整棋子");
        }
        
    }

    private void ChessHeroSetToNormal(object content)
    {
        int id = (int)content;
        isChoosed = (id == chessId);
        UpdateView();
    }
    public void UpdateView()
    {
        if (state == ChessHeroState.Died)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        if (labelState == ChessHeroLabelState.Hide || chessHeroId<0)
        {
            name.gameObject.SetActive(false);
        }
        else
        {
            name.gameObject.SetActive(true);
            name.text = ChessAgainst.ChessHeroNameDefine[chessHeroId];
        }
        if (isChoosed)
        {
            chooseState.SetActive(true);
        }
        else
        {
            chooseState.SetActive(false);
        }
        if (ChessGamePackage.Instance.GetChessGroupById(chessId) == ChessHeroGroup.Myself)
        {
            normal.color = new Color(86 / 255f, 156 / 255f, 214 / 255f);
            light.color = new Color(105 / 255f, 249 / 255f, 255 / 255f);
        }
        else
        {
            normal.color = new Color(255 / 255f, 94 / 255f, 75 / 255f);
            light.color = new Color(255 / 255f, 244 / 255f, 197 / 255f);

        }
    }
    private void OnEnable()
    {
        
        UpdateView();
    }

}
public enum ChessHeroState
{
    Alive = 0,
    Died = 1,
}
public enum ChessHeroLabelState
{
    Hide = 0,
    Show = 1,
}

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
    public GameObject chooseState;

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
        Push("_chessHeroChoosed", true);
        isChoosed = true;
        UpdateView();
    }
    private void ChessHeroSetToNormal(object content)
    {
        isChoosed = false;
        UpdateView();
    }
    private void UpdateView()
    {
        if (state == ChessHeroState.Died)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        if (labelState == ChessHeroLabelState.Hide)
        {
            name.gameObject.SetActive(false);
        }
        else
        {
            name.gameObject.SetActive(true);
            name.text = ChessAgainst.ChessHeroNameDefine[chessId % 100 % ChessAgainst.ChessHeroNameDefine.Length];
        }
        if (isChoosed)
        {
            light.gameObject.SetActive(true);
        }
        else
        {
            light.gameObject.SetActive(false);
        }
        if (chessId < 100)
        {
            normal.color = new Color(86 / 255f, 156 / 255f, 214 / 255f);
            light.color = new Color(105 / 255f, 249 / 255f, 255 / 255f);
        }
        else
        {
            normal.color = new Color(215 / 255f, 85 / 255f, 63 / 255f);
            light.color = new Color(255 / 255f, 183 / 255f, 105 / 255f);

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

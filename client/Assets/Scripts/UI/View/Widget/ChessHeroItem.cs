using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChessHeroItem : UIWidgetBase
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
    public ChessHeroState state = ChessHeroState.Hide;
    [HideInInspector]
    public bool isChoosed = false;
    public static string[] NameDefine = { "地雷", "炸弹", "工兵", "排长", "连长", "营长", "团长", "旅长", "师长", "军长", "司令", "军旗" };
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
        if (state == ChessHeroState.Hide)
        {
            name.gameObject.SetActive(false);
        }
        else
        {
            name.gameObject.SetActive(true);
            name.text = NameDefine[chessId % 100 % NameDefine.Length];
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
    Show = 0,
    Hide = 1,
    Label = 2,
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIGamePanel : UIViewBase
{
    public GameObject m_MyselfFied;
    public GameObject m_EnemyFied;

    private GameObject m_ChessHero;
    public override void OnInit()
    {
        base.OnInit();
        AppInterface.UIManager.HideOverViewByPage("UITopTest");

        //加载
        m_ChessHero = AppInterface.ResourceManager.LoadUI("Game/ChessHero");

        //注册消息事件
        BindEvent("_chessClick", OnChessClick);
        BindEvent("_chessHeroClick", OnChessHeroClick);
    }
    public override void OnOpen(Intent intent)
    {
        base.OnOpen(intent);
        Debuger.Log("GamePanel Open");
        PutChessHero(0);
        PutChessHero(1);
        ChessGamePackage.Instance.Init(null);
    }
    public void BackPage()
    {
        AppInterface.UIManager.PageBack();
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
        Debuger.Log("GamePanel Refresh");
    }
    private void OnChessClick(object content)
    {
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        
        ChessPoint point = new ChessPoint(id % 5, id / 5);
        Debuger.Warn("ChessItem Click:" + id + "|" +ChessGamePackage.Instance.GetFeildRoadStationByPoint(point).type);
        if (ChessGamePackage.Instance.MyselfChooseChessId > -1 && ChessGamePackage.Instance.MyselfChooseChessId < 100)
        {
            ChessHeroData heroChoosed = ChessGamePackage.Instance.GetChessHeroDataById(ChessGamePackage.Instance.MyselfChooseChessId);
            ChessMoveData moveData = ChessAgainst.ChessHeroCanMoveTo(heroChoosed, point);
            if(moveData.crashType > 0)
            {
                Debuger.Warn("ChessHeroItem Cant MoveTo " + point.ToString() + " : " + moveData.crashType);
            }
            else
            {
                string re = "";
                for(int i = 0; i < moveData.points.Length; i++)
                {
                    if (i > 0) re += "->";
                    re += moveData.points[i].ToString();
                }
                heroChoosed.point = point;
                if (point.y / 6 > 0)
                {
                    heroChoosed.gameObject.transform.parent = m_EnemyFied.transform;
                }
                else
                {
                    heroChoosed.gameObject.transform.parent = m_MyselfFied.transform;
                }
                heroChoosed.gameObject.transform.localScale = Vector3.one;
                heroChoosed.gameObject.transform.localPosition = GetChessLocalPosition(heroChoosed.point);
                Debuger.Warn("ChessHeroItem MoveTo " + point.ToString() + ":" + re);
            }
        }

    }

    private void OnChessHeroClick(object content)
    {
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        ChessHeroData hero = ChessGamePackage.Instance.GetChessHeroDataById(id);
        if (ChessGamePackage.Instance.MyselfChooseChessId > -1 && ChessGamePackage.Instance.MyselfChooseChessId < 100 && id >= 100)
        {
            ChessHeroData heroChoosed = ChessGamePackage.Instance.GetChessHeroDataById(ChessGamePackage.Instance.MyselfChooseChessId);
            ChessMoveData moveData = ChessAgainst.ChessHeroCanMoveTo(heroChoosed, hero.point);
            if (moveData.crashType > 0)
            {
                Debuger.Warn("ChessHeroItem Cant MoveTo " + hero.point.ToString() + " : " + moveData.crashType);
            }
            else
            {
                string re = "";
                for (int i = 0; i < moveData.points.Length; i++)
                {
                    if (i > 0) re += "->";
                    re += moveData.points[i].ToString();
                }
                Debuger.Warn("ChessHeroItem MoveToBeat " + hero.point.ToString() + ":" + re);
            }
        }
        else
        {
            Debuger.Warn("ChessHeroItem Click:" + id + " @" + hero.point.ToString());
        }
        ChessGamePackage.Instance.MyselfChooseChessId = id;
    }

    private void PutChessHero(int type)
    {
        TreeRoot treeRoot = GetComponent<TreeRoot>();
        int baseId = type * 100;
        for(int i = 0; i < 30; i++)
        {
            if(!ChessAgainst.IsBarrack(type,i))
            {
                GameObject go = Instantiate(m_ChessHero);
                
                
                

                ChessHeroData heroData = new ChessHeroData();
                heroData.id = baseId + i;
                heroData.remoteId = heroData.id;//服务器给
                heroData.heroTypeId = heroData.id % 12;
                int ids = type * 30 + i;
                heroData.point = new ChessPoint(ids % 5, ids / 5);
                ChessGamePackage.Instance.AddChessToMap(heroData);


                UIWChessHeroItem chessHeroItem = go.GetComponent<UIWChessHeroItem>();
                chessHeroItem.treeRoot = treeRoot;
                treeRoot.Bind(chessHeroItem);//绑定到TreeRoot
                chessHeroItem.chessId = baseId + i;
                switch (type)
                {
                    case 0:
                        go.transform.parent = m_MyselfFied.transform;
                        chessHeroItem.state = ChessHeroState.Alive;
                        chessHeroItem.labelState = ChessHeroLabelState.Show;
                        break;
                    case 1:
                        go.transform.parent = m_EnemyFied.transform;
                        chessHeroItem.state = ChessHeroState.Alive;
                        chessHeroItem.labelState = ChessHeroLabelState.Hide;
                        break;
                }
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = GetChessLocalPosition(i % 5, i / 5);
                go.SetActive(true);
                heroData.gameObject = go;
                ChessGamePackage.Instance.AddChessToMap(heroData);

            }
        }
    }
    public Vector3 GetChessLocalPosition(ChessPoint point)
    {
        return GetChessLocalPosition(point.x, point.y);
    }
    public Vector3 GetChessLocalPosition(int x,int y)
    {
        return new Vector3(-256 + x * 128, y % 6 * 72, 0);
    }
}

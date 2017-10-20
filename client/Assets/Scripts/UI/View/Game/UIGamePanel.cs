using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIGamePanel : UIViewBase
{
    public GameObject m_MyselfFied;
    public GameObject m_EnemyFied;

    private bool m_MyChessIsMoving;
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
        BindEvent("_chessFieldClick", OnChessFieldClick);
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
    private void OnChessClick(object content)//移动
    {
        if (m_MyChessIsMoving) return;//正在移动
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        
        ChessPoint point = new ChessPoint(id % 5, id / 5);
        Debuger.Warn("ChessItem Click:" + id +"|"+ ChessGamePackage.Instance.MyselfChooseChessId + "|" +ChessGamePackage.Instance.GetFeildRoadStationByPoint(point).type);
        if (ChessGamePackage.Instance.MyselfChooseChessId > -1 && ChessGamePackage.Instance.MyselfChooseChessId < 100)
        {
            ChessHeroData heroChoosed = ChessGamePackage.Instance.GetChessHeroDataById(ChessGamePackage.Instance.MyselfChooseChessId);
            StartCoroutine(TweenMoveChess(heroChoosed,point));
        }
    }

    private void OnChessHeroClick(object content)//吃子移动
    {
        if (m_MyChessIsMoving) return;//正在移动
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        ChessHeroData hero = ChessGamePackage.Instance.GetChessHeroDataById(id);
        Push("_chessHeroChoosed", id);

        //明棋模式提醒
        if (id < 100)
        {
            for (int i = 0; i < ChessGamePackage.Instance.ChessDataIds.Count; i++)
            {
                int chessId = ChessGamePackage.Instance.ChessDataIds[i];
                if (chessId < 100) continue;
                ChessHeroData heroPre = ChessGamePackage.Instance.GetChessHeroDataById(chessId);
                UIWChessHeroItem uiChess = heroPre.gameObject.GetComponent<UIWChessHeroItem>();
                uiChess.willLose.SetActive(false);
                uiChess.willWin.SetActive(false);
                uiChess.willTie.SetActive(false);
                if (heroPre.heroTypeId > -1 && hero.heroTypeId > 0 && hero.heroTypeId < 11)
                {
                    int result = ChessAgainst.ChessCanBeat(hero, heroPre);

                    switch (result)
                    {
                        case -1:
                            uiChess.willLose.SetActive(true);
                            break;
                        case 0:
                            uiChess.willTie.SetActive(true);
                            break;
                        case 1:
                        case 2:
                            uiChess.willWin.SetActive(true);
                            break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < ChessGamePackage.Instance.ChessDataIds.Count; i++)
            {
                int chessId = ChessGamePackage.Instance.ChessDataIds[i];
                if (chessId < 100) continue;
                ChessHeroData heroPre = ChessGamePackage.Instance.GetChessHeroDataById(chessId);
                UIWChessHeroItem uiChess = heroPre.gameObject.GetComponent<UIWChessHeroItem>();
                uiChess.willLose.SetActive(false);
                uiChess.willWin.SetActive(false);
                uiChess.willTie.SetActive(false);
            }
        }


        if (ChessGamePackage.Instance.MyselfChooseChessId < 100)
        {
            if(ChessGamePackage.Instance.MyselfChooseChessId > -1 && id >= 100)//之前有选择自己,当前非己方的棋
            {
                ChessHeroData heroChoosed = ChessGamePackage.Instance.GetChessHeroDataById(ChessGamePackage.Instance.MyselfChooseChessId);
                StartCoroutine(TweenMoveChessAndBeat(heroChoosed, hero));
            }
            else
            {
                ChessGamePackage.Instance.MyselfChooseChessId = id;
            }
        }
        else
        {
            
            ChessGamePackage.Instance.MyselfChooseChessId = id;
            Debuger.Warn("ChessHeroItem Click:" + id + " @" + hero.point.ToString());
        }
    }

    private void ChessMoveTo(ChessHeroData hero, ChessPoint point)
    {
        if (point.y / 6 > 0)
        {
            hero.gameObject.transform.parent = m_EnemyFied.transform;
        }
        else
        {
            hero.gameObject.transform.parent = m_MyselfFied.transform;
        }
        hero.point = point;
        hero.gameObject.transform.localScale = Vector3.one;
        hero.gameObject.transform.localPosition = GetChessLocalPosition(hero.point);
    }

    IEnumerator TweenMoveChess(ChessHeroData heroChoosed, ChessPoint moveToPoint)
    {
        m_MyChessIsMoving = true;
        ChessMoveData moveData = ChessAgainst.ChessHeroCanMoveTo(heroChoosed, moveToPoint);

        if (moveData.crashType > 0)
        {
            Common.UI.OpenTips("嗷！走不过去啊！");
            Debuger.Warn("ChessHeroItem Cant MoveTo " + moveToPoint.ToString() + " : " + moveData.crashType);
        }
        else
        {
            float dur = 0.2f / moveData.points.Length;
            for (int i = 0; i < moveData.points.Length; i++)
            {
                ChessMoveTo(heroChoosed, moveData.points[i]);
                yield return new WaitForSeconds(dur);
                
            }
            heroChoosed.point = moveToPoint;
            Debuger.Warn("ChessHeroItem MoveTo " + moveToPoint.ToString());
        }
        m_MyChessIsMoving = false;
    }

    IEnumerator TweenMoveChessAndBeat(ChessHeroData heroChoosed, ChessHeroData hero)
    {
        m_MyChessIsMoving = true;
        ChessMoveData moveData = ChessAgainst.ChessHeroCanMoveTo(heroChoosed, hero.point);
        if (moveData.crashType > 0)
        {
            ChessGamePackage.Instance.MyselfChooseChessId = hero.id;
            Debuger.Warn("ChessHeroItem Cant MoveTo " + hero.point.ToString() + " : " + moveData.crashType);
        }
        else
        {
            float dur = 0.2f / moveData.points.Length;
            for (int i = 0; i < moveData.points.Length - 1; i++)
            {
                ChessMoveTo(heroChoosed, moveData.points[i]);
                yield return new WaitForSeconds(dur);
            }
            int result = ChessAgainst.ChessCanBeat(heroChoosed, hero);
            switch (result)
            {
                case -1:
                    heroChoosed.state = ChessHeroState.Died;
                    heroChoosed.gameObject.SetActive(false);
                    break;
                case 0:
                    heroChoosed.state = ChessHeroState.Died;
                    heroChoosed.gameObject.SetActive(false);
                    hero.state = ChessHeroState.Died;
                    hero.gameObject.SetActive(false);
                    break;
                case 1:
                    hero.state = ChessHeroState.Died;
                    hero.gameObject.SetActive(false);
                    break;
                case 2: //胜利
                    hero.state = ChessHeroState.Died;
                    hero.gameObject.SetActive(false);
                    break;
            }
            ChessMoveTo(heroChoosed, moveData.points[moveData.points.Length - 1]);
            if (result > 0)
            {
                //UIWChessHeroItem uiChess = heroChoosed.gameObject.GetComponent<UIWChessHeroItem>();
                //uiChess.isChoosed = true;
                //uiChess.UpdateView();
            }
            else
            {
                ChessGamePackage.Instance.MyselfChooseChessId = -1;
            }
            Debuger.Warn("ChessHeroItem MoveToBeat " + hero.point.ToString() + " Beat:" + result);
        }
        m_MyChessIsMoving = false;
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
                chessHeroItem.chessHeroId = heroData.heroTypeId;
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
                        chessHeroItem.labelState = ChessHeroLabelState.Show;//
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
    public void OnChessFieldClick(object content)
    {
        Push("_chessHeroChoosed", -1);
        for (int i = 0; i < ChessGamePackage.Instance.ChessDataIds.Count; i++)
        {
            ChessHeroData heroPre = ChessGamePackage.Instance.GetChessHeroDataById(ChessGamePackage.Instance.ChessDataIds[i]);
            UIWChessHeroItem uiChess = heroPre.gameObject.GetComponent<UIWChessHeroItem>();
            uiChess.willLose.SetActive(false);
            uiChess.willWin.SetActive(false);
            uiChess.willTie.SetActive(false);
        }
        ChessGamePackage.Instance.MyselfChooseChessId = -1;
    }
}

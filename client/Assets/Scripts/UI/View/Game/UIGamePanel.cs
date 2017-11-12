using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Violet.Rpc;

public class UIGamePanel : UIViewBase
{
    public GameObject m_MyselfFied;
    public GameObject m_EnemyFied;
    public GameObject m_ReadyButton;
    public GameObject m_SurrenderButton;
    public GameObject m_ChatButton;
    public UIWChessPlayerState m_EnemyState;
    public UIWChessPlayerState m_MyselfState;


    private bool m_MyChessIsMoving;
    private GameObject m_ChessHero;
    public override void OnInit()
    {
        base.OnInit();
        App.Manager.UI.HideOverViewByPage("UITopTest");

        //加载
        m_ChessHero = App.Manager.Resource.LoadUI("Game/ChessHero");

        //注册消息事件
        BindEvent("_chessClick", OnChessClick);
        BindEvent("_chessHeroClick", OnChessHeroClick);
        BindEvent("_chessFieldClick", OnChessFieldClick);
        BindEvent("_chessExchange", OnChessExchange);
        BindEvent("_readyTimeUp", delegate (object content) {
            OnReadyClick();
        });

        BindEvent("#NET_PlayerStateChage",OnReceivPlayerStateChage);
    }

    

    public override void OnOpen(Intent intent)
    {
        base.OnOpen(intent);
        Debuger.Log("GamePanel Open");
        PlayerPackage.Instance.Init(null);
        ChessGamePackage.Instance.Init(null);
        ChessGamePackage.Instance.AddChessFromData("6|1,3|2,0|3,11|4,0|5;10|6,2|7,5|8,0|9,3|10;9|11,,2|12,,4|13;1|14,5|15,,6|16,7|17;7|18,,4|19,,1|20;8|21,2|22,4|23,3|24,8|25;", ChessHeroGroup.Myself);
        ChessGamePackage.Instance.AddChessFromData("6|1,3|2,0|3,-1|4,0|5;10|6,2|7,5|8,0|9,3|10;9|11,,2|12,,4|13;1|14,5|15,,6|16,7|17;7|18,,4|19,,1|20;8|21,2|22,4|23,3|24,8|25;", ChessHeroGroup.Enemy);
        InitChessHero(ChessHeroGroup.Myself);
        InitChessHero(ChessHeroGroup.Enemy);
        m_MyselfState.UpdateState(ChessGamePackage.Instance.MyselfPlayerData);
        m_EnemyState.UpdateState(ChessGamePackage.Instance.EnemyPlayerData);

    }
    public void BackPage()
    {
        App.Manager.UI.PageBack();
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
        Debuger.Log("GamePanel Refresh");
    }

    private void OnChessExchange(object content)
    {
        Intent intent = (Intent)content;
        UIWChessHeroItem moveUI = (UIWChessHeroItem)intent.Value("move");
        UIWChessHeroItem placeUI = (UIWChessHeroItem)intent.Value("place");
        ChessHeroData move = ChessGamePackage.Instance.GetChessHeroDataById(moveUI.chessId);
        ChessHeroData place = ChessGamePackage.Instance.GetChessHeroDataById(placeUI.chessId);
        if (ChessGamePackage.Instance.CanDragChess)
        {
            if (ChessGamePackage.Instance.GetChessGroupById(move.id) == ChessHeroGroup.Myself && ChessGamePackage.Instance.GetChessGroupById(place.id) == ChessHeroGroup.Myself) //只有自己的才可以拖拽
            {
                if((move.heroTypeId == 11 && !ChessAgainst.IsStronghold(place.point)) || (place.heroTypeId == 11 && !ChessAgainst.IsStronghold(move.point)))
                {
                    Common.UI.OpenTips("军旗只能在大本营中哦！");
                    return;
                }
                if((move.heroTypeId == 0 && !ChessAgainst.IsAfterCamp(place.point)) || (place.heroTypeId == 0 && !ChessAgainst.IsAfterCamp(move.point)))
                {
                    Common.UI.OpenTips("地雷只能在后2排哦！");
                    return;
                }
                ChessPoint tpoint = move.point;
                ChessMoveTo(move, place.point);
                ChessMoveTo(place, tpoint);
            }
            else
            {
                Common.UI.OpenTips("不能互换敌方棋子");
            }

        }
        else
        {
            if (ChessGamePackage.Instance.IsReadyGame && !ChessGamePackage.Instance.IsGameStart) Common.UI.OpenTips("已经准备就绪，无法调整棋子");
        }
    }


    private void OnChessClick(object content)//移动
    {
        if (m_MyChessIsMoving) return;//正在移动
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        ChessHeroData chessHero = ChessGamePackage.Instance.GetChessHeroDataById(id);
        ChessPoint point = chessHero.point;
        //Debuger.Warn("ChessItem Click:" + id +"|"+ ChessGamePackage.Instance.MyselfChooseChessId + "|" +ChessGamePackage.Instance.GetFeildRoadStationByPoint(point).type);
        if (ChessGamePackage.Instance.MyselfChooseChessId > -1 && ChessGamePackage.Instance.GetChessGroupById(ChessGamePackage.Instance.MyselfChooseChessId) == ChessHeroGroup.Myself)
        {
            if (ChessGamePackage.Instance.IsGameStart)//游戏开始了之后才能走
            {
                ChessHeroData heroChoosed = ChessGamePackage.Instance.GetChessHeroDataById(ChessGamePackage.Instance.MyselfChooseChessId);
                StartCoroutine(TweenMoveChess(heroChoosed, point));
            }
            else
            {
                Common.UI.OpenTips("比赛还没开始哦，不要心急");
            }
        }
    }

    private void OnChessHeroClick(object content)//吃子移动
    {
        if (m_MyChessIsMoving) return;//正在移动
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        ChessHeroData hero = ChessGamePackage.Instance.GetChessHeroDataById(id);
        ChessHeroGroup group = ChessGamePackage.Instance.GetChessGroupById(id);
        Push("_chessHeroChoosed", id);

        //明棋模式提醒
        if (group == ChessHeroGroup.Myself)
        {
            for (int i = 0; i < ChessGamePackage.Instance.ChessDataIds.Count; i++)
            {
                int chessId = ChessGamePackage.Instance.ChessDataIds[i];
                if (ChessGamePackage.Instance.GetChessGroupById(id) == ChessHeroGroup.Myself) continue;
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
                if (ChessGamePackage.Instance.GetChessGroupById(id) == ChessHeroGroup.Myself) continue;
                ChessHeroData heroPre = ChessGamePackage.Instance.GetChessHeroDataById(chessId);
                UIWChessHeroItem uiChess = heroPre.gameObject.GetComponent<UIWChessHeroItem>();
                uiChess.willLose.SetActive(false);
                uiChess.willWin.SetActive(false);
                uiChess.willTie.SetActive(false);
            }
        }


        if (ChessGamePackage.Instance.GetChessGroupById(ChessGamePackage.Instance.MyselfChooseChessId) == ChessHeroGroup.Myself)
        {
            if(ChessGamePackage.Instance.MyselfChooseChessId > -1 && group != ChessHeroGroup.Myself)//之前有选择自己,当前非己方的棋
            {
                if (ChessGamePackage.Instance.IsGameStart)//游戏开始了之后才能走并吃子
                {
                    ChessHeroData heroChoosed = ChessGamePackage.Instance.GetChessHeroDataById(ChessGamePackage.Instance.MyselfChooseChessId);
                    StartCoroutine(TweenMoveChessAndBeat(heroChoosed, hero));
                }
                else
                {
                    Common.UI.OpenTips("比赛还没开始哦，不要心急");
                    ChessGamePackage.Instance.MyselfChooseChessId = id;
                }
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
        Debuger.Warn(ChessGamePackage.Instance.GetChessStringData(ChessHeroGroup.Myself));
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
                //ChessGamePackage.Instance.MyselfChooseChessId = -1;
            }
            ChessGamePackage.Instance.MyselfChooseChessId = -1;
            Debuger.Warn("ChessHeroItem MoveToBeat " + hero.point.ToString() + " Beat:" + result);
        }
        m_MyChessIsMoving = false;
    }
    /// <summary>
    /// 初始化棋盘放置
    /// </summary>
    private void InitChessHero(ChessHeroGroup group)
    {
        TreeRoot treeRoot = GetComponent<TreeRoot>();
        List<ChessHeroData> chessHeroDatas = ChessGamePackage.Instance.GetChessHeroList(group);
        for (int i = 0; i < chessHeroDatas.Count; i++)
        {
            ChessHeroData heroData = chessHeroDatas[i];
            if (!ChessAgainst.IsBarrack(heroData.point))//不在军营里
            {
                GameObject go = Instantiate(m_ChessHero);
                UIWChessHeroItem chessHeroItem = go.GetComponent<UIWChessHeroItem>();
                chessHeroItem.treeRoot = treeRoot;
                chessHeroItem.chessHeroId = heroData.heroTypeId;
                treeRoot.Bind(chessHeroItem);//绑定到TreeRoot
                chessHeroItem.chessId = heroData.id;
                switch (group)
                {
                    case ChessHeroGroup.Myself:
                        go.transform.parent = m_MyselfFied.transform;
                        chessHeroItem.state = ChessHeroState.Alive;
                        chessHeroItem.labelState = ChessHeroLabelState.Show;
                        break;
                    case ChessHeroGroup.Enemy:
                        go.transform.parent = m_EnemyFied.transform;
                        chessHeroItem.state = ChessHeroState.Alive;
                        chessHeroItem.labelState = (heroData.heroTypeId >= 0)? ChessHeroLabelState.Show: ChessHeroLabelState.Hide;//小于0为未知棋子，应该隐藏
                        break;
                }
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = GetChessLocalPosition(heroData.point);
                go.SetActive(true);
                heroData.gameObject = go;

            }
            else
            {
                Debuger.Error("初始化棋子位置错误！");
            }
        }
    }

    Vector3 GetChessLocalPosition(ChessPoint point)
    {
        return GetChessLocalPosition(point.x, point.y);
    }
    Vector3 GetChessLocalPosition(int x, int y)
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

    /// <summary>
    /// 布子结束，准备
    /// </summary>
    public void OnReadyClick()
    {
        ChessGamePackage.Instance.MyselfPlayerData.state = ChessPlayerState.Ready;
        m_MyselfState.UpdateState(ChessGamePackage.Instance.MyselfPlayerData);
    }

    /// <summary>
    /// 聊天语句选择
    /// </summary>
    public void OnChatClick()
    {

    }
    /// <summary>
    /// 投降
    /// </summary>
    public void OnSurrenderClick()
    {
        Common.UI.OpenAlert("提示", "游戏还在继续，确认投降吗？",
            "确认", delegate (){
                Debuger.Log(1111);
            },
            "不服", delegate () {
                Debuger.Log(222);
            },AlertWindowMode.TwoButton);
    }



    

    /// <summary>
    /// 收到聊天
    /// </summary>
    void OnReceiveChatMsg()
    {

    }
    /// <summary>
    /// 走子情况
    /// </summary>
    void OnReceiveChessMove()
    {

    }
    /// <summary>
    /// 同步棋盘和比赛信息，在网络不稳定或者重连的时候触发
    /// </summary>
    void OnReceiveChessMapChange()
    {

    }

    void OnReceiveGameStart()
    {

    }

    void OnReceivPlayerStateChage(object data)
    {
        Debuger.Warn("enemyState");
        
    }
}

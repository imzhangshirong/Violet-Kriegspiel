using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Google.Protobuf;
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
    public UIWRoundState m_roundOver;

    public GameObject m_FireGameStart;
    public GameObject m_ChatMsgInput;

    public GameObject m_MyselfMsgShow;
    public GameObject m_EnemyMsgShow;


    private bool m_MyChessIsMoving;
    private GameObject m_ChessHero;
    static string[] MsgContent = {
        "快点吧，等的我花都谢了",
        "投降吧！我赢定了！",
        "我看你这是在为难我胖虎:-)",
        "Emmm...",
    };
    public override void OnInit()
    {
        base.OnInit();
        App.Manager.UI.HideAllOverViewByPage();

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

        BindEvent("_roundTimeUp", delegate (object content) {
            //SkipRound();
        });
        BindEvent("_enemyRoundTimeUp", delegate (object content) {
            //NextGameRound();
        });

        //注册响应Push事件
        BindEvent("#NET_PlayerStateChange",OnReceivPlayerStateChange);
        BindEvent("#NET_GameStateChange",OnReceivGameStateChange);
        BindEvent("#NET_ChessMove", OnReceiveChessMove);
        BindEvent("#NET_ChatMessage", OnReceiveChatMsg);

        //消息列表
        BindEvent("_input_msg", delegate (object content) {
            GameObject go = (GameObject)content;
            NClickEvent eclick = go.GetComponent<NClickEvent>();
            SendMsg(MsgContent[eclick.identify]);
            m_ChatMsgInput.SetActive(false);
        });
    }

    

    public override void OnOpen(Intent intent)
    {
        base.OnOpen(intent);
        Debuger.Log("GamePanel Open");

        m_EnemyFied.transform.DestroyChildren();
        m_MyselfFied.transform.DestroyChildren();

        if (App.Package.ChessGame.IsGameStart)
        {
            //恢复之前的游戏
            StartGameRound();
        }
        else
        {
            App.Package.ChessGame.AddChessFromData(App.Package.ChessGame.MyselfChessSetting);
            App.Package.ChessGame.AddChessFromData(
                App.Package.ChessGame.ParseChessDataFromString(ChessAgainst.DefaultUnknowChess, "0/0")
            );
            App.Package.Player.playerInfo.GameRemainTime = Config.Game.WaitingReady;
        }
        UpdateChessMap();
        UpdatePlayer();

    }

    public override void OnClose()
    {
        base.OnClose();
        m_EnemyFied.transform.DestroyChildren();
        m_MyselfFied.transform.DestroyChildren();
    }

    public void UpdateChessMap()
    {
        InitChessHero(ChessHeroGroup.Myself);
        InitChessHero(ChessHeroGroup.Enemy);
    }

    void UpdatePlayer(ChessHeroGroup group){
        switch(group){
            case ChessHeroGroup.Myself:{
                m_MyselfState.UpdateState(App.Package.Player.playerInfo);
                break;
            }
            case ChessHeroGroup.Enemy:{
                m_EnemyState.UpdateState(App.Package.ChessGame.EnemyPlayerList[0]);
                break;
            }
        }
    }
    void UpdatePlayer(){
        m_MyselfState.UpdateState(App.Package.Player.playerInfo);
        m_EnemyState.UpdateState(App.Package.ChessGame.EnemyPlayerList[0]);
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
        Debuger.Log("GamePanel Refresh");
    }

    private void OnChessExchange(object content)
    {
        
        if (App.Package.ChessGame.CanDragChess)
        {
            Intent intent = (Intent)content;
            UIWChessHeroItem moveUI = (UIWChessHeroItem)intent.Value("move");
            UIWChessHeroItem placeUI = (UIWChessHeroItem)intent.Value("place");
            ChessHeroData move = App.Package.ChessGame.GetChessHeroDataById(moveUI.chessId);
            ChessHeroData place = App.Package.ChessGame.GetChessHeroDataById(placeUI.chessId);
            if (move.group == ChessHeroGroup.Myself && place.group == ChessHeroGroup.Myself) //只有自己的才可以拖拽
            {
                if(move.heroTypeId == 1 && ChessAgainst.IsFirstRow(place.point))
                {
                    Common.UI.OpenTips("炸弹不可以放第一排哦！");
                    return;
                }
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
            if (App.Package.ChessGame.IsReadyGame && !App.Package.ChessGame.IsGameStart) Common.UI.OpenTips("已经准备就绪，无法调整棋子");
        }
    }


    private void OnChessClick(object content)//移动
    {
        if (App.Package.ChessGame.IsGameStart)//游戏开始了之后才能走
        {
            if (!App.Package.ChessGame.IsMyRound)
            {
                Common.UI.OpenTips("敌方回合，无法行动");
                return;
            }
            if (m_MyChessIsMoving) return;//正在移动
            Intent intent = (Intent)content;
            ChessPoint point = (ChessPoint)intent.Value("point");
            GameObject go = (GameObject)intent.Value("gameObject");
            //Debuger.Warn("ChessItem Click:" + id +"|"+ App.Package.ChessGame.MyselfChooseChessId + "|" +App.Package.ChessGame.GetFeildRoadStationByPoint(point).type);
            if (App.Package.ChessGame.MyselfChooseChessId > -1)
            {
                ChessHeroData heroChoosed = App.Package.ChessGame.GetChessHeroDataById(App.Package.ChessGame.MyselfChooseChessId);
                if (heroChoosed.group == ChessHeroGroup.Myself)
                {
                    if (ChessAgainst.IsStronghold(heroChoosed.point))
                        Common.UI.OpenTips("大本营的棋子无法移动！");
                    else
                    {
                        ChessMoveData moveData = ChessAgainst.ChessHeroCanMoveTo(heroChoosed, point);
                        if (moveData.crashType > 0)
                        {
                            if(moveData.crashType == 1)
                            {
                                switch (moveData.crashHero.heroTypeId)
                                {
                                    case 0:
                                        Common.UI.OpenTips("地雷无法移动！");
                                        break;
                                    case 11:
                                        Common.UI.OpenTips("军旗无法移动！");
                                        break;
                                    default:
                                        Common.UI.OpenTips("嗷！走不过去啊！");
                                        break;
                                }
                            }
                            else
                            {
                                Common.UI.OpenTips("嗷！走不过去啊！");
                            }
                        }
                        else
                        {
                            RequestToMove(heroChoosed, point,null, moveData);
                        }
                        
                    }
                }
            }
        }
        else
        {
            if (App.Package.ChessGame.IsReadyGame)
            {
                Common.UI.OpenTips("比赛还没开始哦，不要心急");
            }
            else
            {
                Common.UI.OpenTips("赶紧布兵吧！你还没有准备呢\n长按拖动可以交换棋子！");
            }
        }
    }

    private void OnChessHeroClick(object content)//吃子移动
    {
        if (App.Package.ChessGame.IsGameStart)
        {
            if (!App.Package.ChessGame.IsMyRound)
            {
                Common.UI.OpenTips("敌方回合，无法行动");
                return;
            }
            if (m_MyChessIsMoving) return;//正在移动
            Intent intent = (Intent)content;
            int id = (int)intent.Value("id");
            GameObject go = (GameObject)intent.Value("gameObject");
            ChessHeroData hero = App.Package.ChessGame.GetChessHeroDataById(id);
            ChessHeroGroup group = hero.group;
            Push("_chessHeroChoosed", id);

            //明棋模式提醒
            if (group == ChessHeroGroup.Myself)
            {

                for (int i = 0; i < App.Package.ChessGame.ChessDataIds.Count; i++)
                {
                    int chessId = App.Package.ChessGame.ChessDataIds[i];
                    ChessHeroData heroPre = App.Package.ChessGame.GetChessHeroDataById(chessId);
                    if (heroPre.group == ChessHeroGroup.Myself) continue;

                    UIWChessHeroItem uiChess = heroPre.gameObject.GetComponent<UIWChessHeroItem>();
                    uiChess.willLose.SetActive(false);
                    uiChess.willWin.SetActive(false);
                    uiChess.willTie.SetActive(false);
                    if (heroPre.heroTypeId > -1 && ChessAgainst.ChessHeroCanMove(hero))
                    {
                        ChessMoveResult result = ChessAgainst.ChessCanBeat(hero, heroPre);

                        switch (result)
                        {
                            case ChessMoveResult.LOSE:
                                uiChess.willLose.SetActive(true);
                                break;
                            case ChessMoveResult.TIE:
                                uiChess.willTie.SetActive(true);
                                break;
                            case ChessMoveResult.WIN:
                                uiChess.willWin.SetActive(true);
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < App.Package.ChessGame.ChessDataIds.Count; i++)
                {
                    int chessId = App.Package.ChessGame.ChessDataIds[i];
                    ChessHeroData heroPre = App.Package.ChessGame.GetChessHeroDataById(chessId);
                    if (heroPre.group == ChessHeroGroup.Myself) continue;

                    UIWChessHeroItem uiChess = heroPre.gameObject.GetComponent<UIWChessHeroItem>();
                    uiChess.willLose.SetActive(false);
                    uiChess.willWin.SetActive(false);
                    uiChess.willTie.SetActive(false);
                }
            }
            
                
            if (App.Package.ChessGame.MyselfChooseChessId > -1)
            {
                ChessHeroData heroChoosed = App.Package.ChessGame.GetChessHeroDataById(App.Package.ChessGame.MyselfChooseChessId);
                if (heroChoosed.group == ChessHeroGroup.Myself && group != ChessHeroGroup.Myself)//之前有选择自己,当前非己方的棋
                {
                    if (App.Package.ChessGame.IsGameStart)//游戏开始了之后才能走并吃子
                    {
                        Debuger.Log(hero.point);
                        if (ChessAgainst.IsBarrack(hero.point) && hero.point.y>5)//敌方的军营
                        {
                            Common.UI.OpenTips("敌军在行营里，不要浪啊~");
                        }
                        else
                        {
                            if (ChessAgainst.IsStronghold(heroChoosed.point))
                            {
                                Common.UI.OpenTips("大本营的棋子无法移动！");
                            }
                            else
                            {
                                ChessMoveData moveData = ChessAgainst.ChessHeroCanMoveTo(heroChoosed, hero.point);
                                if (moveData.crashType > 0)
                                {
                                    if (moveData.crashType == 1)
                                    {
                                        switch (moveData.crashHero.heroTypeId)
                                        {
                                            case 0:
                                                Common.UI.OpenTips("地雷无法移动！");
                                                break;
                                            case 11:
                                                Common.UI.OpenTips("军旗无法移动！");
                                                break;
                                            default:
                                                Common.UI.OpenTips("嗷！走不过去啊！");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Common.UI.OpenTips("嗷！走不过去啊！");
                                    }
                                }
                                else
                                {
                                    RequestToMove(heroChoosed, hero.point, hero, moveData);
                                }
                            }
                        }
                    }
                }
                
            }
            App.Package.ChessGame.MyselfChooseChessId = id;
        }
        else
        {
            if (App.Package.ChessGame.IsReadyGame)
            {
                Common.UI.OpenTips("比赛还没开始哦，不要心急");
            }
            else
            {
                Common.UI.OpenTips("赶紧布兵吧！你还没有准备呢\n长按拖动可以交换棋子！");
            }
            
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
        hero.point = point.Clone();
        hero.gameObject.transform.localScale = Vector3.one;
        hero.gameObject.transform.localPosition = GetChessLocalPosition(hero.point);
    }

    void RequestToMove(ChessHeroData heroChoosed, ChessPoint moveToPoint, ChessHeroData hero, ChessMoveData moveData) {
        m_MyChessIsMoving = true;
        MoveChessRequest request = new MoveChessRequest();
        request.Source = ChessHeroDataToChessData(App.Package.Player.playerInfo, heroChoosed);
        if (hero != null)
        {
            request.Target = ChessHeroDataToChessData(App.Package.ChessGame.EnemyPlayerList[0], hero);//多人的时候要修改
        }
        else
        {
            request.Target = new ChessData();
            request.Target.Point = new Com.Violet.Rpc.ChessPoint();
            request.Target.Point.X = moveToPoint.x;
            request.Target.Point.Y = moveToPoint.y;
        }
        App.Manager.Network.Request("MoveChess", request, delegate (IMessage responseData) {
            MoveChessResponse response = (MoveChessResponse)responseData;
            int result = response.ChessMoveResult;
            //强制回合同步
            App.Package.ChessGame.GameRoundCounter = response.Counter - 1;
            if (hero == null)
            {
                StartCoroutine(TweenMoveChess(heroChoosed, moveToPoint,(ChessMoveResult)result, moveData));
            }
            else
            {
                StartCoroutine(TweenMoveChessAndBeat(heroChoosed, hero, (ChessMoveResult)result, moveData));
            }
            if (response.Counter == App.Package.ChessGame.GameRoundCounter + 1)
            {
                NextGameRound();
            }
            else
            {
                ChessDataHasProblem();
            }
        },true,false,false);
    }

    ChessData ChessHeroDataToChessData(PlayerInfo playerInfo,ChessHeroData chessHero)
    {
        ChessData chessData = new ChessData();
        chessData.ChessRemoteId = chessHero.remoteId;
        chessData.Belong = playerInfo.ZoneId + "/" + playerInfo.UserId;
        chessData.Group = (int)chessHero.group;
        chessData.Point = new Com.Violet.Rpc.ChessPoint();
        chessData.Point.X = chessHero.point.x;
        chessData.Point.Y = chessHero.point.y;
        return chessData;
    }

    void SkipRound()
    {
        MoveChessRequest request = new MoveChessRequest();
        request.Source = new ChessData();
        request.Source.ChessRemoteId = -1;
        App.Manager.Network.Request("MoveChess", request, delegate (IMessage responseData) {
            MoveChessResponse response = (MoveChessResponse)responseData;
            //强制同步
            App.Package.ChessGame.GameRoundCounter = response.Counter - 1;
            if (response.Counter == App.Package.ChessGame.GameRoundCounter + 1)
            {
                NextGameRound();
            }
            else
            {
                ChessDataHasProblem();
            }
        }, true, false, false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chessData"></param>
    /// <param name="forceTrans">是否强制坐标转换</param>
    /// <returns></returns>
    ChessHeroData ChessDataToChessHeroData(ChessData chessData,bool forceTrans = false)
    {
        ChessHeroData chess = new ChessHeroData();
        
        chess.group = App.Package.ChessGame.GetGroup(chessData.Belong);
        chess.heroTypeId = chessData.ChessType;
        chess.realChessType = chess.heroTypeId;
        chess.remoteId = chessData.ChessRemoteId;
        chess.point = new ChessPoint(chessData.Point.X,chessData.Point.Y);
        chess.belong = chessData.Belong;
        if (chess.group != ChessHeroGroup.Myself || forceTrans)
        {
            chess.point.x = 4 - chess.point.x;
            chess.point.y = 11 - chess.point.y;
        }
        return chess;
    }

    void ForceExitGame()
    {
        Common.UI.BackPage("UILobbyPanel");
    }

    void ChessDataHasProblem()
    {
        Debuger.Error("ChessDataErro!!!! counter:"+App.Package.ChessGame.GameRoundCounter);
        Common.UI.OpenAlert("错误", "数据异常！", "确定", delegate () {
            ForceExitGame();
        });
    }

    IEnumerator TweenMoveChess(ChessHeroData heroChoosed, ChessPoint moveToPoint,ChessMoveResult result,ChessMoveData moveData)
    {
        m_MyChessIsMoving = true;
        //if((ChessMoveResult.CANNOT_MOVE == result && moveData.crashType > 0) || (ChessMoveResult.CAN_MOVE == result && moveData.crashType == 0))
        //{
            float dur = 0.2f / moveData.points.Length;
            for (int i = 0; i < moveData.points.Length; i++)
            {
                ChessMoveTo(heroChoosed, moveData.points[i]);
                yield return new WaitForSeconds(dur);

            }
            heroChoosed.point = moveToPoint;
            m_MyChessIsMoving = false;
        //}
        /*else
        {
            ChessDataHasProblem();
        }*/
    }

    IEnumerator TweenMoveChessAndBeat(ChessHeroData heroChoosed, ChessHeroData hero, ChessMoveResult resultR, ChessMoveData moveData)
    {
        m_MyChessIsMoving = true;
        if((resultR == ChessMoveResult.CANNOT_MOVE && moveData.crashType <= 0) || (resultR == ChessMoveResult.CAN_MOVE && moveData.crashType > 0))
        {
            ChessDataHasProblem();
        }
        else
        {
            if (moveData.crashType > 0)
            {
                App.Package.ChessGame.MyselfChooseChessId = hero.id;
                Debuger.Warn("ChessHeroItem Cant MoveTo " + hero.point.ToString() + " : " + moveData.crashType);
                m_MyChessIsMoving = false;
            }
            else
            {
                ChessMoveResult result = resultR;//ChessAgainst.ChessCanBeat(heroChoosed, hero);
                float dur = 0.2f / moveData.points.Length;
                for (int i = 0; i < moveData.points.Length - 1; i++)
                {
                    ChessMoveTo(heroChoosed, moveData.points[i]);
                    yield return new WaitForSeconds(dur);
                }
                switch (result)
                {
                    case ChessMoveResult.LOSE:
                        heroChoosed.state = ChessHeroState.Died;
                        heroChoosed.gameObject.SetActive(false);
                        break;
                    case ChessMoveResult.TIE:
                        heroChoosed.state = ChessHeroState.Died;
                        heroChoosed.gameObject.SetActive(false);
                        hero.state = ChessHeroState.Died;
                        hero.gameObject.SetActive(false);
                        break;
                    case ChessMoveResult.WIN:
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
                    //App.Package.ChessGame.MyselfChooseChessId = -1;
                }
                int cid = -1;
                bool resultS;
                if(heroChoosed.group == ChessHeroGroup.Myself)
                {
                    cid = heroChoosed.realChessType;
                    resultS = result == ChessMoveResult.WIN;
                    if (cid > 0 && cid < 11 && cid != 1)
                    {
                        m_roundOver.SetState(cid, resultS);
                    }
                }
                else
                {
                    cid = hero.realChessType;
                    resultS = result != ChessMoveResult.WIN;
                    if (cid > 0 && cid < 11 && cid != 1)
                    {
                        if(cid == heroChoosed.realChessType || heroChoosed.realChessType == 1)//同归于尽
                        {
                            m_roundOver.SetState(cid, false);
                        }
                        else
                            m_roundOver.SetState(cid, resultS);
                    }
                }
                
                App.Package.ChessGame.MyselfChooseChessId = heroChoosed.id;
                Push("_chessHeroChoosed", heroChoosed.id);
                m_MyChessIsMoving = false;
            }
        }
    }
    /// <summary>
    /// 初始化棋盘放置
    /// </summary>
    private void InitChessHero(ChessHeroGroup group)
    {
        TreeRoot treeRoot = GetComponent<TreeRoot>();
        List<ChessHeroData> chessHeroDatas = App.Package.ChessGame.GetChessHeroList(group);
        for (int i = 0; i < chessHeroDatas.Count; i++)
        {
            ChessHeroData heroData = chessHeroDatas[i];
            if (!ChessAgainst.IsBarrack(heroData.point))//不在军营里
            {
                GameObject go = Instantiate(m_ChessHero);
                UIWChessHeroItem chessHeroItem = go.GetComponent<UIWChessHeroItem>();
                chessHeroItem.heroData = heroData;
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
        for (int i = 0; i < App.Package.ChessGame.ChessDataIds.Count; i++)
        {
            ChessHeroData heroPre = App.Package.ChessGame.GetChessHeroDataById(App.Package.ChessGame.ChessDataIds[i]);
            UIWChessHeroItem uiChess = heroPre.gameObject.GetComponent<UIWChessHeroItem>();
            uiChess.willLose.SetActive(false);
            uiChess.willWin.SetActive(false);
            uiChess.willTie.SetActive(false);
        }
        App.Package.ChessGame.MyselfChooseChessId = -1;
    }

    void AddPlayerIntoGame()
    {
        App.Package.ChessGame.AllPlayerList.Clear();
        App.Package.Player.playerInfo.State = (int)PlayerState.GAMING;
        App.Package.ChessGame.AllPlayerList.Add(App.Package.Player.playerInfo);
        for (int i=0;i< App.Package.ChessGame.EnemyPlayerList.Count; i++)
        {
            App.Package.ChessGame.EnemyPlayerList[i].State = (int)PlayerState.GAMING;
            App.Package.ChessGame.AllPlayerList.Add(App.Package.ChessGame.EnemyPlayerList[i]);
        }
        App.Package.ChessGame.AllPlayerList.Sort(SortPlayerListByRoundOrder);
    }

    int SortPlayerListByRoundOrder(PlayerInfo p1, PlayerInfo p2)
    {
        return p1.RoundOrder - p2.RoundOrder;
    }

    void StartGameRound()
    {
        App.Package.ChessGame.GameRoundCounter--;
        NextGameRound();
    }

    void NextGameRound(int roundTime = 0)
    {
        App.Package.ChessGame.GameRoundCounter++;
        int cur = App.Package.ChessGame.GameRoundCounter % App.Package.ChessGame.AllPlayerList.Count;
        for (int i = 0; i < App.Package.ChessGame.AllPlayerList.Count; i++)
        {
            if (i != cur)
            {
                App.Package.ChessGame.AllPlayerList[i].GameRemainTime = 0;
            }
            else
            {
                if (App.Package.ChessGame.AllPlayerList[i].GameRemainTime == 0)
                {
                    App.Package.ChessGame.AllPlayerList[i].GameRemainTime = (roundTime == 0) ? Config.Game.WaitingRound : roundTime;
                }
            }
        }
        UpdatePlayer();
    }
    void GameStart(){
        App.Package.ChessGame.Start();
        AddPlayerIntoGame();
        StartGameRound();
        m_FireGameStart.SetActive(true);
        StartCoroutine(HideFireState());
    }
    
    IEnumerator HideFireState(){
        yield return new WaitForSeconds(1.5f);
        m_FireGameStart.SetActive(false);
        Common.UI.OpenTips("赶紧布兵吧！长按拖动可以交换棋子！");
    }

    void GameEnd(bool result){
        if (result)
        {
            Common.UI.OpenAlert("胜利", "恭喜！你赢了", "确定", () => {
                ForceExitGame();
            });
            AudioSource.PlayClipAtPoint(App.Manager.Resource.Load<AudioClip>("Sound/round_win"),Vector3.zero);
        }
        else
        {
            Common.UI.OpenAlert("失败", "对面可真强。。。啊", "确定", () => {
                ForceExitGame();
            });
            AudioSource.PlayClipAtPoint(App.Manager.Resource.Load<AudioClip>("Sound/round_lose"), Vector3.zero);
        }
        App.Package.ChessGame.RoomId = "";//清除RoomId
    }

    /// <summary>
    /// 布子结束，准备
    /// </summary>
    public void OnReadyClick()
    {
        ReadyInRoomRequest request = new ReadyInRoomRequest();
        request.IsReady = true;
        List<ChessData> chessDatas = App.Package.ChessGame.GetChessData(ChessHeroGroup.Myself);
        for(int i = 0; i < chessDatas.Count; i++)
        {
            request.ChessMap.Add(chessDatas[i]);
        }
        App.Manager.Network.Request("ReadyInRoom", request, delegate (IMessage responseData) {
            ReadyInRoomResponse response = (ReadyInRoomResponse)responseData;
            if (response.IsChangeState)
            {
                App.Package.Player.playerInfo.State = (int)PlayerState.READY;
                UpdatePlayer(ChessHeroGroup.Myself);
                App.Package.ChessGame.Ready();
            }
        });
    }

    void ShowMsg(MessageData msg)
    {
        GameObject goShow;
        if(msg.UserId == App.Package.Player.playerInfo.UserId && msg.ZoneId == App.Package.Player.playerInfo.ZoneId)
        {
            goShow = m_MyselfMsgShow;
        }
        else
        {
            goShow = m_EnemyMsgShow;
        }
        goShow.transform.Find("Label").GetComponent<UILabel>().text = msg.Content;
        goShow.SetActive(true);
        TweenAlpha eff = goShow.GetComponent<TweenAlpha>();
        eff.StopAllCoroutines();
        eff.ResetToBeginning();
        eff.PlayForward();
        eff.StartCoroutine(HideMsg(goShow,eff));
    }

    IEnumerator HideMsg(GameObject msgShow, TweenAlpha eff)
    {
        yield return new WaitForSeconds(2.7f);
        eff.PlayReverse();
        yield return new WaitForSeconds(0.3f);
        msgShow.SetActive(false);
    }

    void SendMsg(string msg)
    {
        SendChatMessageRequest request = new SendChatMessageRequest();
        request.Msg = new MessageData();
        request.Msg.Content = msg;
        request.Msg.UserId = App.Package.Player.playerInfo.UserId;
        request.Msg.UserName = App.Package.Player.playerInfo.UserName;
        request.Msg.Level = App.Package.Player.playerInfo.Level;
        request.Msg.ZoneId = App.Package.Player.playerInfo.ZoneId;
        App.Manager.Network.Request("SendChatMessage", request, delegate (IMessage responseData) {
            SendChatMessageResponse response = (SendChatMessageResponse)responseData;
        },true,false,false);
    }

    /// <summary>
    /// 聊天语句选择
    /// </summary>
    public void OnChatClick()
    {
        m_ChatMsgInput.SetActive(true);
    }
    /// <summary>
    /// 投降
    /// </summary>
    public void OnSurrenderClick()
    {
        Common.UI.OpenAlert("提示", "游戏还在继续，确认投降吗？",
            "确认", delegate (){

                ConfirmSurrender();
            },
            "不服", null);
    }


    void ConfirmSurrender()
    {
        SurrenderRequest request = new SurrenderRequest();
        App.Manager.Network.Request("Surrender", request, delegate (IMessage responseData) {
            SurrenderResponse response = (SurrenderResponse)responseData;
            if (response.IsSurrender)
            {
                Common.UI.OpenTips("发起投降");
            }
            
        });
    }
    

    /// <summary>
    /// 收到聊天
    /// </summary>
    void OnReceiveChatMsg(object data)
    {
        ChatMessagePush push = (ChatMessagePush)data;
        ShowMsg(push.Msg);
    }
    /// <summary>
    /// 走子情况
    /// </summary>
    void OnReceiveChessMove(object data)
    {
        ChessMovePush push = (ChessMovePush)data;
        ChessData source = push.Source;
        ChessData target = push.Target;
        ChessMoveResult result = (ChessMoveResult)push.ChessMoveResult;
        ChessHeroData sourceReal = App.Package.ChessGame.GetChessHeroDataByRemoteId(source.ChessRemoteId);
        //强制回合同步
        App.Package.ChessGame.GameRoundCounter = push.Counter - 1;
        if(target!=null){
            ChessHeroData targetReal = App.Package.ChessGame.GetChessHeroDataByRemoteId(target.ChessRemoteId);
            
            if (sourceReal==null || (targetReal==null && result != ChessMoveResult.CAN_MOVE && result != ChessMoveResult.CANNOT_MOVE) || push.Counter != App.Package.ChessGame.GameRoundCounter+1)
            {
                ChessDataHasProblem();
                return;
            }
            ChessHeroData fake = ChessDataToChessHeroData(target, true);
            if (targetReal != null)
            {
                fake = targetReal;
            }
            ChessMoveData moveData = ChessAgainst.ChessHeroCanMoveTo(sourceReal, fake.point);
            switch (result)
            {
                case ChessMoveResult.LOSE:
                    fake.heroTypeId = 12;
                    StartCoroutine(TweenMoveChessAndBeat(sourceReal, fake, result, moveData));
                    NextGameRound();
                    break;
                case ChessMoveResult.WIN:
                    fake.heroTypeId = -2;
                    StartCoroutine(TweenMoveChessAndBeat(sourceReal, fake, result, moveData));
                    NextGameRound();
                    break;
                case ChessMoveResult.TIE:
                    fake.heroTypeId = -3;
                    StartCoroutine(TweenMoveChessAndBeat(sourceReal, fake, result, moveData));
                    NextGameRound();
                    break;
                case ChessMoveResult.CAN_MOVE:
                    StartCoroutine(TweenMoveChess(sourceReal, fake.point, result, moveData));
                    NextGameRound();
                    break;
                case ChessMoveResult.CANNOT_MOVE:
                    break;

            }
        }
        else{
            NextGameRound();
            Debuger.Error("skip");
        }
        
        
    }
    /// <summary>
    /// 同步棋盘和比赛信息，在网络不稳定或者重连的时候触发
    /// </summary>
    void OnReceiveChessMapChange(object data)
    {

    }

    void OnReceivPlayerStateChange(object data)
    {
        PlayerStateChangePush push = (PlayerStateChangePush)data;
        Debuger.Log(push.PlayerInfo.UserName + (PlayerState)push.PlayerInfo.State);
        for (int i=0;i< App.Package.ChessGame.EnemyPlayerList.Count; i++)
        {
            PlayerInfo playerInfo = App.Package.ChessGame.EnemyPlayerList[i];
            if (playerInfo.ZoneId == push.PlayerInfo.ZoneId && playerInfo.UserId == push.PlayerInfo.UserId)
            {
                App.Package.ChessGame.EnemyPlayerList[i] = push.PlayerInfo;
                break;
            }
        }
        
        UpdatePlayer(ChessHeroGroup.Enemy);
        
    }
    void OnReceivGameStateChange(object data)
    {
        GameStateChangePush push = (GameStateChangePush) data;

        switch((GameState)push.State){
            case GameState.READYING:{
                break;
            }
            case GameState.START:{
                App.Package.ChessGame.RemoveAllChessHero();//清除所有的
                App.Package.ChessGame.AddChessFromData(new List<ChessData>(push.ChessMap));
                UpdateChessMap();//更新棋盘
                GameStart();
                break;
            }
            case GameState.END:{
                switch((GameResult)push.Result){
                    case GameResult.UNKNOW:{
                        break;
                    }
                    case GameResult.WIN:{
                        GameEnd(true);
                        break;
                    }
                    case GameResult.LOSE:{
                        GameEnd(false);
                        break;
                    }
                }
                break;
            }
        }
        Debuger.Warn("enemyState");
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class ChessAgainst : MonoBehaviour
{
    public static string[] ChessHeroNameDefine = { "地雷", "炸弹", "工兵", "排长", "连长", "营长", "团长", "旅长", "师长", "军长", "司令", "军旗" };
    public static int[] ChessHeroNumber = { 3, 2, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1 };
    public static string DefaultUnknowChess = "-1|0,-1|0,-1|0,-1|0,-1|0;-1|0,-1|0,-1|0,-1|0,-1|0;-1|0,,-1|0,,-1|0;-1|0,-1|0,,-1|0,-1|0;-1|0,,-1|0,,-1|0;-1|0,-1|0,-1|0,-1|0,-1|0;";
    public static ChessMoveData ChessHeroCanMoveTo(ChessHeroData heroData,ChessPoint point)
    {
        ChessMoveData moveData = new ChessMoveData();
        FieldRoadStation roadStationS = App.Package.ChessGame.GetFeildRoadStationByPoint(heroData.point);
        FieldRoadStation roadStationT = App.Package.ChessGame.GetFeildRoadStationByPoint(point);
        if (ChessHeroCanMove(heroData))//检测棋子本身，地雷、军旗不能走
        {
            //检测目的地是否禁止
            /*for(int i = 0; i < roadStationT.forbidChessHeros.Length; i++)
            {
                if(heroData.heroTypeId == roadStationT.forbidChessHeros[i])
                {
                    moveData.crashType = 2;
                    moveData.crashHero = heroData;
                    return moveData;
                }
            }*/
            
            ///
            for (int i = 0; i < roadStationS.connectedPointIds.Length; i++)
            {
                FieldRoadStation roadStation = App.Package.ChessGame.GetFeildRoadStationById(roadStationS.connectedPointIds[i]);
                if (roadStation == roadStationT)
                {
                    moveData.crashType = 0;
                    moveData.points = new ChessPoint[] { roadStationS.point, roadStationT.point };
                    return moveData;
                }
            }
            if (roadStationS.type == FieldRoadStationType.Rail && roadStationT.type == FieldRoadStationType.Rail)
            {
                if (heroData.heroTypeId == 2)//工兵行走
                {
                    List<FieldRoadPath> paths = new List<FieldRoadPath>();
                    List<int> usedStations = new List<int>();
                    usedStations.Add(roadStationS.id);
                    FieldRoadPath pathStart = new FieldRoadPath();
                    pathStart.pathStations.Add(roadStationS.id);
                    paths.Add(pathStart);
                    LookForRailWayPath(roadStationT, paths, usedStations, 1, 32);
                    for (int i = 0; i < paths.Count; i++)
                    {
                        FieldRoadPath path = paths[i];
                        if (path.pathStations[path.pathStations.Count - 1] != roadStationT.id) continue;
                        ChessPoint[] points = path.ToChessPoints();
                        ChessHeroData crashHero = HasChessHeroOnPathPoints(points);
                        if (crashHero == null)
                        {
                            moveData.crashType = 0;
                            moveData.crashHero = null;
                            moveData.points = points;
                            return moveData;
                        }
                        else
                        {
                            moveData.crashHero = heroData;
                        }
                    }
                    moveData.crashType = 3;
                    moveData.crashHero = heroData;
                    return moveData;

                }
                else if(roadStationS.point.x == roadStationT.point.x)
                {
                    ChessPoint[] points = new ChessPoint[Mathf.Abs(roadStationS.point.y- roadStationT.point.y)+1];
                    int d = (roadStationS.point.y < roadStationT.point.y) ? 1 : -1;
                    points[0] = roadStationS.point;
                    points[points.Length - 1] = roadStationT.point;
                    for (int i = 1; i < points.Length - 1; i++)
                    {
                        points[i] = new ChessPoint(roadStationS.point.x, roadStationS.point.y + d * i );
                    }
                    ChessHeroData crashHero = HasChessHeroOnPathPoints(points);
                    if (crashHero == null)
                    {
                        moveData.crashType = 0;
                        moveData.points = points;
                        
                    }
                    else
                    {
                        moveData.crashType = 3;
                        moveData.crashHero = crashHero;
                    }
                    return moveData;
                }
                else if (roadStationS.point.y == roadStationT.point.y)
                {
                    ChessPoint[] points = new ChessPoint[Mathf.Abs(roadStationS.point.x - roadStationT.point.x) + 1];
                    int d = (roadStationS.point.x < roadStationT.point.x) ? 1 : -1;
                    points[0] = roadStationS.point;
                    points[points.Length - 1] = roadStationT.point;
                    for (int i = 1; i < points.Length - 1; i++)
                    {
                        points[i] = new ChessPoint(roadStationS.point.x + d * i, roadStationS.point.y);
                    }
                    ChessHeroData crashHero = HasChessHeroOnPathPoints(points);
                    if (crashHero == null)
                    {
                        moveData.crashType = 0;
                        moveData.points = points;
                    }
                    else
                    {
                        moveData.crashType = 3;
                        moveData.crashHero = crashHero;
                    }
                    return moveData;
                }
                else
                {
                    //不能走直角
                }
            }
        }
        moveData.crashType = 1;
        moveData.crashHero = heroData;
        return moveData;

    }

    public static ChessHeroData HasChessHeroOnPathPoints(ChessPoint[] points)
    {
        for(int i =1;i< points.Length - 1; i++)
        {
            ChessHeroData heroData = App.Package.ChessGame.GetChessHeroDataByPoint(points[i]);
            if (heroData != null && heroData.state == ChessHeroState.Alive)
            {
                return heroData;
            }
        }
        return null;
    }

    public static ChessHeroData HasChessHeroOnPath(FieldRoadPath path)
    {
        return HasChessHeroOnPathPoints(path.ToChessPoints());
    }

    public static void LookForRailWayPath(FieldRoadStation roadStationT , List<FieldRoadPath> paths, List<int> usedStations,int currentR,int max)
    {
        if (paths.Count < 1) return;
        List<FieldRoadPath> pathsNext = new List<FieldRoadPath>();
        FieldRoadPath path;
        FieldRoadStation roadStationS;
        for (int i = paths.Count - 1; i >= 0; i--)
        {
            path = paths[i];
            if (path.pathStations.Count < currentR) break;
            int startId = path.pathStations[path.pathStations.Count - 1];
            if (startId == roadStationT.id) continue;//找到的不找了
            roadStationS = App.Package.ChessGame.GetFeildRoadStationById(startId);
            for (int j = 0; j < roadStationS.connectedPointIds.Length; j++)
            {
                int id = roadStationS.connectedPointIds[j];
                FieldRoadStation roadStationC = App.Package.ChessGame.GetFeildRoadStationById(id);
                if (roadStationC.type == FieldRoadStationType.Rail)
                {
                    if (path.pathStations.IndexOf(id) == -1)
                    {
                        FieldRoadPath pathNext = new FieldRoadPath(path);
                        pathNext.pathStations.Add(id);
                        pathsNext.Add(pathNext);
                    }
                }
            }
        }
        paths.AddRange(pathsNext);
        if (currentR + 1 <= max)
        {
            LookForRailWayPath(roadStationT, paths, usedStations, currentR + 1, max);
        }
    }

    public static bool ChessHeroCanMove(ChessHeroData heroData)
    {
        //地雷和军旗不能移动
        if(heroData.heroTypeId == 0 || heroData.heroTypeId == 11)
        {
            return false;
        }
        return IsStronghold(heroData.point)==false;//大本营不能走
        return true;
    }
    //单击训练模式
    public static ChessMoveResult ChessCanBeat(ChessHeroData heroS, ChessHeroData heroT) //1胜利，-1失败，0平局消失，2获胜结束，-2未知
    {
        if (heroT.heroTypeId == 12 || heroS.heroTypeId == -2) return ChessMoveResult.LOSE;
        if (heroS.heroTypeId == 12 || heroT.heroTypeId == -2) return ChessMoveResult.WIN;
        if (heroS.heroTypeId == -3 || heroT.heroTypeId == -3) return ChessMoveResult.TIE;
        if (heroT.heroTypeId < 0) return ChessMoveResult.UNKNOW;//未知
        if (heroT.heroTypeId == 1) return ChessMoveResult.TIE;//敌方炸弹
        if (heroS.heroTypeId == 1) return ChessMoveResult.TIE;//我方炸弹
        if (heroS.heroTypeId == 2)//我方工兵
        {
            if (heroT.heroTypeId == 0) return ChessMoveResult.WIN;//敌方地雷
        }
        if (heroT.heroTypeId == 0) return ChessMoveResult.TIE;//敌方地雷
        if (heroT.heroTypeId == 11) return ChessMoveResult.WIN;//敌方军旗
        if(heroT.heroTypeId > heroS.heroTypeId)
        {
            return ChessMoveResult.LOSE;
        }
        else if(heroT.heroTypeId < heroS.heroTypeId)
        {
            return ChessMoveResult.WIN;
        }
        else
        {
            return ChessMoveResult.TIE;
        }
    }
    public static bool IsBarrack(ChessPoint point)
    {
        int id = point.y * 5 + point.x;
        if (point.y <= 5)
        {
            
            if (id == 11 || id == 13 || id == 17 || id == 21 || id == 23) return true;
        }
        else if(point.y > 5)
        {
            if (id == 6 || id == 8 || id == 12 || id == 16 || id == 18) return true;
        }
        return false;
    }
    public static bool IsRailWay(ChessPoint pointS, ChessPoint pointT)
    {
        
        if(InRailArea(pointS) && InRailArea(pointT))
        {
            return true;
        }
        return false;
    }
    public static bool InRailArea(ChessPoint point)
    {
        if (point.x >= 0 && point.x <= 4 && point.y >= 1 && point.y <= 5)
        {
            
            if (point.x < 1 || point.x > 3 || point.y < 2 || point.y > 4)
            {
                return true;
            }
        }
        if (point.x >= 0 && point.x <= 4 && point.y >= 6 && point.y <= 10)
        {
            if (point.x < 1 || point.x > 3 || point.y < 7 || point.y > 9)
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsConnected(FieldRoadStation station1, FieldRoadStation station2)
    {
        if(station1.point.x == station2.point.x && (station1.point.x == 1 || station1.point.x == 3))
        {
            if((station1.point.y == 5 && station2.point.y == 6) || (station1.point.y == 6 && station2.point.y == 5))
            {
                Debuger.Warn(station1.point + "=>" + station2.point);
                return false;
            }
        }
        
        int s1 = Mathf.Abs(station1.point.x - station2.point.x);
        int s2 = Mathf.Abs(station1.point.y - station2.point.y);
        if (s1 > 1 || s2 > 1) return false;
        if (s1 + s2 > 1 && station1.type != FieldRoadStationType.Barrack && station2.type != FieldRoadStationType.Barrack) return false;
        return true;
    }
    /// <summary>
    /// 是否是大本营
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool IsStronghold(ChessPoint point)
    {
        if(point.x==1 || point.x == 3)
        {
            if (point.y == 0 || point.y == 11) return true;
        }
        return false;
    }
    /// <summary>
    /// 是否是后2排
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool IsAfterCamp(ChessPoint point)
    {
        if (point.y < 2 || point.y > 9 ) return true;
        return false;
    }
    /// <summary>
    /// 是否是第一排
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool IsFirstRow(ChessPoint point)
    {
        if (point.y == 5 || point.y == 6) return true;
        return false;
    }
    /// <summary>
    /// 检测棋子摆放是否合法
    /// </summary>
    /// <param name="heros"></param>
    /// <returns></returns>
    public static bool ChessIsLegal(List<ChessHeroData> heros)
    {
        int[] heroNumbers = new int[ChessHeroNumber.Length];
        for (int i = 0; i < heros.Count; i++)
        {
            ChessHeroData item = heros[i];
            heroNumbers[item.heroTypeId]++;
            switch (item.heroTypeId)
            {
                case 0://地雷
                    if (!IsAfterCamp(item.point)) return false;
                    break;
                case 11://军旗
                    if (!IsStronghold(item.point)) return false;
                    break;
                case 1://炸弹
                    if (IsFirstRow(item.point)) return false;
                    break;
            }
        }
        for(int i = 0; i < heroNumbers.Length; i++)
        {
            if (heroNumbers[i] > ChessHeroNumber[i]) return false;
        }
        return true;
    }

}
public enum PlayerState{
  UNKNOW = 0,
  UNREADY = 1,
  READY = 2,
  GAMING = 3,
  SURRENDER = 4,//投降
}

public enum GameState{
  READYING = 0,
  START = 1,
  END = 2,
}

public enum GameResult{
  UNKNOW = 0,
  LOSE = 1,
  WIN = 2,
}

public enum ChessMoveResult{
  UNKNOW = 0,
  LOSE = 1,
  TIE = 2,//同归于尽，平手
  WIN = 3,
  CAN_MOVE = 4,
  CANNOT_MOVE = 5,
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class ChessAgainst
{
    public static string[] ChessHeroNameDefine = { "地雷", "炸弹", "工兵", "排长", "连长", "营长", "团长", "旅长", "师长", "军长", "司令", "军旗" };
    public static ChessMoveData ChessHeroCanMoveTo(ChessHeroData heroData,ChessPoint point)
    {
        ChessMoveData moveData = new ChessMoveData();
        FieldRoadStation roadStationS = ChessGamePackage.Instance.GetFeildRoadStationByPoint(heroData.point);
        FieldRoadStation roadStationT = ChessGamePackage.Instance.GetFeildRoadStationByPoint(point);
        if (ChessHeroCanMove(heroData))//检测棋子本身，地雷、军旗不能走
        {
            //检测目的地是否禁止
            for(int i = 0; i < roadStationT.forbidChessHeros.Length; i++)
            {
                if(heroData.heroTypeId == roadStationT.forbidChessHeros[i])
                {
                    moveData.crashType = 2;
                    moveData.crashHero = heroData;
                    return moveData;
                }
            }
            ///
            for (int i = 0; i < roadStationS.connectedPointIds.Length; i++)
            {
                FieldRoadStation roadStation = ChessGamePackage.Instance.GetFeildRoadStationById(roadStationS.connectedPointIds[i]);
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
            ChessHeroData heroData = ChessGamePackage.Instance.GetChessHeroDataByPoint(points[i]);
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
            roadStationS = ChessGamePackage.Instance.GetFeildRoadStationById(startId);
            for (int j = 0; j < roadStationS.connectedPointIds.Length; j++)
            {
                int id = roadStationS.connectedPointIds[j];
                FieldRoadStation roadStationC = ChessGamePackage.Instance.GetFeildRoadStationById(id);
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
        return true;
    }
    //单击训练模式
    public static int ChessCanBeat(ChessHeroData heroS, ChessHeroData heroT) //1胜利，-1失败，0平局消失，2获胜结束
    {
        Debuger.Warn("Type:"+ heroS.heroTypeId+"|"+ heroT.heroTypeId);
        if (heroT.heroTypeId == 0) return 0;//敌方地雷
        if (heroT.heroTypeId == 1) return 0;//敌方炸弹
        if (heroS.heroTypeId == 1) return 0;//我方炸弹
        if (heroS.heroTypeId == 2)//我方工兵
        {
            if (heroT.heroTypeId == 0) return 1;//敌方地雷
        }
        if (heroT.heroTypeId == 11) return 2;//敌方军旗
        if(heroT.heroTypeId > heroS.heroTypeId)
        {
            return -1;
        }
        else if(heroT.heroTypeId < heroS.heroTypeId)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public static bool IsBarrack(int type, int id)
    {
        switch (type)
        {
            case 0:
                if (id == 11 || id == 13 || id == 17 || id == 21 || id == 23) return true;
                break;
            case 1:
                if (id == 6 || id == 8 || id == 12 || id == 16 || id == 18) return true;
                break;
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
}
public class ChessHeroData
{
    public int id;
    public int remoteId;
    public int heroTypeId;
    public ChessHeroState state = ChessHeroState.Alive;
    public ChessPoint point;
    public GameObject gameObject;
}

public class ChessPoint
{
    public int x;
    public int y;
    public ChessPoint(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return x + "," + y;
    }
    public bool Equals(ChessPoint obj)
    {
        return (this.x==obj.x && this.y == obj.y);
    }

}
public class FieldRoadStation
{
    public int id = -1;
    public ChessPoint point;//起点
    public int[] forbidChessHeros = new int[0];//禁止通过的子，100为敌方所有棋子,负数为敌方某一类棋子
    public int[] connectedPointIds = new int[0];//终点
    public FieldRoadStationType type = FieldRoadStationType.Way;
}

public class FieldRoadPath
{
    public List<int> pathStations = new List<int>();
    public FieldRoadPath()
    {

    }
    public FieldRoadPath(FieldRoadPath path)
    {
        
        this.pathStations = new List<int>(path.pathStations.ToArray());
        
    }
    public string ToString()
    {
        string re = "";
        for(int i = 0; i < pathStations.Count; i++)
        {
            if (i > 0) re += "->";
            re += ChessGamePackage.Instance.GetFeildRoadStationById(pathStations[i]).point.ToString();
        }
        return re;
    }
    public ChessPoint[] ToChessPoints()
    {
        ChessPoint[] points = new ChessPoint[pathStations.Count];
        for(int i = 0; i < points.Length; i++)
        {
            points[i] = ChessGamePackage.Instance.GetFeildRoadStationById(pathStations[i]).point;
        }
        return points;
    }
}

public class ChessMoveData
{
    public int crashType;//1棋子自身，2路径效果（军营类），3路途有棋子
    public ChessPoint[] points;
    public ChessHeroData crashHero;
}

public enum FieldRoadStationType
{
    Way = 0,
    Rail = 1,
    Barrack = 2,
}

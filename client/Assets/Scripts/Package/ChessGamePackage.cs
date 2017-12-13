using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Violet.Rpc;
public class ChessGamePackage : Package<ChessGamePackage>
{
    private Dictionary<int, ChessHeroData> m_ChessData = new Dictionary<int, ChessHeroData>();//客户端id,ChessHeroData
    private List<FieldRoadStation> m_MapRoadStations = new List<FieldRoadStation>();
    public List<int> ChessDataIds = new List<int>();//所有棋子的本地id
    public int MyselfChooseChessId = -1;
    public List<ChessData> MyselfChessSetting;
    public List<PlayerInfo> AllPlayerList = new List<PlayerInfo>();
    public int GameRoundCounter = -1;
    public string RoomId = "";
    int m_roundOrder;
    public int roundOrder{
        get{
            return m_roundOrder;
        }
    }
    public List<FieldRoadStation> MapRoadStations{
        get{
            return m_MapRoadStations;
        }
    }
    public List<HistoryStep> ChessHistorySteps = new List<HistoryStep>();
    List<PlayerInfo> m_EnemyPlayerList = new List<PlayerInfo>();
    public List<PlayerInfo> EnemyPlayerList
    {
        get
        {
            return m_EnemyPlayerList;
        }
    }
    bool m_CanDragChess = true;
    /// <summary>
    /// 是否处于布子状态
    /// </summary>
    public bool CanDragChess
    {
        get
        {
            return m_CanDragChess;
        }
    }

    public bool IsMyRound
    {
        get {
            return GameRoundCounter%AllPlayerList.Count==roundOrder;
        }
    }

    bool m_IsGameStart = false;
    /// <summary>
    /// 游戏开始
    /// </summary>
    public bool IsGameStart {
        get
        {
            return m_IsGameStart;
        }
    }
    bool m_IsReadyGame = false;
    /// <summary>
    /// 我方准备完毕
    /// </summary>
    public bool IsReadyGame
    {
        get
        {
            return m_IsReadyGame;
        }
    }


    bool m_IsEnemyReady = false;
    /// <summary>
    /// 敌方准备完毕
    /// </summary>
    public bool IsEnemyReady
    {
        get
        {
            return m_IsEnemyReady;
        }
    }
    
    public void Ready()
    {
        m_CanDragChess = false;
        m_IsReadyGame = true;
        m_IsGameStart = false;
    }

    public void Start()
    {
        m_CanDragChess = false;
        m_IsReadyGame = false;
        m_IsGameStart = true;
    }

    public override void Init(object data)
    {
        base.Init(data);
        EnterBattleFieldPush push = (EnterBattleFieldPush)data;
        App.Package.ChessGame.RoomId = push.RoomId;
        Config.Game.WaitingReady = push.ReadyTime;
        Config.Game.WaitingRound = push.RoundTime;
        m_EnemyPlayerList = new List<PlayerInfo>(push.PlayerList);
        m_EnemyPlayerList.Sort(SortPlayerListByRoundOrder);
        m_ChessData.Clear();
        ChessDataIds.Clear();
        ChessHistorySteps.Clear();
        MyselfChessSetting = new List<ChessData>(push.ChessSetting);
        m_roundOrder = push.RoundOrder;
        m_IsEnemyReady = false;
        m_IsGameStart = false;
        m_IsReadyGame = false;
        m_CanDragChess = true;
        GameRoundCounter = 0;
        App.Package.Player.playerInfo.State = (int)PlayerState.UNREADY;
        if(m_MapRoadStations==null || m_MapRoadStations.Count==0)InitFieldMap();
    }
    
    public void Recover(EnterBattleFieldResponse response)
    {
        Config.Game.WaitingReady = response.ReadyTime;
        Config.Game.WaitingRound = response.RoundTime;
        m_EnemyPlayerList = new List<PlayerInfo>();
        for(int i = 0; i < response.PlayerList.Count; i++)
        {
            if(response.PlayerList[i].ZoneId != App.Package.Player.playerInfo.ZoneId || response.PlayerList[i].UserId != App.Package.Player.playerInfo.UserId)
            {
                m_EnemyPlayerList.Add(response.PlayerList[i]);
            }
            else
            {
                App.Package.Player.playerInfo = response.PlayerList[i];
                
            }
        }
        m_EnemyPlayerList.Sort(SortPlayerListByRoundOrder);
        AllPlayerList = new List<PlayerInfo>(m_EnemyPlayerList);
        AllPlayerList.Add(App.Package.Player.playerInfo);
        AllPlayerList.Sort(SortPlayerListByRoundOrder);
        m_ChessData.Clear();
        ChessDataIds.Clear();
        m_roundOrder = App.Package.Player.playerInfo.RoundOrder;
        m_IsEnemyReady = false;
        m_IsGameStart = true;
        m_IsReadyGame = false;
        m_CanDragChess = false;
        GameRoundCounter = response.Counter;
        App.Package.ChessGame.AddChessFromData(new List<ChessData>(response.ChessMap));
        if (m_MapRoadStations == null || m_MapRoadStations.Count == 0) InitFieldMap();
    }


    int SortPlayerListByRoundOrder(PlayerInfo p1, PlayerInfo p2)
    {
        return p1.RoundOrder - p2.RoundOrder;
    }
    /// <summary>
    /// 初始化战场的道路连接数据
    /// </summary>
    public void InitFieldMap()
    {
        m_MapRoadStations.Clear();
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                FieldRoadStation roadStation = new FieldRoadStation();
                roadStation.point = new ChessPoint(j, i);
                if (ChessAgainst.InRailArea(roadStation.point))
                {
                    roadStation.type = FieldRoadStationType.Rail;
                }
                else if(ChessAgainst.IsBarrack(roadStation.point)){
                    roadStation.type = FieldRoadStationType.Barrack;
                }
                roadStation.id = m_MapRoadStations.Count;
                m_MapRoadStations.Add(roadStation);
            }
        }
        List<int> connetedIds = new List<int>();
        for (int i = 0; i < m_MapRoadStations.Count; i++)
        {
            connetedIds.Clear();
            for (int j = 0; j < m_MapRoadStations.Count; j++)
            {
                if (i != j)
                {
                    if (ChessAgainst.IsConnected(m_MapRoadStations[i], m_MapRoadStations[j]))
                    {
                        connetedIds.Add(j);
                        //Debugger.Warn(m_MapRoadStations[i].point.ToString()+" => "+ m_MapRoadStations[j].point.ToString());
                    }
                }
            }
            m_MapRoadStations[i].connectedPointIds = connetedIds.ToArray();
        }
    }

    protected void SetChessMap(Dictionary<int, ChessHeroData> chessMap)
    {
        m_ChessData = chessMap;
    }
    public void AddChessToMap(ChessHeroData chessData)
    {
        if (m_ChessData.ContainsKey(chessData.id))
        {
            m_ChessData[chessData.id] = chessData;
        }
        else
        {
            ChessDataIds.Add(chessData.id);
            m_ChessData.Add(chessData.id, chessData);
        }
    }

    public void AddChessFromData(List<ChessData> chessList){
        PlayerInfo playerInfo = App.Package.Player.playerInfo;
        string belong = playerInfo.ZoneId + "/" + playerInfo.UserId;
        Dictionary<ChessHeroGroup,int> map = new Dictionary<ChessHeroGroup, int>();
        map.Add(ChessHeroGroup.Myself,1);
        map.Add(ChessHeroGroup.Enemy,1);
        int baseId = 0;
        for(int i=0;i<chessList.Count;i++){
            ChessData chess = chessList[i];
            Debugger.Warn(chess.Belong + ":" + chess.Point.X + "," + chess.Point.Y + "|" + chess.ChessType);
            ChessHeroData heroData = new ChessHeroData();
            ChessHeroGroup group;
            baseId = GetBaseId(chess.Belong);
            group = (ChessHeroGroup)(baseId/100);
            switch (group)
            {
                case ChessHeroGroup.Myself:
                    heroData.point = new ChessPoint(chess.Point.X, chess.Point.Y);
                    break;
                case ChessHeroGroup.Enemy://敌人要翻转
                    heroData.point = new ChessPoint(4-chess.Point.X, 11-chess.Point.Y);
                    break;
            }
            
            heroData.heroTypeId = chess.ChessType;
            heroData.realChessType = heroData.heroTypeId;
            heroData.id = map[group]+baseId;
            heroData.remoteId = chess.ChessRemoteId;
            heroData.state = ChessHeroState.Alive;
            heroData.belong = chess.Belong;
            heroData.group = group;
            AddChessToMap(heroData);
            map[group]++;
        }
    }

    public int GetBaseId(string belong)
    {
        PlayerInfo playerInfo = App.Package.Player.playerInfo;
        if (playerInfo.ZoneId + "/" + playerInfo.UserId == belong) return 0;
        int baseId = 1;
        for(int i = 0; i < m_EnemyPlayerList.Count; i++)
        {
            if(m_EnemyPlayerList[i].ZoneId+"/"+ m_EnemyPlayerList[i].UserId == belong)
            {
                baseId += i;
                break;
            }
        }
        return baseId * 100;
    }
    public ChessHeroGroup GetGroup(string belong)
    {
        
        return (ChessHeroGroup)(GetBaseId(belong)/100);
    }

    public void RemoveAllChessHero()
    {
        foreach(var item in m_ChessData)
        {
            GameObject.Destroy(item.Value.gameObject);
            
        }
        m_ChessData.Clear();
        ChessDataIds.Clear();
    }

    public void RemoveChessHero(ChessHeroData chessHero)
    {
        m_ChessData.Remove(chessHero.id);
        ChessDataIds.Remove(chessHero.id);
        GameObject.Destroy(chessHero.gameObject);
    }

    public List<ChessData> ParseChessDataFromString(string data,string belong)
    {
        List<ChessData> chessList = new List<ChessData>();
        int baseId = GetBaseId(belong);
        int offsetY = 0;
        ChessHeroGroup group = (ChessHeroGroup)(baseId / 100);
        string[] rows = data.Split(';');
        for(int i = 0; i < rows.Length && i < 6; i++)
        {
            string[] chessTypeIds = rows[i].Split(',');
            for(int j = 0; j < chessTypeIds.Length; j++)
            {
                string id = chessTypeIds[j];
                if (id != "")
                {
                    int type = -1;
                    int remoteId = 0;
                    if (id.IndexOf("|") > -1)
                    {
                        string[] ids = id.Split('|');
                        type = int.Parse(ids[0]);
                        remoteId = int.Parse(ids[1]);
                    }
                    else
                    {
                        type = int.Parse(id);
                    }
                    ChessData heroData = new ChessData();
                    heroData.ChessType = type;
                    heroData.ChessRemoteId = remoteId;
                    heroData.Group = (int)group;
                    heroData.Belong = belong;
                    heroData.Point = new Com.Violet.Rpc.ChessPoint();
                    heroData.Point.X = j;
                    heroData.Point.Y = offsetY + i;
                    chessList.Add(heroData);
                }
                
            }
        }
        return chessList;

    }

    public string GetChessStringData(ChessHeroGroup group)
    {
        int[,] mapPosition = new int[5, 12]; 
        foreach(var item in m_ChessData)
        {
            if(item.Value.group == group) mapPosition[item.Value.point.x, item.Value.point.y] = item.Value.heroTypeId + 1;
        }
        string re = "";
        for(int i = 0; i < mapPosition.GetLength(1); i++)
        {
            for (int j = 0; j < mapPosition.GetLength(0); j++)
            {
                if (j > 0) re += ",";
                if (mapPosition[j, i] > 0)
                {
                    re += (mapPosition[j, i] - 1).ToString();
                }
            }
            re += ";";
        }
        return re;
    }

    public List<ChessData> GetChessData(ChessHeroGroup group)
    {
        List<ChessData> list = new List<ChessData>();
        foreach (var item in m_ChessData)
        {
            if (item.Value.group == group)
            {
                ChessData chess = new ChessData();
                chess.ChessRemoteId = item.Value.remoteId;
                chess.Group = (int)item.Value.group;
                chess.Point = item.Value.point.ParseToRpc();
                chess.ChessType = item.Value.heroTypeId;
                list.Add(chess);
            }
        }

        return list;
    }

    /*public ChessHeroGroup GetChessGroupById(int id)
    {
        if (id < 100) return ChessHeroGroup.Myself;
        if (id>=100 && id < 200) return ChessHeroGroup.Enemy;
        return ChessHeroGroup.Enemy;
    }*/

    public List<ChessHeroData> GetChessHeroList(ChessHeroGroup group)
    {
        List<ChessHeroData> list = new List<ChessHeroData>();
        foreach(var item in m_ChessData)
        {
            switch (group)
            {
                case ChessHeroGroup.Myself:
                    if (item.Value.id < 100) list.Add(item.Value);
                    break;
                case ChessHeroGroup.Enemy:
                    if (item.Value.id >= 100 && item.Value.id < 200) list.Add(item.Value);
                    break;
            }
        }
        return list;
    }

    public ChessHeroData GetChessHeroDataById(int id)
    {
        if (m_ChessData.ContainsKey(id))
        {
            return m_ChessData[id];
        }
        return null;
    }

    
    public ChessHeroData GetChessHeroDataByPoint(ChessPoint point)
    {
        foreach(var item in m_ChessData)
        {
            if (item.Value.point.Equals(point))
            {
                return item.Value;
            }
        }
        return null;
    }

    public ChessHeroData GetChessHeroDataByRemoteId(int remoteId)
    {
        foreach(var item in m_ChessData)
        {
            if(item.Value.remoteId == remoteId)
            {
                return item.Value;
            }
        }
        return null;
    }
    public FieldRoadStation GetFieldRoadStationByPoint(ChessPoint point)
    {
        return GetFieldRoadStationById(point.x+point.y*5);
    }
    public FieldRoadStation GetFieldRoadStationById(int id)
    {
        return m_MapRoadStations[id];
    }

    public override void Release()
    {
        base.Release();
    }

    public ChessPoint[] ChessDataPathToPoints(ChessDataPath path)
    {
        ChessPoint[] re = new ChessPoint[path.ChessPoints.Count];
        for(int i=0;i< path.ChessPoints.Count; i++)
        {
            re[i] = new ChessPoint(path.ChessPoints[i].X, path.ChessPoints[i].Y);
        }
        return re;
    }

    public ChessDataPath BuildChessDataPathFromPoints(ChessPoint[] points)
    {
        ChessDataPath re = new ChessDataPath();
        
        for (int i = 0; i < points.Length; i++)
        {
            re.ChessPoints.Add(points[i].ParseToRpc());
        }
        return re;
    }

    public HistoryStep BuildHistoryStep(int counter,ChessData source,ChessData target, ChessDataPath path, int result)
    {
        HistoryStep re = new HistoryStep();
        re.Counter = counter;
        re.Source = source;
        re.Target = target;
        re.Path = path;
        re.Result = result;
        return re;
    }

    public HistoryStep BuildHistoryStep(int counter, ChessHeroData source, ChessHeroData target, ChessDataPath path, int result)
    {
        ChessData s = new ChessData();
        s.ChessRemoteId = source.remoteId;
        s.Belong = source.belong;
        s.ChessType = source.realChessType;
        s.Point = source.point.ParseToRpc();

        ChessData t = new ChessData();
        t.ChessRemoteId = target.remoteId;
        t.Belong = target.belong;
        t.ChessType = target.realChessType;
        t.Point = target.point.ParseToRpc();
        return BuildHistoryStep(counter,s,t,path,result);
    }
    public HistoryStep BuildHistoryStep(int counter, ChessHeroData source, ChessPoint targetPoint, ChessDataPath path, int result)
    {
        ChessData s = new ChessData();
        s.ChessRemoteId = source.remoteId;
        s.Belong = source.belong;
        s.ChessType = source.realChessType;
        s.Point = source.point.ParseToRpc();

        ChessData t = new ChessData();
        t.Point = targetPoint.ParseToRpc();
        return BuildHistoryStep(counter, s, t, path, result);
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
    public string belong;
    public ChessHeroGroup group;
    public int realChessType;
}


public enum ChessHeroGroup
{
    Myself = 0,
    Enemy = 1,
}

public class ChessPoint
{
    public int x;
    public int y;
    public Com.Violet.Rpc.ChessPoint ParseToRpc()
    {
        Com.Violet.Rpc.ChessPoint p = new Com.Violet.Rpc.ChessPoint();
        p.X = x;
        p.Y = y;
        return p;
    }
    static public ChessPoint ParseFromRpc (Com.Violet.Rpc.ChessPoint point)
    {
        return new ChessPoint(point.X, point.Y);
    }
    public ChessPoint(int x, int y)
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
        return (this.x == obj.x && this.y == obj.y);
    }
    public ChessPoint Clone()
    {
        return new ChessPoint(x, y);
    }
}
public class FieldRoadStation
{
    public int id = -1;
    public ChessPoint point;//起点
    public int[] forbidChessHeros = new int[0];//禁止通过的子，100为敌方所有棋子,负数为敌方某一类棋子
    public int[] connectedPointIds = new int[0];//终点
    public GameObject gameObject;
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
    public override string ToString()
    {
        string re = "";
        for (int i = 0; i < pathStations.Count; i++)
        {
            if (i > 0) re += "->";
            re += App.Package.ChessGame.GetFieldRoadStationById(pathStations[i]).point.ToString();
        }
        return re;
    }
    public ChessPoint[] ToChessPoints()
    {
        ChessPoint[] points = new ChessPoint[pathStations.Count];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = App.Package.ChessGame.GetFieldRoadStationById(pathStations[i]).point;
        }
        return points;
    }


}

public class ChessMoveData
{
    public int crashType;//1棋子自身，2路径，3路途有棋子
    public ChessPoint[] points;
    public ChessHeroData crashHero;
}

public enum FieldRoadStationType
{
    Way = 0,
    Rail = 1,
    Barrack = 2,
}

public enum ChessGameMode
{
    Against = 0,//对战模式
    SelfToSelf = 1,//练习模式
    ShowChess = 2,//明牌模式
}

public enum ChessPlayerState
{
    UnReady = 0,//未准备，摆子中
    Ready = 1,//准备好了
    Gaming = 2,//游戏中
}

public class ChessPlayerData
{
    public ChessHeroGroup group;
    public PlayerInfo playerInfo;
    public ChessPlayerState state;
    public int remainTime;
}
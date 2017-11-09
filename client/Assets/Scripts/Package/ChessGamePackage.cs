using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChessGamePackage : Package<ChessGamePackage>
{
    private Dictionary<int, ChessHeroData> m_ChessData = new Dictionary<int, ChessHeroData>();//客户端id,ChessHeroData
    private List<FieldRoadStation> m_MapRoadStations = new List<FieldRoadStation>();
    public List<int> ChessDataIds = new List<int>();//所有棋子的本地id
    public int MyselfChooseChessId = -1;
    public int EnemyChooseChessId = -1;
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


    bool m_IsEnemyReady = true;
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
    

    public override void Init(object data)
    {
        //throw new NotImplementedException();
        InitFieldMap();
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
                if(ChessAgainst.IsBarrack(new ChessPoint(j,i)))//当前为军营，木有铁路
                {
                    FieldRoadStation roadStation = new FieldRoadStation();
                    roadStation.point = new ChessPoint(j, i);
                    roadStation.forbidChessHeros = new int[] { 100 };
                    roadStation.type = FieldRoadStationType.Barrack;
                    roadStation.id = m_MapRoadStations.Count;
                    m_MapRoadStations.Add(roadStation);
                }
                else
                {
                    FieldRoadStation roadStation = new FieldRoadStation();
                    roadStation.point = new ChessPoint(j, i);
                    if (ChessAgainst.InRailArea(roadStation.point))
                    {
                        roadStation.type = FieldRoadStationType.Rail;
                    }
                    roadStation.id = m_MapRoadStations.Count;
                    m_MapRoadStations.Add(roadStation);
                }
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
                        //Debuger.Warn(m_MapRoadStations[i].point.ToString()+" => "+ m_MapRoadStations[j].point.ToString());
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

    public void AddChessFromData(string data,ChessHeroGroup group)
    {
        int baseId = 0;
        int offsetY = 0;
        switch (group)
        {
            case ChessHeroGroup.Myself:
                baseId = 0;
                offsetY = 0;
                break;
            case ChessHeroGroup.Enemy:
                baseId = 100;
                offsetY = 6;
                break;
        }
        int localId = 1;
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
                    ChessHeroData heroData = new ChessHeroData();
                    heroData.heroTypeId = type;
                    heroData.remoteId = remoteId;
                    heroData.state = ChessHeroState.Alive;
                    heroData.id = baseId + localId;
                    switch (group)
                    {
                        case ChessHeroGroup.Myself:
                            heroData.point = new ChessPoint(j, offsetY + i);
                            break;
                        case ChessHeroGroup.Enemy://敌人在我方是反的
                            heroData.point = new ChessPoint(4 - j, offsetY + 5 - i);
                            break;
                    }
                    AddChessToMap(heroData);
                    localId++;
                }
                
            }
        }

    }

    public string GetChessStringData(ChessHeroGroup group)
    {
        int[,] mapPosition = new int[5, 12]; 
        foreach(var item in m_ChessData)
        {
            if(GetChessGroupById(item.Value.id) == group) mapPosition[item.Value.point.x, item.Value.point.y] = item.Value.heroTypeId + 1;
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

    public ChessHeroGroup GetChessGroupById(int id)
    {
        if (id < 100) return ChessHeroGroup.Myself;
        if (id>=100 && id < 200) return ChessHeroGroup.Enemy;
        return ChessHeroGroup.Enemy;
    }

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
    public FieldRoadStation GetFeildRoadStationByPoint(ChessPoint point)
    {
        foreach (var item in m_MapRoadStations)
        {
            if (item.point.Equals(point))
            {
                return item;
            }
        }
        return null;
    }
    public FieldRoadStation GetFeildRoadStationById(int id)
    {
        return m_MapRoadStations[id];
    }

    public override void Release()
    {
        //throw new NotImplementedException();
    }

    
}
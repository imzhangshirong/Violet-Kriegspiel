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
    public override void Init(object data)
    {
        //throw new NotImplementedException();
        InitFieldMap();
    }

    public void InitFieldMap()
    {
        m_MapRoadStations.Clear();
        for (int i = 0; i < 12; i++)
        {
            int type = i / 6;
            int id = 0;
            for (int j = 0; j < 5; j++)
            {
                id = i % 6 * 5 + j;
                if(ChessAgainst.IsBarrack(type, id))//当前为军营，木有铁路
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
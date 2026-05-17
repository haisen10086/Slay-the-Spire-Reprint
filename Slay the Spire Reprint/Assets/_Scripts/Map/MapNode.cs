using System.Collections.Generic;
using UnityEngine;

public enum RoomType { Unknown, Monster, Elite, Rest, Merchant, Treasure, Mystery, Boss }

[System.Serializable]
public class MapNode
{
    public int X { get; private set; } // 列
    public int Y { get; private set; } // 层 (0 是最底层)
    public RoomType Type;       //节点类型
    public EncounterDataSO EncounterDataSO; //节点存储的敌人数据

    // 存储当前节点可以前往的下一个节点
    public List<MapNode> NextNodes = new List<MapNode>();

    // 用于验证连通性
    public bool HasParents { get; set; }

    public MapNode(int x, int y)
    {
        X = x;
        Y = y;
        Type = RoomType.Unknown;
    }

    public void AddConnection(MapNode node)
    {
        if (!NextNodes.Contains(node))
        {
            NextNodes.Add(node);
            node.HasParents = true;
        }
    }
}
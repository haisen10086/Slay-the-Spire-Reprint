using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth = 7;
    public int mapHeight = 15;
    public int numPaths = 6; // 生成的路径数量，决定了地图的密度

    private MapNode[,] grid;
    private List<MapNode> finalMapNodes;
    public MapGenerator Instance { get; private set; }

    public List<MapNode> GetGeneratedNodes() { return finalMapNodes; }

    public void GenerateMap()
    {
        InitializeGrid();
        GeneratePaths();
        CleanupOrphans();
        //AssignRoomTypesAndEncouter();

        // 此时 finalMapNodes 已经包含了完整的、逻辑上连通的、分配好类型的地图数据
        Debug.Log($"地图生成完毕，共保留有效节点: {finalMapNodes.Count} 个");
    }

    private void InitializeGrid()
    {
        grid = new MapNode[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                grid[x, y] = new MapNode(x, y);
            }
        }
    }

    // 步骤1：路径追踪（加入均分布点与寻路斥力,反交叉）
    // 步骤1：路径追踪（横向版本）
    //更改，每条路径生成时，边生成边分配类型
    private void GeneratePaths()
    {
        int bossY = mapHeight / 2;

        List<int> startYs = GenerateStartRows();

        // =====================================
        // 生成每条路径
        // =====================================

        for (int pathIndex = 0; pathIndex < numPaths; pathIndex++)
        {
            int currentY = startYs[pathIndex];

            MapNode currentNode = grid[0, currentY];
            currentNode.HasParents = true;
            //给第一个节点分配类型
            AssignRoomTypesAndEncouter(currentNode);

            // 路径惯性
            int momentum = Random.Range(-1, 2);

            for (int x = 0; x < mapWidth - 1; x++)
            {
                MapNode bestNode = null;

                int bestY = currentY;

                float bestScore = float.MinValue;

                // 尝试：
                // 上、平、下

                for (int dir = -1; dir <= 1; dir++)
                {
                    int targetY = currentY + dir;

                    // 越界
                    if (targetY < 0 || targetY >= mapHeight)
                        continue;

                    MapNode candidate = grid[x + 1, targetY];

                    // 避免交叉
                    if (IsCrossing(currentNode, candidate))
                        continue;

                    float score = EvaluateNode(
                        x,
                        currentY,
                        targetY,
                        dir,
                        momentum,
                        candidate,
                        bossY
                    );

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestNode = candidate;
                        bestY = targetY;
                    }
                }

                // fallback
                if (bestNode == null)
                {
                    bestNode = grid[x + 1, currentY];
                    bestY = currentY;
                }

                // 建立连接
                currentNode.AddConnection(bestNode);

                // 更新惯性
                momentum = Mathf.Clamp(bestY - currentY, -1, 1);

                currentNode = bestNode;
                currentY = bestY;

                //给当前节点分配类型
                AssignRoomTypesAndEncouter(currentNode);
            }
        }

        ConnectBoss();
    }

    // =========================================
    // 节点评分系统（核心）
    // =========================================

    private float EvaluateNode(
        int x,
        int currentY,
        int targetY,
        int dir,
        int momentum,
        MapNode candidate,
        int bossY)
    {
        float score = 0f;

        float progress = x / (float)(mapWidth - 1);

        // =====================================
        // 1. 路径惯性（核心）
        // =====================================

        if (dir == momentum)
        {
            score += 5f;
        }

        // =====================================
        // 2. 避免急转弯
        // =====================================

        if (dir == -momentum)
        {
            score -= 4f;
        }

        // =====================================
        // 3. 稍微偏向水平移动
        // =====================================

        if (dir == 0)
        {
            score += 1.5f;
        }

        // =====================================
        // 4. 路径排斥
        // =====================================

        int nearbyConnections = CountNearbyConnections(x + 1, targetY);

        score -= nearbyConnections * 2f;

        // =====================================
        // 5. 路径吸引
        // =====================================

        if (nearbyConnections == 1)
        {
            score += 2.5f;
        }

        // =====================================
        // 6. 已占用节点
        // =====================================

        if (candidate.HasParents)
        {
            score -= 1f;
        }

        // =====================================
        // 7. 边缘惩罚
        // =====================================

        if (targetY == 0 || targetY == mapHeight - 1)
        {
            score -= 1.5f;
        }

        // =====================================
        // 8. 后期靠近 Boss
        // =====================================

        if (progress > 0.55f)
        {
            float distToBoss = Mathf.Abs(targetY - bossY);

            score -= distToBoss * 0.9f;
        }

        // =====================================
        // 9. 中期允许更大扩散
        // =====================================

        if (progress > 0.2f && progress < 0.6f)
        {
            score += Mathf.Abs(targetY - bossY) * 0.3f;
        }

        // =====================================
        // 10. 随机扰动
        // =====================================

        score += Random.Range(-0.6f, 0.6f);

        return score;
    }

    // =========================================
    // Boss 汇聚
    // =========================================

    private void ConnectBoss()
    {
        int bossY = mapHeight / 2;

        MapNode bossNode = grid[mapWidth - 1, bossY];

        for (int y = 0; y < mapHeight; y++)
        {
            MapNode node = grid[mapWidth - 2, y];

            if (node.HasParents)
            {
                node.AddConnection(bossNode);
            }
        }
    }

    // =========================================
    // 统计附近路径密度
    // =========================================

    private int CountNearbyConnections(int x, int y)
    {
        int count = 0;

        for (int offset = -2; offset <= 2; offset++)
        {
            int checkY = y + offset;

            if (checkY < 0 || checkY >= mapHeight)
                continue;

            if (grid[x, checkY].HasParents)
            {
                count++;
            }
        }

        return count;
    }

    // =========================================
    // 起点均匀分布
    // =========================================

    private List<int> GenerateStartRows()
    {
        List<int> rows = new();

        for (int i = 0; i < numPaths; i++)
        {
            int y = Mathf.RoundToInt(
                Mathf.Lerp(
                    0,
                    mapHeight - 1,
                    i / (float)(numPaths - 1)
                )
            );

            rows.Add(y);
        }

        Shuffle(rows);

        return rows;
    }
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);

            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
    // 【新增】防交叉检测核心算法
    // 【修改】防交叉检测（横向版本：检查 Y 轴的交错）
    private bool IsCrossing(MapNode nodeA, MapNode targetA)
    {
        int x = nodeA.X;

        for (int y = 0; y < mapHeight; y++)
        {
            MapNode nodeB = grid[x, y];

            if (nodeB == nodeA || nodeB.NextNodes.Count == 0) continue;

            foreach (MapNode targetB in nodeB.NextNodes)
            {
                // 如果 A 在 B 的下方，但 A 的目标跑到了 B 的目标的上方 -> 交叉
                if (nodeA.Y < nodeB.Y && targetA.Y > targetB.Y) return true;

                // 如果 A 在 B 的上方，但 A 的目标跑到了 B 的目标的下方 -> 交叉
                if (nodeA.Y > nodeB.Y && targetA.Y < targetB.Y) return true;
            }
        }
        return false;
    }

    // 步骤2：清理未被连接的死节点
    private void CleanupOrphans()
    {
        finalMapNodes = new List<MapNode>();
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                MapNode node = grid[x, y];
                // 如果节点被路径访问过（有父节点），且不是死胡同（最顶层除外）
                if (node.HasParents && (node.NextNodes.Count > 0 || x == mapWidth - 1))
                {
                    finalMapNodes.Add(node);
                }
            }
        }
    }

    // 步骤3：基于规则系统分配节点类型
    private void AssignRoomTypesAndEncouter()
    {
        // =========================================
        // 固定层规则
        // =========================================

        int bossColumn = mapWidth - 1;
        int preBossColumn = mapWidth - 2;

        // 宝箱层（接近 STS）
        int treasureColumn = Mathf.RoundToInt(mapWidth * 0.55f);

        foreach (var node in finalMapNodes)
        {
            // =====================================
            // 第一层：必小怪
            // =====================================

            if (node.X == 0)
            {
                node.Type = RoomType.Monster;
                AssignEncounter(node);
                continue;
            }

            // =====================================
            // Boss
            // =====================================

            if (node.X == bossColumn)
            {
                node.Type = RoomType.Boss;
                AssignEncounter(node);
                continue;
            }

            // =====================================
            // Boss 前：必营地
            // =====================================

            if (node.X == preBossColumn)
            {
                node.Type = RoomType.Rest;
                continue;
            }

            // =====================================
            // 宝箱层
            // =====================================

            if (node.X == treasureColumn)
            {
                node.Type = RoomType.Treasure;
                continue;
            }

            // =====================================
            // 普通随机层
            // =====================================

            node.Type = GetWeightedRoomType(node);
            //分配敌人数据
            AssignEncounter(node);
        }
    }
    //分配单个房间类型
    private void AssignRoomTypesAndEncouter(MapNode node)
    {
        // =========================================
        // 固定层规则
        // =========================================

        int bossColumn = mapWidth - 1;
        int preBossColumn = mapWidth - 2;

        // 宝箱层（接近 STS）
        int treasureColumn = Mathf.RoundToInt(mapWidth * 0.55f);


        // =====================================
        // 第一层：必小怪
        // =====================================

        if (node.X == 0)
        {
            node.Type = RoomType.Monster;
            AssignEncounter(node);
            return;
        }

        // =====================================
        // Boss
        // =====================================

        if (node.X == bossColumn)
        {
            node.Type = RoomType.Boss;
            AssignEncounter(node);
            return;
        }

        // =====================================
        // Boss 前：必营地
        // =====================================

        if (node.X == preBossColumn)
        {
            node.Type = RoomType.Rest;
            return;
        }

        // =====================================
        // 宝箱层
        // =====================================

        if (node.X == treasureColumn)
        {
            node.Type = RoomType.Treasure;
            return;
        }

        // =====================================
        // 普通随机层
        // =====================================

        node.Type = GetWeightedRoomType(node);
        //分配敌人数据
        AssignEncounter(node);
    }
    //分配遭遇敌人
    private void AssignEncounter(MapNode node)
    {
        List<EncounterDataSO> pool = new List<EncounterDataSO>();
        if (node.Type == RoomType.Monster) 
        {
           pool = AllDataSystem.Instance.ActEnemyPool.NormalEncounters;
        }
        else if(node.Type == RoomType.Elite)
        {
            pool = AllDataSystem.Instance.ActEnemyPool.EliteEncounters;
        }
        else if(node.Type == RoomType.Boss)
        {
            pool = AllDataSystem.Instance.ActEnemyPool.BossEncounters;
        }
        else
        {
            node.EncounterDataSO = null;
            return;
        }
        node.EncounterDataSO = GetRandomEncounter(pool, node.X, node);
    }
    public EncounterDataSO GetRandomEncounter(
    List<EncounterDataSO> pool,
    int floor, MapNode node)
    {
        var valid = pool.Where(x =>
            floor >= x.MinFloor &&
            floor <= x.MaxFloor
        ).ToList();

        return GetValidRandom(valid, node);
    }
    //随机获得敌人数据列表的的数据，优先从未使用的里面随机获得
    private EncounterDataSO GetValidRandom(List<EncounterDataSO> valid, MapNode node)
    {
        if (valid == null || valid.Count == 0)
        {
            Debug.LogError($"当前节点无可用 Encounter ({node.X},{node.Y})");
            return null;
        }

        List<int> unusedIndexes = new List<int>();

        for (int i = 0; i < valid.Count; i++)
        {
            if (!valid[i].isUse)
            {
                unusedIndexes.Add(i);
            }
        }

        // 优先随机未使用
        if (unusedIndexes.Count > 0)
        {
            int rand = Random.Range(0, unusedIndexes.Count);
            valid[unusedIndexes[rand]].isUse = true;
            return valid[unusedIndexes[rand]];
        }

        // 全部使用过
        return valid[Random.Range(0, valid.Count)];
    }

    private RoomType GetWeightedRoomType(MapNode node)
    {
        // =========================================
        // STS 风格权重系统
        // =========================================

        float monsterWeight = 0f;
        float eliteWeight = 0f;
        float restWeight = 0f;
        float merchantWeight = 0f;
        float mysteryWeight = 0f;

        float progress = node.X / (float)(mapWidth - 1);

        // =========================================
        // 基础权重
        // =========================================

        monsterWeight = 45f;
        mysteryWeight = 25f;
        merchantWeight = 12f;
        restWeight = 10f;
        eliteWeight = 8f;

        // =========================================
        // 前期限制
        // =========================================

        // 前4层不出精英
        if (node.X <= 4)
        {
            eliteWeight = 0f;
        }

        // 前3层不出营地
        if (node.X <= 3)
        {
            restWeight = 0f;
        }

        // 前1层不出商店
        if (node.X <= 1)
        {
            merchantWeight = 0f;
        }

        // =========================================
        // 中后期提高精英概率
        // =========================================

        if (progress > 0.45f)
        {
            eliteWeight += 5f;
        }

        if (progress > 0.65f)
        {
            eliteWeight += 8f;
        }

        // =========================================
        // 中后期提高营地概率
        // =========================================

        if (progress > 0.55f)
        {
            restWeight += 8f;
        }

        // =========================================
        // 边缘路线更危险（更容易精英）
        // =========================================

        bool isEdge =
            node.Y <= 1 ||
            node.Y >= mapHeight - 2;

        if (isEdge)
        {
            eliteWeight += 6f;
            mysteryWeight += 4f;

            monsterWeight -= 6f;
        }

        // =========================================
        // 中间路线更安全
        // =========================================

        bool isCenter =
            Mathf.Abs(node.Y - mapHeight / 2f) <= 1;

        if (isCenter)
        {
            restWeight += 4f;
            merchantWeight += 4f;
        }

        // =========================================
        // 防止连续精英
        // =========================================

        if (HasParentType(node, RoomType.Elite))
        {
            eliteWeight = 0f;

            restWeight += 8f;
            mysteryWeight += 5f;
        }

        // =========================================
        // 防止连续营地
        // =========================================

        if (HasParentType(node, RoomType.Rest))
        {
            restWeight = 0f;

            monsterWeight += 10f;
        }

        // =========================================
        // 防止连续商店
        // =========================================

        if (HasParentType(node, RoomType.Merchant))
        {
            merchantWeight *= 0.3f;
        }

        // =========================================
        // 精英后更容易营地
        // =========================================

        if (HasParentType(node, RoomType.Elite))
        {
            restWeight += 10f;
        }

        // =========================================
        // 高路径热度更容易事件
        // =========================================

        int incomingConnections = CountIncomingConnections(node);

        if (incomingConnections >= 2)
        {
            mysteryWeight += 10f;
            merchantWeight += 4f;
        }

        // =========================================
        // 路径孤立区域更危险
        // =========================================

        if (incomingConnections <= 0)
        {
            eliteWeight += 5f;
        }

        // =========================================
        // 权重总和
        // =========================================

        float total =
            monsterWeight +
            eliteWeight +
            restWeight +
            merchantWeight +
            mysteryWeight;

        float rand = Random.Range(0f, total);

        // =========================================
        // Roll
        // =========================================

        if (rand < monsterWeight)
            return RoomType.Monster;

        rand -= monsterWeight;

        if (rand < eliteWeight)
            return RoomType.Elite;

        rand -= eliteWeight;

        if (rand < restWeight)
            return RoomType.Rest;

        rand -= restWeight;

        if (rand < merchantWeight)
            return RoomType.Merchant;

        return RoomType.Mystery;
    }

    private bool HasParentType(MapNode node, RoomType type)
    {
        if (node.X == 0)
            return false;

        for (int dy = -1; dy <= 1; dy++)
        {
            int checkY = node.Y + dy;

            if (checkY < 0 || checkY >= mapHeight)
                continue;

            MapNode parent = grid[node.X - 1, checkY];

            if (parent.NextNodes.Contains(node))
            {
                if (parent.Type == type)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private int CountIncomingConnections(MapNode node)
    {
        int count = 0;

        if (node.X == 0)
            return 0;

        for (int dy = -1; dy <= 1; dy++)
        {
            int checkY = node.Y + dy;

            if (checkY < 0 || checkY >= mapHeight)
                continue;

            MapNode parent = grid[node.X - 1, checkY];

            if (parent.NextNodes.Contains(node))
            {
                count++;
            }
        }

        return count;
    }
}
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
        AssignRoomTypes();

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
    private void GeneratePaths()
    {
        // 【新增策略 1：均匀散布起点】
        // 预先生成一个包含所有底层 X 坐标的列表并打乱，保证起点不重叠且尽量分散
        List<int> startingXs = new List<int>();
        for (int x = 0; x < mapWidth; x++) startingXs.Add(x);

        // 洗牌算法打乱起点
        for (int i = 0; i < startingXs.Count; i++)
        {
            int temp = startingXs[i];
            int randomIndex = Random.Range(i, startingXs.Count);
            startingXs[i] = startingXs[randomIndex];
            startingXs[randomIndex] = temp;
        }

        // 开始生成路径
        for (int i = 0; i < numPaths; i++)
        {
            // 从均匀分布的起点池中取值（如果有 6 条路、7 个宽，就会恰好覆盖 6 个不同的入口）
            int currentX = startingXs[i % mapWidth];
            MapNode currentNode = grid[currentX, 0];
            currentNode.HasParents = true;

            for (int y = 0; y < mapHeight - 2; y++)
            {
                List<int> availableDirs = new List<int> { -1, 0, 1 };

                // 同样打乱方向，保证随机性
                for (int j = 0; j < availableDirs.Count; j++)
                {
                    int temp = availableDirs[j];
                    int randomIndex = Random.Range(j, availableDirs.Count);
                    availableDirs[j] = availableDirs[randomIndex];
                    availableDirs[randomIndex] = temp;
                }

                List<MapNode> validCandidates = new List<MapNode>();
                List<MapNode> emptyCandidates = new List<MapNode>();

                // 扫描所有合法的下一步
                foreach (int dir in availableDirs)
                {
                    int checkX = currentX + dir;
                    if (checkX >= 0 && checkX < mapWidth)
                    {
                        MapNode candidateNode = grid[checkX, y + 1];

                        // 必须不交叉
                        if (!IsCrossing(currentNode, candidateNode))
                        {
                            validCandidates.Add(candidateNode);
                            // 记录哪些节点还是“干净的”（没有被其他路径踩过）
                            if (!candidateNode.HasParents)
                            {
                                emptyCandidates.Add(candidateNode);
                            }
                        }
                    }
                }

                MapNode nextNode = null;
                int nextX = currentX;

                // 【新增策略 2：寻路斥力（优先向空节点延伸）】
                if (emptyCandidates.Count > 0)
                {
                    // 如果前方有空地，优先去空地（延迟路线合并，让地图更饱满）
                    nextNode = emptyCandidates[Random.Range(0, emptyCandidates.Count)];
                    nextX = nextNode.X;
                }
                else if (validCandidates.Count > 0)
                {
                    // 如果迫不得已（前方全被占了），才选择已经有路线的节点进行合并
                    nextNode = validCandidates[Random.Range(0, validCandidates.Count)];
                    nextX = nextNode.X;
                }
                else
                {
                    // 极端保底情况：强行直走
                    nextX = currentX;
                    nextNode = grid[nextX, y + 1];
                }

                currentNode.AddConnection(nextNode);
                currentNode = nextNode;
                currentX = nextX;
            }
        }

        // 统一收束到 Boss
        int bossX = mapWidth / 2;
        MapNode bossNode = grid[bossX, mapHeight - 1];

        for (int x = 0; x < mapWidth; x++)
        {
            MapNode preBossNode = grid[x, mapHeight - 2];
            if (preBossNode.HasParents)
            {
                preBossNode.AddConnection(bossNode);
            }
        }
    }
    // 【新增】防交叉检测核心算法
    private bool IsCrossing(MapNode nodeA, MapNode targetA)
    {
        int y = nodeA.Y;

        // 遍历当前层的每一个节点
        for (int x = 0; x < mapWidth; x++)
        {
            MapNode nodeB = grid[x, y];

            // 跳过自己，或者没有连线的节点
            if (nodeB == nodeA || nodeB.NextNodes.Count == 0) continue;

            // 遍历其他节点连接的目标
            foreach (MapNode targetB in nodeB.NextNodes)
            {
                // 规则 1：如果 A 在 B 的左边，但 A 的目标在 B 的目标的右边 -> 交叉！
                if (nodeA.X < nodeB.X && targetA.X > targetB.X) return true;

                // 规则 2：如果 A 在 B 的右边，但 A 的目标在 B 的目标的左边 -> 交叉！
                if (nodeA.X > nodeB.X && targetA.X < targetB.X) return true;
            }
        }

        return false; // 安全路线
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
                if (node.HasParents && (node.NextNodes.Count > 0 || y == mapHeight - 1))
                {
                    finalMapNodes.Add(node);
                }
            }
        }
    }

    // 步骤3：基于规则系统分配节点类型
    private void AssignRoomTypes()
    {
        foreach (var node in finalMapNodes)
        {
            if (node.Y == 0)
            {
                node.Type = RoomType.Monster; // 第一层必是小怪
                continue;
            }
            if (node.Y == mapHeight - 1)
            {
                node.Type = RoomType.Boss; // 顶层 Boss
                continue;
            }
            if (node.Y == mapHeight - 2)
            {
                node.Type = RoomType.Rest; // Boss 前必定是营地
                continue;
            }
            if (node.Y == mapHeight / 2)
            {
                node.Type = RoomType.Treasure; // 中段固定宝箱房
                continue;
            }

            // 其他层级根据随机权重分配 (需要结合你的实际策划案调整)
            node.Type = GetRandomRoomType(node);
        }
    }

    private RoomType GetRandomRoomType(MapNode node)
    {
        // 简单权重池实现
        float rand = Random.value;

        // 规则限制：前 4 层不能出现精英怪
        bool canBeElite = node.Y > 4 && !HasConsecutiveEliteParent(node);

        if (rand < 0.1f && canBeElite) return RoomType.Elite;
        if (rand < 0.25f) return RoomType.Merchant;
        if (rand < 0.4f && !HasConsecutiveRestParent(node)) return RoomType.Rest;
        if (rand < 0.6f) return RoomType.Mystery;

        return RoomType.Monster; // 默认 fallback
    }

    // 规则检测：防止连续精英
    private bool HasConsecutiveEliteParent(MapNode node)
    {
        // 实际应用中，由于图是单向的，我们需要在生成时从父向子传递状态，
        // 或者简单点，在这里通过检查其下方可能连接它的节点类型来近似判断
        if (node.Y == 0) return false;

        // 检查这一层所有的父节点候选（左下，正下，右下）
        for (int dx = -1; dx <= 1; dx++)
        {
            int checkX = node.X + dx;
            if (checkX >= 0 && checkX < mapWidth)
            {
                if (grid[checkX, node.Y - 1].NextNodes.Contains(node) &&
                    grid[checkX, node.Y - 1].Type == RoomType.Elite)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool HasConsecutiveRestParent(MapNode node)
    {
        // 实际应用中，由于图是单向的，我们需要在生成时从父向子传递状态，
        // 或者简单点，在这里通过检查其下方可能连接它的节点类型来近似判断
        if (node.Y == 0) return false;

        // 检查这一层所有的父节点候选（左下，正下，右下）
        for (int dx = -1; dx <= 1; dx++)
        {
            int checkX = node.X + dx;
            if (checkX >= 0 && checkX < mapWidth)
            {
                if (grid[checkX, node.Y - 1].NextNodes.Contains(node) &&
                    grid[checkX, node.Y - 1].Type == RoomType.Rest)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
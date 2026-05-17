using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUIManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform mapContainer; // 放置地图节点的父级容器 (通常是 ScrollRect 的 Content)
    public RectTransform mapBackground;// 地图背景图
    public GameObject nodePrefab;      // 节点预制体 (Image/Button)
    public GameObject linePrefab;      // 连线预制体 (挂载 LineRenderer 的空物体)

    //记录房间类型和Roomtype对应的Sprite
    [SerializeField]
    private SerializedDictionary<RoomType, Sprite> nodeSpriteByRoomType
        = new();

    [Header("Layout Settings")]
    public float nodeSpacingX = 200f;  // X轴基础间距
    public float nodeSpacingY = 100f;  // Y轴基础间距
    public float leftPadding = 150f; // 【新增】底部留白，把整个地图往上托一点
    public float jitterAmountX = 40f;  // X轴最大抖动幅度
    public float jitterAmountY = 30f;  // Y轴最大抖动幅度

    [Header("Line Settings")]
    public int bezierResolution = 20;  // 贝塞尔曲线的平滑度（分段数）
    public float controlPointOffset = 80f; // 贝塞尔控制点的垂直偏移量

    // 用于解耦：记录逻辑节点与它在 UI 上的实际坐标的映射
    private Dictionary<MapNode, Vector2> nodeUIPositions = new Dictionary<MapNode, Vector2>();

    /// <summary>
    /// 接收逻辑层的节点数据并开始绘制
    /// </summary>
    public void DrawMap(List<MapNode> mapNodes, int mapWidth, int mapHeight)
    {
        ClearMap();
        CalculatePositions(mapNodes, mapWidth, mapHeight);
        DrawConnections(mapNodes);
        InstantiateNodes(mapNodes);
    }

    // 1. 计算带有随机抖动的坐标
    private void CalculatePositions(List<MapNode> mapNodes,int mapWidth, int mapHeight) // 注意这里传入的辅助居中参数可能需要变成 mapHeight
    {
        // 计算 Y 轴整体偏移，让整张地图在竖直方向居中
        float startY = -(mapHeight * nodeSpacingY) / 2f + (nodeSpacingY / 2f);

        foreach (var node in mapNodes)
        {
            // 【修改】：X 是向右推进的进度
            float baseX = node.X * nodeSpacingX + leftPadding;

            // 【修改】：如果是 Boss 房，强制 Y 轴处于绝对正中 (0f)
            float baseY = (node.X == mapWidth - 1) ? 0f : startY + (node.Y * nodeSpacingY);

            // 第一列(入口)和最后一列(Boss)不抖动
            float currentJitterX = (node.X == 0 || node.X == mapWidth - 1) ? 0 : Random.Range(-jitterAmountX, jitterAmountX);
            float currentJitterY = (node.X == 0 || node.X == mapWidth - 1) ? 0 : Random.Range(-jitterAmountY, jitterAmountY);

            Vector2 finalPos = new Vector2(baseX + currentJitterX, baseY + currentJitterY);
            nodeUIPositions[node] = finalPos;
        }
    }

    // 2. 绘制节点之间的贝塞尔曲线
    private void DrawConnections(List<MapNode> mapNodes)
    {
        foreach (var node in mapNodes)
        {
            if (node.NextNodes.Count == 0) continue;

            Vector2 startPos = nodeUIPositions[node];

            foreach (var nextNode in node.NextNodes)
            {
                Vector2 endPos = nodeUIPositions[nextNode];
                CreateStraightDashedLine(startPos, endPos);
            }
        }
    }

    /// <summary>
    /// 创建点到点的直线，并开启纹理平铺以支持虚线材质
    /// </summary>
    private void CreateStraightDashedLine(Vector2 p0, Vector2 p3)
    {
        GameObject lineObj = Instantiate(linePrefab, mapContainer);
        RectTransform lineRectTransform = lineObj.GetComponent<RectTransform>();

        if (lineRectTransform != null)
        {
            // 保持左中锚点 (适应横向地图)
            lineRectTransform.anchorMin = new Vector2(0f, 0.5f);
            lineRectTransform.anchorMax = new Vector2(0f, 0.5f);
            lineRectTransform.pivot = new Vector2(0.5f, 0.5f);
            lineRectTransform.sizeDelta = Vector2.zero;
            lineRectTransform.anchoredPosition = p0;
        }

        lineObj.transform.localRotation = Quaternion.identity;
        lineObj.transform.localScale = Vector3.one;

        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;

        // 【核心修改 1】：直线只需要 2 个点（起点和终点）
        lineRenderer.positionCount = 2;

        Vector2 startPosRelative = p0;
        Vector2 endPosRelative = p3;

        // 设置起点和终点坐标
        lineRenderer.SetPosition(0, new Vector3(startPosRelative.x, startPosRelative.y, -0.5f));
        lineRenderer.SetPosition(1, new Vector3(endPosRelative.x, endPosRelative.y, -0.5f));

        // 【核心修改 2】：虚线逻辑支持
        // 将贴图模式设置为平铺 (Tile)，这是让虚线不被拉伸变形的关键
        lineRenderer.textureMode = LineTextureMode.Tile;

        // 计算这条线的实际物理长度
        float distance = Vector2.Distance(startPosRelative, endPosRelative);

        // 调整这个值可以改变虚线的疏密程度（值越大，虚线段越长）
        float dashLength = 30f;

        // 动态修改材质的 Tiling (X轴平铺次数)，确保线越长，重复的虚线段越多
        lineRenderer.material.mainTextureScale = new Vector2(distance / dashLength, 1f);
    }
    //private void CreateBezierLine(Vector2 p0, Vector2 p3)
    //{
    //    // 实例化线段预制体
    //    GameObject lineObj = Instantiate(linePrefab, mapContainer);
    //    LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

    //    lineRenderer.positionCount = bezierResolution;
    //    lineRenderer.useWorldSpace = false; // 确保在 UI 容器的局部空间内绘制


    //    // 设置贝塞尔控制点
    //    // P1 在 P0 正上方，P2 在 P3 正下方，这样画出的线会有“S”型的柔和过渡
    //    Vector2 p1 = p0 + new Vector2(controlPointOffset, 0);
    //    Vector2 p2 = p3 - new Vector2(controlPointOffset, 0);

    //    // 计算曲线上的点
    //    for (int i = 0; i < bezierResolution; i++)
    //    {
    //        float t = i / (float)(bezierResolution - 1);
    //        Vector2 point = CalculateCubicBezierPoint(t, p0, p1, p2, p3);
    //        lineRenderer.SetPosition(i, new Vector3(point.x, point.y, 0));
    //    }
    //}

    // 三次贝塞尔曲线核心算法
    private Vector2 CalculateCubicBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * p0; // (1-t)^3 * P0
        p += 3 * uu * t * p1; // 3(1-t)^2 * t * P1
        p += 3 * u * tt * p2; // 3(1-t) * t^2 * P2
        p += ttt * p3;        // t^3 * P3

        return p;
    }

    // 3. 实例化节点 UI
    private void InstantiateNodes(List<MapNode> mapNodes)
    {
        foreach (var node in mapNodes)
        {
            GameObject nodeObj = Instantiate(nodePrefab, mapContainer);

            MapNodeView mapNodeView = nodeObj.GetComponent<MapNodeView>();
            if (mapNodeView != null )
            {
                Sprite nodeSprite;
                if(!nodeSpriteByRoomType.ContainsKey(node.Type))
                {
                    nodeSprite = nodeSpriteByRoomType[RoomType.Unknown];
                }
                else nodeSprite = nodeSpriteByRoomType[node.Type];
                mapNodeView.Setup(node, nodeSprite);
            }

            RectTransform rectTransform = nodeObj.GetComponent<RectTransform>();

            // 【核心修复】：强制把节点的锚点设为 Bottom Center (底部中心)
            // 这样代码里算出的 Y 坐标才会严丝合缝地对准容器的底部！
            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);

            // 设置局部坐标
            rectTransform.anchoredPosition = nodeUIPositions[node];

            // 这里可以根据 node.Type 替换 Image 的 Sprite 或改变颜色
            // nodeObj.GetComponent<Image>().sprite = GetSpriteByType(node.Type);
        }
    }

    private void ClearMap()
    {
        //foreach (Transform child in mapContainer)
        //{
        //    if(child != mapBackground)
        //     Destroy(child.gameObject);
        //}
        for (int i = mapContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = mapContainer.GetChild(i);
            if (child != mapBackground)
                Destroy(child.gameObject);            
        }
        nodeUIPositions.Clear();
    }
}
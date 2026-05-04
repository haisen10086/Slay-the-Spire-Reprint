using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUIManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform mapContainer; // 放置地图节点的父级容器 (通常是 ScrollRect 的 Content)
    public GameObject nodePrefab;      // 节点预制体 (Image/Button)
    public GameObject linePrefab;      // 连线预制体 (挂载 LineRenderer 的空物体)

    [Header("Layout Settings")]
    public float nodeSpacingX = 150f;  // X轴基础间距
    public float nodeSpacingY = 200f;  // Y轴基础间距
    public float bottomPadding = 150f; // 【新增】底部留白，把整个地图往上托一点
    public float jitterAmountX = 30f;  // X轴最大抖动幅度
    public float jitterAmountY = 40f;  // Y轴最大抖动幅度

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
    private void CalculatePositions(List<MapNode> mapNodes, int mapWidth, int mapHeight)
    {
        // 计算整体偏移，让地图在容器中居中
        float startX = -(mapWidth * nodeSpacingX) / 2f + (nodeSpacingX / 2f);

        foreach (var node in mapNodes)
        {
            float baseX = startX + (node.X * nodeSpacingX);

            // 【关键修改】：如果是最顶层 (Boss层)
            if (node.Y == mapHeight - 1)
            {
                // 因为我们的 Content 锚点是 Bottom Center，X = 0 就是绝对的正中间！
                baseX = 0f;
            }

            float baseY = node.Y * nodeSpacingY + bottomPadding;

            // 取消最顶层 (Boss) 和 最底层 (入口) 的抖动，让两端显得规整庄重
            float currentJitterX = (node.Y == 0 || node.Y == mapHeight - 1) ? 0 : Random.Range(-jitterAmountX, jitterAmountX);
            float currentJitterY = (node.Y == 0 || node.Y == mapHeight - 1) ? 0 : Random.Range(-jitterAmountY, jitterAmountY);

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
                CreateBezierLine(startPos, endPos);
            }
        }
    }

    private void CreateBezierLine(Vector2 p0, Vector2 p3)
    {
        // 实例化线段预制体
        GameObject lineObj = Instantiate(linePrefab, mapContainer);
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

        lineRenderer.positionCount = bezierResolution;
        lineRenderer.useWorldSpace = false; // 确保在 UI 容器的局部空间内绘制

        // 设置贝塞尔控制点
        // P1 在 P0 正上方，P2 在 P3 正下方，这样画出的线会有“S”型的柔和过渡
        Vector2 p1 = p0 + new Vector2(0, controlPointOffset);
        Vector2 p2 = p3 - new Vector2(0, controlPointOffset);

        // 计算曲线上的点
        for (int i = 0; i < bezierResolution; i++)
        {
            float t = i / (float)(bezierResolution - 1);
            Vector2 point = CalculateCubicBezierPoint(t, p0, p1, p2, p3);
            lineRenderer.SetPosition(i, new Vector3(point.x, point.y, 0));
        }
    }

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
            RectTransform rectTransform = nodeObj.GetComponent<RectTransform>();

            // 【核心修复】：强制把节点的锚点设为 Bottom Center (底部中心)
            // 这样代码里算出的 Y 坐标才会严丝合缝地对准容器的底部！
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);

            // 设置局部坐标
            rectTransform.anchoredPosition = nodeUIPositions[node];

            // 这里可以根据 node.Type 替换 Image 的 Sprite 或改变颜色
            // nodeObj.GetComponent<Image>().sprite = GetSpriteByType(node.Type);
        }
    }

    private void ClearMap()
    {
        foreach (Transform child in mapContainer)
        {
            Destroy(child.gameObject);
        }
        nodeUIPositions.Clear();
    }
}
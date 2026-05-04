using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MapUIManager mapUIManager;

    [Header("UI Scroll Settings")]
    public RectTransform mapContent;
    public float topPadding = 300f; // 顶部留白

    [Header("Toggle Settings")]
    // 拖入包含整个地图 UI 的父级对象（通常是 Scroll View 本身，或者包裹它的 Panel）
    public GameObject mapUIPanel;

    // 记录地图当前的显示状态
    private bool isMapActive = true;

    void Start()
    {
        GenerateAndDrawMap();
    }
    void Update()
    {
        // 额外赠送：键盘快捷键支持 (按 M 键或 Tab 键开关地图)
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMap();
        }
    }
    /// <summary>
    /// 供外部 UI 按钮调用的开关方法
    /// </summary>
    public void ToggleMap()
    {
        isMapActive = !isMapActive;
        SetMapState(isMapActive);
    }

    /// <summary>
    /// 设置地图的具体可见状态
    /// </summary>
    private void SetMapState(bool active)
    {
        if (mapUIPanel != null)
        {
            mapUIPanel.SetActive(active);

            // 可选体验优化：每次打开地图时，让滚动条自动回到底部
            if (active && mapContent != null)
            {
                mapContent.anchoredPosition = new Vector2(mapContent.anchoredPosition.x, 0);
            }
        }
    }
    // 你可以将这个方法绑定到一个 UI 按钮上，用于重新生成地图
    //public void GenerateAndDrawMap()
    //{
    //    // 1. 生成纯逻辑地图
    //    mapGenerator.GenerateMap();

    //    // 2. 获取生成的有效节点
    //    var nodes = mapGenerator.GetGeneratedNodes();

    //    // 3. 动态调整 ScrollView Content 的高度，确保能滚到最上面
    //    // 高度 = (层数 - 1) * Y轴间距 + 顶部留白
    //    float requiredHeight = (mapGenerator.mapHeight - 1) * mapUIManager.nodeSpacingY + topPadding;
    //    mapContent.sizeDelta = new Vector2(mapContent.sizeDelta.x, requiredHeight);

    //    // 4. 将逻辑数据交给 UI 管理器进行绘制
    //    mapUIManager.DrawMap(nodes, mapGenerator.mapWidth);
    //}
    public void GenerateAndDrawMap()
    {
        // 1. 生成纯逻辑地图
        mapGenerator.GenerateMap();

        // 2. 获取生成的有效节点
        var nodes = mapGenerator.GetGeneratedNodes();

        // 3. 动态调整 ScrollView Content 的高度
        // 总高度 = (层数 - 1) * Y轴间距 + 顶部留白 + 底部留白
        float bottomPadding = 100f; // 额外增加一点底部留白，防止第一层贴边
        float topPadding = 300f;    // 确保最顶层的 Boss 房能完全显示并划到视野中间

        float requiredHeight = (mapGenerator.mapHeight - 1) * mapUIManager.nodeSpacingY + topPadding + bottomPadding;

        // 强制应用高度
        mapContent.sizeDelta = new Vector2(mapContent.sizeDelta.x, requiredHeight);

        // 可选：每次生成新地图时，将滚动条重置回最底部
        mapContent.anchoredPosition = new Vector2(mapContent.anchoredPosition.x, 0);

        // 4. 将逻辑数据交给 UI 管理器进行绘制
        mapUIManager.DrawMap(nodes, mapGenerator.mapWidth, mapGenerator.mapHeight);
    }
}
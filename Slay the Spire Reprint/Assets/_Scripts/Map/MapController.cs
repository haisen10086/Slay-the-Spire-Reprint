using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MapUIManager mapUIManager;

    [Header("UI Scroll Settings")]
    public RectTransform mapContent;
    public float rightPadding = 300f; // 顶部留白
    public RectTransform mapBackground;

    [Header("Toggle Settings")]
    // 拖入包含整个地图 UI 的父级对象（通常是 Scroll View 本身，或者包裹它的 Panel）
    public GameObject mapUIPanel;

    // 记录地图当前的显示状态
    private bool isMapActive = true;

    public static MapController Instance { get; private set; }
    private void Awake()
    {
        if(Instance !=  null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

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
    //    float requiredHeight = (mapGenerator.mapHeight - 1) * mapUIManager.nodeSpacingY + rightPadding;
    //    mapContent.sizeDelta = new Vector2(mapContent.sizeDelta.x, requiredHeight);

    //    // 4. 将逻辑数据交给 UI 管理器进行绘制
    //    mapUIManager.DrawMap(nodes, mapGenerator.mapWidth);
    //}
    public void GenerateAndDrawMap()
    {
        mapGenerator.GenerateMap();
        var nodes = mapGenerator.GetGeneratedNodes();

        // 动态计算所需的横向总宽度
        // 总宽度 = (总深度 - 1) * 深度间距 + 左留白 + 右留白
        float requiredWidth = (mapGenerator.mapWidth - 1) * mapUIManager.nodeSpacingX + mapUIManager.leftPadding + rightPadding;

        // 强制扩展 Content 的 Width，保留其本身的 Height
        mapContent.sizeDelta = new Vector2(requiredWidth, mapContent.sizeDelta.y);
        mapBackground.sizeDelta = new Vector2(requiredWidth, mapBackground.sizeDelta.y);

        // 每次重新生成地图，将 ScrollView 的滚动位置重置到最左侧
        mapContent.anchoredPosition = new Vector2(0, mapContent.anchoredPosition.y);

        // 调用 UI 绘制
        mapUIManager.DrawMap(nodes, mapGenerator.mapWidth, mapGenerator.mapHeight);
    }
}
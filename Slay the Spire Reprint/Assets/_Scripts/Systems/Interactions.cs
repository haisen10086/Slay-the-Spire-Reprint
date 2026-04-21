using UnityEngine;

public class Interactions : MonoBehaviour
{
    //单例
    public static Interactions Instance {  get; private set; }

    public bool PlayerIsDragging {  get; set; } = false;    //存储玩家是否正在拖拽状态

    //判断玩家是否可以交互，动作系统执行时不可交互
    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming) return true;
        else return false;
    }

    //判断玩家是否可以悬浮放大卡牌，拖拽时不可以
    public bool PlayerCanHover()
    {
        if (PlayerIsDragging) return false;
        else return true;
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }else
        {
            Instance = this;
        }
    }
}

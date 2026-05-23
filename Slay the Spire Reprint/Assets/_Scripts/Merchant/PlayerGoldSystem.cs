using TMPro;
using UnityEngine;

/// <summary>
/// 玩家金币管理
/// </summary>
public class PlayerGoldSystem : MonoBehaviour
{
    public static PlayerGoldSystem Instance {  get; private set; }
    // 当前金币
    public int currentGold = 300;
    [Header("UI")]
    [SerializeField] private TMP_Text goldText;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    /// <summary>
    /// 是否能够消费
    /// </summary>
    public bool CanAfford(int amount)
    {
        return currentGold >= amount;
    }

    /// <summary>
    /// 扣除金币
    /// </summary>
    public void SpendGold(int amount)
    {
        currentGold -= amount;

        // 防止出现负数
        if (currentGold < 0)
        {
            currentGold = 0;
        }
        RefreshGoldUI();
    }

    /// <summary>
    /// 增加金币
    /// </summary>
    public void AddGold(int amount)
    {
        currentGold += amount;
        RefreshGoldUI();
    }

    public void RefreshGoldUI()
    {
        goldText.text = "金币" + currentGold;
    }
}
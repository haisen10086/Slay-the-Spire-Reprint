using UnityEngine;

/// <summary>
/// 商店商品数据
/// </summary>
[System.Serializable]
public class MerchantItemData
{
    // 商品名称
    public string itemName;

    // 商品价格
    public int price;

    // 商品图标
    public Sprite icon;

    // 商品类型
    public MerchantItemType itemType;

    // 是否已售出
    public bool isSold;

    /// <summary>
    /// 构造函数
    /// </summary>
    public MerchantItemData(
        string name,
        int price,
        MerchantItemType type)
    {
        this.itemName = name;
        this.price = price;
        this.itemType = type;
        this.isSold = false;
    }
}
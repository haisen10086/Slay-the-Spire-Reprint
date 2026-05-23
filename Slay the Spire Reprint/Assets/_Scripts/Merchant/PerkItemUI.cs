using UnityEngine;
using UnityEngine.UI;

public class PerkItemUI : ItemUI
{
    [SerializeField] private Image image;
    [Header("Button")]
    [SerializeField] private Button buyButton;
 
    public Perk Perk {  get; private set; }

    public override object GetItemData()
    {
        return Perk;
    }

    public void SetUp(Perk perk, int price)
    {
        SetUpPeice(price);
        image.sprite = perk.Image;
        Perk = perk;

        // 注册按钮事件
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnClickBuy);
    }

    /// <summary>
    ///点击购买
    /// </summary>
    private void OnClickBuy()
    {
        MerchantSystem.Instance.TryBuyItem(this);
    }
}

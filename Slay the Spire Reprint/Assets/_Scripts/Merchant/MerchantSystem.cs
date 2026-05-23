using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 商店管理器
/// </summary>
public class MerchantSystem : MonoBehaviour
{
    public static MerchantSystem Instance { get; private set; }       //单例
    [Header("UI")]
    public TMP_Text goldText;
    public Transform MerchantPanelUI;
    public Transform MerchantUI;
    public Button SkipButton;

    [Header("Prefab")]
    public CardItemUI cardItemPrefab;
    public PerkItemUI perkItemPrefab;

    [Header("Parent")]
    public Transform cardItemContainer;
    public Transform perkItemContainer;


    // 当前商品列表
    private List<Card> currentCardItemData = new List<Card>();
    private List<Perk> currentPerkItemData = new List<Perk>();

    private int CardItemMaxCount = 7;
    private int PerkItemMaxCount = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        GenerateShopItems();

        CreateShopUI();

        //RefreshGoldUI();
    }
    /// <summary>
    /// 生成商店商品
    /// </summary>
    private void GenerateShopItems()
    {
        currentCardItemData.Clear();
        currentPerkItemData.Clear();

        for(int i= 0; i<CardItemMaxCount; i++)
        {
            Card card = CardSystem.Instance.GenerateCard();
            currentCardItemData.Add(card);
        }
        for(int i= 0; i<PerkItemMaxCount; i++)
        {
            Perk perk = PerkSystem.Instance.GeneratePerk();
            currentPerkItemData.Add(perk);
        }       
    }
    /// <summary>
    /// 创建商店 UI
    /// </summary>
    private void CreateShopUI()
    {
        // 清空旧商品
        for(int i= cardItemContainer.childCount-1; i>=0; i--)
        {
            Transform child = cardItemContainer.GetChild(i);
            Destroy(child.gameObject);
        }
        for(int i= perkItemContainer.childCount-1; i>=0; i--)
        {
            Transform child = perkItemContainer.GetChild(i);
            Destroy(child.gameObject);
        }

        //创建新商品
        foreach(Card card in currentCardItemData)
        {
            CardItemUI cardItemUI = Instantiate(cardItemPrefab, cardItemContainer);
            int price = ReturnRandomPrice(card);
            cardItemUI.Setup(card, price);
        }
        foreach(Perk perk in currentPerkItemData)
        {
            PerkItemUI perkItemUI = Instantiate(perkItemPrefab, perkItemContainer);
            //遗物价格随机100到300之间
            int price = UnityEngine.Random.Range(100, 300);
            perkItemUI.SetUp(perk, price);
        }
    }
    /// <summary>
    /// 随机价格
    /// </summary>
    private int ReturnRandomPrice(Card card)
    {
        int minPrice = 0;
        int maxPrice = 0;
        switch (card.CardRarityType)
        {
            case CardRarityType.Common:
                minPrice = 45;
                maxPrice = 55;
                break;
            case CardRarityType.UnCommon:
                minPrice = 68;
                maxPrice = 82;
                break;
            case CardRarityType.Rare:
                minPrice = 135;
                maxPrice = 165;
                break;
            default:
                break;
        }
        return UnityEngine.Random.Range(minPrice, maxPrice+1);
    }

    /// <summary>
    /// 尝试购买商品
    /// </summary>
    public void TryBuyItem(
        ItemUI itemUI)
    {

        // 金币不足
        if (!PlayerGoldSystem.Instance.CanAfford(itemUI.price))
        {
            Debug.Log("金币不足");
            //可添加金币不足提示
            return;
        }

        // 扣除金币
        PlayerGoldSystem.Instance.SpendGold(itemUI.price);

        // 刷新金币 UI
        //RefreshGoldUI();

        // 发放奖励
        StartCoroutine(GiveItemReward(itemUI));
    }
    /// <summary>
    /// 发放商品奖励
    /// </summary>
    private IEnumerator GiveItemReward(ItemUI itemUI)
    {

        object data = itemUI.GetItemData();
        Debug.Log($"data 的类型是：{data?.GetType()}, 值是：{data}");
        if (data is Card card)
        {
            //执行添加卡牌
            Debug.Log("购买了卡牌" + card.Title);
            HeroSystem.Instance.AddCard(card);
            //可以添加动画
            Tween tween1 = itemUI.transform.DOScale(Vector3.zero, 0.3f);
            Tween tween = itemUI.transform.DOMove(HeroSystem.Instance.DeckButtonUI.position, 0.3f);
            yield return tween.WaitForCompletion();
        }
        if (data is Perk perk)
        {
            //执行添加遗物
            Debug.Log("添加了遗物" + perk.Title);
            PerkSystem.Instance.AddPerk(perk);
            //可以添加动画
        }
        yield return null;
        Destroy(itemUI.gameObject);
    }
    /// <summary>
    /// 刷新金币 UI
    /// </summary>
    //private void RefreshGoldUI()
    //{
    //    goldText.text = "Gold: " +
    //                    playerGold.currentGold;
    //}

    /// <summary>
    /// 刷新商店
    /// </summary>
    public void RefreshShop()
    {
        // 花费金币刷新
        int refreshCost = 50;

        if (!PlayerGoldSystem.Instance.CanAfford(refreshCost))
        {
            Debug.Log("金币不足，无法刷新");
            return;
        }

        PlayerGoldSystem.Instance.SpendGold(refreshCost);

        GenerateShopItems();

        CreateShopUI();

        //RefreshGoldUI();
    }
    //显示商店界面
    public void MerchantPanelUIShow()
    {
        MerchantPanelUI.gameObject.SetActive(true);
    }
    //隐藏商店界面
    public void MerchantPanelUIHide()
    {
        MerchantPanelUI.gameObject.SetActive(false);
    }
    public void MerchantUIShow()
    {
        MerchantUI.gameObject.SetActive(true);
    }
    public void MerchantUIHide()
    {
        MerchantUI.gameObject.SetActive(false);
    }
}
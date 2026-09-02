using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradingCardUI : MonoBehaviour
{
    [Header("Prefab")]
    public CardItemNoGoldView CardItemNoGoldPrefab;
    public GameObject upgradeVFX;       //锻造特效
    [Header("Container")]
    public RectTransform cardUIContainer;
    public CardItemNoGoldView unUpgradeCardContainer;             //未升级的卡牌容器
    public CardItemNoGoldView upgradeCardContainer;               //升级后的卡牌容器
    [Header("UI")]
    public GameObject showUpgradeCardPanel;     //展示升级后的卡牌
    [field: SerializeField, Header("Button")] public Button UpgradeButton {  get;private set; }
    private bool isInit = false;

    private void Start()
    {
        //生成Card数据

        //生成CardItemNoGold
        //CreatCardUI();

        //在start添加委托，这样不管有没有被禁用都可以添加组件
        HeroSystem.Instance.AddCardEvent += Instance_AddCardEvent;

    }

    //每次英雄系统添加卡牌，
    private void Instance_AddCardEvent(object sender, HeroSystem.AddCardEventArgs e)
    {
        AddCardUI(e.card);
    }
    public void OnDestroy()
    {
        if(HeroSystem.Instance != null)
        {
            HeroSystem.Instance.AddCardEvent -= Instance_AddCardEvent;
        }
        
    }

    //生成CardItemNoGold
    private IEnumerator CreatCardUI()
    {
        //移除之前的卡牌UI
        for (int i = cardUIContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = cardUIContainer.GetChild(i);
            Destroy(child.gameObject);
            yield return null;
        }
        if (HeroSystem.Instance.Deck == null) Debug.Log("当前英雄卡牌为空");
        else Debug.Log("当前英雄卡牌不为空");
        Debug.Log($"Deck 中的卡牌数量: {HeroSystem.Instance.Deck.Count}");
        //从英雄系统拿当前卡牌实例数据；
        foreach (Card card in HeroSystem.Instance.Deck)
        {
            Debug.Log("增加卡牌UI" + card.Title);           
            AddCardUI(card);
        }
    }

    //每次英雄系统添加卡牌都会在这增加一次
    public void AddCardUI(Card card)
    {
        CardItemNoGoldView cardItemNoGoldView = Instantiate(CardItemNoGoldPrefab, cardUIContainer);
        cardItemNoGoldView.Setup(card, transform);
        cardItemNoGoldView.BuyButton.onClick.RemoveAllListeners();
        cardItemNoGoldView.BuyButton.onClick.AddListener(()=>ShowShowUpgradeCardPanel(cardItemNoGoldView));


        //每次添加CardUI都重新计算cardUIContainer大小
        int CountY = cardUIContainer.childCount / 3 + 1;
        cardUIContainer.sizeDelta = new Vector2(cardUIContainer.sizeDelta.x, CountY * 400);
    }


    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(KeepCardUIsIsUnupgrade());
        
    }

    private IEnumerator KeepCardUIsIsUnupgrade()
    {
        if (!isInit)
        {
            Debug.Log("初始化成功");
            yield return StartCoroutine(CreatCardUI());
            isInit = true;
        }
        //每次打开时，遍历所有卡牌ui，如果不能升级了，移除该卡牌UI
        for (int i = cardUIContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = cardUIContainer.GetChild(i);
            Card card = child.GetComponent<CardItemNoGoldView>().Card;
            //当卡牌已经升级且不能连续升级时
            if (card == null) { Debug.Log("card不存在"); }

                if (card.Upgraded && !card.CanBeUpgradeInfinitely)
                {
                    Destroy(child.gameObject);
                    yield return null;
                }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //显示展示升级卡牌界面
    public void ShowShowUpgradeCardPanel(CardItemNoGoldView cardItemNoGold)
    {
        unUpgradeCardContainer.Setup(cardItemNoGold.Card, transform);
        Vector3 position = unUpgradeCardContainer.transform.position;
        unUpgradeCardContainer.transform.position = cardItemNoGold.transform.position;
        unUpgradeCardContainer.transform.DOMove(position, 0.5f);

        //克隆一个卡牌，将他升级，最后传递数据给升级后卡牌容器
        Card card1 = cardItemNoGold.Card.Clone();
        card1.Upgrade();
        Debug.Log("克隆的卡牌描述文本为：" +  card1.Description);
        upgradeCardContainer.Setup(card1);        
        //记得移除点击事件
        unUpgradeCardContainer.BuyButton.onClick.RemoveAllListeners();
        upgradeCardContainer.BuyButton.onClick.RemoveAllListeners();

        showUpgradeCardPanel.SetActive(true);
    }
    //隐藏展示升级卡牌界面
    public void HideShowUpgradeCardPanel()
    {
        showUpgradeCardPanel.SetActive(false);
    }
    //确定升级，升级的同时将升级界面隐藏
    public void SureToUpgrade()
    {
        //unUpgradeCardContainer.UpgradeCard();
        //HideShowUpgradeCardPanel();
        StartCoroutine(SureToUpgradeAndVFX());
    }
    private IEnumerator SureToUpgradeAndVFX()
    {

        GameObject vfx = Instantiate(upgradeVFX, Vector3.zero, Quaternion.identity);
        //Destroy(vfx, 1.5f);
        yield return new WaitForSeconds(0.3f);
        Instantiate(upgradeVFX, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        unUpgradeCardContainer.UpgradeCard();
        HideShowUpgradeCardPanel();
    }

}

using UnityEngine;

public class HeroCurrentDeckUI : MonoBehaviour
{
    [Header("Prefab")]
    public CardItemNoGoldView CardItemNoGoldPrefab;
    [Header("Container")]
    public RectTransform cardUIContainer;

    private bool isInit = false;

    private void Start()
    {
        //生成Card数据

        //生成CardItemNoGold
        //CreatCardUI();
    }

    //生成CardItemNoGold
    private void CreatCardUI()
    {
        //移除之前的卡牌UI
        for(int i = cardUIContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = cardUIContainer.GetChild(i); 
            Destroy(child.gameObject);
        }
        if (HeroSystem.Instance.Deck == null) Debug.Log("当前英雄卡牌为空");
        else Debug.Log("当前英雄卡牌不为空");
        Debug.Log($"Deck 中的卡牌数量: {HeroSystem.Instance.Deck.Count}");
        //从英雄系统拿当前卡牌实例数据；
        foreach (Card card in HeroSystem.Instance.Deck)
        {
            Debug.Log("增加卡牌UI"+card.Title);
            AddCardUI(card);

        }
    }

    //每次英雄系统添加卡牌都会在这增加一次
    public void AddCardUI(Card card)
    {
        CardItemNoGoldView cardItemNoGoldView = Instantiate(CardItemNoGoldPrefab, cardUIContainer);
        cardItemNoGoldView.Setup(card);


        //每次添加CardUI都重新计算cardUIContainer大小
        int CountY = cardUIContainer.childCount / 3 + 1;
        cardUIContainer.sizeDelta = new Vector2(cardUIContainer.sizeDelta.x, CountY*400);
    }

    public void Show()
    {
        if(!isInit)
        {
            CreatCardUI();
            isInit = true;
        }
            
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

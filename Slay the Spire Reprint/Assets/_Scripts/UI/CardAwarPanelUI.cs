using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAwarPanelUI : MonoBehaviour
{
    
    [SerializeField] private CardItemNoGoldView cardItemNoGoldViewPrefab;   //预制体
    [SerializeField] private Transform container;               //容器
    [SerializeField] private Button SkipCardButton;
    private List<Card> cardAwards = new List<Card>();

    private void Start()
    {
        SkipCardButton.onClick.RemoveAllListeners();
        SkipCardButton.onClick.AddListener(Hide);
    }

    public void Show(List<Card> cardList)
    {
        GenerateCardAwardUI(cardList);
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //生成卡牌奖励UI
    public void GenerateCardAwardUI(List<Card> cardList)
    {
        //移除之前的
        for(int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);
            Destroy(child.gameObject);
        }
        //生成现在的
        foreach(Card card in cardList)
        {
            CardItemNoGoldView cardItemNoGoldView = Instantiate(cardItemNoGoldViewPrefab,container);
            cardItemNoGoldView.Setup(card, transform);
            
        }        
    }



}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeroSystem : MonoBehaviour
{
    //单例
    public static HeroSystem Instance {  get; private set; }

    //属性
    [field: SerializeField] public HeroView HeroView { get; private set; }                   //持有场景视图里英雄视图的引用

    public List<Card> Deck { get; private set; } = new();          //当前持有的卡牌

    [field: SerializeField] public HeroCurrentDeckUI CurrentDeckUI { get; private set; }        //当前持有卡牌可视化UI
    [field: SerializeField] public Transform DeckButtonUI { get; private set; }     //当前牌组查看按钮
    
    private bool DeckUIActive = false;
    //初始化
    public void Setup(HeroDataSO heroDataSO)
    {
        HeroView.Setup(heroDataSO);
        foreach(var cardDataSO in heroDataSO.Deck)
        {
            Card card = new(cardDataSO);
            Deck.Add(card);
        }
    }

    public void AddCard(Card card)
    {
        Deck.Add(card);
        CurrentDeckUI.AddCardUI(card);
    }

    public void DeckUIToggle()
    {
        DeckUIActive = !DeckUIActive;
        if (DeckUIActive) DeckUIShow();
        else DeckUIHide();
    }
    public void DeckUIShow()
    {
        CurrentDeckUI.Show();
    }
    public void DeckUIHide()
    {
        CurrentDeckUI.Hide();
    }




    private void Awake()
    {
        if(Instance != null && Instance !=  this)
        {
            Destroy(Instance);
        }else
        {
            Instance = this;
        }
    }
}

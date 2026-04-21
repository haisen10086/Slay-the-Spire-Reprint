using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    [SerializeField] private HandView handView; //引用手牌
    [SerializeField] private Transform drawPilePoint; //引用手牌
    [SerializeField] private Transform discardPilePoint; //引用手牌


    //三个列表分别存放抽牌堆，手牌堆，弃牌堆的实例数据
    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> handPile  = new();

    //单例
    public static CardSystem Instance {  get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance );
        }else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        //添加动作的执行者函数
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscordAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);

        //添加订阅反应的函数
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
        
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();

        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    //设置抽牌堆,也就是实例化卡牌数据
    public void SetUp(List<CardDataSO> deckData)
    {
        foreach(var  cardDataSO in deckData)
        {
            Card card = new(cardDataSO);
            drawPile.Add(card);
        }
    }

    //performer
    //抽牌执行者函数
    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardGA)
    {
        int actualAmount = Mathf.Min(drawCardGA.Amount, drawPile.Count);
        int notDrawnAmount = drawCardGA.Amount - actualAmount;
        for(int  i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if(notDrawnAmount > 0)
        {
            if(discardPile.Count <= 0)
            {
                Debug.Log("无法抽牌，抽牌堆和弃牌堆均为0");
                yield break;
            }
            //洗牌重抽
            RefillDeck();
            for(int i = 0;i < notDrawnAmount;i++)
            {
                yield return DrawCard();
            }
        }
    }
    //丢弃掉所有手牌执行者函数
    private IEnumerator DiscordAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        List<Card> cardsToDiscard = new(handPile);
        handPile.Clear();

        foreach(Card card in cardsToDiscard)
        {
            CardView cardView = handView.RemoveCard(card);
            yield return DiscordCard(cardView);
        }
    }
    //打出卡牌执行者函数
    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        handPile.Remove(playCardGA.Card);
        //discardPile.Add(playCardGA.Card);这个应该在弃牌动作完成
        CardView cardView = handView.RemoveCard(playCardGA.Card);
        yield return DiscordCard(cardView);


        SpendManaGA spendManaGA = new(playCardGA.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        if(playCardGA.Card.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGA = new(playCardGA.Card.ManualTargetEffect, new() { playCardGA.Manualtarget });
            ActionSystem.Instance.AddReaction(performEffectGA); 
        }
        foreach(var effectWrapper in playCardGA.Card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            //为每个效果创建执行效果的游戏动作
            PerformEffectGA performEffectGA = new(effectWrapper.Effect, targets);
            //打牌动作执行中的连锁反应，实现效果
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    //Reactions反应
    //敌人回合前预反应,丢弃所有手牌
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        DiscardAllCardsGA discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }
    //敌人执行回合结束后的反应，为回合抽牌
    private void EnemyTurnPostReaction(EnemyTurnGA deemyTurnGA)
    {
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }

    //Helpers
    //抽牌函数
    private IEnumerator DrawCard()
    {
        Card card = drawPile.Draw();
        if (card == null) yield break;
        handPile.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }
    //将所有弃牌堆的拍并入抽牌堆
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }

    //丢弃目标牌
    private IEnumerator DiscordCard(CardView cardView)
    {
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        //动画完成后才将牌加入弃牌堆
        discardPile.Add(cardView.Card);
        Destroy(cardView.gameObject);
    }

}

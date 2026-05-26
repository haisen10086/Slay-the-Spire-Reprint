using System.Collections;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public static BuffSystem Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }


    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddBuffGA>(AddBuffGAPerformer);
        ActionSystem.AttachPerformer<AtTurnEndBuffGA>(AtTurnEndBuffGAPerform);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddBuffGA>();
        ActionSystem.DetachPerformer<AtTurnEndBuffGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public IEnumerator AddBuffGAPerformer(AddBuffGA addBuffGA)
    {
        foreach (var target in addBuffGA.Targets)
        {
            target.AddBuff(addBuffGA.buff);
            Debug.Log("对"+ target.GetType()+ "addBuffGA");
            yield return null;//添加一些通用的添加状态动画

            //添加buff时,当且仅当目标为英雄时，刷新一下所有卡牌的伤害文本
            if (target != HeroSystem.Instance.HeroView)
            {
                foreach(var cardView in CardSystem.Instance.handView.cards)
                {
                    cardView.ReplaceDamageDescription();
                }
            }
        }
    }
    //敌人回合后反应
    public void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        AtTurnEndBuffGA atTurnEndBuffGA = new();
        ActionSystem.Instance.AddReaction(atTurnEndBuffGA);
    }
    public IEnumerator AtTurnEndBuffGAPerform(AtTurnEndBuffGA atTurnEndBuffGA)
    {
        //遍历敌人buff
        foreach (CombatantView enemy in EnemySystem.Instance.enemyViews)
        {
            for(int i=enemy.buffs.Count-1; i>=0; i--)
            {
                enemy.buffs[i].AtTurnEnd();
                yield return null;//可添加动画
            }
        }
        //遍历英雄buff
        foreach (BuffBase buff in HeroSystem.Instance.HeroView.buffs)
        {
            buff.AtTurnEnd();
            yield return null;//可添加动画
        }

    }
}

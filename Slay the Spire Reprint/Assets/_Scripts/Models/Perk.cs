using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Perk 
{
    public Sprite Image => data.Image;
    private readonly PerkDataSO data;
    private readonly PerkCondition condition;
    private readonly AutoTargetEffect effect;
    public string Title { get;private set; }

    private PerkUI ownerPerkUI;         //存储这个Perk实例对应的PerkUI实例

    //设置
    public void SetOwnerPerkUI(PerkUI perkUI)
    {
        ownerPerkUI = perkUI;
    }

    public Perk(PerkDataSO perkDataSO)
    {
        data = perkDataSO; 
        condition = data.PerkCondition;
        effect = data.AutoTargetEffect;
        Title = data.name;
    }

    public void OnAdd()
    {
        condition.SubscribeCondition(Reaction);
    }
    public void OnRemove()
    {
        condition.UnsubscribeCondition(Reaction);
    }

    private void Reaction(GameAction gameAction)
    {
        //先检查子条件是否满足
        if (condition.SubConditionIsMet(gameAction))
        {
            List<CombatantView> targets = new();
            //检查是否应将动作施法者设置为委托目标以及游戏动作是否有施法者
            if (data.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                targets.Add(haveCaster.Caster);
            }
            if (data.UseAutoTargetEffect)
            {
                targets.AddRange(effect.TargetMode.GetTargets());
            }
            GameAction perkEffeckAction = effect.Effect.GetGameAction(targets, HeroSystem.Instance.HeroView);
            ActionSystem.Instance.AddReaction(perkEffeckAction);
            if(ownerPerkUI !=  null)
            {
                Debug.Log("震动了");
                ownerPerkUI.SharkeUI();
            }
            
        }
    }
}

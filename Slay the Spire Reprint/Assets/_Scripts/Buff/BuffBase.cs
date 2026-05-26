using System;
using UnityEngine;

[Serializable]
public abstract class BuffBase
{
    public string BuffId;
    public string BuffName;
    public Sprite Icon;

    // 层数
    public int Amount;

    // 持有者
    protected CombatantView owner;


    public abstract BuffBase DeepClone();
    public virtual void OnApply(CombatantView target)
    {
        owner = target;
    }

    public virtual void OnRemove()
    {
        Amount = 0;
    }

    // 回合开始
    public virtual void AtTurnStart()
    {

    }

    // 回合结束
    public virtual void AtTurnEnd()
    {
        Amount--;

        if (Amount <= 0)
            owner.RemoveBuff(this);
        else
            owner.RefreshBuffUI(this);
    }

    // 打出卡牌
    public virtual void OnPlayCard(Card card)
    {

    }

    // 受到伤害前
    public virtual void ModifyDamageTaken(DamageInfo info)
    {

    }

    // 造成伤害前
    public virtual void ModifyDamageGive(DamageInfo info)
    {

    }

    // 每帧UI刷新文本
    public virtual string GetDescription()
    {
        return "";
    }

}
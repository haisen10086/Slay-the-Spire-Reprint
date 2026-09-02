using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [Tooltip("如果有卡牌，基础伤害由卡牌决定，如果没有卡牌,基础伤害由这个参数的决定")]
    [field: SerializeField] public int baseDamage { get; private set; }      //伤害值

    //通过伤害信息的计算，传递最终伤害,参数添加卡牌来源
    //如果有卡牌，基础伤害由卡牌决定，如果没有卡牌,基础伤害由这个参数的决定
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster, Card sourceCard = null)
    {
        if(sourceCard != null)
        {
            baseDamage = sourceCard.BaseDamage;
        }
        
        List<DamageInfo> damageList = new List<DamageInfo>();
        foreach(CombatantView target in targets)
        {
            DamageInfo damageInfo = new DamageInfo(caster, target, baseDamage);  
            
            DamageSystem.CalculateDamage(damageInfo);  
            damageList.Add(damageInfo);
        }

        DealDamageGA dealDamageGA = new(damageList);
        return dealDamageGA;
    }


}

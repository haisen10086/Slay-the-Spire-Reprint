using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [field: SerializeField] public int baseDamage { get; private set; }      //伤害值

    //通过伤害信息的计算，传递最终伤害
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
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

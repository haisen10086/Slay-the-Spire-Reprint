using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;      //…À∫¶÷µ

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
    
        DealDamageGA dealDamageGA = new(damageAmount, targets, caster);
        return dealDamageGA;
    }


}

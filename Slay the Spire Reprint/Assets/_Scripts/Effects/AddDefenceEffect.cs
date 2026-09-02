using System.Collections.Generic;
using UnityEngine;

public class AddDefenceEffect : Effect
{
    public int defenceAmount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster, Card sourceCard = null)
    {
        if (sourceCard != null) defenceAmount = sourceCard.BaseBlock;
        AddDefenceGA addDefenceGA = new AddDefenceGA(caster, defenceAmount);
        return addDefenceGA;
    }
}

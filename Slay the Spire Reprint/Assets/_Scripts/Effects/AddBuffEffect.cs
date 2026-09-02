using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 添加buff效果，可返回添加buff游戏动作
/// </summary>
public class AddBuffEffect : Effect
{
    [field: SerializeReference, SR] public BuffBase Buff { get; private set; } = null;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster, Card sourceCard = null)
    {
        if(sourceCard != null)
        {
            Buff.Amount = sourceCard.BaseMagic;
        }
        return new AddBuffGA(Buff, targets);
    }
}

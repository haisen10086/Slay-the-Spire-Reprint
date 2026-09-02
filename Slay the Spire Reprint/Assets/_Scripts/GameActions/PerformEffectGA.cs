using System.Collections.Generic;
using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect Effect { get; set; }
    public List<CombatantView> Targets { get; set; }
    public Card sourceCard;
    public PerformEffectGA(Effect effect, List<CombatantView> targets, Card sourceCard)
    {
        Effect = effect;
        Targets = targets == null ? null : new(targets);
        this.sourceCard = sourceCard;
    }
}

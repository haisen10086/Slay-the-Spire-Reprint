using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public abstract class Effect 
{
    public abstract GameAction GetGameAction(List<CombatantView> targets, CombatantView caster, Card sourceCard = null);    //效果需要传递攻击目标和施法者
}

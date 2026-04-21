using System.Collections.Generic;
using UnityEngine;

//造成伤害游戏动作
public class DealDamageGA : GameAction,IHaveCaster
{
    //属性
    public int Amount {  get;private set; }     //伤害数值
    public List<CombatantView> Targets{ get; private set; }     //攻击目标列表

    public CombatantView Caster {  get; private set; }

    public DealDamageGA(int amount, List<CombatantView> targets, CombatantView caster)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
    }
}

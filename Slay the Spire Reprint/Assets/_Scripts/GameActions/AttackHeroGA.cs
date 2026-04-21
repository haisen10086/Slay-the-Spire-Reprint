using UnityEngine;

//攻击敌人游戏动作
public class AttackHeroGA : GameAction, IHaveCaster
{
    //默认攻击目标是英雄，所以不需要游戏目标
    //属性
    public EnemyView Attacker { get; private set; }         //攻击者

    public CombatantView Caster {  get; private set; }      //施法者

    public AttackHeroGA(EnemyView attacker)
    {
        Attacker = attacker;
        Caster = Attacker;
    }
}

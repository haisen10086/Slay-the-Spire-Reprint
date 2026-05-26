using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 伤害信息：攻击者，目标，基础伤害，当前伤害，伤害卡牌来源
/// </summary>
public class DamageInfo
{
    public CombatantView attacker;

    public CombatantView target;

    public int baseDamage;

    public int currentDamage;

    public Card sourceCard;

    /// <summary>
    /// 参数：攻击者，目标，基础伤害，伤害卡牌来源
    /// </summary>
    public DamageInfo(CombatantView attacker, CombatantView target, int baseDamage, Card sourceCard)
    {
        this.attacker = attacker;
        this.target = target;
        this.baseDamage = baseDamage;
        this.sourceCard = sourceCard;
    }
    /// <summary>
    /// 参数：
    /// </summary>
    public DamageInfo()
    {

    }
    /// <summary>
    /// 参数：攻击者，目标，基础伤害
    /// </summary>
    public DamageInfo(CombatantView attacker, CombatantView target, int baseDamage)
    {
        this.attacker = attacker;
        this.target = target;
        this.baseDamage = baseDamage;
        this.currentDamage = baseDamage;
    }
}
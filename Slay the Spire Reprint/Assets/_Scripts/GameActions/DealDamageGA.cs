using System.Collections.Generic;
using UnityEngine;

//造成伤害游戏动作，存储一个伤害信息列表
public class DealDamageGA : GameAction,IHaveCaster
{
    //属性
    //public List<int> DamageList {  get;private set; }     //伤害数值
    //public List<CombatantView> Targets{ get; private set; }     //攻击目标列表

    public List<DamageInfo> damageInfoList;          //伤害信息列表

    public CombatantView Caster {  get; private set; }

    public DealDamageGA(List<DamageInfo> damageInfoList)
    {

        this.damageInfoList = damageInfoList;
        //DamageList = DamageList;
        //Targets = new(targets);
        //Caster = caster;
    }
}

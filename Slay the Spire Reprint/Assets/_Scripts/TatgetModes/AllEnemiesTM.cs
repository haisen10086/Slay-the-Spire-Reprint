using System.Collections.Generic;
using UnityEngine;

//返回全部敌人目标列表
public class AllEnemiesTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        return new(EnemySystem.Instance.enemyViews);
    }
}

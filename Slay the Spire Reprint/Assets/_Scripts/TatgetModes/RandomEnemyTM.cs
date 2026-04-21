using System.Collections.Generic;
using UnityEngine;

//返回随机敌人目标
public class RandomEnemyTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        //从所有敌人目标中随机选择一个
        CombatantView target = EnemySystem.Instance.enemyViews[Random.Range(0, EnemySystem.Instance.enemyViews.Count)];
        return new() { target };
    }


}

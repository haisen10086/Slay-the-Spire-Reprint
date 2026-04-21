using UnityEngine;

public class KillEnemyGA : GameAction
{
    //属性
    public EnemyView TargetEnemyView { get; private set; }        //目标敌人
    
    public KillEnemyGA(EnemyView targetEnemyView)
    {
        TargetEnemyView = targetEnemyView;
    }
}

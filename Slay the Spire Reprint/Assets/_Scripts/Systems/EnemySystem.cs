using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    //引用
    [SerializeField] private EnemyBoardView enemyBoardView;                           //引用敌人棋盘视图

    public List<EnemyView> enemyViews => enemyBoardView.EnemyViews;                   //返回

    //单例
    public static EnemySystem Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    //初始化敌人系统，创建多个敌人视图
    public void Setup(List<EnemyDataSO> enemyDataSOs)
    {
        Debug.Log("开始加载敌人列表");
        foreach(var enemyDataSO in enemyDataSOs)
        {
            Debug.Log("加载敌人"+ enemyDataSO.id);
            enemyBoardView.AddEnemy(enemyDataSO);
        }
    }
    //销毁所有敌人
    public void RemoveAllEnemyView()
    {
        //StartCoroutine(enemyBoardView.RemoveAllEnemyView());
        for (int i = enemyBoardView.EnemyViews.Count - 1; i >= 0; i--)
        {
            KillEnemyGA killEnemyGA = new KillEnemyGA(enemyViews[i]);
            Debug.Log("杀死敌人:" + killEnemyGA.TargetEnemyView.myEnemyDataSO.id);
            ActionSystem.Instance.Perform(killEnemyGA);
        }
        Debug.Log("当前场景敌人数量:"+enemyBoardView.EnemyViews.Count);
    }
    //等待特定时间
    public void WaitTime(float duration)
    {
        float x=0;
        while(x < duration)
        {
            x += Time.deltaTime;
        }
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        Debug.Log("进入敌人回合");
        //遍历所有敌人，并添加相应攻击英雄反应
        foreach(var enemyView in enemyBoardView.EnemyViews)
        {
            AttackHeroGA attackHeroGA = new(enemyView);
            ActionSystem.Instance.AddReaction(attackHeroGA);
        }
        yield return null;
    }

    //攻击英雄动作执行者函数
    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        EnemyView attacker = attackHeroGA.Attacker;
        //攻击效果（动画）
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.25f);
        yield return tween.WaitForCompletion();
        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);
        //造成伤害(添加造成伤害动作进反应里，反应列表会执行这个造成伤害动作)
        DealDamageGA dealDamageGA = new(attacker.AttckPower, new() { HeroSystem.Instance.HeroView },attackHeroGA.Caster);
        ActionSystem.Instance.AddReaction(dealDamageGA);


    }

    //杀死敌人动作执行这函数
    public IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return enemyBoardView.RemoveEnemyView(killEnemyGA.TargetEnemyView);

        //每次杀死敌人都判断以下敌人棋盘是否为空,为空就加载奖励界面,
        if(enemyBoardView.EnemyViews.Count == 0)
        {
            AwardSystem.Instance.AwardShow();
            CardSystem.Instance.ReMoveAllPileAddReaction();
        }

    }
}

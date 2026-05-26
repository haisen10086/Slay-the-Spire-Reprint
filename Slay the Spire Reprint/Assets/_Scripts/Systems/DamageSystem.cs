using System.Collections;
using UnityEngine;

//管理伤害
public class DamageSystem : MonoBehaviour
{
    //引用
    [SerializeField] private GameObject damageVFX;                    //伤害特效


    //函数
    //unity运行函数
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }


    //执行者函数
    public IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        //遍历攻击目标列表，造成生命值减少和生成伤害特效
        foreach(var damageInfo in dealDamageGA.damageInfoList)
        {
            if(damageInfo.target == null)      //假如积累过多对同一目标的伤害动作（作为某个动作的链式动作时有可能），
                                    //会造成目标死亡后仍访问原目标进行伤害动作，而目标死亡会销毁
                                    //为防止访问空对象，需判断目标是否为空
            {
                yield break;
            }
            damageInfo.target.Damage(damageInfo.currentDamage);
            Instantiate(damageVFX, damageInfo.target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);

            //判断目标血量是否为0
            if(damageInfo.target.CurrentHealth <= 0)
            {
                if(damageInfo.target is EnemyView)
                {
                    //如果敌人血量为0，执行杀死敌人动作
                    KillEnemyGA killEnemyGA = new((EnemyView)damageInfo.target);
                    ActionSystem.Instance.AddReaction(killEnemyGA);
                }
                else
                {
                    //玩家血量为0，执行结束逻辑
                }
            }
        }
    }

    //专门的伤害计算器
    public static int CalculateDamage(DamageInfo info)
    {
        // 攻击者Buff
        foreach (var buff in info.attacker.buffs)
        {
            buff.ModifyDamageGive(info);
        }
        if(info.target != null)
        {
            // 目标Buff
            foreach (var buff in info.target.buffs)
            {
                buff.ModifyDamageTaken(info);
            }
        }


        Debug.Log("当前伤害为：" + info.currentDamage);

        return Mathf.Max(0, info.currentDamage);
    }

}

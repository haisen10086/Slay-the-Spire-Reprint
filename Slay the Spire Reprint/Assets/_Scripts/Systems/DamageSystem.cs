using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//管理伤害
public class DamageSystem : MonoBehaviour
{
    //引用
    [SerializeField] private GameObject damageVFX;                    //伤害特效
    [SerializeField] private TMP_Text damageTextVFX;                //伤害文本特效


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

            //生成伤害文本动画
            //Vector3 vector3 = damageInfo.target.transform.clickPosition;
            //TMP_Text dmgTextVFX = Instantiate(TextVFX, vector3- Vector3.up, Quaternion.identity);
            //dmgTextVFX.text = damageInfo.currentDamage.ToString();
            //Tween tween =  dmgTextVFX.gameObject.transform.DOMove(vector3 + Vector3.up, 0.5f);
            //yield return tween;
            //Tween tween1 = dmgTextVFX.gameObject.transform.DOMove(vector3 - Vector3.up, 0.5f);
            //yield return tween1;
            // 生成伤害文本动画
            //StartCoroutine(ShowDamage(damageInfo));
            CombatFeedbackSystem.Instance.ShowDamageText(damageInfo);

            damageInfo.target.Damage(damageInfo.currentDamage);
            //GameObject vfx = Instantiate(damageVFX, damageInfo.target.transform.clickPosition, Quaternion.identity);
            CombatFeedbackSystem.Instance.ShowDamageVFX(damageInfo);
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

    //伤害文本特效
    public IEnumerator ShowDamage(DamageInfo damageInfo)
    {

        Vector3 pos = damageInfo.target.transform.position;

        // 随机一点偏移，避免数字重叠
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(-0.1f, 0.1f),
            0
        );

        TMP_Text dmgText = Instantiate(
            damageTextVFX,
            pos + randomOffset,
            Quaternion.identity
        );

        dmgText.text = damageInfo.currentDamage.ToString();

        Transform tf = dmgText.transform;

        // 初始状态
        Vector3 startScale = tf.localScale;
        tf.localScale = Vector3.zero;

        Color color = dmgText.color;
        color.a = 1f;
        dmgText.color = color;

        // 目标位置
        Vector3 targetPos = tf.position + Vector3.up * 2f;

        // Sequence
        DG.Tweening.Sequence seq = DOTween.Sequence();

        // 1. 出现爆开
        seq.Append(
            tf.DOScale(startScale *1.3f, 0.12f)
                .SetEase(Ease.OutBack)
        );

        // 2. 回弹到正常大小
        seq.Append(
            tf.DOScale(startScale *1f, 0.08f)
        );

        // 3. 上浮（和后续并行）
        seq.Join(
            tf.DOMove(targetPos, 0.8f)
                .SetEase(Ease.OutQuad)
        );

        // 4. 淡出
        seq.Join(
            dmgText.DOFade(0f, 0.8f).SetEase(Ease.InQuad)
        );

        // 5. 微旋转（可选）
        seq.Join(
            tf.DORotate(
                new Vector3(0, 0, Random.Range(-10f, 10f)),
                0.2f
            )
        );

        // 自动销毁
        seq.OnComplete(() =>
        {
            Destroy(dmgText.gameObject);
        });
        yield return seq.WaitForCompletion();
    }

    IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }
}

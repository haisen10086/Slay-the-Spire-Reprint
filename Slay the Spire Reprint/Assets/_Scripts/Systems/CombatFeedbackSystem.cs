using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatFeedbackSystem : MonoBehaviour
{
    [Header("VFX")]
    public GameObject damageVFX;
    public TMP_Text TextVFX;
    public SpriteRenderer buffSprite;

    [Header("UI")]
    public TurnPanelUI turnPanelUI;


    public static CombatFeedbackSystem Instance {  get; private set; }

    private Coroutine hitStopCoroutine;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance );
        }
        Instance = this;
    }
    public void PlayHitStop(float duration)
    {
        if (hitStopCoroutine != null)
        {
            StopCoroutine(hitStopCoroutine);
        }

        hitStopCoroutine = StartCoroutine(HitStop(duration));
    }

    private IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0.05f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }

    public void ShowDamageText(DamageInfo damageInfo)
    {
        StartCoroutine(DamageText(damageInfo));
    }

    public IEnumerator DamageText(DamageInfo damageInfo)
    {

        Vector3 pos = damageInfo.target.transform.position;

        // 随机一点偏移，避免数字重叠
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(-0.1f, 0.1f),
            0
        );

        TMP_Text dmgText = Instantiate(
            TextVFX,
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


        Vector3 dir = new Vector3(
            Random.Range(-0.5f, 0.5f),
            1f,
            0
        ).normalized;
        // 目标位置
        Vector3 targetPos = tf.position + dir * 2f;

        // Sequence
        DG.Tweening.Sequence seq = DOTween.Sequence();

        // 1. 出现爆开
        seq.Append(
            tf.DOScale(startScale * 1.3f, 0.12f)
                .SetEase(Ease.OutBack)
        );

        // 2. 回弹到正常大小
        seq.Append(
            tf.DOScale(startScale * 1f, 0.08f)
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
    public void ShowBuffText(CombatantView target, string text)
    {
        StartCoroutine(BuffText(target, text));
    }
    public IEnumerator BuffText(CombatantView target, string text)
    {

        //Vector3 pos = buff.GetOwner().transform.position;
        Vector3 pos = target.transform.position;

        //// 随机一点偏移，避免数字重叠
        //Vector3 randomOffset = new Vector3(
        //    Random.Range(-0.3f, 0.3f),
        //    Random.Range(-0.1f, 0.1f),
        //    0
        //);

        TMP_Text BuffText = Instantiate(
            TextVFX,
            pos,
            Quaternion.identity
        );

        BuffText.text = text;

        Transform tf = BuffText.transform;

        // 初始状态
        Vector3 startScale = tf.localScale;
        tf.localScale = Vector3.zero;

        Color color = BuffText.color;
        color.a = 1f;
        BuffText.color = color;


        //Vector3 dir = new Vector3(
        //    Random.Range(-0.5f, 0.5f),
        //    1f,
        //    0
        //).normalized;
        // 目标位置
        Vector3 targetPos = tf.position +Vector3.up * 2f;

        // Sequence
        DG.Tweening.Sequence seq = DOTween.Sequence();

        // 1. 出现爆开
        seq.Append(
            tf.DOScale(startScale * 1.3f, 0.12f)
                .SetEase(Ease.OutBack)
        );

        // 2. 回弹到正常大小
        seq.Append(
            tf.DOScale(startScale * 1f, 0.08f)
        );

        // 3. 上浮（和后续并行）
        seq.Join(
            tf.DOMove(targetPos, 0.8f)
                .SetEase(Ease.OutQuad)
        );

        // 4. 淡出
        seq.Join(
            BuffText.DOFade(0f, 0.8f).SetEase(Ease.InQuad)
        );

        //// 5. 微旋转（可选）
        //seq.Join(
        //    tf.DORotate(
        //        new Vector3(0, 0, Random.Range(-10f, 10f)),
        //        0.2f
        //    )
        //);

        // 自动销毁
        seq.OnComplete(() =>
        {
            Destroy(BuffText.gameObject);
        });
        yield return seq.WaitForCompletion();
    }

    public void ShowDamageVFX(DamageInfo damageInfo)
    {
        StartCoroutine(DamageVFX(damageInfo));
    }

    public IEnumerator DamageVFX(DamageInfo damageInfo)
    {
        GameObject vfx = Instantiate(damageVFX, damageInfo.target.transform.position, Quaternion.identity);
        yield return vfx;
    }

    //buff特效
    public IEnumerator BuffSpriteVFX( CombatantView target, Sprite icon)
    {
        yield return null;
        SpriteRenderer spriteRenderer =  Instantiate(buffSprite, target.transform.position, Quaternion.identity);
        spriteRenderer.sprite = icon;
        spriteRenderer.transform.DOScale(Vector3.one * 2f, 1f);      // 放大到 2 倍
        spriteRenderer.DOFade(0f, 1f);                // 透明度降到 0（完全透明）s

        Destroy(spriteRenderer.gameObject, 1f);

    }
    public void ShowBuffSpriteVFX(CombatantView target, Sprite icon)
    {
        StartCoroutine(BuffSpriteVFX(target, icon));
    }

    //显示回合名
    public void ShowTurnText(string text)
    {
        turnPanelUI.turnText.text = text;

        turnPanelUI.PlayTurnStartAnimation();
        
    }

    /// <summary>
    /// 播放动画：放大到1.1倍 + 变淡 → 恢复原样
    /// </summary>
    public void PlayPopAnimation(Image image)
    {
        Transform tf = image.transform;
        Vector3 originalScale = image.transform.localScale;
        Color originalColor = image.color;
        // 杀死当前物体上的所有动画，防止冲突
        tf.DOKill();
        image.DOKill();

        // 创建动画序列
        Sequence seq = DOTween.Sequence();

        // 第一阶段：放大到1.1倍 + 透明度降到0.7（同时进行）
        seq.Join(tf.DOScale(originalScale * 1.2f, 0.25f).SetEase(Ease.OutQuad));
        seq.Join(image.DOColor(new Color(originalColor.r, originalColor.g, originalColor.b, 0.4f), 0.25f));

        // 第二阶段：恢复原大小 + 恢复原透明度（同时进行）
        seq.Append(tf.DOScale(originalScale, 0.25f).SetEase(Ease.InQuad));
        seq.Join(image.DOColor(originalColor, 0.25f));

        seq.Play();
    }


}

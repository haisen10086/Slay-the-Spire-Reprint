using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnPanelUI : MonoBehaviour
{
    [Header("组件引用")]
    public Image panelImage;      // TurnPanelUI 的 Image 组件
    public TMP_Text turnText;         // TurnText 的 Text 组件

    [Header("动画参数")]
    public float fadeInDuration = 0.5f;   // 出现时长
    public float displayDuration = 1.5f;  // 停留时长
    public float fadeOutDuration = 0.5f;  // 消失时长

    private CanvasGroup canvasGroup;

    void Awake()
    {
        // 获取或添加 CanvasGroup（用于控制整体透明度）
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 初始状态：完全透明
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 播放回合开始动画（出现 -> 停留 -> 消失）
    /// </summary>
    public void PlayTurnStartAnimation()
    {
        StopAllCoroutines();
        // 激活面板
        gameObject.SetActive(true);
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        //// 激活面板
        //gameObject.SetActive(true);

        // 1. 淡入（出现）
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, fadeInDuration).SetEase(Ease.OutQuad);

        // 等待淡入完成
        yield return new WaitForSeconds(fadeInDuration);

        // 2. 停留显示
        yield return new WaitForSeconds(displayDuration);

        // 3. 淡出（消失）
        canvasGroup.DOFade(0f, fadeOutDuration).SetEase(Ease.InQuad);

        // 等待淡出完成
        yield return new WaitForSeconds(fadeOutDuration);

        // 隐藏面板
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 立即隐藏（不播放动画）
    /// </summary>
    public void HideImmediately()
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
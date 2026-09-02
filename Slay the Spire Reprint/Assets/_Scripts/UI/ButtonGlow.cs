using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image glowImage;

    private void Start()
    {
        // 初始完全透明
        glowImage.color = new Color(1, 1, 1, 0);
    }

    // 鼠标进入按钮
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowGlow();
    }

    // 鼠标离开按钮
    public void OnPointerExit(PointerEventData eventData)
    {
        HideGlow();
    }

    public void ShowGlow()
    {
        glowImage.DOFade(1, 0.15f);
    }

    public void HideGlow()
    {
        glowImage.DOFade(0, 0.15f);
    }
}
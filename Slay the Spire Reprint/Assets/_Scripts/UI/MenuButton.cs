using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    Vector3 originScale;
    Vector3 originPos;
    public Image glowImage;

    private void Awake()
    {
        originScale = transform.localScale;
        originPos = transform.localPosition;
        glowImage.color = new Color(0, 0, 0, 0);
    }

    // 鼠标进入
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();

        transform.DOScale(
            originScale * 1.1f,
            0.15f);

        ShowGlow();

        //transform.DOLocalMoveX(
        //    originPos.x + 20,
        //    0.15f);
    }

    // 鼠标离开
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();

        transform.DOScale(
            originScale,
            0.15f);

        HideGlow();

        //transform.DOLocalMoveX(
        //    originPos.x,
        //    0.15f);
    }

    // 点击
    public void OnPointerClick(PointerEventData eventData)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOScale(
                originScale * 0.9f,
                0.05f));

        seq.Append(
            transform.DOScale(
                originScale * 1.1f,
                0.1f));
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
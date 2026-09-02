using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardViewHoverSystem : MonoBehaviour
{
    public static CardViewHoverSystem Instance {  get; private set; }
    [SerializeField] private CardView cardViewHover;

    [Header("Material")]
    [Tooltip("正常状态材质（默认无效果）")]
    public Material normalMaterial;
    [Tooltip("鼠标悬停时的材质（可修改Shader实现高亮等效果）")]
    public Material hoverMaterial;
    private Image uiImage;
    private Material originalMaterial;
    private Vector3 startScale;
    private Vector3 startPosition;

    //单例模式
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("当前存在多个CardViewHoverSystem实例");
        }
        Hide(); //初始隐藏悬停卡片
    }
    //显示
    public void Show(Card card, Vector3 position)
    {
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card);
        cardViewHover.transform.position = position;
    }
    //隐藏
    public void Hide()
    {
        cardViewHover.gameObject.SetActive(false);
    }

    public void ReSetCardViewHoverDescription(string description)
    {
        cardViewHover.SetDescription(description);
    }

    public void ShowCardViewHover(CardView cardView)
    {
        cardView.transform.DOKill();

        cardView.transform.DOScale(
            cardView.hoverStartScale * 1.1f,
            0.15f
        );

        cardView.transform.DOMove(
            cardView.hoverStartPosition + Vector3.up,
            0.15f
        );

        cardView.transform.rotation = Quaternion.Euler( 0f, 0f, 0f );

        if (hoverMaterial != null)
        {
            cardView.SetMaterial(hoverMaterial);
        }
    }

    public void HideCardViewHover(CardView cardView)
    {
        cardView.transform.DOKill();

        cardView.transform.DOScale(
            cardView.hoverStartScale,
            0.15f
        );

        cardView.transform.DOMove(
            cardView.hoverStartPosition,
            0.15f
        );

        cardView.transform.DORotate(
            cardView.hoverStartRotation.eulerAngles,
            0.15f
        );

        cardView.SetMaterial(cardView.hoverOriginalMaterial);

    }
}

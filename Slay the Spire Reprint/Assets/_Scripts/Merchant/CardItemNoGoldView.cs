using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItemNoGoldView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text CardTypeText;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private Image image;
    [SerializeField] private Transform DeckButton;
    [Header("Button")]
    [field: SerializeField] public Button BuyButton {  get;private set; }
    public Card Card { get; private set; }
    [Header("Material")]
    [Tooltip("正常状态材质（默认无效果）")]
    public Material normalMaterial;
    [Tooltip("鼠标悬停时的材质（可修改Shader实现高亮等效果）")]
    public Material hoverMaterial;
    private Image uiImage;
    private Material originalMaterial;
    private bool isPressed = false;


    private void Awake()
    {
        uiImage = GetComponent<Image>();
        originalMaterial = uiImage.material;
        if(normalMaterial == null )
            normalMaterial = originalMaterial;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isPressed)
        {
            transform.DOScale(1.1f, 0.15f);
            if (hoverMaterial != null)
            {
                SetMaterial(hoverMaterial);
            }
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed)
        {
            transform.DOScale(1.0f, 0.15f);
            if (normalMaterial != null)
            {
                SetMaterial(normalMaterial);
            }
        }
        
    }
    private void SetMaterial(Material mat)
    {
        if (uiImage != null)
            uiImage.material = mat;
    }
    private void OnDestroy()
    {
        // 恢复原始材质，避免修改后残留
        SetMaterial(originalMaterial);
    }

    public void Setup(Card card)
    {
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        image.sprite = card.Image;
        CardTypeText.text = CardSystem.GetChinceseTypeText(card.CardType);
        this.Card = card;

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(CarryCardAward);

    }
    //
    private void CarryCardAward()
    {
        StartCoroutine(CarryCard());
    }
    //得到卡牌，同时执行动画
    public IEnumerator CarryCard()
    {
        isPressed = true;
        //LayoutGroup parentLayout = GetComponentInParent<LayoutGroup>();
        //if (parentLayout) parentLayout.enabled = false;
        transform.SetParent(transform.parent.parent, true);
        //动画
        Tween tween1 = transform.DOScale(Vector3.zero, 0.3f);
        Tween tween = transform.DOMove(HeroSystem.Instance.DeckButtonUI.position, 0.3f);
        yield return tween.WaitForCompletion();
        //将卡牌示例数据添加到英雄系统的牌组中
        HeroSystem.Instance.AddCard(Card);
        AwardSystem.Instance.CardAwardPanelUIHide();
        AwardSystem.Instance.DestroyWaitDestroyAwardView();
        Destroy(gameObject);
    }


}

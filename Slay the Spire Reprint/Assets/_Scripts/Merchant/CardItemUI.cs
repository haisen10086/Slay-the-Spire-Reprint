using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItemUI : ItemUI, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text CardTypeText;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private Image image;
    [Header("Button")]
    [SerializeField] private Button buyButton;
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
        if (normalMaterial == null)
            normalMaterial = originalMaterial;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPressed)
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

    public override object GetItemData()
    {
        return Card;
    }

    public void Setup(Card card, int price)
    {
        SetUpPeice(price);
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        image.sprite = card.Image;
        CardTypeText.text = CardSystem.GetChinceseTypeText(card.CardType);
        this.Card = card;

        // 注册按钮事件
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnClickBuy);
    }
    /// <summary>
    ///点击购买
    /// </summary>
    private void OnClickBuy()
    {
        MerchantSystem.Instance.TryBuyItem(this);
    }


}

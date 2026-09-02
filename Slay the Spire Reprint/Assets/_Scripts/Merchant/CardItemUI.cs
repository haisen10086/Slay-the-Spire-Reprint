using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
            FingerSelectUI.Instance.MoveToSelectItem(transform.position);
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
            FingerSelectUI.Instance.MoveToSelectItem(FingerSelectUI.Instance.StartPosition);
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
        this.Card = card;
        SetUpPeice(price);
        title.text = card.Title;
        description.text = card.Description;

        ReplaceDamageDescription();
        ReplaceMagicDescription();
        ReplaceBlockDescription();
        mana.text = card.Mana.ToString();
        image.sprite = card.Image;
        CardTypeText.text = CardSystem.GetChinceseTypeText(card.CardType);

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
    //伤害信息替换函数,将伤害信息的最终伤害替换伤害描述里的伤害文本
    public string ReplaceDamageDescription(CombatantView target = null)
    {
        string finalDescription = description.text.Replace("{damage}", GetPreviewDamage(target).ToString());
        this.description.text = finalDescription;
        return finalDescription;
    }
    //防御信息替换函数,将文本里的防御值替换
    public string ReplaceBlockDescription()
    {
        string finalDescription = description.text.Replace("{block}", ColorUtil.ColorText(Card.BaseBlock.ToString(), "green"));

        this.description.text = finalDescription;
        return finalDescription;
    }
    //魔法数字信息替换函数,将文本里的魔法数字替换
    public string ReplaceMagicDescription()
    {
        string finalDescription = description.text.Replace("{magic}", ColorUtil.ColorText(Card.BaseMagic.ToString(), "green"));

        this.description.text = finalDescription;
        return finalDescription;
    }

    //获得预览伤害,先查找手动目标伤害，
    public int GetPreviewDamage(CombatantView target = null)
    {
        DamageInfo info = new DamageInfo()
        {
            attacker = HeroSystem.Instance.HeroView,
            target = target,
            baseDamage = Card.BaseDamage,
            currentDamage = Card.BaseDamage,
            sourceCard = this.Card
        };
        //if (Card.ManualTargetEffect != null && Card.ManualTargetEffect is DealDamageEffect dealDamageEffect)
        //{
        //    info.baseDamage = dealDamageEffect.baseDamage;
        //    info.currentDamage = info.baseDamage;
        //}
        //else
        //{
        //    foreach(var autoTargetEffect in  Card.OtherEffects)
        //    {
        //        if(autoTargetEffect.Effect is DealDamageEffect dealDamageEffect1)
        //        {
        //            info.baseDamage = dealDamageEffect1.baseDamage;
        //            info.currentDamage = info.baseDamage;
        //        }
        //    }
        //}


        int calculateDamage = DamageSystem.CalculateDamage(info);
        return calculateDamage;
    }

}

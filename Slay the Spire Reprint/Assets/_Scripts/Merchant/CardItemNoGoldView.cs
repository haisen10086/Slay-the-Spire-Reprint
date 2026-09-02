using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    public Transform parentUI;      //作为物品所依附的PenalUI对象


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
    //初始化
    public void Setup(Card card)
    {
        this.Card = card;
        title.text = card.Title;
        description.text = card.Description;
        Debug.Log("没有价格卡牌物品修改前的描述文本为：" + description.text);
        ReplaceDamageDescription();
        ReplaceBlockDescription();
        ReplaceMagicDescription();
        Debug.Log("没有价格卡牌物品修改后的描述文本为：" + description.text);
        //description.text = card.Description;
        mana.text = card.Mana.ToString();
        image.sprite = card.Image;
        CardTypeText.text = CardSystem.GetChinceseTypeText(card.CardType);

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(CarryCardAward);

    }
    //将父UI赋值给parentUI，赋值Card
    public void Setup(Card card, Transform parentUI)
    {
        this.Card = card;
        title.text = card.Title;
        description.text = card.Description;
        ReplaceDamageDescription();        
        ReplaceBlockDescription();
        ReplaceMagicDescription();

        //description.text = card.Description;
        mana.text = card.Mana.ToString();
        image.sprite = card.Image;
        CardTypeText.text = CardSystem.GetChinceseTypeText(card.CardType);

        this.parentUI = parentUI;

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
        int calculateDamage = DamageSystem.CalculateDamage(info);
        return calculateDamage;
    }

    public void UpgradeCard()
    {
        Card.Upgrade();
        ReplaceDamageDescription();
        //升级完后将依附的升级UI界面关闭,同时禁用升级卡牌按钮
        if (parentUI != null)
        {
            UpgradingCardUI upgradingCardUI = parentUI.GetComponent<UpgradingCardUI>();
            upgradingCardUI.Hide();
            upgradingCardUI.UpgradeButton.interactable = false;
        }
        else Debug.Log("parentUI未赋值，没将该物品的所依附的父UI赋值");
    }


}

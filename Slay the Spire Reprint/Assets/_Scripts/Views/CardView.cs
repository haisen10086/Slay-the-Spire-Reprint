using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class CardView : MonoBehaviour
{
    //卡牌基本数据，卡牌名，描述，消耗，精灵渲染器，包裹器
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text CardTypeText;
    [SerializeField] private TMP_Text mana;

    [SerializeField] private SpriteRenderer imageSR;

    [SerializeField] private GameObject wrapper;

    [SerializeField] private LayerMask dropAreaLayer;

    [SerializeField] private SortingGroup sortingGroup;

    public Card Card { get; private set; }
    private Vector3 dragStartPotion;    //记录拖拽时的初始位置
    private Quaternion dragStartRotation;   //记录拖拽时的初始四元数
    private bool IsTestingManualTarget = false;

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        //创建卡牌时，第一次替换伤害文本
        string finalDescription = card.Description.Replace("{damage}", GetPreviewDamage().ToString());
        description.text = finalDescription;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
        CardTypeText.text = CardSystem.GetChinceseTypeText(card.CardType);
    }
    //伤害信息替换函数,将伤害信息的最终伤害替换伤害描述里的伤害文本
    public string ReplaceDamageDescription(CombatantView target = null)
    {
        string finalDescription = Card.Description.Replace("{damage}", GetPreviewDamage(target).ToString());
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
            baseDamage = 0,
            currentDamage = 0,
            sourceCard = this.Card
        };
        if (Card.ManualTargetEffect != null && Card.ManualTargetEffect is DealDamageEffect dealDamageEffect)
        {
            info.baseDamage = dealDamageEffect.baseDamage;
            info.currentDamage = info.baseDamage;
        }
        else
        {
            foreach(var autoTargetEffect in  Card.OtherEffects)
            {
                if(autoTargetEffect.Effect is DealDamageEffect dealDamageEffect1)
                {
                    info.baseDamage = dealDamageEffect1.baseDamage;
                    info.currentDamage = info.baseDamage;
                }
            }
        }


        int calculateDamage = DamageSystem.CalculateDamage(info);
        Debug.Log("计算之后函数给出的伤害为"+calculateDamage);
        Debug.Log("计算之后DamageInfo的currentDamage为："+info.currentDamage);
        return calculateDamage;
    }

    //获得描述文本
    public string GetDescription()
    {
        return description.text;
    }
    //设置描述文本
    public void SetDescription(string description)
    {
        this.description.text = description;
    }

    ////根据卡牌类型获得中文版文字
    //public static string GetChinceseTypeText(CardType cardType)
    //{
    //    switch (cardType)
    //    {
    //        case CardType.Attack:
    //            return "攻击";
    //        case CardType.Skill:
    //            return "技能";
    //        case CardType.Power:
    //            return "能力";
    //        default:
    //            break;
    //    }
    //    return "";
    //}

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new Vector3(transform.position.x, -2.5f, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }
    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }
    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        Interactions.Instance.PlayerIsDragging = true;
        if (Card.ManualTargetEffect != null)        //检查是否有手动瞄准目标
        {
            //有手动选择时，将卡牌设置为悬浮状态
            wrapper.SetActive(false);
            Vector3 pos = new Vector3(transform.position.x, -2.5f, 0);
            CardViewHoverSystem.Instance.Show(Card, pos);

            manualTargetSystem.Instance.StartTargeting(transform.position);
            sortingGroup.sortingOrder++;
            //正在手动选择卡牌目标，将IsTestingManualTarget设为true
            IsTestingManualTarget = true;
        }
        else
        {
            //Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            dragStartPotion = transform.position;
            dragStartRotation = transform.rotation;

            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        }

        

    }
    //当松开卡牌时有判定对象时，将这张牌打出，否则迅速返回原位
    private void OnMouseUp()
    {

        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)        //判断是否含有手动瞄准目标效果
        {
            //鼠标松开，卡牌悬浮消失
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();

            EnemyView target = manualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                Debug.Log("打出卡牌" + Card.Title);
                PlayCardGA playCardGA = new(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
            sortingGroup.sortingOrder--;
            //手动选择卡牌目标结束，将IsTestingManualTarget设为true
            IsTestingManualTarget = false;
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana)
            && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropAreaLayer))
            {
                Debug.Log("打出卡牌" + Card.Title);
                PlayCardGA playCardGA = new(Card);
                ActionSystem.Instance.Perform(playCardGA);

            }
            else
            {
                transform.position = dragStartPotion;
                transform.rotation = dragStartRotation;
            }
            //Interactions.Instance.PlayerIsDragging = false; 
        }
        Interactions.Instance.PlayerIsDragging = false;
    }
    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    private void Update()
    {
        //当开始拖动箭头时，时刻计算当前鼠标是否有可攻击目标，如果有，给目标加上框框，同时更新卡牌伤害文本
        if(IsTestingManualTarget)
        {
            Debug.Log("开始持续监测是否选中目标");
            EnemyView currentEnemyView = manualTargetSystem.Instance.TestingTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));

            //加上框框
            //更新文本
            ReplaceDamageDescription(currentEnemyView);
            Debug.Log("选中目标修改伤害文本");
            Debug.Log("当前拥有目标后的伤害文本为："+ GetDescription());

            CardViewHoverSystem.Instance.ReSetCardViewHoverDescription(GetDescription());
        }
        
    }
}

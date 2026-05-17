using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CardView : MonoBehaviour
{
    //卡牌基本数据，卡排名，描述，消耗，精灵渲染器，包裹器
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description
        ;
    [SerializeField] private TMP_Text mana;

    [SerializeField] private SpriteRenderer imageSR;

    [SerializeField] private GameObject wrapper;

    [SerializeField] private LayerMask dropAreaLayer;

    [SerializeField] private SortingGroup sortingGroup;

    public Card Card { get; private set; }
    private Vector3 dragStartPotion;    //记录拖拽时的初始位置
    private Quaternion dragStartRotation;   //记录拖拽时的初始四元数

    public void Setup(Card card)
    {
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
        Card = card;
    }

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
        if(Card.ManualTargetEffect != null)        //检查是否有手动瞄准目标
        {
            manualTargetSystem.Instance.StartTargeting(transform.position);
            sortingGroup.sortingOrder++;
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
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
            EnemyView target = manualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                Debug.Log("打出卡牌" + Card.Title);
                PlayCardGA playCardGA = new(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
            sortingGroup.sortingOrder--;
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
            Interactions.Instance.PlayerIsDragging = false; 
        }
        
    }
    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public class CardViewHoverSystem : MonoBehaviour
{
    public static CardViewHoverSystem Instance {  get; private set; }
    [SerializeField] private CardView cardViewHover;
    
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
}

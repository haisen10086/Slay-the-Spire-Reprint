using DG.Tweening;
using UnityEngine;

public class CardViewCreator : MonoBehaviour
{
    //单例模式,设置静态实例，为了方便任何地方都能访问创建卡片，设为单例
    public static CardViewCreator Instance { get; private set; }
    //保存实例
	private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    [SerializeField] private CardView cardViewPrefab;

    //通过卡牌数据创建卡片
    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation )
    {
        CardView cardView = Instantiate( cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, 0.15f);
        cardView.Setup(card);
        return cardView;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//奖励系统
public class AwardSystem : MonoBehaviour
{
    public static AwardSystem Instance {  get; private set; }   //单例

    [SerializeField] private AwardView awardViewPrefab;         //奖品预制件
    private List<CardDataSO> allCardsDataSO;   //所有卡牌数据
    private List<PerkDataSO> allPerksDataSO;   //所有遗物数据

    [SerializeField] private AwardUI awardUI;                   //引用奖励界面UI
    [SerializeField] private CardAwarPanelUI cardAwarPanelUI;   //引用卡牌奖励UI
    [SerializeField] private TMP_Text skipAwardButtonText;       //引用跳过按钮的文本UI，方便修改
    [SerializeField] private Sprite coinAwardSprite;            //金币奖励图示
    [SerializeField] private Sprite cardAwardSprite;            //卡片奖励图示
    [SerializeField] private Sprite perkAwardSprite;            //遗物奖励图示

    public CoinAward CurrentCoinAward { get; private set; }
    public CardAward CurrentCardAward { get; private set; }
    public PerkAward CurrentPerkAward { get; private set; }

    private bool isAwardActive = true;
    //待销毁的AwardView
    public AwardView WaitDestroyAwardView {  get; private set; }
    public void SetUpWaitDestroyAwardView(AwardView awardView)
    {
        WaitDestroyAwardView = awardView;
    }
    public void DestroyWaitDestroyAwardView()
    {
        if (WaitDestroyAwardView != null)
            Destroy(WaitDestroyAwardView.gameObject);
    }

    public void AwardShow()
    {
        ChangeButtonTextToSkipAward();
        awardUI.Show();
    }

    public void AwardHide()
    {        
        awardUI.Hide();        
    }
    
    //显示卡牌奖励界面
    public void CardAwardPanelUIShow(List<Card> cards)
    {
        cardAwarPanelUI.Show(cards);
    }
    //隐藏卡牌奖励界面
    public void CardAwardPanelUIHide()
    {
        cardAwarPanelUI.Hide();
    }
    //当奖励全被拿走时，将SkipButtonText设置为继续前进
    public void ChangeButtonTextToAdvance(int newCount)
    {
        if( newCount == 0)
        {
            skipAwardButtonText.text = "继续前进";
        }        
    }
    public int GetAwardContentChildCount()
    {
        return awardUI.AwardComtent.childCount;
    }
    public void ChangeButtonTextToSkipAward()
    {
        skipAwardButtonText.text = "跳过奖励";
    }


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;

        allCardsDataSO = AllDataSystem.Instance.AllCardsDataSO;
        allPerksDataSO = AllDataSystem.Instance.PerkPoolDataSO.PerkPool;
        //CurrentCoinAward = GenerateCoinAward();
        //CurrentCardAward = GenerateCardAward();
        //CurrentPerkAward = GeneratePerkAward();
    }

    private void OnEnable()
    {
        //ActionSystem.AttachPerformer<UpdateAwardByRoomTypeGA>(UpdateAwardsByRoomTypePerformer);
    }
    private void OnDisable()
    {
        //ActionSystem.DetachPerformer<UpdateAwardByRoomTypeGA>();
    }

    private void Start()
    {
        //CurrentCoinAward = GenerateCoinAward();
        //CurrentCardAward = GenerateCardAward();
        //CurrentPerkAward = GeneratePerkAward();
    }

    public void UpdateAwardsByRoomType(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Unknown:
                break;
            case RoomType.Monster:
                CurrentCoinAward = GenerateCoinAward();
                CurrentCardAward = GenerateCardAward();
                break;
            case RoomType.Elite:
                CurrentCoinAward = GenerateCoinAward();
                CurrentCardAward = GenerateCardAward();
                CurrentPerkAward = GeneratePerkAward();
                break;
            case RoomType.Rest:
                break;
            case RoomType.Merchant:
                break;
            case RoomType.Treasure:
                CurrentCoinAward = GenerateCoinAward();
                CurrentPerkAward = GeneratePerkAward();
                AwardShow();
                break;
            case RoomType.Mystery:
                break;
            case RoomType.Boss:
                break;
            default:
                break;
        }
        //yield return null;
    }
    //生成随机奖励
    private CoinAward GenerateCoinAward()
    {
        CoinAward coinAward = new CoinAward();
        int coinAmount = Random.Range(15, 30);
        coinAward.Setup(coinAmount, coinAmount.ToString() + "金币", AwardType.GetCoin, coinAwardSprite);
        return coinAward;
    }
    private CardAward GenerateCardAward()
    {
        CardAward cardAward = new CardAward();
        List<Card> cards = new List<Card>();
        for(int i=0; i<3; i++)
        {
            Card card = CardSystem.Instance.GenerateCard();
            cards.Add(card);
        }
        
        //CardDataSO randomCardDataSO = allCardsDataSO[Random.Range(0, allCardsDataSO.Count)];
        //Card card = new Card(randomCardDataSO);
        cardAward.Setup(cards, "将一张卡牌加入你的牌组中", AwardType.GetCard, cardAwardSprite);
        return cardAward;
    }
    private PerkAward GeneratePerkAward()
    {
        PerkAward perkAward = new PerkAward();
        PerkDataSO randomPerkDataSO = allPerksDataSO[Random.Range(0, allPerksDataSO.Count)];
        Perk perk = new Perk(randomPerkDataSO);
        perkAward.Setup(perk, perk.Title, AwardType.GetPerk, perkAwardSprite);
        return perkAward;
    }
}

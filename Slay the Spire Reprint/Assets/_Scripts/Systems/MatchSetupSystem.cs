using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    ////所有卡牌数据
    //[SerializeField] private List<CardDataSO> allCardsDataSO;
    //[SerializeField] private List<PerkDataSO> allPerksDataSO;
    
    public HeroDataSO heroDataSO;     //英雄数据
    public List<EnemyDataSO> enemyDataSO; //敌人数据列表
    public List<PerkDataSO> perkDataSOs;         //遗物数据
    public int Coin{ get; set; }                     //金币数据
    public int Layer { get; set; }                //初始层数
    public RoomType CurrentRoomType = RoomType.Monster;   //当前房间类型


    public static MatchSetupSystem Instance {  get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance!= this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    private void Start()
    {
        HeroSystem.Instance.Setup(heroDataSO);
        //EnemySystem.Instance.Setup(enemyDataSO);    
        //CardSystem.Instance.SetUp(heroDataSO.Deck);
        foreach(var perkDataSO in  perkDataSOs)
        {
            PerkSystem.Instance.AddPerk(new Perk(perkDataSO));
        }

        //DrawCardsGA drawCardsGA = new(5);
        //ActionSystem.Instance.Perform(drawCardsGA);

        CurrentRoomType = RoomType.Monster;


    }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnterRoomGA>(EnterRoomPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnterRoomGA>();
    }

    //设置房间数据，改变敌人数据和当前房间类型
    public void SetupRoomData(EncounterDataSO EncounterEnemys, RoomType roomType)
    {
        Debug.Log($"SetupRoomData 收到列表，Count = {EncounterEnemys?.EnemyDataSos.Count ?? -1}");
        if(EncounterEnemys != null)
            enemyDataSO = EncounterEnemys.EnemyDataSos;
        else enemyDataSO = new List<EnemyDataSO>();
        CurrentRoomType = roomType;
    }

    //进入房间函数，进行战斗初始化
    private IEnumerator EnterRoomPerformer(EnterRoomGA enterRoomGA)
    {
        //清除上场战斗数据
        //EnemySystem.Instance.RemoveAllEnemyView();
        //CardSystem.Instance.ReMoveAllPileAddReaction();

        if (enemyDataSO == null) Debug.Log("当前敌人数据为空");
        else Debug.Log("当前敌人数据不为空");
        Debug.Log("敌人数据数量：" + enemyDataSO.Count);
        EnemySystem.Instance.Setup(enemyDataSO);
        CardSystem.Instance.SetUpClonedCard(HeroSystem.Instance.Deck);

        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
        Debug.Log("进入房间后抽牌");
        yield  return null;
        //可以加入进入房间动画
    }



}

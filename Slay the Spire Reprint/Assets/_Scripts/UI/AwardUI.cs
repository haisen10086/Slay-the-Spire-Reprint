using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//根据所处场景设置奖励
public class AwardUI : MonoBehaviour
{
    [SerializeField] private AwardView awardViewPrefab;
    [SerializeField] private Transform AwardComtent;

    private void Start()
    {
        //按照房间类型生成奖励
        //Show();
        
    }
    //显示的同时按照房间生成随机的奖励
    public void Show()
    {
        Debug.Log("基于当前房间类型："+MatchSetupSystem.Instance.CurrentRoomType.ToString()+"生成随机奖励");
        //按房间类型生成奖励
        GenerateAwardByRoomType(MatchSetupSystem.Instance.CurrentRoomType);

        //显示
        gameObject.SetActive(true);
    }

    //
    private void GenerateAwardByRoomType(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Unknown:
                break;
            case RoomType.Monster:
                GenerateMonsterAwardView();
                break;
            case RoomType.Elite:
                GenerateEliteAwardView();
                break;
            case RoomType.Rest:
                break;
            case RoomType.Merchant:
                break;
            case RoomType.Treasure:
                GenerateTreasureAwardView();
                break;
            case RoomType.Mystery:
                break;
            case RoomType.Boss:
                break;
            default:
                break;
        }
    }

    private void GenerateMonsterAwardView()
    {
        AwardView coinAwardView = Instantiate(awardViewPrefab, AwardComtent);
        coinAwardView.Setup(AwardSystem.Instance.CurrentCoinAward);
        AwardView cardAwardView = Instantiate(awardViewPrefab, AwardComtent);
        cardAwardView.Setup(AwardSystem.Instance.CurrentCardAward);
    }
    private void GenerateEliteAwardView()
    {
        AwardView coinAwardView = Instantiate(awardViewPrefab, AwardComtent);
        coinAwardView.Setup(AwardSystem.Instance.CurrentCoinAward);
        AwardView cardAwardView = Instantiate(awardViewPrefab, AwardComtent);
        cardAwardView.Setup(AwardSystem.Instance.CurrentCardAward);
        AwardView perkAwardView = Instantiate(awardViewPrefab, AwardComtent);
        perkAwardView.Setup(AwardSystem.Instance.CurrentPerkAward);
    }
    private void GenerateTreasureAwardView()
    {
        AwardView coinAwardView = Instantiate(awardViewPrefab, AwardComtent);
        coinAwardView.Setup(AwardSystem.Instance.CurrentCoinAward);
        AwardView perkAwardView = Instantiate(awardViewPrefab, AwardComtent);
        perkAwardView.Setup(AwardSystem.Instance.CurrentPerkAward);
    }
    //需要生成金卡
    private void GenerateBossAwardView()
    {
        AwardView coinAwardView = Instantiate(awardViewPrefab, AwardComtent);
        coinAwardView.Setup(AwardSystem.Instance.CurrentCoinAward);
        AwardView cardAwardView = Instantiate(awardViewPrefab, AwardComtent);
        cardAwardView.Setup(AwardSystem.Instance.CurrentCardAward);
    }


    //隐藏的同时将销毁未领取的所有奖励
    public void Hide()
    {
        gameObject.SetActive(false);
        //foreach(Transform child in AwardComtent)
        //{
        //    Destroy(child.gameObject);
        //}
        for (int i = AwardComtent.childCount - 1; i >= 0; i--)
        {
            Transform child = AwardComtent.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}

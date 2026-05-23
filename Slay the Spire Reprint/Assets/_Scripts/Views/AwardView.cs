using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 奖励的视觉表现
/// </summary>
public class AwardView : MonoBehaviour
{
    public Award Award {  get; private set; }
    [SerializeField] private TMP_Text awardText;    //奖励文本
    [SerializeField] private Button myButton;       //引用按钮
    [SerializeField] private Image awardImage;      //引用奖励图片

    //初始化
    public void Setup(Award award)
    {
        Award = award;
        awardText.text = award.awardText;
        myButton.onClick.AddListener(OnButtonClick);
        awardImage.sprite = award.awardSprite;
    }

    //添加奖励的点击事件
    public void OnButtonClick()
    {
        switch (Award)  
        {
            case CardAward cardAward:
                Debug.Log("cardAward.cardAwardList.Count=" + cardAward.cardAwardList.Count);
                Debug.Log("奖励牌分别是:");
                for(int i = 0; i<3; i++)
                {
                    Debug.Log(cardAward.cardAwardList[i].Title);
                }
                AwardSystem.Instance.CardAwardPanelUIShow(cardAward.cardAwardList);
                AwardSystem.Instance.SetUpWaitDestroyAwardView(this);
                break;
            case CoinAward coinAward:
                Debug.Log("获得金币：" + coinAward.coinAmount);
                MatchSetupSystem.Instance.Coin += coinAward.coinAmount;
                Destroy(this.gameObject);
                break;
            case PerkAward perkAward:
                Debug.Log("获得遗物：" + perkAward.perkAward);
                PerkSystem.Instance.AddPerk(perkAward.perkAward);
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
        //Destroy(this.gameObject);
        int newCount = AwardSystem.Instance.GetAwardContentChildCount()-1;
        AwardSystem.Instance.ChangeButtonTextToAdvance(newCount);
    }
    void OnDestroy()
    {
        if (myButton != null)
            myButton.onClick.RemoveAllListeners();
    }
}

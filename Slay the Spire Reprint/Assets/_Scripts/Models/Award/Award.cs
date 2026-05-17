using UnityEngine;

public class Award
{
    public AwardType awardType;    //奖励类型
    public string awardText;             //奖励文本
    public Sprite awardSprite;           //奖励图片
    
   public void SetupBase(AwardType awardType, string awardText,Sprite awardSprite)
    {
        this.awardText = awardText;
        this.awardType = awardType;
        this.awardSprite = awardSprite;
    }
}

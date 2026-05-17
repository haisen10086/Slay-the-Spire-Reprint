using UnityEngine;

public class CoinAward : Award
{
    public int coinAmount;
    public void Setup(int coinAmount, string awardText, AwardType awardType, Sprite awardSprite)
    {
        this.coinAmount = coinAmount;
        SetupBase(awardType, awardText, awardSprite);
    }
}

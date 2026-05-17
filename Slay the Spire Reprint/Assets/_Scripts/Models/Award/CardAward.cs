using UnityEngine;

public class CardAward : Award
{
    public Card cardAward;

    public void Setup(Card cardAward, string awardText, AwardType awardType, Sprite awardSprite)
    {
        this.cardAward = cardAward;
        SetupBase(awardType, awardText, awardSprite);
    }

}

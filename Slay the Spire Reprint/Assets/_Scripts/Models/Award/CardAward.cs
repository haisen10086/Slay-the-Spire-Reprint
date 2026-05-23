using System.Collections.Generic;
using UnityEngine;

public class CardAward : Award
{
    public List<Card> cardAwardList;

    public void Setup(List<Card> cardAwardList, string awardText, AwardType awardType, Sprite awardSprite)
    {
        this.cardAwardList = cardAwardList;
        SetupBase(awardType, awardText, awardSprite);
    }

}

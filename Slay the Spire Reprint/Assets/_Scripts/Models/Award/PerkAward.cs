using UnityEngine;

public class PerkAward : Award
{
    public Perk perkAward;
    public void Setup(Perk perkAward, string awardText, AwardType awardType, Sprite awardSprite)
    {
        this.perkAward = perkAward;
        SetupBase(awardType, awardText, awardSprite);
    }
}

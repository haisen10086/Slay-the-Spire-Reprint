using UnityEngine;

public class HeroView : CombatantView
{
    public void  Setup(HeroDataSO heroDataSO)
    {
        SetupBase(heroDataSO.Health, heroDataSO.Image);
    }
}

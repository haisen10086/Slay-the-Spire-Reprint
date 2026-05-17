using System.Collections.Generic;
using UnityEngine;

public class HeroView : CombatantView
{
    public List<CardDataSO> Deck {  get; set; }
    public void  Setup(HeroDataSO heroDataSO)
    {
        SetupBase(heroDataSO.Health, heroDataSO.Image);
        Deck = heroDataSO.Deck;
    }
}

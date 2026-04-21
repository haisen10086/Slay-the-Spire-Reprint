using System.Collections.Generic;
using UnityEngine;

//¿¨ÅÆÄ£ÐÍ
public class Card 
{
    public string Title => cardData.name;
    public string Description => cardData.Description;
    public Sprite Image => cardData.Image;
    public Effect ManualTargetEffect => cardData.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => cardData.OtherEffects;  

    public int Mana {  get; private set; }


    private readonly CardDataSO cardData;
     public Card(CardDataSO cardDataSO)
    {
        cardData = cardDataSO;
        Mana = cardDataSO.Mana;
    }
}

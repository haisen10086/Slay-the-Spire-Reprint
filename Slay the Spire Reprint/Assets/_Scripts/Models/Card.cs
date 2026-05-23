using System.Collections.Generic;
using UnityEngine;

//¿¨ÅÆÄ£ÐÍ
public class Card 
{
    public string Title => CardData.name;
    public string Description => CardData.Description;
    public CardType CardType => CardData.CardType;
    public CardRarityType CardRarityType => CardData.CardrarityType;
    public Sprite Image => CardData.Image;
    public Effect ManualTargetEffect => CardData.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => CardData.OtherEffects;  

    public int Mana {  get; private set; }


    public  CardDataSO CardData {  get; private set; }
     public Card(CardDataSO cardDataSO)
    {
        CardData = cardDataSO;
        Mana = cardDataSO.Mana;
    }

    public Card Clone()
    {
        Card clone = new Card(this.CardData);
        clone.Mana = this.Mana;
        return clone;
    }


}

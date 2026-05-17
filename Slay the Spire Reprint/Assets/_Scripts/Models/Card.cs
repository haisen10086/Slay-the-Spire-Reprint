using System.Collections.Generic;
using UnityEngine;

//¿¨ÅÆÄ£ÐÍ
public class Card 
{
    public string Title => CardData.name;
    public string Description => CardData.Description;
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
}

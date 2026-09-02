using System.Collections.Generic;
using UnityEngine;

//卡牌模型
public class Card 
{
    public string Description => CardData.Description;
    public CardType CardType => CardData.CardType;
    public CardRarityType CardRarityType => CardData.CardrarityType;
    public Sprite Image => CardData.Image;
    public Effect ManualTargetEffect => CardData.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => CardData.OtherEffects;

    public bool CanBeUpgradeInfinitely => CardData.CanBeUpgradeInfinitely;

    public string Title { get; private set; }
    public int BaseDamage {  get;private set; }
    public int BaseBlock { get; private set; }
    public int BaseMagic { get; private set; }

    public int Mana {  get; private set; }

    //卡牌是否升级,默认没升级
    public bool Upgraded {  get; private set; }=false;


    public  CardDataSO CardData {  get; private set; }
     public Card(CardDataSO cardDataSO)
    {
        CardData = cardDataSO;
        Mana = cardDataSO.Mana;
        BaseDamage = cardDataSO.baseDamage;
        BaseBlock = cardDataSO.baseBlock;
        BaseMagic = cardDataSO.baseMagic;
        Title = CardData.name;
    }

    public Card Clone()
    {
        Card clone = new Card(this.CardData);
        clone.Mana = this.Mana;
        clone.BaseDamage = this.BaseDamage;
        clone.BaseBlock = this.BaseBlock;
        clone.BaseMagic = this.BaseMagic;
        clone.Title = this.Title;
        Debug.Log("原来的卡牌描述文本为：" + Description);

        return clone;
    }
    //设置为升级过了
    public  void Upgrade()
    {
        //当已经升级过且不能连续升级时，直接返回，否则继续升级
        if(Upgraded && !CanBeUpgradeInfinitely) return;
        Upgraded = true;
        BaseDamage += CardData.upgradeDamage;
        BaseBlock += CardData.upgradeBlock;
        BaseMagic += CardData.upgradeMagic;
        Mana += CardData.upgradeMana;
        Title += CardData.upgradeName;
       
    }


}

using System;
using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    [TextArea]
    public string Description;
    [field : SerializeField] public int Mana {  get; private set; }
    [field : SerializeField] public Sprite Image { get; private set; }
    [field : SerializeReference, SR] public Effect ManualTargetEffect { get; private set; } = null;
    [field : SerializeField]  public List<AutoTargetEffect> OtherEffects { get; private set; }
    [field : SerializeField] public CardType CardType { get; private set; }
    [field : SerializeField] public CardRarityType CardrarityType { get; private set; }

    public int baseDamage;
    public int baseBlock;
    public int baseMagic;


    public int upgradeDamage;
    public int upgradeBlock;
    public int upgradeMagic;
    public int upgradeMana;
    public string upgradeName = "+";
    [Tooltip("是否可以无限升级")]
    public bool CanBeUpgradeInfinitely = false;




}

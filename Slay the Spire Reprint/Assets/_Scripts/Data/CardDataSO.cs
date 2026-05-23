using System;
using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    [field : SerializeField] public string Description {  get; private set; }
    [field : SerializeField] public int Mana {  get; private set; }
    [field : SerializeField] public Sprite Image { get; private set; }
    [field : SerializeReference, SR] public Effect ManualTargetEffect { get; private set; } = null;
    [field : SerializeField]  public List<AutoTargetEffect> OtherEffects { get; private set; }
    [field : SerializeField] public CardType CardType { get; private set; }
    [field : SerializeField] public CardRarityType CardrarityType { get; private set; }


}

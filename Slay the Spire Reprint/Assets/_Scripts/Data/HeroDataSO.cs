using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/HeroDataSO")]
public class HeroDataSO : ScriptableObject
{
    //英雄具有三个属性：图像，生命值，卡组
    [field : SerializeField] public Sprite Image {  get;private set; }
    [field : SerializeField] public int Health { get;private set; }
    [field : SerializeField] public List<CardDataSO> Deck {  get; private set; }

}

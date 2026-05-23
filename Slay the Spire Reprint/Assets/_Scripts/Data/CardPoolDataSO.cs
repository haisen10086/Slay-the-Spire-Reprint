using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/CardPoolDataSO")]
public class CardPoolDataSO : ScriptableObject
{
    [field: SerializeField] public List<CardDataSO> CommonCardPool {  get;private set; }  //ÆÕÍ¨¿¨ÅÆ
    [field: SerializeField] public List<CardDataSO> UncommonCardPool {  get;private set; }    //º±¼û¿¨ÅÆ
    [field: SerializeField] public List<CardDataSO> RareCardPool {  get;private set; }    //Ï¡ÓÐ¿¨ÅÆ

}

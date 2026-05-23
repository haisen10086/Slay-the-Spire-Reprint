using System.Collections.Generic;
using UnityEngine;

public class AllDataSystem : MonoBehaviour
{
    public static AllDataSystem Instance {  get; private set; }     //单例
    [field: SerializeField] public  List<CardDataSO> AllCardsDataSO { get;private set; }   //所有卡牌数据
    [field: SerializeField] public  PerkPoolDataSO PerkPoolDataSO { get; private set; }   //所有遗物数据
    [field: SerializeField] public  ActEnemyPool ActEnemyPool { get; private set; }         //敌人池
    [field: SerializeField] public  CardPoolDataSO IroncladCardPool { get; private set; }         //卡牌池池


    private void Awake()
    {
        if(Instance != null &&  Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
}

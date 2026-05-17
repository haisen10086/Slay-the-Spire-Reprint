using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EncounterDataSO")]

public class EncounterDataSO : ScriptableObject
{
    [field: SerializeField] public List<EnemyDataSO> EnemyDataSos;      //敌人数据列表
    public int MinFloor;

    public int MaxFloor;

    public bool isUse = false;
}

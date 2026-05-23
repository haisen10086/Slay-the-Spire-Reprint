using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PerkPoolDataSO")]
public class PerkPoolDataSO : ScriptableObject
{
    [field: SerializeField] public List<PerkDataSO> PerkPool { get; private set; }  //遗物池
}

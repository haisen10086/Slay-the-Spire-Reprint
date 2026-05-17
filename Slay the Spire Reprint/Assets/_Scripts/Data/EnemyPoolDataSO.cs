using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy Pool")]
public class ActEnemyPool : ScriptableObject
{
    public List<EncounterDataSO> WeakEncounters;    //Èõ³Ø¹Ö

    public List<EncounterDataSO> NormalEncounters;  //Ç¿³Ø¹Ö

    public List<EncounterDataSO> EliteEncounters;   //¾«Ó¢³Ø

    public List<EncounterDataSO> BossEncounters;    //boss³Ø
}
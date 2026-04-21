using UnityEngine;
[CreateAssetMenu(menuName ="Data/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    // Ù–‘
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int Health {  get; private set; }
    [field: SerializeField] public int AttckPower {  get; private set; }

} 

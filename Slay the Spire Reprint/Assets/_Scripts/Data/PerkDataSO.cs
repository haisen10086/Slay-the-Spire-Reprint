using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Perk")]
public class PerkDataSO : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }       //遗物的图标
    [field: SerializeReference, SR] public PerkCondition PerkCondition { get; private set; }
    [field: SerializeReference, SR] public AutoTargetEffect AutoTargetEffect { get; private set; }           //条件满足时触发的效果

    [field: SerializeField] public bool UseAutoTargetEffect { get; private set; } = true;                   //告诉是否使用自动目标效果
    [field: SerializeField] public bool UseActionCasterAsTarget { get; private set; } = false;               //是否将动作实施者作为目标

}

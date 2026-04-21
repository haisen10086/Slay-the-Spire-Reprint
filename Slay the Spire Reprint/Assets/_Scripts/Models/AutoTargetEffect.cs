using SerializeReferenceEditor;
using UnityEngine;

//作为一个包装器，将固定目标与效果绑定起来，每个效果有一个目标
[System.Serializable]
public class AutoTargetEffect 
{
    [field: SerializeReference, SR] public TargetMode TargetMode {  get;private set; }  //目标
    [field: SerializeReference, SR] public Effect Effect {  get;private set; }          //效果
}

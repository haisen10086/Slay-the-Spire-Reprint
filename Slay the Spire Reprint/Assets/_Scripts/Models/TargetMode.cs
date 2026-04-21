using System.Collections.Generic;
using UnityEngine;

//抽象类,返回目标列表
[System.Serializable]
public abstract class TargetMode 
{
    //返回选中的目标
    public abstract List<CombatantView> GetTargets();
}

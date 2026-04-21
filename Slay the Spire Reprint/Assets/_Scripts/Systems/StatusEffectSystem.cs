using System.Collections;
using UnityEngine;

public class StatusEffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectGAPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
    }
    private IEnumerator AddStatusEffectGAPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        foreach(var target in addStatusEffectGA.Targets)
        {
            target.AddStatusEffects(addStatusEffectGA.statusEffectType, addStatusEffectGA.StackCount);
            Debug.Log("对" + target.GetType() + "执行了addStatusEffectGA");
            yield return null;//添加一些通用的添加状态动画
        }
    }
}

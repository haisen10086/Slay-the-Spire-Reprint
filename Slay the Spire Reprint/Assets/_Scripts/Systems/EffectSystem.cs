using System.Collections;
using UnityEngine;

//效果系统会执行效果游戏动作，而保留执行者
public class EffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);


    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }
    //执行效果执行函数，作用为将效果对应的动作添加到反应里,作为反应的反应存在，形成一条动作链
    public IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        GameAction effectAction = performEffectGA.Effect.GetGameAction(performEffectGA.Targets, HeroSystem.Instance.HeroView);
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }

}

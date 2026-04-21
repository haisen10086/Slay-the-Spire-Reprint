using System;
using UnityEngine;

[System.Serializable]
public abstract class PerkCondition
{
    //含有三个抽象函数
    //工作方式如下,
    //当添加一个遗物时，该方法会订阅一个游戏动作，每当执行该游戏动作时，就会触发该遗物效果
    public abstract void SubscribeCondition(Action<GameAction> reaction);
    //移除遗物时，也要移除订阅
    public abstract void UnsubscribeCondition(Action<GameAction> reaction);

    public abstract bool SubConditionIsMet(GameAction gameAction);

    [SerializeField] protected ReactionTiming reactionTiming;
}

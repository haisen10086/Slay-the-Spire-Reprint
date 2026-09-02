using System.Collections.Generic;
using UnityEngine;

public class DrawCardEffect : Effect
{
    [SerializeField] private int drawAmount;        //存储抽卡数
    

    //返回抽卡动作，让动作系统知道这个效果执行的是哪个动作
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster, Card sourceCard = null)
    {
        if(sourceCard != null)
        {
            drawAmount = sourceCard.BaseMagic;
        }
        DrawCardsGA drawCardsGA = new(drawAmount);
        return drawCardsGA;
    }
}

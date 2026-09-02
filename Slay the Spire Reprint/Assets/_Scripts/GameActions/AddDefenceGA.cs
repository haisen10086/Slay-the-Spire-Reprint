using UnityEngine;

public class AddDefenceGA : GameAction
{
    public int amount;      //防御值
    public CombatantView caster;    //施法者

    public AddDefenceGA(CombatantView caster, int amount)
    {
        this.caster = caster;
        this.amount = amount;
    }
}

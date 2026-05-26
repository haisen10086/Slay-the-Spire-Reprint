using System.Collections.Generic;
using UnityEngine;

public class AddBuffGA : GameAction
{
    public BuffBase buff;

    public List<CombatantView> Targets { get; private set; }

    public AddBuffGA(BuffBase buff, List<CombatantView> targets)
    {
        this.buff = buff;   
        Targets = targets;
    }
}

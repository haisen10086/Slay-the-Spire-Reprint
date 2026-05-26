using UnityEngine;

public class VulnerableBuff : BuffBase
{
    public override void ModifyDamageTaken(DamageInfo damageInfo)
    {
        damageInfo.currentDamage = Mathf.RoundToInt(damageInfo.currentDamage * 1.5f);
    }


    public override BuffBase DeepClone()
    {
        return new VulnerableBuff
        {
            BuffId = this.BuffId,
            BuffName = this.BuffName,
            Icon = this.Icon,
            Amount = this.Amount,
            owner = this.owner
        };
    }
}
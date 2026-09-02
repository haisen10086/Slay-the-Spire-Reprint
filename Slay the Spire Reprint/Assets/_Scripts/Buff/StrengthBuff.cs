public class StrengthBuff : BuffBase
{

    public override void ModifyDamageGive(DamageInfo info)
    {
        info.currentDamage += Amount;
    }



    public override string GetDescription()
    {
        return $"π•ª˜‘Ï≥… {Amount} ∂ÓÕ‚…À∫¶";
    }

    public override BuffBase DeepClone()
    {
        return new StrengthBuff
        {
            BuffId = this.BuffId,
            BuffName = this.BuffName,
            Icon = this.Icon,
            Amount = this.Amount,
            owner = this.owner
        };
    }
}
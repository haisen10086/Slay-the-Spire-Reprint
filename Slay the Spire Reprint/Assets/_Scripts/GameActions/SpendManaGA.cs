using UnityEngine;

//消耗法力动作
public class SpendManaGA : GameAction
{
    public int Amount {  get; set; }
    public SpendManaGA(int amount)
    {
        this.Amount = amount;
    }
}

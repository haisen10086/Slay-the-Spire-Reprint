using UnityEngine;
/// <summary>
/// 回血的行为动作
/// </summary>
public class RegainHPGA : GameAction
{
    public int Amount {  get;private set; }     //回血的数值
    public RegainHPGA(int amount)
    {
        Amount = amount;
    }
}

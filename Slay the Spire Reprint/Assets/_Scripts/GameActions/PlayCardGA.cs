using UnityEngine;

public class PlayCardGA : GameAction
{
    //ÓÐ¿¨ÅÆÊý¾Ý
    public Card Card { get; set; }

    public EnemyView Manualtarget { get;private set; }
    public PlayCardGA(Card card)
    {
        Card = card;
        Manualtarget = null;
    }
    public PlayCardGA(Card card,EnemyView target)
    {
        Card = card;
        Manualtarget = target;
    }

}

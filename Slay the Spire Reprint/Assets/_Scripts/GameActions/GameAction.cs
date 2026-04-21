using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//抽象类,三个列表存储动作发生前，动作发生后，动作执行中的动作

public abstract class GameAction
{
    //这些列表允许动作在执行前、执行过程中、执行后插入其他子动作，形成动作链。
    public List<GameAction> PreReactions { get; private set; } = new();
    public List<GameAction> PerformReactions { get; private set; } = new();
    public List<GameAction> PostReactions { get; private set; } = new();

}

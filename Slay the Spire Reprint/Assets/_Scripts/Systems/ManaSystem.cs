using System.Collections;
using UnityEngine;

//保存当前的法力值，并包含我们的法力执行者
public class ManaSystem : MonoBehaviour
{
    //单例
    public static ManaSystem Instance {  get; private set; }

    //法力UI的引用
    [SerializeField] private ManaUI manaUI;
    //表示最大法力值的常数整数
    private const int MAX_MANA = 3;
    //表示当前法力值的数
    private int currentMana = MAX_MANA;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer); 
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);

        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReacton, ReactionTiming.POST);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();

        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReacton, ReactionTiming.POST);
    }

    //判断当前法力值足不足够
    public bool HasEnoughMana(int mana)
    {
        return currentMana >= mana;
    }
    //消耗法力执行者
    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        currentMana -= spendManaGA.Amount;
        manaUI.UpdataManaText(currentMana);
        yield return null;  
    }
    //重置法力值执行者
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        currentMana = MAX_MANA;
        manaUI.UpdataManaText(currentMana);
        yield return null;  
    }

    //reaction反应
    //敌人回合结束重置玩家法力值,所以在敌人回合后订阅该反应
    private void EnemyTurnPostReacton(EnemyTurnGA enemyTurnGA)
    {
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddReaction(refillManaGA);
    }

    private void Awake()
    {
        if(Instance != null && Instance!=this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;    
        }
        manaUI.UpdataManaText(MAX_MANA);
    }


}

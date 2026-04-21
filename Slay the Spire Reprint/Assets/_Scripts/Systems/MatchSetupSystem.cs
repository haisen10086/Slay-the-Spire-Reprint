using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroDataSO heroDataSO;     //英雄数据
    [SerializeField] private List<EnemyDataSO> enemyDataSO; //敌人数据列表
    [SerializeField] private List<PerkDataSO> perkDataSOs;         //遗物数据
    private void Start()
    {
        HeroSystem.Instance.Setup(heroDataSO);
        EnemySystem.Instance.Setup(enemyDataSO);    
        CardSystem.Instance.SetUp(heroDataSO.Deck);
        foreach(var perkDataSO in  perkDataSOs)
        {
            PerkSystem.Instance.AddPerk(new Perk(perkDataSO));
        }
        
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}

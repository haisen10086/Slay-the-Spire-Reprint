using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : MonoBehaviour
{
    //单例
    public static PerkSystem Instance { get; private set; }

    //引用
    [SerializeField] private PerksUI perksUI;           //引用PerksUI界面

    //属性
    private List<Perk> perks = new();                   //存储Perk的列表

    public void AddPerk(Perk perk)
    {
        perks.Add(perk);
        perksUI.AddPerkUI(perk);
        perk.OnAdd();
    }
    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        perksUI.RemovePerkUI(perk);
        perk.OnRemove();
    }



    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance );
        }
        else
        {
            Instance = this;
        }
    }

}

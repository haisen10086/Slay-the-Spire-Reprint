using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerksUI : MonoBehaviour
{
    //引用
    [SerializeField] private PerkUI perkUIPrefab;                       //PerkUI预制件
    private readonly List<PerkUI> perkUIs = new();                          //存储Perk列表

    //添加PerkUI
    public void AddPerkUI(Perk perk)
    {
        PerkUI perkUI = Instantiate(perkUIPrefab, transform);           //以transform作为父级，实例化新的PerkUI
        perkUI.Setup(perk);
        perkUIs.Add(perkUI);
    }

    public void RemovePerkUI(Perk perk)
    {
        PerkUI perkUI = perkUIs.Where(pui => pui.Perk == perk).FirstOrDefault();
        if(perkUI != null)
        {
            //移除并销毁
            perkUIs.Remove(perkUI);
            Destroy(perkUI.gameObject);
        }
    }

}

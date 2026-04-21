using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 记录特定角色的所有状态效果UI
/// </summary>
public class StatusEffectsUI : MonoBehaviour
{
    //引用
    [SerializeField] private StatusEffectUI statusEffectPrefab;        //状态效果UI预制体
    [SerializeField] private Sprite armorSprite, burnSprite;            //存放状态图像

    private Dictionary<StatusEffectType, StatusEffectUI> statusEffectUIs = new();                   //一个字典，状态类型为Key，状态效果UI为值，

    public void UpdateStatusEffectUI(StatusEffectType statusEffectType, int stackCount)
    {
        if(stackCount <= 0)
        {
            if(statusEffectUIs.ContainsKey(statusEffectType))
            {
                StatusEffectUI statusEffectUI = statusEffectUIs[statusEffectType];  
                statusEffectUIs.Remove(statusEffectType);
                Destroy(statusEffectUI.gameObject);
            }
        }
        else
        {
            if(!statusEffectUIs.ContainsKey(statusEffectType))
            {
                StatusEffectUI statusEffectUI = Instantiate(statusEffectPrefab, transform);
                statusEffectUIs.Add(statusEffectType, statusEffectUI);
            }
            Sprite sprite = GetSpriteByType(statusEffectType);
            statusEffectUIs[statusEffectType].Set(sprite, stackCount);
        }
    }

    private Sprite GetSpriteByType(StatusEffectType statusEffectType)
    {
        return statusEffectType switch
        {
            StatusEffectType.ARMOR => armorSprite,
            StatusEffectType.BURN => burnSprite,
            _ => null
        };
    }
}

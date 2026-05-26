using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//战斗基础脚本,英雄和敌人均继承它
public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;       //生命值Text
    [SerializeField] private SpriteRenderer spriteRenderer;      //Image的精灵渲染器

    [SerializeField] private StatusEffectsUI statusEffectsUI;    //存放状态栏UI

    public List<BuffBase> buffs = new();
    [SerializeField] private BuffsUI buffsUI;       //存放buff栏UI

    public int MaxHealth { get; private set; }     //最大生命值
    public int CurrentHealth { get; private set; }  //当前生命值

    private Dictionary<StatusEffectType, int> statusEffects = new();


    public event EventHandler OnHealthChange;

    //基础属性设置方法
    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        UpdataHealthText();
    }
    //生命值文本更新方法
    private void UpdataHealthText()
    {
        healthText.text = "HP:" + CurrentHealth.ToString() + "/" + MaxHealth.ToString();
    }
    //减少当前生命值
    public void Damage(int damageAmount)
    {
        int remainingDamage = damageAmount;
        int currentArmor = GetStatusEffectStacks(StatusEffectType.ARMOR);
        if(currentArmor > 0)
        {
            if(currentArmor >= damageAmount)
            {
                RemoveStatusEffects(StatusEffectType.ARMOR, remainingDamage);
                remainingDamage = 0;
            }
            else if(currentArmor < damageAmount)
            {
                RemoveStatusEffects(StatusEffectType.ARMOR, currentArmor);
                remainingDamage -= currentArmor;
            }
        }
        if(remainingDamage > 0)
        {
            CurrentHealth -= remainingDamage;
            OnHealthChange?.Invoke(this, EventArgs.Empty);      //传递生命值修改事件
            if(CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }

        //动画
        transform.DOShakePosition(0.2f, 0.5f);
        UpdataHealthText();
    }

    //添加状态堆叠数量
    public void AddStatusEffects(StatusEffectType statusEffectType, int stackCount)
    {
        if(statusEffects.ContainsKey(statusEffectType))
        {
            statusEffects[statusEffectType] += stackCount;
        }
        else
        {
            statusEffects.Add(statusEffectType, stackCount);
        }
        statusEffectsUI.UpdateStatusEffectUI(statusEffectType, GetStatusEffectStacks(statusEffectType));
    }
    //移除状态数量
    public void RemoveStatusEffects(StatusEffectType statusEffectType, int stackCount)
    {
        if(statusEffects.ContainsKey(statusEffectType))
        {
            statusEffects[statusEffectType] -= stackCount;
            if (statusEffects[statusEffectType] <= 0)
            {
                statusEffects.Remove(statusEffectType);
            }
        }
        statusEffectsUI.UpdateStatusEffectUI(statusEffectType, GetStatusEffectStacks(statusEffectType));

    }

    //获取状态的堆叠数量
    public int GetStatusEffectStacks(StatusEffectType statusEffectType)
    {
        if(statusEffects.ContainsKey(statusEffectType)) return statusEffects[statusEffectType];
        else return 0;
    }




    //添加buff
    public void AddBuff(BuffBase buff)
    {
        // 查找是否已有同类 Buff
        BuffBase existing = buffs.Find(b => b.BuffId == buff.BuffId);
       
        if (existing != null)
        {
            existing.Amount += buff.Amount;
        }
        else
        {
            existing = buff.DeepClone();
            buffs.Add(existing);
            existing.OnApply(this);
        }

        RefreshBuffUI(existing);
    }
    //刷新UI
    public void RefreshBuffUI(BuffBase buff)
    {
        buffsUI.UpdateBuffUI(buff, buff.Amount);
    }

    //移除buff
    public void RemoveBuff(BuffBase buff)
    {
        buff.OnRemove();
        buffs.Remove(buff);

        RefreshBuffUI(buff);
    }
   
}

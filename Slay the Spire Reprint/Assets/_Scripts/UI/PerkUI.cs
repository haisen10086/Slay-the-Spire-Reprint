using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour
{
    // Ù–‘
    //PerkÕº∆¨
    [SerializeField] private Image image;
    //Perk µ¿˝
    public Perk Perk { get; private set; }

    public void Setup(Perk perk)
    {
        Perk = perk;
        image.sprite = perk.Image;
        perk.SetOwnerPerkUI(this);
    }

    public void SharkeUI()
    {
        Tween tween = transform.DOShakePosition(0.3f, 10);
    }


}

using System;
using TMPro;
using UnityEngine;

public abstract class ItemUI: MonoBehaviour
{
    public int price;
    [SerializeField] private TMP_Text priceText;

    public void SetUpPeice(int price)
    {
        this.price = price;
        priceText.text = price.ToString();
    }
    //返回商品数据
    public abstract System.Object GetItemData();
}

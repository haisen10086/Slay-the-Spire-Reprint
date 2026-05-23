//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

///// <summary>
///// 商店商品 UI
///// </summary>
//public class MerchantItemUI : MonoBehaviour
//{
//    [Header("UI")]
//    public Image iconImage;

//    public TMP_Text nameText;

//    public TMP_Text priceText;

//    public Button buyButton;

//    public GameObject soldOutText;

//    // 当前商品数据
//    private MerchantItemData currentData;

//    // 商店管理器
//    private MerchantSystem merchantManager;

//    /// <summary>
//    /// 初始化商品 UI
//    /// </summary>
//    public void Setup(
//        MerchantItemData data,
//        MerchantSystem manager)
//    {
//        currentData = data;
//        merchantManager = manager;

//        // 设置名称
//        nameText.text = data.itemName;

//        // 设置价格
//        priceText.text = data.price + " Gold";

//        // 设置图标
//        iconImage.sprite = data.icon;

//        // 注册按钮事件
//        buyButton.onClick.RemoveAllListeners();
//        buyButton.onClick.AddListener(OnClickBuy);

//        // 刷新状态
//        RefreshUI();
//    }
//    /// <summary>
//    ///点击购买
//    /// </summary>
//    private void OnClickBuy()
//    {
//        merchantManager.TryBuyItem(currentData, this);
//    }

//    /// <summary>
//    /// 刷新 UI
//    /// </summary>
//    public void RefreshUI()
//    {
//        if (currentData.isSold)
//        {
//            // 显示售罄
//            soldOutText.SetActive(true);

//            // 禁用按钮
//            buyButton.interactable = false;

//            // 整体变灰
//            CanvasGroup group = GetComponent<CanvasGroup>();

//            if (group != null)
//            {
//                group.alpha = 0.5f;
//            }
//        }
//        else
//        {
//            soldOutText.SetActive(false);
//            buyButton.interactable = true;
//        }
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class RestUI : MonoBehaviour
{
    public Button upgradeButton;
    public void Show()
    {
        ResetUpgradeButton();
        gameObject.SetActive(true);
        Debug.Log(gameObject.activeSelf);
    }

    public void ResetUpgradeButton()
    {
        upgradeButton.interactable = true;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    

}

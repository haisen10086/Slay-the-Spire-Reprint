using UnityEngine;

public class RestUI : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        Debug.Log(gameObject.activeSelf);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

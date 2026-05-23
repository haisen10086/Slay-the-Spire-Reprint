using UnityEngine;

public class RestSystem : MonoBehaviour
{
    //引用
    [SerializeField] private RestUI restUI;         //引用休息UI

    public static RestSystem Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void RestShow()
    {
        Debug.Log("显示RestUI");
        restUI.Show();
    }

    public void RestHide()
    {
        restUI.Hide();
    }

}

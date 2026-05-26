using TMPro;
using UnityEngine;
using UnityEngine.UI;

//代表单个状态效果
public class BuffUI : MonoBehaviour
{
    [SerializeField] private Image image;   //存放状态效果的图像
    [SerializeField] private TMP_Text StackCountText;   //显示当前状态层数

    public void Set(Sprite sprite, int stackCount)
    {
        image.sprite = sprite;
        StackCountText.text = stackCount.ToString();
    }
}
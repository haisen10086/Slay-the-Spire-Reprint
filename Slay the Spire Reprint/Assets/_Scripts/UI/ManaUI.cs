using TMPro;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private TMP_Text mana;     //法力值文本
    //公共的更新法力值UI的方法
    public void UpdataManaText(int currentMana)
    {
        mana .text = currentMana.ToString();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class BuffsUI : MonoBehaviour
{
    //引用
    [SerializeField] private BuffUI BuffUIPrefab;        //状态效果UI预制体

    private Dictionary<BuffBase, BuffUI> buffUIs = new();                   //一个字典，状态类型为Key，状态效果UI为值，

    //更新特定buff的层数
    public void UpdateBuffUI(BuffBase buff, int stackCount)
    {
        if (stackCount <= 0)
        {
            if (buffUIs.ContainsKey(buff))
            {
                BuffUI buffUI = buffUIs[buff];
                buffUIs.Remove(buff);
                Destroy(buffUI.gameObject);
            }
        }
        else
        {
            if (!buffUIs.ContainsKey(buff))
            {
                BuffUI buffUI = Instantiate(BuffUIPrefab, transform);
                buffUIs.Add(buff, buffUI);
            }
            Sprite sprite = buff.Icon;
            buffUIs[buff].Set(sprite, stackCount);
        }
    }

}

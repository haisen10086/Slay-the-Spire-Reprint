using UnityEditor;
using UnityEngine;

public class HeroSystem : MonoBehaviour
{
    //单例
    public static HeroSystem Instance {  get; private set; }

    //属性
    [field: SerializeField] public HeroView HeroView { get; private set; }                   //持有场景视图里英雄视图的引用

    //初始化
    public void Setup(HeroDataSO heroDataSO)
    {
        HeroView.Setup(heroDataSO);
    }





    private void Awake()
    {
        if(Instance != null && Instance !=  this)
        {
            Destroy(Instance);
        }else
        {
            Instance = this;
        }
    }
}

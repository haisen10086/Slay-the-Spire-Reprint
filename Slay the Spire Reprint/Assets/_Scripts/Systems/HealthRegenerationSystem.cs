using System.Collections;
using UnityEngine;

public class HealthRegenerationSystem : MonoBehaviour
{
    public static HealthRegenerationSystem Instance {  get; private set; }      //单例








    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<RegainHPGA>(RegainHPGAPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<RegainHPGA>();
    }

    //回血动作执行者
    private IEnumerator RegainHPGAPerformer(RegainHPGA regainHPGA)
    {
        yield return null;
        
    }
}

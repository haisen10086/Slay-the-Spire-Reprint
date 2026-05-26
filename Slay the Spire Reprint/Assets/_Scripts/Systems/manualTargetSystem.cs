using System.Runtime.InteropServices;
using UnityEngine;

public class manualTargetSystem : MonoBehaviour
{
    //单例
    public static manualTargetSystem Instance;

    //引用
    [SerializeField] private ArrowView arrowView;               //箭头视图的引用
    [SerializeField] private LayerMask targetLayerMask;         //目标遮罩层

    //函数

    private void Awake()
    {
        if(Instance != null && Instance != this )
        {
            Destroy( Instance );
        }
        else
        {
            Instance = this;
        }
    }

    //起始瞄准，将箭头的初始位置设定好,同时激活箭头
    public void StartTargeting(Vector3 startPotion)
    {
        arrowView.gameObject.SetActive( true );
        arrowView.SetupArrow(startPotion);
    }

    //结束瞄准，当结束瞄准时，会返回敌人目标,同时禁用箭头视图
    public EnemyView EndTargeting(Vector3 endPotion)
    {
        arrowView.gameObject.SetActive ( false );
        if(Physics.Raycast(endPotion, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }
        return null;
    }
    //检测是否有目标
    public EnemyView TestingTargeting(Vector3 endPotion)
    {
        if (Physics.Raycast(endPotion, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }
        return null;
    }

}

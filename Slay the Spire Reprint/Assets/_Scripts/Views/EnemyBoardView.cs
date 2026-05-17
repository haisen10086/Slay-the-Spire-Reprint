using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardView : MonoBehaviour
{
    //引用
    [SerializeField] private List<Transform> slots;     //引用插槽

    //属性
    public List<EnemyView> EnemyViews { get; private set; } = new();    //存储敌人视图列表

    //方法
    //添加敌人
    public void AddEnemy(EnemyDataSO enemyDataSO)
    {
        Transform slot = slots[EnemyViews.Count];
        EnemyView enemyView = EnemyViewCreator.Instance.CreateEnemyView(enemyDataSO, slot.position, slot.rotation);
        enemyView.transform.parent = slot.transform;
        EnemyViews.Add(enemyView);
    }
    //移除并销毁敌人
    public IEnumerator RemoveEnemyView(EnemyView enemyView)
    {
        EnemyViews.Remove(enemyView);
        Tween tween = enemyView.transform.DOScale(Vector3.zero, 0.25f);
        yield return tween.WaitForCompletion();
        Destroy(enemyView.gameObject);
    }

    //移除并销毁所有敌人
    public IEnumerator RemoveAllEnemyView()
    {
        for (int i = EnemyViews.Count - 1; i>=0; i--)
        {
            yield return StartCoroutine(RemoveEnemyView(EnemyViews[i]));
        }
    }
}

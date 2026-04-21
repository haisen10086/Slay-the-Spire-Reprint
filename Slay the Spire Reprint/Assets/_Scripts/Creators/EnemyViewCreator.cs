using UnityEngine;

public class EnemyViewCreator : MonoBehaviour
{
    //单例
    public static EnemyViewCreator Instance { get; private set; }

    //引用
    [SerializeField] private EnemyView enemyViewPregab;     //预制体

    //方法
    //创建敌人视图
    public EnemyView CreateEnemyView(EnemyDataSO enemyDataSO, Vector3 position, Quaternion rotation)
    {
        EnemyView enemyView = Instantiate(enemyViewPregab, position, rotation);
        enemyView.Setup(enemyDataSO);
        return enemyView;
    }


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
}

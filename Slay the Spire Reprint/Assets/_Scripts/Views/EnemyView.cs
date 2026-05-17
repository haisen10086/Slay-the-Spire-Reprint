using TMPro;
using UnityEngine;

//敌人视图
public class EnemyView : CombatantView
{
    //攻击力Text
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text nameText;

    public EnemyDataSO myEnemyDataSO {  get; private set; }
    //攻击力属性
    public int AttckPower {  get;  set; }
    //设置基本属性
    public void Setup(EnemyDataSO enemyDataSO)
    {
        myEnemyDataSO = enemyDataSO;
        nameText.text = enemyDataSO.id;
        AttckPower = enemyDataSO.AttckPower;
        UpdateAttackText();
        SetupBase(enemyDataSO.Health, enemyDataSO.Image);
        
    }
    //更新敌人攻击文本
    public void UpdateAttackText()
    {
        attackText.text = "ATK:" + AttckPower.ToString();
    }

}

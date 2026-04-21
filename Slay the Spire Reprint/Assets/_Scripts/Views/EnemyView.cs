using TMPro;
using UnityEngine;

//敌人视图
public class EnemyView : CombatantView
{
    //攻击力Text
    [SerializeField] private TMP_Text attackText;
    //攻击力属性
    public int AttckPower {  get;  set; }
    //设置基本属性
    public void Setup(EnemyDataSO enemyDataSO)
    {
        AttckPower = enemyDataSO.AttckPower;
        updateAttackText();
        SetupBase(enemyDataSO.Health, enemyDataSO.Image);
    }
    //更新敌人攻击文本
    public void updateAttackText()
    {
        attackText.text = "ATK:" + AttckPower.ToString();
    }

}

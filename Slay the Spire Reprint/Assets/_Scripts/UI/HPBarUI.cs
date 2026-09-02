using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [SerializeField] private Image white;
    [SerializeField] private Image red;
    [SerializeField] private Image pink;

    [SerializeField] private CombatantView combatantView;

    private Coroutine delayCotoutine;

    private void OnEnable()
    {
        combatantView.OnHealthChange += CombatantView_OnHealthChange;
    }
    private void OnDisable()
    {
        combatantView.OnHealthChange -= CombatantView_OnHealthChange;
    }

    private void CombatantView_OnHealthChange(object sender, System.EventArgs e)
    {
        float targetFill = combatantView.CurrentHealth*1f / combatantView.MaxHealth;
        Debug.Log("更新血条"+ targetFill);
        UpdateBar(targetFill);
    }

    //更新血条
    private void UpdateBar(float targetFill)
    {
        red.fillAmount = targetFill;
        if(delayCotoutine != null)
        {
            StopCoroutine(delayCotoutine);
        }
        delayCotoutine = StartCoroutine(DelayBarLerp(targetFill));
    }
    //协程减少虚血
    private IEnumerator DelayBarLerp(float targetFill)
    {
        yield return new WaitForSeconds(0.3f);

        float startFill = pink.fillAmount;
        for (float t = 0; t < 0.25f; t += Time.deltaTime)
        {
            pink.fillAmount = Mathf.Lerp(startFill, targetFill, t / 0.25f);
            yield return null;
        }
        pink.fillAmount = targetFill;
    }

    public float GetRedFillAmount()
    {
        return red.fillAmount;
    }

    //
}

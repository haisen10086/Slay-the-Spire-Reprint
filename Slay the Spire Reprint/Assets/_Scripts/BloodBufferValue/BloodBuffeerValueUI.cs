using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodBuffeerValueUI : MonoBehaviour
{
    [SerializeField] private Image write;
    [SerializeField] private Image pink;
    [SerializeField] private Image red;

    private float maxHealth = 200f;
    private float currentHealth;

    private Coroutine delayCotoutine;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q)) TakeDamage(20f);
        if (Input.GetKeyUp(KeyCode.R)) FullHealth();
    }
    //造成伤害
    private void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth -  damage, 0f);
        float targetFill = currentHealth / maxHealth;
        UpdateBars(targetFill);
    }

    private void FullHealth()
    {
        currentHealth = maxHealth;
        red.fillAmount = 1f;
        pink.fillAmount = 1f;
    }
    //更新血条
    private void UpdateBars(float targetFill)
    {
        red.fillAmount = targetFill;
        if(delayCotoutine != null)
        {
            StopCoroutine(delayCotoutine);
        }
        delayCotoutine = StartCoroutine(DelayBarLerp(targetFill));
    }

    private IEnumerator DelayBarLerp(float targetFill)
    {
        yield return new WaitForSeconds(0.2f);

        float startFill = pink.fillAmount;
        for(float t =0; t<0.25f; t+=Time.deltaTime)
        {
            pink.fillAmount = Mathf.Lerp(startFill, targetFill, t / 0.25f);
            yield return null;
        }
        pink.fillAmount = targetFill;
    }
}

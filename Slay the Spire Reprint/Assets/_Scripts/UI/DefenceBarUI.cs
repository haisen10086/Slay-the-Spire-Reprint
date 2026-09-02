using System.Collections;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenceBarUI : MonoBehaviour
{
    public Transform defenceBar;
    public Image blue;          //防御条
    public Image bluePink;      //蓝色虚血
    public Image defenceIamge;
    public TMP_Text defenceText;

    public CombatantView owner;

    private int currentDefence = 0;
    private Coroutine delayCotoutine;
    private Coroutine DefenceHitCotoutine;

    public void Awake()
    {
        Hide();
    }

    private void Start()
    {
        owner.OnDefenceChange += Owner_OnDefenceChange;
    }

    private void OnDestroy()
    {
        owner.OnDefenceChange -= Owner_OnDefenceChange;
    }
    //防御值改变时，如果大于零显示
    private void Owner_OnDefenceChange(object sender, System.EventArgs e)
    {
        Debug.Log("防御值修改,当前为："+owner.CurrentDefence);
        if (owner.CurrentDefence > 0)
        {
            if(owner.CurrentDefence < currentDefence)
            {

                if(DefenceHitCotoutine != null)
                {
                    StopCoroutine(DefenceHitCotoutine);
                }
                DefenceHitCotoutine = StartCoroutine(PlayDefenseHitAnimation());
            }
            else
            {
                Show();
                CombatFeedbackSystem.Instance.PlayPopAnimation(defenceIamge);
            }
        }
        else Hide();
    }

    //防御受击动画
    private IEnumerator PlayDefenseHitAnimation()
    {
        UpdateBar(0f);
        yield return new WaitForSeconds(0.1f);
        UpdateBar(owner.GetHPBarUI().GetRedFillAmount());
        defenceText.text = owner.CurrentDefence.ToString();

    }

    //添加防御时，让blue长度等于血条长度
    public void SetBlueFillAmount()
    {
        blue.fillAmount = owner.GetHPBarUI().GetRedFillAmount();
        bluePink.fillAmount = owner.GetHPBarUI().GetRedFillAmount();

    }    

    public void Show()
    {
        defenceBar.gameObject.SetActive(true);
        blue.gameObject.SetActive(true);
        bluePink.gameObject.SetActive(true);
        defenceIamge.gameObject.SetActive(true);
        defenceText.gameObject.SetActive(true);
        defenceText.text = owner.CurrentDefence.ToString();
        currentDefence = owner.CurrentDefence;

        SetBlueFillAmount();
    }

    public void Hide()
    {
        defenceBar.gameObject.SetActive(false);
        blue.gameObject.SetActive(false);
        bluePink.gameObject.SetActive(false);
        defenceIamge.gameObject.SetActive(false);
        defenceText.gameObject.SetActive(false);

        //防御结束动画
    }



    private void UpdateBar(float targetFill)
    {
        blue.fillAmount = targetFill;
        if (delayCotoutine != null)
        {
            StopCoroutine(delayCotoutine);
        }
        delayCotoutine = StartCoroutine(DelayBarLerp(targetFill));
    }
    //协程减少虚血
    private IEnumerator DelayBarLerp(float targetFill)
    {
        yield return new WaitForSeconds(0.1f);

        float startFill = bluePink.fillAmount;
        for (float t = 0; t < 0.1f; t += Time.deltaTime)
        {
            bluePink.fillAmount = Mathf.Lerp(startFill, targetFill, t / 0.15f);
            yield return null;
        }
        bluePink.fillAmount = targetFill;
    }



}

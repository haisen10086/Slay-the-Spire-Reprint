using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    //public CanvasGroup menuGroup;
    public Image fadePanel;


    public void SrartGame()
    {
        StartCoroutine(StartingGame());
    }
    public IEnumerator StartingGame()
    {

        FadePanelSystem.Instance.Transition();

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);

        //Sequence seq = DOTween.Sequence();

        // UIµ­³ö
        //seq.Append(
        //    menuGroup.DOFade(
        //        0,
        //        0.5f));

        // ºÚÄ»µ­Èë
        //seq.Join(
        //    fadePanel.DOFade(
        //        1,
        //        0.8f));

        //seq.OnComplete(() =>
        //{
        //SceneManager.LoadScene("Game");
        //});
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
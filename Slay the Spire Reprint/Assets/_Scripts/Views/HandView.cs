using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;   //线条容器
     public  List<CardView> cards = new(); //存放卡片
    
    //添加卡片，同时每添加一个卡片，更新一次位置
    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        yield return UpdateCardPositions(0.15f);
    }
    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if(cardView == null ) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPositions(0.15f));

        return cardView;
    }

    //获得目标CardView
    private CardView GetCardView(Card card)
    {
        //LINQ里面的操作，将第一个符合条件的cardView返回
        return cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }
    //更新卡片位置，包含计算位置算法
    public IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0) yield break;
        float cardSpacing = 1f / 10f;
        float firstCardposition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardposition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);

        }
        yield return new WaitForSeconds(duration);
    }
}


using UnityEngine;
using DG.Tweening;

public class MenuTitle : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(
            1.05f,
            2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
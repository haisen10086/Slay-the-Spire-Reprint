using UnityEngine;
using DG.Tweening;

public class CameraIdle : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveY(
            transform.position.y + 0.2f,
            3f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
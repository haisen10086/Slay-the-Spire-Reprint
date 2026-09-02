using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FadePanelSystem : MonoBehaviour
{
    public static FadePanelSystem Instance {  get; private set; }
    public Canvas canvas;          // 你的 Canvas
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    public IEnumerator EnlargeCycle()
    {
        transform.DOScale(1, 0.8f);
        yield return null;
    }
    public IEnumerator NarrowingCycle()
    {
        MoveToClickPosition();
        Tween tween = transform.DOScale(0.065f, 0.8f);
        yield return tween;
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnlargeCycle());
    }
    public void Transition()
    {
        StartCoroutine(NarrowingCycle());
    }
    private void MoveToClickPosition()
    {
        // 1. 获取鼠标屏幕坐标
        Vector2 screenPos = Input.mousePosition;

        // 2. 转换成 Canvas 的本地坐标
        RectTransform canvasRect = canvas.transform as RectTransform;

        // 关键：Camera 模式必须传入 canvas.worldCamera
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.worldCamera,   // ← 这里传 canvas 上挂的 Camera
            out Vector2 localPos
        );

        Vector3 worldPos = canvas.transform.TransformPoint(localPos);

        transform.position = worldPos;
    }

}

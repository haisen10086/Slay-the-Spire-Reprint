using UnityEngine;

public class ArrowView : MonoBehaviour
{
    //引用
    [SerializeField] private GameObject arrowHead;                   //箭头
    [SerializeField] private LineRenderer lineRenderer;

    //属性
    private Vector3 startPosition;

    private void Update()
    {
        Vector3 endPosition = MouseUtil.GetMousePositionInWorldSpace();
        Vector3 direction = -(startPosition - arrowHead.transform.position).normalized;
        lineRenderer.SetPosition(1, endPosition - direction * 0.5f);
        arrowHead.transform.position = endPosition;
        arrowHead.transform.right = direction;
    }

    //设置箭头方法
    public void SetupArrow(Vector3 startPosition)
    {
        this.startPosition = startPosition;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());

    }
}

using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FingerSelectUI : MonoBehaviour
{
    public Vector3 StartPosition {  get; private set; } 
    public static FingerSelectUI Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        StartPosition = transform.position;
    }

    //移动到指定位置
    public void MoveToSelectItem(Vector3 position)
    {
        StartCoroutine(Move(position));
    }
    public IEnumerator Move(Vector3 position)
    {
        transform.DOKill();
        yield return null;
        transform.DOMove(position, 0.5f).SetEase(Ease.InQuad);
    }


}

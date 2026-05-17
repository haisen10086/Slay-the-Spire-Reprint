using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapNodeView : MonoBehaviour
{
    private MapNode mapNode;
    [SerializeField] private TMP_Text mapNodeTypeText;
    [SerializeField] private Image mapNodeImage;
    [SerializeField] private Image ClickedImage;

    private float duration = 0.5f;


    private bool hasClicked = false;


    public void Setup(MapNode mapNode, Sprite sprite)
    {
        this.mapNode = mapNode;
        mapNodeImage.sprite = sprite;
        mapNodeTypeText.text = mapNode.Type.ToString();
    }


    //节点点击函数
    public void OnButtonClick()
    {
        if (hasClicked) return;
        else hasClicked = true;
        StartCoroutine(ExpandAndEnterRoom());
    }
    //进入节点房间
    private IEnumerator ExpandAndEnterRoom()
    {
        yield return StartCoroutine(ExpandImage());


        if (mapNode != null)
        {
            if (mapNode.EncounterDataSO == null) Debug.Log("当前节点战斗数据缺失");
            MatchSetupSystem.Instance.SetupRoomData(mapNode.EncounterDataSO, mapNode.Type);
            if (mapNode.Type == RoomType.Monster || mapNode.Type == RoomType.Elite || mapNode.Type == RoomType.Boss)
            {
                EnterRoomGA enterRoomGA = new();
                ActionSystem.Instance.Perform(enterRoomGA);
            }

            //让地图关闭，这里可添加动画
            MapController.Instance.ToggleMap();
            //让奖励关闭
            AwardSystem.Instance.AwardHide();
            //同时更新AwardSystem的默认奖励
            //注意这里要等节点数据加载完全
            AwardSystem.Instance.UpdateAwardsByRoomType(MatchSetupSystem.Instance.CurrentRoomType);
        }
    }

    // 供外部调用的展开方法
    public void Expand()
    {
        StartCoroutine(ExpandImage());
    }

    private IEnumerator ExpandImage()
    {
        float elapsed = 0f;
        float startValue = ClickedImage.fillAmount;
        float endValue = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // 平滑曲线（可选）
            t = Mathf.SmoothStep(0f, 1f, t);
            ClickedImage.fillAmount = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }

        ClickedImage.fillAmount = 1f;
        Debug.Log("ClickedImage.fillAmount=" + ClickedImage.fillAmount);
    }
}

using System.Collections;
using UnityEngine;

public class DefenceSysytem : MonoBehaviour
{
    public static DefenceSysytem Instance {  get; private set; }

    [field : SerializeField] public Sprite DefenceSprite {  get; private set; }
    [field : SerializeField] public string DefenceName { get; private set; } = "防御";

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddDefenceGA>(AddDefenceGAPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddDefenceGA>();
    }

    private IEnumerator AddDefenceGAPerformer(AddDefenceGA addDefenceGA)
    {
        Debug.Log("运行到AddDefenceGAPerformer里添加了" + addDefenceGA.amount + "点防御");
        CombatFeedbackSystem.Instance.ShowBuffSpriteVFX(addDefenceGA.caster, DefenceSprite);
        CombatFeedbackSystem.Instance.ShowBuffText(addDefenceGA.caster, DefenceName);
        addDefenceGA.caster.AddDefence(addDefenceGA.amount);

        yield return null;
    }
}

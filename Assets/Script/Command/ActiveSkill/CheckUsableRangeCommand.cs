using UnityEngine;

public class CheckUsableRangeCommand : ICommand
{
    private readonly ClickTypeSkill _skill;

    private const float waitTime = 3.0f;
    private float currentWaitTime = 0.0f;

    public CheckUsableRangeCommand(ClickTypeSkill skill)
    {
        _skill = skill;

        Vector3 ownerPos = _skill.weapon.owner.transform.position;
        IndicatorManager.Instance.ShowUsableIndicator(ownerPos, _skill.Data.AvailableRange);

        _skill.weapon.owner.enabled = false;

        currentWaitTime = waitTime;
        _skill.PivotPosition = ownerPos;

        SkillUIManager.Instance.ShowPopupPanel();
    }

    public bool Execute()
    {
        currentWaitTime -= Time.deltaTime;

        if (currentWaitTime <= 0)
        {
            _skill.CancelSkill();
            return true;
        }

#if UNITY_EDITOR
        if (!Input.GetMouseButton(0))
            return false;
#else
        if (Input.touchCount <= 0)
            return false;
#endif

#if UNITY_EDITOR
        Vector2 touchPosition = Input.mousePosition;
        Touch touch = new() { position = touchPosition };
#else
        Touch touch = Input.GetTouch(0);
#endif

        if (UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("SkillUI"), touch))
        {
            return false;
        }

        if (UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("UI"), touch))
        {
            _skill.CancelSkill();
            return true;
        }

        _skill.PivotPosition = Camera.main.ScreenToWorldPoint(touch.position);

        var currentSetting = SettingManager.Instance.CurrentActiveSettingType;

        switch (currentSetting)
        {
            case SettingManager.ActiveSettingType.SemiAuto:
                _skill.AddCommand(new ActiveSkillCommand(_skill));
                break;
            case SettingManager.ActiveSettingType.Manual:
                _skill.AddCommand(new IndicatorCommand(_skill));
                break;
            default:
                break;
        }

        return true;
    }

    public void OnComplete()
    {
        IndicatorManager.Instance.HideUsableIndicator();
    }

    public void Undo()
    {
        IndicatorManager.Instance.HideUsableIndicator();
    }
}
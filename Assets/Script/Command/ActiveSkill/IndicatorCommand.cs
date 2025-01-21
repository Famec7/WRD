using UnityEngine;

public class IndicatorCommand : ICommand
{
    private readonly ActiveSkillBase _skill;

    private const float indicatorTime = 3.0f;
    private float _currentIndicatorTime = 0.0f;

    private const float clickBufferTime = 0.2f; // 클릭 버퍼 시간 추가
    private float _clickCooldown = 0.0f;       // 클릭 버퍼 타이머

    private IndicatorManager.Type IndicatorType => _skill.IndicatorType;

    private Vector2 PivotPosition => _skill.PivotPosition;

    public IndicatorCommand(ActiveSkillBase skill)
    {
        _skill = skill;

        _currentIndicatorTime = indicatorTime;
        _clickCooldown = clickBufferTime; // 초기화 시 클릭 버퍼 활성화

        _skill.ShowIndicator(PivotPosition);

        _skill.ClearTargetMonsters();
        SkillUIManager.Instance.ShowPopupPanel(1);
        
        _skill.weapon.owner.enabled = false;
    }

    public bool Execute()
    {
        _currentIndicatorTime -= Time.deltaTime;

        if (_clickCooldown > 0) // 클릭 버퍼 타이머 체크
        {
            _clickCooldown -= Time.deltaTime;
            return false;
        }

        if (_currentIndicatorTime <= 0)
        {
            _skill.CancelSkill();
            return false;
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

        _skill.ClickPosition = Camera.main.ScreenToWorldPoint(touch.position);
        _skill.AddCommand(new ActiveSkillCommand(_skill));

        return true;
    }

    public void OnComplete()
    {
        _skill.Indicator.HideIndicator();
        _skill.weapon.owner.enabled = true;
    }

    public void Undo()
    {
        _skill.Indicator.HideIndicator();
        _skill.weapon.owner.enabled = true;
    }
}

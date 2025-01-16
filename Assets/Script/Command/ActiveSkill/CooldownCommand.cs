using UnityEngine;

public class CooldownCommand : ICommand
{
    private readonly ActiveSkillBase _skill;

    public CooldownCommand(ActiveSkillBase skill)
    {
        _skill = skill;
        
        if (_skill.weapon.owner != null)
            _skill.weapon.owner.enabled = true;

        SkillUIManager.Instance.ClosePopupPanel();
    }

    public bool Execute()
    {
        _skill.CurrentCoolTime -= Time.deltaTime;

        // 테스트용
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            _skill.CurrentCoolTime = 1.0f;
        }
#endif

        if (_skill.CurrentCoolTime <= 0.0f)
        {
            _skill.CurrentCoolTime = 0.0f;

            if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
            {
                _skill.AddCommand(new ActiveSkillCommand(_skill));
            }
            
            return true;
        }

        return false;
    }
    
    public void OnComplete()
    {
        _skill.OnButtonActivate?.Invoke(true);
    }

    public void Undo()
    {
        _skill.OnButtonActivate?.Invoke(true);
        _skill.CurrentCoolTime = _skill.Data.CoolTime;
    }
}
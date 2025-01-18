using UnityEngine;

public class CooldownCommand : ICommand
{
    protected readonly ActiveSkillBase skill;

    public CooldownCommand(ActiveSkillBase skill)
    {
        this.skill = skill;
        
        if (this.skill.weapon.owner != null)
            this.skill.weapon.owner.enabled = true;

        SkillUIManager.Instance.ClosePopupPanel();
    }

    public virtual bool Execute()
    {
        skill.CurrentCoolTime -= Time.deltaTime;

        // 테스트용
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            skill.CurrentCoolTime = 1.0f;
        }
#endif

        if (skill.CurrentCoolTime <= 0.0f)
        {
            skill.CurrentCoolTime = 0.0f;
            return true;
        }

        return false;
    }
    
    public virtual void OnComplete()
    {
        if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
        {
            skill.AddCommand(new CheckForEnemiesCommand(skill as ClickTypeSkill));
        }
        
        skill.OnButtonActivate?.Invoke(true);
    }

    public void Undo()
    {
        skill.OnButtonActivate?.Invoke(true);
        skill.CurrentCoolTime = skill.Data.CoolTime;
    }
}
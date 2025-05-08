using UnityEngine;

public class ActiveSkillCommand : ICommand
{
    private readonly ActiveSkillBase _skill;
    
    public ActiveSkillCommand(ActiveSkillBase skill)
    {
        _skill = skill;

        if (_skill.IsActive)
        {
            _skill.OnActiveExit();
        }
        
        SkillUIManager.Instance.ShowPopupPanel(3);
        _skill.IsActive = true;

        LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
        _skill.IndicatorMonsters = RangeDetectionUtility.GetAttackTargets(_skill.Indicator.Collider, layerMask);
        
        _skill.OnActiveEnter();
        
        if (_skill is InstantaneousSkill)
        {
            _skill.CurrentCoolTime = _skill.Data.CoolTime;
            _skill.ExecuteCoolTimeCommand();
        }
    }
    
    public bool Execute()
    {
        return _skill.OnActiveExecute();
    }
    
    public void OnComplete()
    {
        _skill.OnActiveExit();
        _skill.IsActive = false;

        if (_skill is not InstantaneousSkill)
        {
            _skill.CurrentCoolTime = _skill.Data.CoolTime;
            _skill.ExecuteCoolTimeCommand();
        }
    }

    public void Undo()
    {
        _skill.OnActiveExit();
        _skill.IsActive = false;
        
        if (_skill is not InstantaneousSkill)
        {
            _skill.CurrentCoolTime = _skill.Data.CoolTime;
            _skill.ExecuteCoolTimeCommand();
        }
    }
}
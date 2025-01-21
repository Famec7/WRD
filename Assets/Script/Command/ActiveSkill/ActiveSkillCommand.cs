using UnityEngine;

public class ActiveSkillCommand : ICommand
{
    private readonly ActiveSkillBase _skill;
    
    public ActiveSkillCommand(ActiveSkillBase skill)
    {
        _skill = skill;
        
        SkillUIManager.Instance.ShowPopupPanel(3);
        _skill.OnButtonActivate?.Invoke(false);
        _skill.IsActive = true;

        LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
        _skill.IndicatorMonsters = RangeDetectionUtility.GetAttackTargets(_skill.Indicator.Collider, layerMask);
        
        _skill.OnActiveEnter();
    }
    
    public bool Execute()
    {
        return _skill.OnActiveExecute();
    }
    
    public void OnComplete()
    {
        _skill.OnActiveExit();
        _skill.CurrentCoolTime = _skill.Data.CoolTime;
        _skill.IsActive = false;
        
        _skill.ExecuteCoolTimeCommand();
    }

    public void Undo()
    {
        _skill.OnActiveExit();
        _skill.CurrentCoolTime = _skill.Data.CoolTime;
        _skill.IsActive = false;
        
        _skill.ExecuteCoolTimeCommand();
    }
}
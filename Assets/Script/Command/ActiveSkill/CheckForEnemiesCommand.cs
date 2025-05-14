using System.Collections;
using UnityEngine;

public class CheckForEnemiesCommand : ICommand
{
    private readonly ClickTypeSkill _skill;
    
    public CheckForEnemiesCommand(ClickTypeSkill skill)
    {
        _skill = skill;
    }

    public bool Execute()
    {
        if (SettingManager.Instance.CurrentActiveSettingType != SettingManager.ActiveSettingType.Auto)
        {
            return true;
        }

        if (_skill.weapon.owner.Target is null)
        {
            return false;
        }

        Vector2 targetPosition = _skill.weapon.owner.Target.transform.position;
        
        _skill.ClickPosition = targetPosition;
        _skill.ShowIndicator(_skill.ClickPosition, false);
        Physics2D.SyncTransforms();
        
        _skill.AddCommand(new ActiveSkillCommand(_skill));

        return true;
    }

    public void OnComplete()
    {
        ;
    }

    public void Undo()
    {
        _skill.ClearTargetMonsters();
    }

}
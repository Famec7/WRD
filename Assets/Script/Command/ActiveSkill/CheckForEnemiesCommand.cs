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
        GameObject nearestTarget = _skill.weapon.owner.FindNearestTarget();
        if (nearestTarget == null)
        {
            return false;
        }
        
        _skill.ClickPosition = nearestTarget.transform.position;
        _skill.ShowIndicator(_skill.ClickPosition);

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
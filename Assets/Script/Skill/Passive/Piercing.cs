using UnityEngine;

public class Piercing : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (!CheckTrigger()) return false;
        
        if(target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.GetValue(0));
        }

        return false;
    }
}
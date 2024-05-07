using UnityEngine;

public class Pierce : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (CheckTrigger())
        {
            if(target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(skillData.skillDamage);
                return true;
            }
        }

        return false;
    }
}
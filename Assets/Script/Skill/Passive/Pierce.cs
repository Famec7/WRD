using UnityEngine;

public class Pierce : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (CheckTrigger())
        {
            if(target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(data.values[1]);
                return true;
            }
        }

        return false;
    }
}
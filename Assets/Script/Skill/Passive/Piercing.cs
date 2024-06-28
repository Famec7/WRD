using UnityEngine;

public class Pierce : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (CheckTrigger())
        {
            if(target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));
                return true;
            }
        }

        return false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAttack : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        CompoundBow compoundBow = weapon as CompoundBow;
        compoundBow.IsMarked = false;
        
        if (target.TryGetComponent(out Status status))
        {
            var mark = StatusEffectManager.Instance.GetStatusEffect(status, typeof(Mark));

            if (mark != null)
            {
                compoundBow.IsMarked = true;
            }
        }
        
        return false;
    }
}
